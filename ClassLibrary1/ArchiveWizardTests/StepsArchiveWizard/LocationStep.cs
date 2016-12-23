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

namespace ArchiveWizardTests.StepsArchiveWizard
{
    class LocationStep : StepBase
    {
        public LocationStep(IWebDriver driver)
            : base (driver)
        {
        }


        public override StepBase GetNext()
        {
            return new MachinesStep(driver);
        }

        public override StepBase GoToThisStep(ArchiveType type)
        {
            ArchiveTypeStep archiveType = new ArchiveTypeStep(driver);
            archiveType.SetArchiveType(type);
            var location = (LocationStep)archiveType.GoNext();
            return location;
        }

        public void setLocation(LocationType locationType, string locationLocal, string locationNetwork, string networkUserName, string networkPassword)
        {
            driver.FindElement(By.Id("dropdown-wrapper-locationType")).Click();
            IList<IWebElement> listLocations = driver.FindElement(By.Id("dropdown-menu-locationType")).FindElements(By.TagName("li"));
            if (locationType == LocationType.Local)
            {
                listLocations[0].Click();
                sendText(driver.FindElement(By.Id("localPath")), locationLocal); 
            }
            else if (locationType == LocationType.Network)
            {
                listLocations[1].Click();
                sendText(driver.FindElement(By.Id("networkPath")), locationNetwork);
                sendText(driver.FindElement(By.Id("networkUsername")), networkUserName);
                sendText(driver.FindElement(By.Id("networkPassword")), networkPassword);
            }
            else if (locationType == LocationType.Cloud)
            {
                listLocations[2].Click();
            }
 
        }
    }

    public enum LocationType
    {
        Local,
        Network,
        Cloud
    }
}
