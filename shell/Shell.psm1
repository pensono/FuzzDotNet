function Start-VisualStudio() {
    dotnet new sln --force
    Get-ChildItem -Recurse *.csproj | ForEach { dotnet sln add $_.FullName }
    start FuzzDotNet.sln
}

Export-ModuleMember *-*