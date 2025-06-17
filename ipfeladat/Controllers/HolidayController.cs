// HolidayController.cs

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using ipfeladat.Controllers;
using NLog;
using static System.Runtime.InteropServices.JavaScript.JSType;


[Route("api/")]
[ApiController]
public class HolidayController : ControllerBase
{
    private List<Holiday> munkaszunetiNapok;
    private string jsonData;
    private readonly string jsonFilePath;
    private readonly IWebHostEnvironment _hostingEnvironment;
    //private  Logger log = LogManager.GetCurrentClassLogger();
    private readonly ILogger<HolidayController> _log;

    private readonly Helper helper = new Helper();
    public HolidayController(IWebHostEnvironment hostingEnvironment, ILogger<HolidayController> log)
    {
        _log = log;
        _log.LogDebug("holidays.json fájl beolvasása");
        try
        {
            _hostingEnvironment = hostingEnvironment;
            var rootPath = _hostingEnvironment.ContentRootPath;
            jsonFilePath = Path.Combine(rootPath, "data/holidays.json");
            string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            munkaszunetiNapok = JsonConvert.DeserializeObject<List<Holiday>>(jsonContent);
            _log.LogDebug("Sikeres beolvasás");
        }
        catch (FileNotFoundException e)
        {
            _log.LogDebug("A fájl nem található");
            throw;
        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());
            throw;
        }

    }
    [HttpGet("checkdate")]
    public IActionResult GetHoliday(DateTime date)
    {
        _log.LogDebug("checkdate start. beolvasott dátum:" + date);

        try
        {
            if (helper.ProperDate(date))
            {
                var jsoncontent = new
                {
                    Date = date,
                    Result = !helper.HolidayOrNot(date, munkaszunetiNapok) ? "Munkanap" : "Munkaszüneti nap"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);

            }
            else
            {
                var jsoncontent = new
                {
                    Date = date,
                    Result = "Hibás input"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            _log.LogDebug("Visszaadott json tartalma:" + jsonData.ToString());
            JObject jsonObject = JObject.Parse(jsonData);
            return Content(jsonData, "application/json");
        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());
            throw;
        }
    }
    [HttpGet("countworkdays")]
    public IActionResult GetWorkDaysByYear(DateTime date1, DateTime date2)
    {
        _log.LogDebug("countworkdays start. Kapott dátumok: 1:" + date1 + "2:" + date2);
        try
        {

            if (!helper.ProperDate(date1) && !helper.ProperDate(date2))
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result = "Hibás input"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            else
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result = helper.WorkdaysByInterval(date1, date2, munkaszunetiNapok).ToString()
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            _log.LogDebug("Visszaadott json tartalma:" + jsonData.ToString());
            JObject jsonObject = JObject.Parse(jsonData);
            return Content(jsonData, "application/json");

        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());
            throw;
        }
    }
    [HttpGet("countholidays")]
    public IActionResult GetHolidaysByYear(DateTime date1, DateTime date2)
    {
        _log.LogDebug("countholidays start. Kapott dátumok: 1:" + date1 + "2:" + date2);
        try
        {
            if (!helper.ProperDate(date1) && !helper.ProperDate(date2))
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result = "Hibás input"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            else
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result = helper.HolidaysByInterval(date1, date2, munkaszunetiNapok).ToString()
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            _log.LogDebug("checkholiday visszaadott json tartalma:" + jsonData.ToString());
            JObject jsonObject = JObject.Parse(jsonData);
            return Content(jsonData, "application/json");
        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());
            throw;
        }
    }

    [HttpGet("changedate")]
    public IActionResult ChangeDayType(DateTime date)
    {

        _log.LogDebug("changedate start. Kapott dátum:" + date);
        try
        {
            if (!helper.ProperDate(date))
            {
                var jsoncontent = new
                {
                    ChangeDate = date,
                    Result = "Hibás input"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            else
            {
                List<Holiday> new_munkaszunetiNapok = new List<Holiday>(helper.ChangeDaysType(date, munkaszunetiNapok));
                var json = JsonConvert.SerializeObject(new_munkaszunetiNapok, Formatting.Indented);
                munkaszunetiNapok = new_munkaszunetiNapok;
                System.IO.File.WriteAllText(jsonFilePath, json);
                var jsoncontent = new
                {
                    ChangeDate = date,
                    Result = "Megváltoztatva"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            _log.LogDebug("changedate visszaadott json tartalma:" + jsonData.ToString());
            JObject jsonObject = JObject.Parse(jsonData);
            return Content(jsonData, "application/json");
        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());

            throw;
        }
    }
    [HttpGet("countdays")]
    public IActionResult CountAllDays(DateTime date1, DateTime date2)
    {
        _log.LogDebug("countdays start. Kapott dátumok: 1:" + date1 + "2:" + date2);
        try
        {
            if (!helper.ProperDate(date1) && !helper.ProperDate(date2))
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result = "Hibás input"
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            else
            {
                var jsoncontent = new
                {
                    StartDate = date1,
                    LastDate = date2,
                    Result1 = helper.WorkdaysByInterval(date1, date2, munkaszunetiNapok),
                    Result2 = helper.HolidaysByInterval(date1, date2, munkaszunetiNapok)
                };
                jsonData = JsonConvert.SerializeObject(jsoncontent);
            }
            _log.LogDebug("countdays visszaadott json tartalma:" + jsonData.ToString());
            JObject jsonObject = JObject.Parse(jsonData);
            return Content(jsonData, "application/json");
        }
        catch (Exception e)
        {
            _log.LogDebug(e.ToString());
            throw;
        }
    }
}

