using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Enroll.Models
{
    public class EnrollModel
    {
        public int Id { get; set; }

        public int EnrolledId { get; set; }

        public int CourseId { get; set; }

        public int EnrollTypeId { get; set; }
    }
}
