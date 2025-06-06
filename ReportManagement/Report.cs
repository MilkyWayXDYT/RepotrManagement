﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement
{
    public class Report
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string Category { get; set; }
        public Report(string title, string content, DateTime creationDate, string category = "Без категории")
        {
            Title = title;
            Content = content;
            CreationDate = creationDate;
            Category = category;
        }
    }
}
