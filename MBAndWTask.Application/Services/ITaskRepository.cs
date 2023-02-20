using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MBAndWTask.Model;

namespace MBAndWTask.Application.Services
{
    public interface ITaskRepository
    {
        public string Add(MBAndWTask.Model.Task task);

        public string Update(MBAndWTask.Model.Task task);

        //public bool CheckTasksCountByStatus(DateTime dueDate);

        public int GetHighPriorityTasksByDueDate(DateTime dueDate);

    }
}
