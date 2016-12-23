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
    class DateRangeStep : StepBase
    {
        public DateRangeStep(IWebDriver driver)
            : base (driver)
        {
        }

        public override StepBase GetNext()
        {
            throw new NotImplementedException();
        }

        public override StepBase GoToThisStep(ArchiveType type)
        {
            throw new NotImplementedException();
        }
   
    }
}
