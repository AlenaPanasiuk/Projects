using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
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
            SetProtectionValues(Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Default, false);
        }


        public override StepBase GetNext()
        {
            if (GetStepId() == "repository")
            {
                return new RepositoryStep(driver);
            }
            else if (GetStepId() == "volumes")
            {
                return new VolumesStep(driver);
            }
            return this;
        }


        /// <summary>
        /// Goes to Protection step with typical or advanced protection settings
        /// </summary>
        public override StepBase GoToThisStep(ProtectionType type)
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
            else if (next.GetStepId() == "pushInstall")
            {
                next = next.GoNext();
            }
            else if (next.GetStepId() == "warnings")
            {
                next = next.GoNext();
                if (next.GetStepId() == "upgrade")
                {
                    next = next.GoNext();
                }
            }
            return (ProtectionStep)next;
        }


        /// <summary>
        /// Goes to Protection step with typical protection settings
        /// </summary>
        public StepBase GoToThisStep()
        {
            GoToThisStep(ProtectionType.Typical);
            return this;
        }

    }

    public enum ProtectionSchedule
    {
        Default,
        Custom,
        KeepCurrent
    }
}
