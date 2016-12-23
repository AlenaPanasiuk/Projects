using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArchiveWizardTests.StepsArchiveWizard
{
    public abstract class StepBase
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public StepBase(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(180));
        }

        public string GetName()
        {
            string stepName = string.Empty;
            try 
            {
                VerifyLoading();
                driver.FindElement(By.Id("wizardContentContainer"));
                VerifyLoading();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("wizardContentContainer")));
                stepName = driver
                .FindElement(By.Id("wizardContentContainer"))
                .FindElement(By.TagName("h3"))
                .Text;
            }
            catch 
            {
                stepName = string.Empty;
            }
            return stepName;
        }

        public StepBase GetCurrentStep()
        {
            StepBase currentStep = new ArchiveTypeStep(driver);
            var stepId =  currentStep.GetStepId();
            if (stepId == "selectArchiveType")
            {
                currentStep = new ArchiveTypeStep(driver);
            }
            else if (stepId == "location")
            {
                currentStep = new LocationStep(driver);
            }
            else if (stepId == "machines")
            {
                currentStep = new MachinesStep(driver);
            }
            else if (stepId == "options")
            {
                currentStep = new OptionsStep(driver);
            }
            else if (stepId == "daterange")
            {
                currentStep = new DateRangeStep(driver);
            }
            else if (stepId == "schedule")
            {
                currentStep = new ScheduleStep(driver);
            }
            else if (stepId == "scheduleOptions")
            {
                currentStep = new ScheduleOptionsStep(driver);
            }
            return currentStep;
        }

        public string GetStepId()
        {
            string stepId = string.Empty;
            try
            {
                VerifyLoading();
                driver.FindElement(By.Id("wizardContentContainer"));
                VerifyLoading();
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

        public void sendText(IWebElement field, string text)
        {
            field.Clear();
            for (int i = 0; i < 5; i++)
            {
               VerifyLoading();
               field.SendKeys(text);
               VerifyLoading();
               if (field.GetAttribute("value").ToString() != string.Empty)
               {
                   Console.WriteLine("The field {0} is successfully filed", field.GetAttribute("id"));
                   break;
               }
               else
               {
                   Console.WriteLine("The field {0} is empty", field.GetAttribute("id"));
               }
            }
        }

        public StepBase GoNext()
        {
            if (driver.FindElement((By.Id("wizardContentContainer"))) != null)
            {
                VerifyLoading();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnWizardDefault")));
                VerifyLoading();
                driver.FindElement(By.Id("btnWizardDefault")).Click();
                VerifyLoading();
            }
            return GetNext();
        }

        public abstract StepBase GetNext();

        public abstract StepBase GoToThisStep(ArchiveType type);

        public StepBase GoToFinish()
        {
            string currentStepId = string.Empty;
            string nextStepId = string.Empty;
            StepBase step = GetCurrentStep();
            try
            {
                while ((driver.FindElement(By.Id("wizardContentContainer")).Displayed))
                {
                    VerifyLoading();
                    currentStepId = GetStepId();
                    VerifyLoading();
                    step.SetValidData();
                    VerifyLoading();
                    step = step.GoNext();
                    VerifyLoading();
                    nextStepId = GetStepId();
                    VerifyLoading();
                    if (currentStepId == nextStepId)
                    {
                        Console.WriteLine("Error!!! The next step cannot be loaded");
                        break;
                    }
                }
            }
            catch
            {

            }
            return step;
        }

        public void VerifyLoading()
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