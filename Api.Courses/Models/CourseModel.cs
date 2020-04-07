﻿
using System;
namespace Api.Courses.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
               
        public string Name { get; set; }
               
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
