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

---

## Project Tracking and Documentation

### Master Plan Maintenance
- **Location**: `MASTER_PLAN.md` (root) and `docs/PHASE_*.md` files
- **Current Status**: ECharts 6.0 upgrade in progress (see MASTER_PLAN.md)

### Session Documentation Requirements
When completing work in any session, **always update both**:

1. **Individual Phase Documents** (`docs/PHASE_*.md`)
   - Mark completed tasks with ?
   - Update status (? Complete | ?? In Progress | ? Pending)
   - Add results, findings, or artifacts created
   - Update completion criteria

2. **Master Plan** (`MASTER_PLAN.md`)
   - Update phase status in the Phase Summary table
   - Update "Current Progress" section
   - Move completed items from "In Progress" to "Completed"
   - Update blocking issues if resolved
   - Update time estimates if needed

### Documentation Update Checklist
At the end of each significant work session:
- [ ] Updated relevant `docs/PHASE_*.md` file(s)
- [ ] Updated `MASTER_PLAN.md` Phase Summary table
- [ ] Updated `MASTER_PLAN.md` Current Progress section
- [ ] Updated `.slnx` file "Project Planning" solution items (if phase documents added/modified)
- [ ] Committed changes with descriptive message
- [ ] Verified links between documents are correct

### When to Update
- ? After completing a phase or sub-phase
- ? After fixing critical bugs
- ? After making significant progress on a task
- ? After discovering new issues or blockers
- ? At the end of each work session
- ? Don't update for minor code tweaks or exploratory work

### Example Update Pattern
```markdown
# In docs/PHASE_XX.md
- [x] ? Task completed successfully
  - Result: XYZ achieved
  - Artifacts: File.cs created

# In MASTER_PLAN.md
| 11 | Build and Package | ? Complete | 2-3 hours | [PHASE_11](docs/PHASE_11_BUILD_AND_PACKAGE.md) |
```

### Commit Message Format
When updating documentation:
```
Update MASTER_PLAN and Phase X: [Brief description of completion]

- Completed [task name]
- Updated status to [new status]
- [Any notable findings or next steps]
