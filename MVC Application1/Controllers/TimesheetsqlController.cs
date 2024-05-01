using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MVC_Application1.Models;
using MVC_Application1.Infrastructure;

namespace MVC_Application1.Controllers
{
public class TimesheetsqlController : Controller
{
    public static List<TimeSheet> formData = new List<TimeSheet>();
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Show()
    {
    SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString());

    formData.Clear();

    string sqlquery = "SELECT * FROM [dbo].[TimeSheet]";
    SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
    con.Open();

    SqlDataReader reader = sqlcomm.ExecuteReader();
    while (reader.Read())
    {
        TimeSheet timeSheet = new TimeSheet
        {
            TimeSheetID = Convert.ToInt32(reader["TimeSheetID"]),
            employeeID = Convert.ToInt32(reader["employeeID"]),
            Date = Convert.ToDateTime(reader["Date"]),
            projectID = Convert.ToInt32(reader["projectID"]),
            taskID = Convert.ToInt32(reader["taskID"]),
            Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString(),
            WorkStatus = reader["WorkStatus"] == DBNull.Value ? TimeSheet.Status.Unknown : (TimeSheet.Status)Enum.Parse(typeof(TimeSheet.Status), reader["WorkStatus"].ToString()),
            StartTime = DateTime.Parse(reader["StartTime"].ToString()), // Assuming StartTime is of type TimeSpan in your TimeSheet class
            EndTime = DateTime.Parse(reader["EndTime"].ToString())

        };
        if (reader["HoursOfWork"] != DBNull.Value && TimeSpan.TryParse(reader["HoursOfWork"].ToString(), out TimeSpan hoursOfWork))
        {
            timeSheet.HoursOfWork = hoursOfWork;
        }
        else
        {
            timeSheet.HoursOfWork = TimeSpan.Zero;
        }

        formData.Add(timeSheet);
    }

    con.Close();
    return View(formData);

    }

    [HttpPost]
    public IActionResult Index(TimeSheet timeSheet)
    {
        TimeSpan hoursOfWork = timeSheet.EndTime - timeSheet.StartTime;
        timeSheet.HoursOfWork = hoursOfWork;


        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString());

        string sqlquery = "insert into [dbo].[TimeSheet] (employeeID,Date,projectID,taskID,Description,WorkStatus,HoursOfWork,StartTime,EndTime) values(@employeeID,@Date,@projectID,@taskID,@Description,@WorkStatus,@HoursOfWork,@StartTime,@EndTime) ";
        SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
        con.Open();

        sqlcomm.Parameters.AddWithValue("@employeeID", timeSheet.employeeID);
        sqlcomm.Parameters.AddWithValue("@Date", timeSheet.Date);
        sqlcomm.Parameters.AddWithValue("@projectID", timeSheet.projectID);
        sqlcomm.Parameters.AddWithValue("@taskID", timeSheet.taskID);
        sqlcomm.Parameters.AddWithValue("@Description", timeSheet.Description);
        sqlcomm.Parameters.AddWithValue("@WorkStatus", timeSheet.WorkStatus.ToString());
        sqlcomm.Parameters.AddWithValue("@HoursOfWork", timeSheet.HoursOfWork);
        sqlcomm.Parameters.AddWithValue("@StartTime", timeSheet.StartTime);
        sqlcomm.Parameters.AddWithValue("@EndTime", timeSheet.EndTime);

        sqlcomm.ExecuteNonQuery();
        con.Close();

