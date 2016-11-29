using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtectWizardTests.Steps
{
    public class WelcomeStep : StepBase
    {
        public WelcomeStep(IWebDriver driver)
            : base (driver)
        {
        }

        public override StepBase GetNext()
        {
            return new ConnectionStep(driver);
        }


        public void SetProtectionType(ProtectionType protectionType)
        {
            if (protectionType != ProtectionType.Typical)
            {
                driver.FindElement(By.Id("advancedWizard")).Click();
               
            }
        }

    }
    public enum ProtectionType
    {
        Typical,
        Advanced
    }
}
