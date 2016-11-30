using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support;
using NUnit;
using NUnit.Compatibility;
using ProtectWizardTests.Steps;
using ProtectWizardTests.Helpers;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;

namespace ProtectWizardTests
{
    [TestFixture(BrowserType.Chrome)]
    [TestFixture(BrowserType.Firefox)]
    [TestFixture(BrowserType.IE)]
    public class TestBase : WebDriveFactory
    {
        public const string Agent1IP = "10.35.176.166";
        public const string Agent1UserName = "administrator";
        public const string Agent1Password = "123asdQ";
        public const string CoreHost = "10.35.176.167";
        public const string CoreUserName = "administrator";
        public const string CorePassword = "123asdQ";
        public const string Agent1Port = "8006";
        public const int CorePort = 8006;
        private string url = String.Format("https://{0}:{1}@{2}:{3}/apprecovery/admin", CoreUserName, CorePassword, CoreHost, CorePort);

        public TestBase(BrowserType browser)
            : base( browser)
        {
        }


        [SetUp]
        public void OpenWizard()
        {
            Driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 900));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='protectEntity']")));

            Driver.FindElement(By.XPath(".//*[@id='protectEntity']")).Click();
            Driver.FindElement(By.XPath(".//*[@id='protectMachine']/div[2]/ul/li[1]/a")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
        }

        [TearDown]
        public void CleanUp()
        {
            APIHelpers helper = new APIHelpers(CoreHost, CorePort);
            helper.TestResults();
            //helper.DeleteAgents();
            

        }


        public StepBase GoToStep(string targetStepName, SetDataType setDataType)
        {
            StepBase step = new WelcomeStep(Driver);

            while (targetStepName != step.GetName())
            {
                if (setDataType == SetDataType.Default)
                {
                    step.SetValidData();
                    step = step.GoNext();
                }
                else 
                {
                    step.SetCustomeValidData();
                    step = step.GoNext();
                }
            }

            return step;
        }

        public enum SetDataType
        {
            Default,
            Custom
        }

    }
}
