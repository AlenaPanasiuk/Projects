using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtectWizardTests.StepsProtectWizard;

namespace ProtectWizardTests.StepsProtectWizard
{
    class ScheduleStep : StepBase
    {
        public ScheduleStep(IWebDriver driver)
            : base(driver)
        {
        }

        public override StepBase GetNext()
        {
            return new RepositoryStep(driver);
        }

        public override StepBase GoToThisStep(ProtectionType type)
        {
            VolumesStep volumesstep = new VolumesStep(driver);
            volumesstep.GoToThisStep(type);
            volumesstep.SetValidData();
            volumesstep.GoNext();
            return new ScheduleStep(driver);
        }

        public override void SetValidData()
        {
            SetSchedule(Schedule.Periods, true, true, false, "12:00 AM", "11:59 PM", "60", "60", "60", false, "60");
        }


        /// <summary>
        /// Sets the protection schedule with Daily ptotection only
        /// </summary>
        public void SetSchedule(string protectionTimeDaily, bool pause)
        {
            SetSchedule(Schedule.Daily, false, false, false, "12:00 AM", "11:59 PM", "60", "60", "60", pause, protectionTimeDaily);
        }

        /// <summary>
        /// Sets the protection schedule with Periods protection only
        /// </summary>
        public void SetSchedule(bool weekdays, bool weekends, bool protectWeekdaysRest, 
            string from, string to, string weekdaysPeriopd, string weekendsPeriod, string weekdaysRestPeriod, bool pause)
        {
            SetSchedule(Schedule.Periods, weekdays, weekends, protectWeekdaysRest, from, to, 
                weekdaysPeriopd, weekendsPeriod, weekdaysRestPeriod, false, string.Empty);
        }


        /// <summary>
        /// Sets the protection schedule 
        /// </summary>
        private void SetSchedule(Schedule scheduleType, bool weekdays, bool weekends, bool protectWeekdaysRest, 
            string from, string to, string weekdaysPeriopd, string weekendsPeriod, string weekdaysRestPeriod, bool pause, string protectionTimeDaily)
        {
            
            driver.FindElement(By.Id("periods")).Click();
            if (pause)
            {
                driver.FindElement(By.Id("initialPauseProtection")).Click();
            }
            if (scheduleType == Schedule.Periods)
            {
                if (weekdays)
                {
                    sendText(driver.FindElement(By.Id("weekdaysFrom")).FindElement(By.TagName("input")), from);
                    sendText(driver.FindElement(By.Id("weekdaysTo")).FindElement(By.TagName("input")), to);
                    sendText(driver.FindElement(By.Id("weekdaysPeriod")), weekdaysPeriopd);
                    if (protectWeekdaysRest)
                    {
                        driver.FindElement(By.Id("protectWeekdaysRest")).Click();
                        sendText(driver.FindElement(By.Id("weekdaysRestPeriod")), weekdaysRestPeriod);
                    }
                }
                else
                {
                    driver.FindElement(By.Id("protectWeekdays")).Click();
                }
                if (weekends)
                {
                    sendText(driver.FindElement(By.Id("weekendsPeriod")), weekendsPeriod);
                }
                else
                {
                    driver.FindElement(By.Id("protectWeekends")).Click();
                }
            }
            else if (scheduleType == Schedule.Daily)
            {
                driver.FindElement(By.Id("protectionTime")).Click();
                sendText(driver.FindElement(By.Id("dailyProtection")).FindElement(By.TagName("input")), protectionTimeDaily);
            }
        }
    }

    public enum Schedule
    {
        Periods,
        Daily
    }
}
