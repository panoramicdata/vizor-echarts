# Publish.ps1
# Publishes PanoramicData.ECharts to NuGet after running all validation checks

param(
    [switch]$SkipTests,
    [switch]$Force,
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$OriginalLocation = Get-Location

# ANSI color codes for output
$Red = "`e[31m"
$Green = "`e[32m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = $Reset
    )
    Write-Host "$Color$Message$Reset"
}

function Write-Step {
    param([string]$Message)
    Write-ColorOutput "`n===> $Message" $Blue
}

function Write-Success {
    param([string]$Message)
    Write-ColorOutput "? $Message" $Green
}

function Write-Error {
    param([string]$Message)
    Write-ColorOutput "? $Message" $Red
}

function Write-Warning {
    param([string]$Message)
    Write-ColorOutput "? $Message" $Yellow
}

function Test-GitClean {
    Write-Step "Checking Git working directory status..."
    
    $status = git status --porcelain
    if ($status) {
        Write-Error "Git working directory is not clean. Uncommitted changes detected:"
        git status --short
        
        if (-not $Force) {
            throw "Cannot publish with uncommitted changes. Commit or stash changes, or use -Force to override."
        }
        
        Write-Warning "Proceeding despite uncommitted changes due to -Force flag"
    }
    
    Write-Success "Git working directory is clean"
}

function Test-NuGetApiKey {
    Write-Step "Checking for NuGet API key..."
    
    $keyFile = Join-Path $PSScriptRoot "nuget-key.txt"
    
    if (-not (Test-Path $keyFile)) {
        Write-Error "NuGet API key file not found: $keyFile"
        Write-Host "`nPlease create a file named 'nuget-key.txt' in the repository root containing your NuGet API key."
        Write-Host "This file is .gitignored and will not be committed."
        throw "NuGet API key file not found"
    }
    
    $apiKey = (Get-Content $keyFile -Raw).Trim()
    
    if ([string]::IsNullOrWhiteSpace($apiKey)) {
        Write-Error "NuGet API key file is empty: $keyFile"
        throw "NuGet API key is required"
    }
    
    Write-Success "NuGet API key found"
    return $apiKey
}

function Build-Solution {
    Write-Step "Building solution in $Configuration configuration..."
    
    dotnet build --configuration $Configuration
    
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE"
    }
    
    Write-Success "Build completed successfully"
}

function Start-DemoServer {
    Write-Step "Starting demo server..."
    
    $demoProjectPath = Join-Path $PSScriptRoot "PanoramicData.ECharts.Demo\PanoramicData.ECharts.Demo.csproj"
    
    if (-not (Test-Path $demoProjectPath)) {
        throw "Demo project not found: $demoProjectPath"
    }
    
    # Start the demo server in a background job
    $job = Start-Job -ScriptBlock {
        param($projectPath, $configuration)
        Set-Location (Split-Path $projectPath -Parent)
        dotnet run --configuration $configuration --no-build
    } -ArgumentList $demoProjectPath, $Configuration
    
    # Wait for server to start (max 30 seconds)
    Write-Host "Waiting for demo server to start..."
    $maxWaitSeconds = 30
    $waited = 0
    $serverReady = $false
    
    while ($waited -lt $maxWaitSeconds) {
        Start-Sleep -Seconds 1
        $waited++
        
        try {
            $response = Invoke-WebRequest -Uri "http://localhost:5185" -TimeoutSec 2 -UseBasicParsing -ErrorAction SilentlyContinue
            if ($response.StatusCode -eq 200) {
                $serverReady = $true
                break
            }
        }
        catch {
            # Server not ready yet, continue waiting
        }
        
        # Check if job failed
        if ($job.State -eq "Failed" -or $job.State -eq "Stopped") {
            $jobError = Receive-Job -Job $job 2>&1
            Remove-Job -Job $job -Force
            throw "Demo server failed to start: $jobError"
        }
    }
    
    if (-not $serverReady) {
        Stop-Job -Job $job
        Remove-Job -Job $job -Force
        throw "Demo server did not start within $maxWaitSeconds seconds"
    }
    
    Write-Success "Demo server started successfully on http://localhost:5185"
    return $job
}

