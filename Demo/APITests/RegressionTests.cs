using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using APIDemo;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports;

namespace APITests
{
    [TestClass]
    public class RegressionTests
    {
        public object JsonConvert { get; private set; }
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            var dir = testContext.TestRunDirectory;
            Reporter.SetupExtentReporter("API Regression Test","API Regression Test Report", dir);
        }

        [TestInitialize]
        public void SetupTest()
        {
            Reporter.CreateTest(TestContext.TestName);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            var testStatus = TestContext.CurrentTestOutcome;
            Status logStatus;

            switch (testStatus)
            {
                case UnitTestOutcome.Failed:
                    logStatus = Status.Fail;
                    Reporter.TestStatus(logStatus.ToString());
                    break;
                case UnitTestOutcome.Inconclusive:
                    break;
                case UnitTestOutcome.Passed:
                    break;
                case UnitTestOutcome.InProgress:
                    break;
                case UnitTestOutcome.Error:
                    break;
                case UnitTestOutcome.Timeout:
                    break;
                case UnitTestOutcome.Aborted:
                    break;
                case UnitTestOutcome.Unknown:
                    break;
                case UnitTestOutcome.NotRunnable:
                    break;
                default:
                    break;
            }
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Reporter.FlushReport();
        }


        [TestMethod]
        public async Task VerifyListOfUsers()
        {
            string endpoint = "api/users?page=2";
            var demo = new Demo<ListOfUsersDTO>();
            var user = await demo.GetUsers(endpoint);
            
            try
            {
                Assert.AreEqual(2, user.Page);
            }
            catch (Exception ex)
            { 
                Reporter.LogToReport(Status.Fail, "Page number doesn't match");
                Assert.Fail(ex.Message);
            }

            try
            {
                Assert.AreEqual("Michael", user.Data[0].first_name);
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, "User first name doesn't match");
                Assert.Fail(ex.Message);
            }
        }

        [DeploymentItem(@"|DataDirectory|\TestData\TestCase.csv"),
        DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "../../TestData/TestCase.csv", "TestCase#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public async Task CreateNewUser()
        {
            string endpoint = "api/users";

            var users = new CreateUserRequestDTO();
            users.name = TestContext.DataRow["name"].ToString();
            Reporter.LogToReport(Status.Info, "TestData Username: " + users.name.ToString());
            users.job= TestContext.DataRow["job"].ToString();
            Reporter.LogToReport(Status.Info, "TestData Job: " + users.job.ToString());

            //string jsonArray = "{\"name\":\"Mike\",\"job\":\"Teacher\",\"dep\":\"IT\"}";
            var demo = new Demo<CreateUserDTO>();
            CreateUserDTO user = await demo.CreateUser(endpoint, users);
            
            //Assert.AreEqual("Mike", user.name);
            //Reporter.LogToReport(Status.Pass, "Username is matching: " + user.name.ToString());
            //Assert.AreEqual("Lead", user.job);
            //Reporter.LogToReport(Status.Pass, "Job is matching: " + user.job.ToString());

            try
            {
                //Assert.AreEqual("Mike", user.name);
                Assert.AreEqual(users.name, user.name);
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, "User name doesn't match");
                Assert.Fail(ex.Message);
            }

            try
            {
                //Assert.AreEqual("Lead", user.job);
                Assert.AreEqual(users.job, user.job);
            }
            catch (Exception ex)
            {
                Reporter.LogToReport(Status.Fail, "Job doesn't match");
                Assert.Fail(ex.Message);
            }

            string endpointVerify = "api/users?page=2";
            var demoVerify = new Demo<ListOfUsersDTO>();
            var userVerify = await demo.GetUsers(endpointVerify);
            
            Assert.AreEqual(2, userVerify.Page);
            Assert.AreEqual("Michael", userVerify.Data[0].first_name);
        }
    }
}
