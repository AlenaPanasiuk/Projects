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
        public ProtectionStep(IWebDriver driver)
            : base(driver)
        {
        }

        public void SetProtectionValues(string name, ProtectionSchedule schedule, bool changeRepo)
        {
            IWebElement keepcurrentcheckbox = null;
            try
            {
                keepcurrentcheckbox = driver.FindElement(By.Id("restoreProtection"));
            }
            catch
            { }

            if (driver.FindElement(By.Id("displayName")).Enabled)
            {
                driver.FindElement(By.Id("displayName")).Clear();
                driver.FindElement(By.Id("displayName")).SendKeys(name);
            }

            if (schedule == ProtectionSchedule.Default) 
            { 
                driver.FindElement(By.Id("defaultProtection")).Click(); 
            }
            else if (schedule == ProtectionSchedule.Custom)
            {
                driver.FindElement(By.Id("customProtection")).Click();
            }
            else if (schedule == ProtectionSchedule.KeepCurrent && keepcurrentcheckbox != null)
            {
                driver.FindElement(By.Id("restoreProtection")).Click();
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
            SetProtectionValues(TestBase.Agent1IP, ProtectionSchedule.Default, false);
        }

        public ProtectionStep GoToProtectionStep(ProtectionType type)
        {
            var welcome = new WelcomeStep(driver);
            welcome.SetProtectionType(type);
            
            var connection = (ConnectionStep)welcome.GoNext();
            connection.SetValidData();
            
            var next = connection.GoNext();
            if (next.GetStepId() == "upgrade")
            { 
                next = next.GoNext(); 
            }

            return (ProtectionStep)next;

        }

        public override void SetCustomeValidData()
        {
            SetProtectionValues(TestBase.Agent1IP, ProtectionSchedule.Custom, false);
        }
        public override StepBase GetNext()
        {
            return new VolumesStep(driver);
        }
    }

    public enum ProtectionSchedule
    {
        Default,
        Custom,
        KeepCurrent
    }
}
