using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Configuration;

namespace MVC_Application1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["dbconn"].ToString()))
            {
                con.Open();
                Console.WriteLine(con.State);


                con.Close();
                Console.WriteLine(con.State);
            };
           
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
