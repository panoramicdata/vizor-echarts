# Publishing to NuGet

This document describes how to publish PanoramicData.ECharts to NuGet.org using the automated `Publish.ps1` script.

## Prerequisites

1. **NuGet API Key**: You need a valid NuGet.org API key
2. **Git Clean Working Directory**: All changes should be committed (unless using `-Force`)
3. **Demo Server Available**: The demo project must be able to run on http://localhost:5185
4. **Tests Must Pass**: All tests in the Test project must pass

## Setup

1. Create a file named `nuget-key.txt` in the repository root:
   ```powershell
   # Replace YOUR_API_KEY with your actual NuGet.org API key
   echo "YOUR_API_KEY" > nuget-key.txt
   ```

2. This file is automatically .gitignored and will NOT be committed to the repository.

## Usage

### Basic Usage (Recommended)

```powershell
.\Publish.ps1
```

This will:
1. ? Check that Git working directory is clean
2. ? Verify NuGet API key exists
3. ? Build the solution in Release configuration
4. ? Start the demo server
5. ? Run all tests
6. ? Stop the demo server
7. ? Prompt for confirmation
8. ? Publish to NuGet.org

### Options

#### Skip Tests
```powershell
.\Publish.ps1 -SkipTests
```
Skips running the test suite (not recommended for production releases).

#### Force Publish
```powershell
.\Publish.ps1 -Force
```
Publishes even if Git working directory has uncommitted changes (not recommended).

#### Debug Configuration
```powershell
.\Publish.ps1 -Configuration Debug
```
Builds and publishes using Debug configuration instead of Release.

#### Combined Options
```powershell
.\Publish.ps1 -SkipTests -Force -Configuration Debug
```

## What the Script Does

### 1. Git Status Check
- Verifies the working directory is clean (no uncommitted changes)
- Can be overridden with `-Force` flag

### 2. NuGet API Key Verification
- Checks for `nuget-key.txt` in repository root
- Validates the file is not empty

### 3. Build Solution
- Builds the entire solution in specified configuration (default: Release)
- Fails if build errors occur

### 4. Start Demo Server
- Launches the demo project in a background process
- Waits up to 30 seconds for server to be ready
- Verifies server responds on http://localhost:5185

### 5. Run Tests
- Executes all tests in PanoramicData.ECharts.Test project
- Tests run against the live demo server
- Fails if any tests fail

### 6. Stop Demo Server
- Gracefully stops the demo server
- Cleans up any lingering processes

### 7. Publish Package
- Locates the generated .nupkg file
- Displays package information
- Prompts for confirmation
- Publishes to NuGet.org using the API key

## Troubleshooting

### "Git working directory is not clean"
**Solution**: Commit or stash your changes, or use `-Force` to override.

### "NuGet API key file not found"
**Solution**: Create `nuget-key.txt` in the repository root with your NuGet.org API key.

### "Demo server did not start within 30 seconds"
**Solution**: 
- Check if port 5185 is already in use
- Verify the demo project builds successfully
- Check demo project logs for errors

### "Tests failed"
**Solution**: 
- Run tests manually: `dotnet test PanoramicData.ECharts.Test\PanoramicData.ECharts.Test.csproj`
- Fix failing tests before publishing
- Alternatively, use `-SkipTests` (not recommended for production)

### "Package already exists on NuGet"
**Solution**: 
- Increment the version number in the project
- NuGet.org does not allow overwriting published packages

## Security Notes

- ?? **Never commit `nuget-key.txt` to Git**
- ?? The file is .gitignored, but always verify before pushing
- ?? Regenerate your API key if it's accidentally committed
- ?? Use scoped API keys with minimum required permissions

## Getting a NuGet API Key

1. Log in to https://www.nuget.org
2. Go to your account settings
3. Select "API Keys"
4. Create a new API key:
   - **Glob Pattern**: `PanoramicData.ECharts`
   - **Scopes**: Push
   - **Packages**: Select specific packages or use glob pattern
5. Copy the generated key to `nuget-key.txt`

## Version Management

The package version is managed by Nerdbank.GitVersioning and automatically incremented based on:
- Git tags
- Commit height
- Branch name

To create a new release:
```powershell
# Tag the current commit
git tag v1.2.3
git push origin v1.2.3

# Then publish
.\Publish.ps1
```

## Post-Publication

After successful publication:

1. Verify the package on NuGet.org: https://www.nuget.org/packages/PanoramicData.ECharts/
2. Create a GitHub release with release notes
3. Update the changelog
4. Notify users of the new version

## Example Session

```powershell
PS> .\Publish.ps1

??????????????????????????????????????????????
?  PanoramicData.ECharts NuGet Publisher     ?
??????????????????????????????????????????????

===> Checking Git working directory status...
? Git working directory is clean

===> Checking for NuGet API key...
? NuGet API key found

===> Building solution in Release configuration...
? Build completed successfully

===> Starting demo server...
Waiting for demo server to start...
? Demo server started successfully on http://localhost:5185

===> Running tests...
? All tests passed

===> Stopping demo server...
? Demo server stopped

===> Publishing package to NuGet...
Package: PanoramicData.ECharts.1.2.3.nupkg
Size: 2.45 MB

Publish this package to NuGet.org? (yes/no): yes
? Package published successfully to NuGet.org

Package URL: https://www.nuget.org/packages/PanoramicData.ECharts/

??????????????????????????????????????????????
?       Publication completed successfully!  ?
??????????????????????????????????????????????
