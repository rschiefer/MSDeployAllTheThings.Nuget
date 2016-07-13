param($rootPath, $toolsPath, $package, $project)

# Rename app.targets file
$appFile = $project.ProjectItems.Item($project.Properties.Item("RootNamespace").Value + '.app.targets')
$appFile.Name = "UpdateToProjectName.app.targets"

# Remove app.targets file
#$appFile.Remove()
#$appFile.Delete()

# Remove targets import
$TargetsFile = 'AppPublishingPipeline.targets'
Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) |
    Select-Object -First 1

$ExistingImports = $MSBProject.Xml.Imports |
    Where-Object { $_.Project -like "*\$TargetsFile" }
if ($ExistingImports) {
    $ExistingImports | 
        ForEach-Object {
            $MSBProject.Xml.RemoveChild($_) | Out-Null
        }
    $project.Save()
}