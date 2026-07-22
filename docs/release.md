# Releasing a new package version

### 1. Decide on the new release's version number

Version numbers should follow [semantic versioning](https://semver.org/).

### 2. Add or update CHANGELOG.md

Add an entry to the change log for the new version & push to main.

### 3. Create a new GitHub release

From [GitHub's releases page](https://github.com/DFE-Digital/CSharp-.NET-Template/releases), click 'Draft a new release'.

In the 'Tag' drop-down, 'Create a new tag' and enter 'v1.2.3' where '1.2.3' is the version number for the new release.

In the 'Release title', enter 'v1.2.3' where '1.2.3' is the version number for the new release.

In the 'Release notes', copy the contents of the CHANGELOG.md entry for the new release.

Click 'Publish release'.

Publishing the release will create a new git tag, which will run the `build.yml` workflow.
This workflow will build the package and push to nuget.org.