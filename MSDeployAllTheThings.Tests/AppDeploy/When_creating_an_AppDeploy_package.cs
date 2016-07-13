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
    public class When_creating_an_AppDeploy_package : BaseTest
    {
        protected string _postSyncFilePath;
        protected string _postSyncText;
        private string _relativeSampleProjectPath;
        private string _packagePath;
        private string _sampleProjectPath;
        private string _projectName;

        static bool _packageCreated = false;
        private List<string> _msbuildArgs = new List<string>();

        public override void Context()
        {
            base.Context();

            _projectName = "MSDeployAllTheThings.Tests.AppDeploySample";
            _relativeSampleProjectPath = $"\\..\\..\\..\\{_projectName}\\";
            _sampleProjectPath = TestContext.CurrentContext.WorkDirectory + _relativeSampleProjectPath;
            _packagePath = _sampleProjectPath + "bin\\debug\\appDeployment\\";
        }

        public override void Because()
        {
            base.Because();
            
            if (_packageCreated == false)
            {
                var workingDirectory = TestContext.CurrentContext.WorkDirectory + _relativeSampleProjectPath;
                CmdExecutionHelper.ExecuteCommand(new[]
                    {
                        "/c",
                        @"""C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild""",
                        "/p:DeployOnBuild=true",
                        string.Join(" ", _msbuildArgs) 
                    }
                    , workingDirectory);
                _packageCreated = true;
            }
        }

        [Test]
        public void Should_copy_postSync_file_to_package_folder()
        {
            Because();

            Assert.That(File.Exists($"{_packagePath}postSync.bat"), Is.True, "PostSync file exists in package folder.");
        }
        [Test]
        public void Should_copy_preSync_file_to_package_folder()
        {
            Because();

            Assert.That(File.Exists($"{_packagePath}preSync.bat"), Is.True, "PreSync file exists in package folder.");
        }
        [Test]
        public void Should_create_deploy_command_file_in_package_folder()
        {
            Because();

            Assert.That(File.Exists(_packagePath + $"{_projectName}.appDeploy.cmd"), Is.True, "Deploy command file exists in package folder.");
        }
        [Test]
        public void Should_create_MSDeploy_package_in_package_folder()
        {
            Because();

            Assert.That(File.Exists($"{_packagePath}{_projectName}.appDeploy.package.zip"), Is.True, "MSDeploy package file exists in package folder.");
        }
        [Test]
        public void Should_update_app_type_folder()
        {
            _packageCreated = false;
            _msbuildArgs.Add("/p:AppTypeFolder=TestFolder");

            Because();

            var deployCmdText = File.ReadAllText(_packagePath + $"{_projectName}.appDeploy.cmd");
            Assert.That(deployCmdText, Is.StringContaining("TestFolder"));
        }
    }
}
