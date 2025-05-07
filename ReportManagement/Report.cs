using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement
{
    class Report
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public Report(string title, string content, DateTime creationDate)
        {
            Title = title;
            Content = content;
            CreationDate = creationDate;
        }
    }
}
