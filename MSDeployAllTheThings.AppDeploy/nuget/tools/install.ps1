param($rootPath, $toolsPath, $package, $project)

# Set pre/post sync commands to copy to output directory
$project.ProjectItems.Item("deployment").ProjectItems.Item("preSync.bat").Properties.Item("CopyToOutputDirectory").Value = 1
$project.ProjectItems.Item("deployment").ProjectItems.Item("postSync.bat").Properties.Item("CopyToOutputDirectory").Value = 1

# Rename app.targets file
$appFile = $project.ProjectItems.Item("UpdateToProjectName.app.targets")
$appFile.Name = $project.Properties.Item("RootNamespace").Value + '.app.targets'

# Add targets file to .csproj
$TargetsFile = 'AppPublishingPipeline.targets'
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

$import = $MSBProject.Xml.AddImport($RelativePath)
$import.Condition = '''$(DeployOnBuild)''==''true'''
$project.Save()