        return RedirectToAction("Show");
    }

    public IActionResult Update(int id)
    {
            TimeSheet timeSheet = new TimeSheet();
            DataTable dtable = new DataTable();

            SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString());
            con.Open();
            string updateQuery = "SELECT * from [dbo].[TimeSheet] TimeSheet WHERE TimeSheetID = @timesheetID";
            SqlDataAdapter command = new SqlDataAdapter(updateQuery, con);
            command.SelectCommand.Parameters.AddWithValue("@timesheetID", id);
            command.Fill(dtable);


                if (dtable.Rows.Count == 1)
                {
                    timeSheet.TimeSheetID = Convert.ToInt32(dtable.Rows[0][0].ToString());
                    timeSheet.employeeID = Convert.ToInt32(dtable.Rows[0][1].ToString());
                    timeSheet.Date = Convert.ToDateTime(dtable.Rows[0][2].ToString());
                    timeSheet.projectID = Convert.ToInt32(dtable.Rows[0][3].ToString());
                    timeSheet.taskID = Convert.ToInt32(dtable.Rows[0][4].ToString());
                    timeSheet.Description = dtable.Rows[0][5].ToString();
                    //string workStatusStr = dtable.Rows[0][6].ToString();
                    timeSheet.StartTime = DateTime.Parse(dtable.Rows[0][7].ToString());
                    timeSheet.EndTime = DateTime.Parse(dtable.Rows[0][8].ToString());
                };
            
            return View(timeSheet);
    }

    [HttpPost]
    public IActionResult Update(int id,TimeSheet timeSheet)
    {
        TimeSpan hoursOfWork = timeSheet.EndTime - timeSheet.StartTime;
        timeSheet.HoursOfWork = hoursOfWork;


        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString());
        con.Open();
        string query = "UPDATE [dbo].[TimeSheet] SET  employeeID = @employeeID, Date = @Date, projectID = @projectID, taskID = @taskID, Description = @Description, WorkStatus = @WorkStatus, StartTime = @StartTime, EndTime = @EndTime WHERE TimeSheetID ="+id;

        SqlCommand sqlCommand = new SqlCommand(query, con);
           
        sqlCommand.Parameters.AddWithValue("@employeeID", timeSheet.employeeID);
        sqlCommand.Parameters.AddWithValue("@Date", timeSheet.Date);
        sqlCommand.Parameters.AddWithValue("@projectID", timeSheet.projectID);
        sqlCommand.Parameters.AddWithValue("@taskID", timeSheet.taskID);
        sqlCommand.Parameters.AddWithValue("@Description", timeSheet.Description);
        sqlCommand.Parameters.AddWithValue("@WorkStatus", timeSheet.WorkStatus.ToString());
        sqlCommand.Parameters.AddWithValue("@HoursOfWork", timeSheet.HoursOfWork);
        sqlCommand.Parameters.AddWithValue("@StartTime", timeSheet.StartTime);
        sqlCommand.Parameters.AddWithValue("@EndTime", timeSheet.EndTime);
        sqlCommand.Parameters.AddWithValue("@TimeSheetID", id);

        sqlCommand.ExecuteNonQuery();
        con.Close();


        return RedirectToAction("Show");
    }

    public IActionResult Delete(int id)
    {
        SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString());

        string sqlquery = "DELETE FROM [dbo].[TimeSheet] WHERE employeeID = @EmployeeID";
        con.Open();

        SqlCommand sqlcomm = new SqlCommand(sqlquery, con);
        sqlcomm.Parameters.AddWithValue("@EmployeeID", id);

        var timeSheet = formData.Where(s => s.employeeID == id).FirstOrDefault();
        formData.Remove(timeSheet);
        sqlcomm.ExecuteNonQuery();
        con.Close();

        return RedirectToAction("Show",formData);
    }

    private struct WorkStatus
    {
        public static implicit operator TimeSheet.Status(WorkStatus v)
        {
            throw new NotImplementedException();
        }
    }

    //Implementing Dependency Injection

    ITimeSheet timeSheet = new TimeSheetRepo();

    public IActionResult AddData(TimeSheet timesheet)
    {
        timeSheet.Add(timesheet);
        return RedirectToAction("Show");
    }

        
    public IActionResult GetData(int id)
    {
        return View(timeSheet.GetUpdate(id));
    }
       
    [HttpPost]
    public IActionResult UpdateData(int id, TimeSheet timesheet)
    {
        timeSheet.Update(id, timesheet);
        return RedirectToAction("Show");
    }

    public IActionResult DeleteData(int id)
    {                                                                                                                                                                                                                                                             
        timeSheet.Delete(id);
        return RedirectToAction("Show");
    }

}

}