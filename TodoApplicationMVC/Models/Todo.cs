using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoApplicationMVC.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}