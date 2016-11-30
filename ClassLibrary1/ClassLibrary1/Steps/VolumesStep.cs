﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.Steps;

namespace ProtectWizardTests.Steps
{
    class VolumesStep : StepBase
    {
        public VolumesStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new VolumesStep(driver);
        }

        public override void SetValidData()
        {
            SelectVolumes(Volumes.All);
        }

        public VolumesStep GoToVolumesStep(ProtectionType type)
        {
            ProtectionStep protection = new ProtectionStep(driver);
            protection.GoToProtectionStep(type);

            protection.SetProtectionValues("Agent1IP", ProtectionSchedule.Custom, false);
            protection.GoNext();
            return new VolumesStep(driver);
        }

        public void SelectVolumes(Volumes volumes)
        {
          int count;
          count = driver.FindElement(By.Id("gview_wizardVolumesGrid")).FindElement(By.Id("wizardVolumesGrid")).FindElements(By.ClassName("checkbox")).Count();
          IList<IWebElement> listvolumes = driver.FindElement(By.Id("gview_wizardVolumesGrid")).FindElement(By.Id("wizardVolumesGrid")).FindElements(By.ClassName("checkbox"));
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

    }
    public enum Volumes
    {
        All,
        Fisrt,
        None
    }
}
