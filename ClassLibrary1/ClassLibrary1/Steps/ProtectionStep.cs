using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.Steps;

namespace ProtectWizardTests.Steps
{
    public class ProtectionStep : StepBase
    {
        private const string Agent1IP = "10.35.176.166";

        public ProtectionStep(IWebDriver driver)
            : base(driver)
        {
        }

        public void SetProtectionValues(string name, ProtectionSchedule schedule, bool changeRepo)
        {
            driver.FindElement(By.Id("displayName")).Clear();
            driver.FindElement(By.Id("displayName")).SendKeys(name);

            if (schedule == ProtectionSchedule.Default) 
            { 
                driver.FindElement(By.Id("defaultProtection")).Click(); 
            }
            else 
            {
                driver.FindElement(By.Id("customProtection")).Click();
            }

            if (changeRepo)
            {
                var checkbox = driver.FindElement(By.Id("deleteExistingRP"));

                if (checkbox != null && checkbox.Displayed)
                {
                    checkbox.Click();
                }
            }          
        }

        public override void SetValidData()
        {
            SetProtectionValues(Agent1IP, ProtectionSchedule.Default, false);
        }

        public ProtectionStep GoToProtectionStep(ProtectionType type)
        {
          string stepID = "";
            WelcomeStep welcome = new WelcomeStep(driver);
            welcome.SetProtectionType(type);
            welcome.GoNext();
            ConnectionStep connection = new ConnectionStep(driver);
            connection.SetValidData();
            connection.GoNext();
            stepID = connection.GetStepId();
            if (stepID == "upgrade")
                connection.GoNext();
            return new ProtectionStep(driver);

        }

        public override void SetCustomeValidData()
        {
            SetProtectionValues(Agent1IP, ProtectionSchedule.Custom, false);
        }
        public override StepBase GetNext()
        {
            return new VolumesStep(driver);
        }
    }

    public enum ProtectionSchedule
    {
        Default,
        Custom
    }
}
