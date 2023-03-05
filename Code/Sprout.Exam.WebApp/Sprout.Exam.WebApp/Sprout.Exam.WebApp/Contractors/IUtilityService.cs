using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Contractors
{
    public interface IUtilityService
    {
        decimal CalculateSalary(EmployeeType type, decimal noOfdays);
        Task<string> ValidateEmployee(EmployeeDto newData);
    }
}