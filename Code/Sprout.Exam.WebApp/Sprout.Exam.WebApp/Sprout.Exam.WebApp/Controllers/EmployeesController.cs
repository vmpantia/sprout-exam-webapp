using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.WebApp.Services;
using Sprout.Exam.WebApp.Contractors;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public readonly IBaseRepository<EmployeeDto> _employee;
        public EmployeesController(IBaseRepository<EmployeeDto> employee)
        {
            _employee = employee;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _employee.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _employee.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {
                //Update data in Database
                await _employee.Update(input.Id, new
                {
                    input.FullName,
                    input.Tin,
                    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                    input.TypeId
                });

                //Get Latest Data
                var latestData = _employee.GetById(input.Id);
                return Ok(latestData);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            try
            {
                var employee = new EmployeeDto
                {
                    FullName = input.FullName,
                    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                    Tin = input.Tin,
                    TypeId = input.TypeId
                };

                await _employee.Add(employee);
                return Created($"/api/employees/{employee.Id}", employee.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employee.Delete(id);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(CalculateDto input)
        {
            try
            {
                var result = await _employee.GetById(input.Id);

                if (result == null) return NotFound();
                var type = (EmployeeType) result.TypeId;

                decimal totalSalary;
                switch (type)
                {
                    case EmployeeType.Regular:
                        totalSalary = UtilityService.CalculateSalary(EmployeeType.Regular, input.AbsentDays);
                        break;

                    case EmployeeType.Contractual:
                        totalSalary = UtilityService.CalculateSalary(EmployeeType.Contractual, input.WorkedDays);
                        break;

                    default:
                        return NotFound("Employee Type not found");
                }
                return Ok(totalSalary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
