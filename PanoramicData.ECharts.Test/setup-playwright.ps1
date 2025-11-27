# Playwright Browser Installation Script
# Run this script from the PanoramicData.ECharts.Test directory

$InformationPreference = 'Continue'

Write-Information "==================================================" -InformationAction Continue
Write-Information "  Playwright Browser Installation for E2E Tests" -InformationAction Continue
Write-Information "==================================================" -InformationAction Continue
Write-Information ""

# Check if we're in the correct directory
if (-not (Test-Path "PanoramicData.ECharts.Test.csproj")) {
    Write-Information "ERROR: Please run this script from the PanoramicData.ECharts.Test directory" -InformationAction Continue
    exit 1
}

# Build the project first
Write-Information "Step 1: Building test project..." -InformationAction Continue
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Information "ERROR: Build failed. Please fix compilation errors first." -InformationAction Continue
    exit 1
}

Write-Information "✓ Build successful" -InformationAction Continue
Write-Information ""

# Check if playwright.ps1 exists
$playwrightScript = "bin\Debug\net10.0\playwright.ps1"

if (-not (Test-Path $playwrightScript)) {
    Write-Information "ERROR: Playwright script not found at $playwrightScript" -InformationAction Continue
    Write-Information "Make sure the Microsoft.Playwright package is installed." -InformationAction Continue
    exit 1
}

# Install browsers
Write-Information "Step 2: Installing Playwright browsers..." -InformationAction Continue
Write-Information "This may take a few minutes on first run..." -InformationAction Continue
Write-Information ""

& pwsh $playwrightScript install

if ($LASTEXITCODE -ne 0) {
    Write-Information "ERROR: Browser installation failed." -InformationAction Continue
    exit 1
}

Write-Information "" -InformationAction Continue
Write-Information "✓ Browsers installed successfully" -InformationAction Continue
Write-Information ""

# Summary
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  Installation Complete!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Start the demo application:" -ForegroundColor White
Write-Host "   cd ..\PanoramicData.ECharts.Demo" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "2. In another terminal, run the tests:" -ForegroundColor White
Write-Host "   cd PanoramicData.ECharts.Test" -ForegroundColor Gray
Write-Host "   dotnet test" -ForegroundColor Gray
Write-Host ""
Write-Host "For more information, see README.md in this directory." -ForegroundColor Cyan
Write-Host ""
