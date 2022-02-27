using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIDemo
{
    public static class Reporter
    {
        public static ExtentReports extentReports;
        public static ExtentReports extent;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest testcase;

        public static void SetupExtentReporter(string reportName, string documentTitle, string path)
        {
            htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.DocumentTitle = documentTitle;
            htmlReporter.Config.ReportName = reportName;

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extentReports = extent;
        }

        public static void CreateTest(string testName) 
        { 
            testcase = extentReports.CreateTest(testName);
        }

        public static void LogToReport(Status status, string message)
        {
            testcase.Log(status, message);
        }

        public static void FlushReport()
        {
            extentReports.Flush();
        }

        public static void TestStatus(string status)
        {
            if (status.Equals("Pass"))
            {
                testcase.Pass("Test is passed");
            }
            else
            {
                testcase.Fail("Test is failed");
            }
        }

    }
}
