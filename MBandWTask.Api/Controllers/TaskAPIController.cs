using DateTimeExtensions;
using MBAndWTask.Api.Models;
using MBAndWTask.Application.Services;
using Microsoft.AspNetCore.Mvc;
using MBAndWTask.Model;

namespace MBAndWTask.Api.Controllers
{
    
    [ApiController]
    [Route("task")]
    public class TaskAPIController :ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskAPIController(ITaskRepository taskRepository)
        {
            this._taskRepository = taskRepository;
        }

        [HttpPost("add")]
        public IActionResult Add(Model.Task task)
        {
            var apiResponse = new APIResponse();
            string message;
            if (!Enum.IsDefined(typeof(Priority), task.priority))
            {
                ModelState.AddModelError(nameof(task.priority), "Invalid Priority value.");
            }
            if (task.startDate.Date >= task.endDate.Date)
            {
                ModelState.AddModelError(nameof(task.startDate), "Start date can not be greater than end date.");
            }
            else if (!IsValidDueDate(task.dueDate, out message))
            {
                ModelState.AddModelError(nameof(task.dueDate), message);
            }   
            
            try
            {                
                if (ModelState.IsValid)
                {
                    var result = _taskRepository.Add(task);
                    apiResponse.Success = true;
                    apiResponse.Message= result;
                }
                else
                    return BadRequest(ModelState);
            }            
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }
            return Ok(apiResponse);
        }

        [HttpPut("update")]
        public IActionResult Update(Model.Task task)
        {
            if (task == null || task.id == 0)
                return BadRequest();
            var apiResponse = new APIResponse();
            try
            {
                string message;
                if (task.startDate.Date >= task.endDate.Date)
                {
                    ModelState.AddModelError(nameof(task.startDate), "Start date can not be greater than end date.");
                }
                else if (!IsValidDueDate(task.dueDate, out message)) { 
                    ModelState.AddModelError(nameof(task.dueDate), message);
                }
                 
                if (ModelState.IsValid)
                {
                    var result = _taskRepository.Update(task);
                    apiResponse.Success = true;
                    apiResponse.Message = result;
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }
            return Ok(apiResponse);
        }

        private bool IsValidDueDate(DateTime dueDate, out string message)
        {
            bool isValid = false;
            message = "";
            if (DateOnly.FromDateTime(DateTime.Now) >= DateOnly.FromDateTime(dueDate))
            {                
                message = "The due date cannot be in the past.";                
            }
            else if (dueDate.IsHoliday() || !(dueDate.IsWorkingDay()))
            {
                message = "The due date cannot be on a Holiday or weekend.";
            }
            //else if (!_taskRepository.CheckTasksCountByStatus(dueDate))
            else if(_taskRepository.GetHighPriorityTasksByDueDate(dueDate) >= 100)
            {
                message =  "More than 100 high priority tasks with the same due date are not yet finished.";
            }
            else
                isValid = true;

            return isValid;

        }
    }
}
