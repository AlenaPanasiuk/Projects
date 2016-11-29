using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.Steps;

namespace ProtectWizardTests.Steps
{
    class ScheduleStep : StepBase
    {
        public ScheduleStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new VolumesStep(driver);
        }

        public ScheduleStep GoToScheduleStep(ProtectionType type)
        {
            VolumesStep volumesstep = new VolumesStep(driver);
            volumesstep.GoToVolumesStep(type);
            volumesstep.SetValidData();
            volumesstep.GoNext();
            return new ScheduleStep(driver);
        }
        public override void SetValidData()
        {
            
        }

        public void SetSchedulePeriods(bool weekdays, bool weekends, bool protectWeekdaysRest, string from, string to, string weekdaysPeriopd, string weekendPeriod)
        {
            if (weekdays)
            {
                driver.FindElement(By.Id("periods")).Click();
                
                driver.FindElement(By.Id("weekdaysFrom")).FindElement(By.TagName("input")).Clear();
                driver.FindElement(By.Id("weekdaysFrom")).FindElement(By.TagName("input")).SendKeys(from);
               
                driver.FindElement(By.Id("weekdaysTo")).FindElement(By.TagName("input")).Clear();
                driver.FindElement(By.Id("weekdaysTo")).FindElement(By.TagName("input")).SendKeys(to);

                driver.FindElement(By.Id("weekdaysRow")).FindElement(By.TagName("input")).Clear();
                driver.FindElement(By.ClassName("weekdaysRow")).FindElement(By.TagName("input")).SendKeys(weekdaysPeriopd);
            }


        }
        public void SetScheduleDaily()
        {
            driver.FindElement(By.Id("protectionTime")).Click();
            

        }


    }
}
