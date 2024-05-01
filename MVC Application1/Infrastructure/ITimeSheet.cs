using MVC_Application1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Application1.Infrastructure
{
   public interface ITimeSheet
    {
        List<TimeSheet> GetData();

        void Add(TimeSheet entity);
        List<TimeSheet>Update (int id,TimeSheet timeSheet);
        TimeSheet GetUpdate(int id);
        void Delete(int id);

    }
}
