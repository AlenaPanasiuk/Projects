using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void SetCustomeValidData()
        {
            SelectVolumes("all");
        }

        public void SelectVolumes(string volumes)
        {
           // this.SetValidData();
            int count;
            count =  driver.FindElement(By.Id("gview_wizardVolumesGrid")).FindElement(By.Id("wizardVolumesGrid")).FindElements(By.ClassName("checkbox")).Count();
 
        }

    }
}
