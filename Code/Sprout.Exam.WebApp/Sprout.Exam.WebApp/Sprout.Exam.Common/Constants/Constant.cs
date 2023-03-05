using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Common.Constants
{
    public class Constant
    {
        public const int     DECIMAL_PLACE = 2;
        public const int     TOTAL_DAYS_PER_MONTH = 22;

        public const decimal RATE_PER_MONTH_REG_EMP = 20000;
        public const decimal RATE_PER_DAY_CONT_EMP = 500;

        public const decimal TAX_PERCENT = 12;
        public const decimal MAX_PERCENT = 100;

        public const string FORMAT_NUMBER = "N2";
        public const string FORMAT_DATE = "yyyy-MM-dd";

        public const string ERROR_MESSAGE_EMPTYPE_NOT_FOUND = "Employee Type not found.";
        public const string ERROR_MESSAGE_EMPLOYEE_NOT_FOUND = "Employee not found in the system.";
        public const string ERROR_MESSAGE_NO_CHANGES_MADE = "No changes made in the data.";
        public const string ERROR_MESSAGE_FULLNAME_EXIST = "Fullname is already exist in the System.";
        public const string ERROR_MESSAGE_TIN_NO_EXIST = "TIN number is already exist in the System.";
        public const string ERROR_MESSAGE_DATA_NOT_SAME = "New data and old data cannot be NULL.";
        public const string ERROR_MESSAGE_INVALID_PROPERTIES = "Invalid Properties.";
    }
}
