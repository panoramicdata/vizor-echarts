# Playwright Browser Installation Script
# Run this script from the PanoramicData.ECharts.Test directory

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  Playwright Browser Installation for E2E Tests" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Check if we're in the correct directory
if (-not (Test-Path "PanoramicData.ECharts.Test.csproj")) {
    Write-Host "ERROR: Please run this script from the PanoramicData.ECharts.Test directory" -ForegroundColor Red
    exit 1
}

# Build the project first
Write-Host "Step 1: Building test project..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Build failed. Please fix compilation errors first." -ForegroundColor Red
    exit 1
}

Write-Host "? Build successful" -ForegroundColor Green
Write-Host ""

# Check if playwright.ps1 exists
$playwrightScript = "bin\Debug\net10.0\playwright.ps1"

if (-not (Test-Path $playwrightScript)) {
    Write-Host "ERROR: Playwright script not found at $playwrightScript" -ForegroundColor Red
    Write-Host "Make sure the Microsoft.Playwright package is installed." -ForegroundColor Red
    exit 1
}

# Install browsers
Write-Host "Step 2: Installing Playwright browsers..." -ForegroundColor Yellow
Write-Host "This may take a few minutes on first run..." -ForegroundColor Gray
Write-Host ""

& pwsh $playwrightScript install

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Browser installation failed." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "? Browsers installed successfully" -ForegroundColor Green
Write-Host ""

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
