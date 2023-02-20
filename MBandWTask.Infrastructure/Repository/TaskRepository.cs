using System.Data;
using MBAndWTask.Application.Services;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace MBAndWTask.Infrastructure.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IConfiguration configuration;

        public TaskRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Add(MBAndWTask.Model.Task task)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            using (var command = new SqlCommand("SP_Add_Task", connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add(new SqlParameter("name", task.name));
                command.Parameters.Add(new SqlParameter("description", task.description));
                command.Parameters.Add(new SqlParameter("duedate", task.dueDate.Date));
                command.Parameters.Add(new SqlParameter("startdate", task.startDate.Date));
                command.Parameters.Add(new SqlParameter("enddate", task.endDate.Date));
                command.Parameters.Add(new SqlParameter("priority", task.priority));
                command.Parameters.Add(new SqlParameter("status", task.status));
                command.Parameters.Add("Msg", SqlDbType.NVarChar, 100);
                command.Parameters["Msg"].Direction = ParameterDirection.Output;
                connection.Open();
                command.ExecuteNonQuery();
                return Convert.ToString(command.Parameters["Msg"].Value);

            }
        }

        public string Update(MBAndWTask.Model.Task task)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            using (var command = new SqlCommand("SP_Update_Task", connection)
            {
                CommandType = CommandType.StoredProcedure,

            })
            {
                command.Parameters.Add(new SqlParameter("id", task.id));
                command.Parameters.Add(new SqlParameter("name", task.name));
                command.Parameters.Add(new SqlParameter("description", task.description));
                command.Parameters.Add(new SqlParameter("duedate", task.dueDate.Date));
                command.Parameters.Add(new SqlParameter("startdate", task.startDate.Date));
                command.Parameters.Add(new SqlParameter("enddate", task.endDate.Date));
                command.Parameters.Add(new SqlParameter("priority", task.priority));
                command.Parameters.Add(new SqlParameter("status", task.status));

                command.Parameters.Add("Msg", SqlDbType.NVarChar, 100);
                command.Parameters["Msg"].Direction = ParameterDirection.Output;
                connection.Open();
                command.ExecuteNonQuery();

                return Convert.ToString(command.Parameters["Msg"].Value);
            }
        }
        //public bool CheckTasksCountByStatus(DateTime dueDate)
        //{
        //    using (var connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
        //    using (var command = new SqlCommand("SP_CheckTasksCountByStatus", connection)
        //    {
        //        CommandType = CommandType.StoredProcedure
        //    })
        //    {
        //        command.Parameters.Add(new SqlParameter("dueDate", dueDate.Date));
        //        command.Parameters.Add("isValid", SqlDbType.Bit);
        //        command.Parameters["isValid"].Direction = ParameterDirection.Output;
        //        connection.Open();
        //        command.ExecuteNonQuery();
        //        return Convert.ToBoolean(command.Parameters["isValid"].Value);

        //    }
        //}

        public int GetHighPriorityTasksByDueDate(DateTime dueDate)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            using (var command = new SqlCommand("SELECT Count(*) FROM dbo.Task where status != 3 and priority = 1 and Due_Date = @dueDate", connection)
            {
                CommandType = CommandType.Text
            })
            {
                command.Parameters.AddWithValue("@dueDate", dueDate.ToShortDateString());
                connection.Open();
               var result =  (int)command.ExecuteScalar();
                return result;

            }
        }


    }
}
