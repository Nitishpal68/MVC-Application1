using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_Application1.Models;

namespace MVC_Application1.Controllers
{
    public class TimeSheetController : Controller
    {
        public static List<TimeSheet> formData = new List<TimeSheet>();
        public IActionResult Index()
        {
            return View(formData);
        }

        public IActionResult Create()
        {
            return View();

        }

        public IActionResult Add(TimeSheet timeSheet)
        {
            if (!ModelState.IsValid)
            {
                return View("Create"); 
            }

            if (timeSheet.EndTime <= timeSheet.StartTime)
            {
                ModelState.AddModelError("endtime", "end time must be greater than start Time.");
                return View("Create");
            }

            if (formData.Any(m => m.TimeSheetID == timeSheet.TimeSheetID))
            {
                ModelState.AddModelError("TimeSheetId", "TimeSheet ID must be unique.");
                return View("Create");
            }
            if (formData.Count > 0)
            {

                timeSheet.TimeSheetID = formData.Max(m => m.TimeSheetID) + 1;
            }
            else
            {

                timeSheet.TimeSheetID = 1;
            }

            TimeSpan hoursOfWork = timeSheet.EndTime - timeSheet.StartTime;
            timeSheet.HoursOfWork = hoursOfWork;

            formData.Add(timeSheet);


            return RedirectToAction("Index");
        }
       
        public IActionResult Delete(int id)
        {
            var timeSheet = formData.FirstOrDefault(m => m.TimeSheetID == id);
           
            formData.Remove(timeSheet); 
            return RedirectToAction("Index",timeSheet); 
        }

        public IActionResult Edit(int id)
        {
            var timeSheet = formData.Where(s => s.TimeSheetID == id).FirstOrDefault();
            
            return View(timeSheet);
             
        }

        [HttpPost]
        public IActionResult Edit(int id, TimeSheet timeSheet)
        {
            var existingTimeSheet = formData.FirstOrDefault(m => m.TimeSheetID == id);
            if (existingTimeSheet != null)
            {
               
                existingTimeSheet.employeeID = timeSheet.employeeID;
                existingTimeSheet.Date = timeSheet.Date;
                existingTimeSheet.StartTime = timeSheet.StartTime;
                existingTimeSheet.EndTime = timeSheet.EndTime;
                existingTimeSheet.projectID = timeSheet.projectID;
                existingTimeSheet.taskID = timeSheet.taskID;
                existingTimeSheet.Description = timeSheet.Description;
                existingTimeSheet.WorkStatus = timeSheet.WorkStatus;

                TimeSpan hoursOfWork = existingTimeSheet.EndTime - existingTimeSheet.StartTime;
                existingTimeSheet.HoursOfWork = hoursOfWork;

                return RedirectToAction("Index"); 
            }
            else
            {
                return NotFound();
                return View(timeSheet);
            }
           
        }            
    
    }   
}