using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MVC_Application1.Models;
using static MVC_Application1.Models.TimeSheet;

namespace MVC_Application1.Controllers
{
    public class TimeController : Controller
    {
        public static List<TimeSheet> formData = new List<TimeSheet>();

        public Status NewStatus { get; private set; }

        public IActionResult Index()
        {
            return View(formData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(TimeSheet timeSheet)
        {
            var employeeID = Request.Form["employeeID"];
            var Date = Request.Form["Date"];
            var StartTime = Request.Form["StartTime"];
            var EndTime = Request.Form["EndTime"];
            var projectID = Request.Form["projectID"];
            var taskID = Request.Form["taskID"];
            var Description = Request.Form["Description"];
            var WorkStatus = Request.Form["WorkStatus"];

            Status parsedStatus;
            if (Enum.TryParse(WorkStatus, out parsedStatus))
            {
                NewStatus = parsedStatus;
            }

           

            var newTimeSheet = new TimeSheet
            {
                employeeID = Convert.ToInt32(employeeID),
                Date = DateTime.Parse(Date),
                StartTime = DateTime.Parse(StartTime),
                EndTime = DateTime.Parse(EndTime),
                projectID = Convert.ToInt32(projectID),
                taskID = Convert.ToInt32(taskID),
                Description = Description,
                WorkStatus = parsedStatus
            };

            TimeSheet item = formData.Find(p => p.employeeID == newTimeSheet.employeeID);
            int index = formData.IndexOf(item);
            if (index >= 0)
            {
                formData[index] = newTimeSheet;
            }
            else
            {
                formData.Add(newTimeSheet);
            }

            return Ok();
        }
       
        public IActionResult Delete(int id)
        {
          
            var itemToRemove = formData.FirstOrDefault(item => item.employeeID == id);
            if (itemToRemove != null)
            {
                formData.Remove(itemToRemove);
                return Ok("Item deleted successfully.");
            }
            else
            {
                return NotFound("Item not found.");
            }
        }

        public IActionResult Edit(int id)
        {
            var timeSheet = formData.FirstOrDefault(m => m.employeeID == id);
            if (timeSheet == null)
            {
                return NotFound(); 
            }
            return View(timeSheet); 
        }
    }
}