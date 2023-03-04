using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using System.Linq;
using System.Threading.Tasks;
using System;
using Sprout.Exam.Common.Constants;
using System.Reflection.Metadata.Ecma335;

namespace Sprout.Exam.WebApp.Services
{
    public class UtilityService
    {
        public static decimal CalculateSalary(EmployeeType type, decimal noOfdays)
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
    }
}
