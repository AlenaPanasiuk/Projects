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
using ProtectWizardTests.StepsProtectWizard;
using ProtectWizardTests.Helpers;
using Replay.Core.Contracts.Agents;
using Replay.Logging;
using Replay.Core.Client;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Drawing;
using System.ComponentModel;

namespace ProtectWizardTests
{
    [TestFixture(BrowserType.Chrome)]
  //  [TestFixture(BrowserType.Firefox)]
 //   [TestFixture(BrowserType.IE)]
    public class TestBase : WebDriveFactory
    {
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

        protected void UITest(Action action, string testName)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot(); 
                DateTime time = DateTime.Now;
                string dateToday = "_date_" + time.ToString("yyyy-MM-dd") + "_time_" + time.ToString("HH-mm-ss");
                var filePath = @"C:\screenshots\";
                var filename = filePath + testName + dateToday + ".png";
                screenshot.SaveAsFile(filename, ImageFormat.Png);
                Bitmap bitmap = new Bitmap(filename);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
                throw;
            } 
        }

        public static XElement ImageToXElement(System.Drawing.Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
            return new XElement("Image",
                new XAttribute("PixelData",
                    Convert.ToBase64String(
                    (byte[])converter.ConvertTo(image, typeof(byte[])))));
        }

        [SetUp]
        public void OpenWizard()
        {
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 1800));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='protectEntity']")));

            try
            {
                Driver.FindElement(By.XPath(".//*[@id='popup1']/div/div[5]/div/div[5]/button[1]")).Click();
            }
            catch { }

            Driver.FindElement(By.XPath(".//*[@id='protectEntity']")).Click();
            Driver.FindElement(By.XPath(".//*[@id='protectMachine']/div[2]/ul/li[1]/a")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
        }

        [TearDown]
        public void CleanUp()
        {
            APIHelpers helper = new APIHelpers(CoreHost, CorePort);
            helper.TestResults();
            helper.DeleteAgents();
            helper.DeleteRepositories();
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
