# SA3D.Rendering.WPF
Supplies a WPF control for displaying and handling a `SA3D.Rendering` render context.

---

## Releasing
!! Requires authorization via the X-Hax organisation

1. Edit the version number in src/SA3D.Rendering.WPF/SA3D.Rendering.WPF.csproj; Example: `<Version>1.0.0</Version>` -> `<Version>2.0.0</Version>`
2. Commit the change but dont yet push.
3. Tag the commit: `git tag -a [version number] HEAD -m "Release version [version number]"`
4. Push with tags: `git push --follow-tags`

This will automatically start the Github `Build and Publish` workflow
