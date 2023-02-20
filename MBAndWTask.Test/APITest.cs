
using MBAndWTask.Model;
using MBAndWTask.Api.Controllers; 
using Microsoft.Extensions.Configuration;
using MBAndWTask.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using DateTimeExtensions;

namespace MBAndWTask.Test
{
    [TestClass]
    public class APIContollerTest
    {
        protected readonly IConfigurationRoot _configuration;
        private readonly TaskAPIController _controllerObj;

        public APIContollerTest()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .Build();

            var repository = new TaskRepository(_configuration);
            _controllerObj = new TaskAPIController(repository);

        }



        [TestMethod]
        public void AddTask()
        {
            var task = new MBAndWTask.Model.Task
            {
                name = "Test",
                description = "Testing",
                dueDate = DateTime.Now.AddDays(4).IsWorkingDay() && !DateTime.Now.AddDays(4).IsHoliday() ? DateTime.Now.AddDays(4) : DateTime.Now.AddDays(2),
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddDays(2),
                status = Status.New,
                priority = Priority.Medium,
                
            };
            var result = _controllerObj.Add(task);
            var okResult = result as OkObjectResult;

            // assert           
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
        [TestMethod]
        public void UpdateTask()
        {
            //Update Email Address.
            var task = new MBAndWTask.Model.Task
            {
                id = 1,
                name = "Test Updated",
                description = "Testing",
                dueDate = DateTime.Now.AddDays(4).IsWorkingDay() && !DateTime.Now.AddDays(4).IsHoliday() ? DateTime.Now.AddDays(4) : DateTime.Now.AddDays(2),
                startDate = DateTime.Now,
                endDate = DateTime.Now.AddDays(2),
                status = Status.New,
                priority = Priority.Medium,

            };
            var result = _controllerObj.Update(task);
            var okResult = result as OkObjectResult;

            // assert           
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }


    }
}

