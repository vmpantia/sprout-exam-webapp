using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class CalculateDto
    {
        public int Id { get; set; }
        [Required]
        public decimal AbsentDays { get; set; }
        [Required]
        public decimal WorkedDays { get; set; }
    }
}
