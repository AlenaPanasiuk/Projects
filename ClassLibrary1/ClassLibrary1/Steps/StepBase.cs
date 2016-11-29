using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProtectWizardTests.Steps
{
    public abstract class StepBase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public StepBase(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(90));
        }

        public string GetName()
        {
            string stepName = "";
            try 
            {
                driver.FindElement(By.Id("wizardContentContainer"));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
                stepName = driver
                .FindElement(By.Id("wizardContentContainer"))
                .FindElement(By.TagName("h3"))
                .Text;
            }
            catch 
            {
                stepName = "";
            }
            return stepName;
        }

        public string GetStepId()
        {
            string stepId = "";
            try
            {
                driver.FindElement(By.Id("wizardContentContainer"));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
                stepId = driver
                .FindElement(By.Id("wizardLegendContainer"))
                .FindElement(By.ClassName("active-step"))
                .FindElement(By.TagName("a"))
                .GetAttribute("id").ToString(); 
            }
            catch
            {
            }
            return stepId;
        }

        public StepBase GoNext()
        {
            if (driver.FindElement((By.Id("wizardContentContainer"))) != null)
            {
                VerifyLoading();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
                //  Thread.Sleep(10000);
                VerifyLoading();
                driver.FindElement(By.Id("btnWizardDefault")).Click();
                VerifyLoading();
            }
           // Thread.Sleep(10000);

            return GetNext();
        }

        public abstract StepBase GetNext();

        private void VerifyLoading()
        {
            for (int i = 0; i < 10; i++)
            {
                if (driver.FindElement(By.Id("lpLoading")).Displayed && driver.FindElement(By.Id("lpLoading")).Enabled)
                {
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("lpLoading")));
                }
            }
        }

        public virtual void SetValidData()
        { }


        public virtual void SetCustomeValidData()
        { }
    }
}