using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Application1.Models
{
    public class TimeSheet
    {
        internal StringValues hoursOfWork;

        [Required]
        [Key]
        public int TimeSheetID { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "only Digits allowed")]
        public int employeeID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        [BindProperty]
        public DateTime EndTime { get; set; }

        [ReadOnly(true)]
        public TimeSpan HoursOfWork { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "only Digits allowed")]
        public int projectID { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "only Digits allowed")]
        public int taskID { get; set; }

        
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "only Characters allowed")]
        public string Description { get; set; }

        [Required]
        public Status WorkStatus { get; set; }

        public enum Status
        {
            Pending,
            Completed,
            Unknown
        }
    }
}
