using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDeployAllTheThings.Tests.AppDeploy
{
    [TestFixture]
    public class When_deploying_an_AppDeploy_package : BaseTest
    {
        protected string _postSyncFilePath;
        protected string _postSyncText;
        private string _relativeSampleProjectPath;
        private string _packagePath;
        private string _sampleProjectPath;
        private string _projectName;

        static bool _packageCreated = false;
        static bool _packageDeployed = false;
        private string _testDeploymentPath;

        public override void Context()
        {
            base.Context();

            _projectName = "MSDeployAllTheThings.Tests.AppDeploySample";
            _relativeSampleProjectPath = $"\\..\\..\\..\\{_projectName}\\";
            _sampleProjectPath = new DirectoryInfo(TestContext.CurrentContext.WorkDirectory + _relativeSampleProjectPath).FullName;
            _packagePath = _sampleProjectPath + "bin\\debug\\appDeployment\\";
            _testDeploymentPath = $"{_packagePath}testing\\";
        }

        public override void Because()
        {
            base.Because();
            
            if (_packageCreated == false || true)
            {
                if (Directory.Exists(_testDeploymentPath))
                    Directory.Delete(_testDeploymentPath);
                var deploymentDirectory = new DirectoryInfo(_testDeploymentPath).FullName;
                var workingDirectory = TestContext.CurrentContext.WorkDirectory + _relativeSampleProjectPath;

                CmdExecutionHelper.ExecuteCommand(new[] 
                    {
                        "/c",
                        @"""C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild""",
                        "/p:DeployOnBuild=true",
                        $@"/p:DestinationFilePath='{deploymentDirectory.Replace(":\\", "$\\")}'"
                    }
                    , workingDirectory);
                _packageCreated = true;
            }
            if (_packageDeployed == false)
            {

                CmdExecutionHelper.ExecuteCommand(new[]
                    {
                        "/c",
                        @"""C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe""",
                        "-verb:sync",
                        $"-source:package='{_packagePath}{_projectName}.appDeploy.package.zip'",
                        $"-dest:dirPath='{_testDeploymentPath}'"
                    },
                    _packagePath);
                _packageDeployed = true;
            }
        }

        [Test]
        public void Should_create_test_deployment_directory_in_package_folder()
        {
            Because();

            Assert.That(Directory.Exists(_testDeploymentPath), Is.True, "Test deployment directory exists in package folder.");
        }
    }
}