function Run-Tests {
    param($DemoServerJob)
    
    Write-Step "Running tests..."
    
    $testProjectPath = Join-Path $PSScriptRoot "PanoramicData.ECharts.Test\PanoramicData.ECharts.Test.csproj"
    
    if (-not (Test-Path $testProjectPath)) {
        throw "Test project not found: $testProjectPath"
    }
    
    dotnet test $testProjectPath --configuration $Configuration --no-build --logger "console;verbosity=normal"
    
    $testResult = $LASTEXITCODE
    
    if ($testResult -ne 0) {
        throw "Tests failed with exit code $testResult"
    }
    
    Write-Success "All tests passed"
}

function Stop-DemoServer {
    param($Job)
    
    if ($null -eq $Job) {
        return
    }
    
    Write-Step "Stopping demo server..."
    
    Stop-Job -Job $Job -ErrorAction SilentlyContinue
    Remove-Job -Job $Job -Force -ErrorAction SilentlyContinue
    
    # Also kill any lingering dotnet processes on port 5185
    $processes = Get-NetTCPConnection -LocalPort 5185 -ErrorAction SilentlyContinue | 
                 Select-Object -ExpandProperty OwningProcess -Unique
    
    foreach ($processId in $processes) {
        try {
            $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
            if ($process -and $process.ProcessName -eq "dotnet") {
                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
            }
        }
        catch {
            # Ignore errors stopping processes
        }
    }
    
    Write-Success "Demo server stopped"
}

function Publish-Package {
    param([string]$ApiKey)
    
    Write-Step "Publishing package to NuGet..."
    
    $projectPath = Join-Path $PSScriptRoot "PanoramicData.ECharts\PanoramicData.ECharts.csproj"
    
    if (-not (Test-Path $projectPath)) {
        throw "Project file not found: $projectPath"
    }
    
    # Find the generated package
    $packagePattern = Join-Path $PSScriptRoot "PanoramicData.ECharts\bin\$Configuration\*.nupkg"
    $packages = Get-ChildItem -Path $packagePattern -ErrorAction SilentlyContinue
    
    if (-not $packages) {
        throw "No NuGet package found at: $packagePattern"
    }
    
    # Get the most recent package
    $package = $packages | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    
    Write-Host "Package: $($package.Name)"
    Write-Host "Size: $([math]::Round($package.Length / 1MB, 2)) MB"
    Write-Host ""
    
    # Ask for confirmation
    $confirmation = Read-Host "Publish this package to NuGet.org? (yes/no)"
    
    if ($confirmation -ne "yes") {
        throw "Publication cancelled by user"
    }
    
    # Publish to NuGet
    dotnet nuget push $package.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json
    
    if ($LASTEXITCODE -ne 0) {
        throw "NuGet publish failed with exit code $LASTEXITCODE"
    }
    
    Write-Success "Package published successfully to NuGet.org"
    Write-Host ""
    Write-ColorOutput "Package URL: https://www.nuget.org/packages/PanoramicData.ECharts/" $Green
}

# Main execution
try {
    Write-ColorOutput "`n??????????????????????????????????????????????" $Blue
    Write-ColorOutput "?  PanoramicData.ECharts NuGet Publisher     ?" $Blue
    Write-ColorOutput "??????????????????????????????????????????????" $Blue
    
    # Step 1: Check Git status
    Test-GitClean
    
    # Step 2: Check for NuGet API key
    $apiKey = Test-NuGetApiKey
    
    # Step 3: Build solution
    Build-Solution
    
    if (-not $SkipTests) {
        $demoJob = $null
        
        try {
            # Step 4: Start demo server
            $demoJob = Start-DemoServer
            
            # Step 5: Run tests
            Run-Tests -DemoServerJob $demoJob
        }
        finally {
            # Step 6: Stop demo server
            Stop-DemoServer -Job $demoJob
        }
    }
    else {
        Write-Warning "Skipping tests due to -SkipTests flag"
    }
    
    # Step 7: Publish to NuGet
    Publish-Package -ApiKey $apiKey
    
    Write-ColorOutput "`n??????????????????????????????????????????????" $Green
    Write-ColorOutput "?       Publication completed successfully!  ?" $Green
    Write-ColorOutput "??????????????????????????????????????????????" $Green
}
catch {
    Write-ColorOutput "`n??????????????????????????????????????????????" $Red
    Write-ColorOutput "?            Publication failed!              ?" $Red
    Write-ColorOutput "??????????????????????????????????????????????" $Red
    Write-Error "Error: $_"
    
    # Cleanup
    if ($demoJob) {
        Stop-DemoServer -Job $demoJob
    }
    
    Set-Location $OriginalLocation
    exit 1
}
finally {
    Set-Location $OriginalLocation
}
