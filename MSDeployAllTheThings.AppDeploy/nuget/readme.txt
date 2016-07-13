Application Publishing Pipeline - readme.txt
2013.01.07

----------------------------------------------------------------------------
Project Setup Instructions:

1 - Add any pre/post deployment steps to the batch files in the new deployment folder.

2 - Override the AppTypeFolder property in the *.app.targets file to set the application type for your app if its not a Service (Services is the default).

3 - Set the files you wish to deploy in the FilesToPackage ItemGroup.  

----------------------------------------------------------------------------
Build Definition Setup Instructions: 

1-Add the following MSBuild Parameters to your DEV build definition:

	/p:DeployOnBuild=true /p:CreatePackageAndDeployOnPublish=true /p:DestinationComputerName=[DEV Server]

2-Add the following MSBuild Parameters to your QA build definition:

	/p:DeployOnBuild=true /p:CreatePackageAndDeployOnPublish=true /p:DestinationComputerName=[QA Server]

3-Add the following MSBuild Parameters to your MOCK build definition:

	/p:DeployOnBuild=true /p:DestinationComputerName=[MOCK Server]

4-Add the following MSBuild Parameters to your PROD build definition:

	/p:DeployOnBuild=true /p:DestinationComputerName=[PROD Server]

Options:

You may optionally add a "AppName" MSBuild parameter to override the default (project name).