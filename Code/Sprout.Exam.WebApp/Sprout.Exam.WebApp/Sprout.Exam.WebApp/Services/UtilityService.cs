using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using System;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.WebApp.Contractors;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IBaseRepository<EmployeeDto> _employee;
        public UtilityService(IBaseRepository<EmployeeDto> employee)
        {
            _employee = employee;
        }

        public decimal CalculateSalary(EmployeeType type, decimal noOfdays)
        {
            decimal totalSalary = 0m;
            switch (type)
            {
                case EmployeeType.Regular:
                    //Compute rate per day & absent deductions
                    var perDayRate = Math.Round(Constant.RATE_PER_MONTH_REG_EMP / Constant.TOTAL_DAYS_PER_MONTH, Constant.DECIMAL_PLACE);
                    var absentDeductions = Math.Round(perDayRate * noOfdays, Constant.DECIMAL_PLACE);

                    //Compute tax rate & monthly tax
                    var taxRate = (Constant.TAX_PERCENT / Constant.MAX_PERCENT);
                    var monthlyTax = Math.Round(Constant.RATE_PER_MONTH_REG_EMP * taxRate, Constant.DECIMAL_PLACE);

                    //Compute total deductions
                    var totalDeductions = Math.Round(monthlyTax + absentDeductions, Constant.DECIMAL_PLACE);

                    //Compute total salary
                    totalSalary = Math.Round(Constant.RATE_PER_MONTH_REG_EMP - totalDeductions, Constant.DECIMAL_PLACE);
                    break;

                case EmployeeType.Contractual:
                    //Compute total salary
                    totalSalary = Math.Round(Constant.RATE_PER_DAY_CONT_EMP * noOfdays, Constant.DECIMAL_PLACE);
                    break;
            }

            return totalSalary;
        }

        public async Task<string> ValidateEmployee(EmployeeDto newData)
        {
            EmployeeDto currentData = new EmployeeDto();
            var isAdd = newData.Id == 0;

            if (!isAdd)
            {
                currentData = await _employee.GetById(newData.Id);
                if (currentData == null)
                    return Constant.ERROR_MESSAGE_EMPLOYEE_NOT_FOUND;

                if (IsPristine(newData, currentData))
                    return Constant.ERROR_MESSAGE_NO_CHANGES_MADE;
            }

            //Check duplicate data
            if (isAdd || (!isAdd && newData.FullName != currentData.FullName))
            {
                //Check if fullname is already exist in the System
                if (_employee.IsDataExist((data) => data.FullName == newData.FullName))
                    return Constant.ERROR_MESSAGE_FULLNAME_EXIST;
            }
            if (isAdd || (!isAdd && newData.Tin != currentData.Tin))
            {
                //Check if the TIN is already exist in the System
                if (_employee.IsDataExist((data) => data.Tin == newData.Tin))
                    return Constant.ERROR_MESSAGE_TIN_NO_EXIST;
            }
            return string.Empty;
        }

        private bool IsPristine<T>(T newData, T oldData)
        {
            if (newData == null || oldData == null)
                throw new Exception(Constant.ERROR_MESSAGE_DATA_NOT_SAME);

            var properties = newData.GetType().GetProperties();
            if (properties == null)
                throw new Exception(Constant.ERROR_MESSAGE_INVALID_PROPERTIES);

            foreach (var property in properties)
            {
                var newValue = property.GetValue(newData).ToString();
                var oldValue = property.GetValue(oldData).ToString();
                if (newValue == oldValue)
                    continue;

                return false;
            }
            return true;
        }
    }
}
