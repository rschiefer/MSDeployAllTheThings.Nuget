param($rootPath, $toolsPath, $package, $project)

# Add targets file to .csproj
$TargetsFile = 'WebDeployPlus.targets'
$path = [System.IO.Path]
$ProjectDirectory = $path::GetDirectoryName($project.FileName)

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) |
    Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($project.FullName)"
#$TargetsPath = $project.ProjectItems.Item("deployment").ProjectItems.Item($TargetsFile).FileName
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$toolsPath/$TargetsFile"

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

