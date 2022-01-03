using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace curdoperation.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
