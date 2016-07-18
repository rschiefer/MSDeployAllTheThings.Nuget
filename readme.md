![MSDeploy All The Things](https://rschiefer.gallery.vsassets.io/_apis/public/gallery/publisher/rschiefer/extension/MSDeployAllTheThings/0.0.3/assetbyname/Microsoft.VisualStudio.Services.Icons.Default "MSDeploy All The Things")

# #MSDeployAlltheThings Nuget Packages

## The purpose of this project is to make deployment of .NET applications and SQL databases easier.  

This is accomplished using MSDeploy.exe.  MSDeploy provides advanced deployment features which are utilized by WebDeploy.  Most developers know about WebDeploy but don't realize MSDeploy is doing the bulk of the work behind the scenes.

More specifically MSDeploy supports packaging of files for deployment.  This is an incredibly powerful tool that can be leveraged for more than just Web Applications. 

## WebDeploy Overview

WebDeploy is basically a set of MSBuild extensions (.targets files) and Visual Studio extensions which trigger MSDeploy to create a package and, if desired, deploy the package.  WebDeploy additionally provide plenty of bells and whistle if needed.

## MSDeploy Overview

MSDeploy is a commandline utility and framework for handling the deployment of code.  It a [vast array of providers](https://technet.microsoft.com/en-us/library/dd569040(v=ws.10).aspx) that can be used to accomplish different tasks from creating an IIS Web Application to installing a COM component.  It is also extensible, allowing you to create your own custom providers.  You can easily leverage it in automated CI/CD orchestration systems because it is a commandline utility.  It also support configuration changes via WebDeploy Parameterization.

## Nuget Packages

The MSDeployAllTheThings Nuget packages make it easier to create MSDeploy packages as part of your build process.  These packages can be deployed just like WebDeploy packages in most cases.

## Setup

### Step 1 - Choose the right Nuget Package

For any application hosted via ASP.NET (MVC, Webforms, WCF, etc) you should use the WebDeployPlus Nuget package.

For database projects use the SqlDeploy Nuget package.

For almost any other type of application (console, windows service, etc) use the AppDeploy package.  This option basically provides directory copy deployment functionality.  It can be use in many non-standard scenarios.  For example, we have even used AppDeploy for SSIS deployments.

### Step 2 - Enable MSDeploy Packaging

Add the `/p:DeployOnBuild=true` MSBuild flag to enable packaging.  This will engage the MSBuild scripts added by the Nuget package to run MSDeploy.exe and create an MSDeploy deployment package of the component.

### Step 3 - Run Deploy the Package

At this point you can run MSDeploy.exe manually referencing the package (zip) file or you can utilize the generated `*.deploy.cmd` file to deploy the package.  The deploy command will provide help details as the default output.
