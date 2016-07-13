param($rootPath, $toolsPath, $package, $project)

# Rename app.targets file
$appFile = $project.ProjectItems.Item("UpdateToProjectName.spp.targets")
$appFile.Name = $project.Properties.Item("RootNamespace").Value + '.spp.targets'

# Add targets file to .csproj
$TargetsFile = 'SqlPublishingPipeline.targets'
$path = [System.IO.Path]
$ProjectDirectory = $path::GetDirectoryName($project.FileName)
$TargetsPath = [System.IO.Path]::Combine($toolsPath, $TargetsFile)

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) |
    Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($project.FullName)"
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$TargetsPath"

$RelativePath = $ProjectUri.MakeRelativeUri($TargetUri) -replace '/','\'

$ExistingImports = $MSBProject.Xml.Imports |
    Where-Object { $_.Project -like "*\$TargetsFile" }
if ($ExistingImports) {
    $ExistingImports | 
        ForEach-Object {
            $MSBProject.Xml.RemoveChild($_) | Out-Null
        }
}
$MSBProject.Xml.AddImport($RelativePath) | Out-Null
$project.Save()

