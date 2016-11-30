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
using OpenQA.Selenium.Remote;

namespace ProtectWizardTests
{
    public class WebDriveFactory
    {
        public IWebDriver Driver;

        public WebDriveFactory(BrowserType type)
        {
            Driver = WebDriver(type);
        }

        [OneTimeTearDown]
        public void TestFixtureTearnDown()
        {
            Driver.Quit();
        }

        /// <summary>
        /// Types of browser available for proxy examples.
        /// </summary>
        public enum BrowserType
        {
            IE,
            Chrome,
            Firefox
        }

        public static IWebDriver WebDriver(BrowserType type)
        {
            IWebDriver driver = null;

            switch (type)
            {
                case BrowserType.IE:
                    driver = IeDriver();
                    break;
                case BrowserType.Firefox:
                    driver = FirefoxDriver();
                    break;
                default:
                    driver = ChromeDriver();
                    break;
            }

            return driver;
        }

        /// <summary>
        /// Creates Internet Explorer Driver instance.
        /// </summary>
        /// <returns>A new instance of IEDriverServer</returns>
        private static IWebDriver IeDriver()
        {


            InternetExplorerOptions options = new InternetExplorerOptions();
            options.EnsureCleanSession = true;
            IWebDriver driver = new InternetExplorerDriver(options);
            return driver;
        }

        /// <summary>
        /// Creates Firefox Driver instance.
        /// </summary>
        /// <returns>A new instance of Firefox Driver</returns>
        private static IWebDriver FirefoxDriver()
        {
            FirefoxProfile profile = new FirefoxProfile();
            IWebDriver driver = new FirefoxDriver(profile);
            return driver;
        }


        /// <summary>
        /// Creates Chrome Driver instance.
        /// </summary>
        /// <returns>A new instance of Chrome Driver</returns>
        private static IWebDriver ChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(chromeOptions);
            return driver;
        }

    }
}
