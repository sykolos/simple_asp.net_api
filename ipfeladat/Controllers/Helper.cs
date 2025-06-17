using System.Globalization;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NLog;

namespace ipfeladat.Controllers
{
    public class Helper
    {
        private readonly DateTime now = DateTime.Now;
        public bool HolidayOrNot(DateTime date, List<Holiday> msznapok)
        {
            try
            {
                int year = date.Year;
                var holidaycheck = msznapok.Find(m => m.Ev == year);
                if (date.DayOfWeek == DayOfWeek.Sunday) return true;
                else
                {
                    for (int i = 0; i < holidaycheck.MunkaszunetiNapok.Count; i++)
                    {
                        if (holidaycheck.MunkaszunetiNapok[i] == date)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public bool ProperDate(DateTime date)
        {
            try
            {
                if (date.Year < 2016) return false;
                else
                {
                    if (now.Year < date.Year && Math.Abs(now.Year - date.Year) <= 5) return true;
                    else if (now.Year > date.Year) return true;
                    else return false;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public int WorkdaysByInterval(DateTime date1, DateTime date2, List<Holiday> msznapok)
        {
            try
            {
                int munkanap = 0;
                DateTime start = date1; DateTime finish = date2;
                List<Holiday> mszall = msznapok;
                for (DateTime i = start; i <= finish; i = i.AddDays(1))
                {
                    if (!HolidayOrNot(i, mszall)) munkanap++;
                }
                return munkanap;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        public int HolidaysByInterval(DateTime date1, DateTime date2, List<Holiday> msznapok)
        {
            try
            {
                int msznap = 0;
                DateTime start = date1; DateTime finish = date2;
                List<Holiday> mszall = msznapok;
                for (DateTime i = start; i <= finish; i = i.AddDays(1))
                {
                    if (HolidayOrNot(i, mszall)) msznap++;
                }
                return msznap;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public List<Holiday> ChangeDaysType(DateTime date, List<Holiday> msznapok)
        {
            try
            {
                int year = date.Year;
                List<Holiday> result = new List<Holiday>(msznapok);
                var spec_year = result.Find(m => m.Ev == year);

                if (HolidayOrNot(date, result))
                {
                    for (int i = 0; i < spec_year.MunkaszunetiNapok.Count; i++)
                    {
                        if (spec_year.MunkaszunetiNapok[i] == date)
                        {
                            spec_year.MunkaszunetiNapok.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    spec_year.MunkaszunetiNapok.Add(date);
                }
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
