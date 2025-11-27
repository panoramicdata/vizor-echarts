# Phase 14: Deployment ?

**Status**: PENDING  
**Duration**: 1-2 hours  
**Priority**: High (Final release step)

---

## Overview

Publish the NuGet package and symbol package to NuGet.org, create GitHub release, and announce the update.

---

## 14.1 Publish to NuGet

### Publish Main Package
- [ ] Push .nupkg to NuGet.org using `Publish.ps1`
  ```powershell
  .\Publish.ps1
  ```
- [ ] Verify package appears correctly on NuGet.org
- [ ] Check package page displays properly
  - [ ] Version number correct
  - [ ] Description accurate
  - [ ] Dependencies listed
  - [ ] README displays

### Publish Symbol Package  
- [ ] Push .snupkg to NuGet.org
  ```powershell
  dotnet nuget push <package>.snupkg --api-key <key> --source https://api.nuget.org/v3/index.json
  ```
- [ ] Verify symbols are available
- [ ] Test debugging with symbols

### Verify Installation
- [ ] Install from NuGet.org in test project
- [ ] Verify version is correct
- [ ] Test basic functionality
- [ ] Check for any installation errors

---

## 14.2 Update GitHub

### Create GitHub Release
- [ ] Go to https://github.com/panoramicdata/vizor-echarts/releases
- [ ] Click "Draft a new release"
- [ ] Select tag `v6.0.0`
- [ ] Title: "Version 6.0.0 - ECharts 6.0 Upgrade"
- [ ] Attach release notes from Phase 13.2
- [ ] Attach artifacts (optional):
  - [ ] .nupkg file
  - [ ] .snupkg file

### Update Repository
- [ ] Update README if needed
- [ ] Close related issues
- [ ] Update project board (if applicable)

---

## 14.3 Announce

### NuGet Package
- [ ] Monitor NuGet package page
- [ ] Respond to any immediate feedback

### GitHub
- [ ] Post release announcement
- [ ] Update repository description if needed
- [ ] Pin important issues/discussions

### Community (Optional)
- [ ] Announce on social media/forums
- [ ] Notify existing users of upgrade
- [ ] Post in relevant .NET communities
- [ ] Update documentation sites

---

## Deployment Checklist

### Pre-Deployment Verification
- [ ] All tests passing
- [ ] Documentation complete
- [ ] Release notes reviewed
- [ ] Package validated locally
- [ ] NuGet API key available

### Deployment Steps
1. [ ] Run `Publish.ps1` script
2. [ ] Verify package on NuGet.org
3. [ ] Create GitHub release
4. [ ] Update repository

### Post-Deployment Verification
- [ ] Package installs correctly
- [ ] Version number is correct
- [ ] Dependencies resolve
- [ ] Symbols available for debugging

---

## Rollback Plan

If critical issues are discovered after deployment:

1. **Immediate Actions**
   - [ ] Unlist broken NuGet package
   - [ ] Post warning on GitHub
   - [ ] Assess severity and impact

2. **Fix Options**
   - [ ] Hotfix branch and patch release
   - [ ] OR revert and plan better upgrade

---

## Success Metrics

- [ ] Package published successfully
- [ ] No installation errors reported
- [ ] GitHub release created
- [ ] Community notified
- [ ] Initial downloads within 24 hours

---

## Completion Status

? **PENDING** - Requires Phase 13 completion

Next: After deployment, proceed to [Phase 15](PHASE_15_POST_DEPLOYMENT_MONITORING.md)

---

[? Back to Master Plan](../MASTER_PLAN.md) | [? Previous Phase](PHASE_13_DEPLOYMENT_PREPARATION.md) | [Next Phase ?](PHASE_15_POST_DEPLOYMENT_MONITORING.md)
