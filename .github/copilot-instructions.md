# GitHub Copilot Instructions

## Project Overview
This is PanoramicData.ECharts (Vizor ECharts), a Blazor component library that provides ECharts integration for .NET applications.

## Technology Stack
- **Framework**: .NET 10
- **UI Framework**: Blazor
- **Charting Library**: Apache ECharts (JavaScript interop)

## Project Structure
- `PanoramicData.ECharts` - Main library with Blazor components
- `PanoramicData.ECharts.Demo` - Demo Blazor application
- `PanoramicData.ECharts.Samples` - Sample implementations
- `PanoramicData.ECharts.Test` - Test project (xUnit)
- `PanoramicData.ECharts.Sandbox` - Experimental/development workspace
- `PanoramicData.ECharts.BindingGenerator` - Code generation utilities

## Coding Conventions
- Target framework: .NET 10
- Primary UI paradigm: Blazor (prioritize Blazor patterns over Razor Pages or MVC)
- Follow existing code style and conventions in the codebase
- Minimize dependencies - use existing libraries when possible

## Testing Configuration
- Test framework: xUnit
- Tests run with `stopOnFail: true` (fail-fast mode)
- Parallel test collections enabled
- Max 4 parallel threads
- Assembly parallelization disabled

## Publishing Workflow
- **Default publishing**: Always run `Publish.ps1` without any options unless specifically requested otherwise
- Quick publish script (`Quick-Publish.ps1`) available for rapid iterations
- See `PUBLISH_GUIDE.md` for detailed publishing instructions

## Repository Information
- Primary repository: https://github.com/panoramicdata/vizor-echarts
- Upstream fork: https://github.com/datahint-eu/vizor-echarts
- Branch: main

## Development Guidelines
1. Prioritize Blazor-specific solutions and patterns
2. Maintain compatibility with .NET 10
3. Keep changes minimal and focused
4. Validate changes don't break existing functionality
5. Follow existing comment style (only add when necessary or matching existing patterns)

## Common Tasks
- Chart component development and customization
- JavaScript interop for ECharts functionality
- Blazor component lifecycle management
- Type-safe bindings generation
