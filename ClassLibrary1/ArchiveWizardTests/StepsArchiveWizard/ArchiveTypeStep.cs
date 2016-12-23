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
    class ArchiveTypeStep : StepBase
    {
        public ArchiveTypeStep(IWebDriver driver)
            : base (driver)
        {
        }

        public override StepBase GetNext()
        {
            return new LocationStep(driver);
        }

        public void SetArchiveType(ArchiveType archiveType)
        {
            VerifyLoading();
            if (archiveType != ArchiveType.OneTime)
            {
                driver.FindElement(By.Id("scheduledArchive")).Click();
            }
        }

        public override void SetValidData()
        {
            SetArchiveType(ArchiveType.OneTime);
        }

        public override StepBase GoToThisStep(ArchiveType type)
        {
            throw new NotImplementedException();
        }

    }

    public enum ArchiveType
    {
        OneTime,
        Continuous
    }


}
