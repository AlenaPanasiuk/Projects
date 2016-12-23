using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
{
    class VolumesStep : StepBase
    {
        public VolumesStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new ScheduleStep(driver);
        }

        public override void SetValidData()
        {
            SelectVolumes(Volumes.All);
        }

        public override StepBase GoToThisStep(ProtectionType type)
        {
            ProtectionStep protection = new ProtectionStep(driver);
            protection.GoToThisStep(type);

            protection.SetProtectionValues(Properties.Settings.Default.agentIP.ToString(), ProtectionSchedule.Custom, false);
            protection.GoNext();
            return new VolumesStep(driver);
        }


        public void SelectVolumes(Volumes volumes)
        {
         // int count;
        //  count = driver.FindElement(By.Id("gview_wizardVolumesGrid")).FindElement(By.Id("wizardVolumesGrid")).FindElements(By.ClassName("checkbox")).Count();
            try
            {
                driver.FindElement(By.Id("gview_wizardVolumesGrid")).FindElement(By.Id("wizardVolumesGrid"));
                IList<IWebElement> listvolumes = driver.FindElement(By.Id("gview_wizardVolumesGrid"))
                    .FindElement(By.Id("wizardVolumesGrid"))
                    .FindElements(By.ClassName("checkbox"));
                if (volumes == Volumes.None)
                {
                    foreach (IWebElement volume in listvolumes)
                    {
                        volume.Click();
                    }
                }

                if (volumes == Volumes.Fisrt)
                {
                    foreach (IWebElement volume in listvolumes)
                    {
                        volume.Click();
                    }
                    listvolumes.First().Click();
                }
            }
            catch 
            { 
            }
        }

    }
    public enum Volumes
    {
        All,
        Fisrt,
        None
    }
}
