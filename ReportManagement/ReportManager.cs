using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement
{
    public class ReportManager
    {
        public List<Report> Reports { get; private set; }
        public ReportManager()
        {
            Reports = new List<Report>();
            LoadReports();
        }
        public void AddReport(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            Reports.Add(report);
            SaveReports();
        }
        public void RemoveReport(Report report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            Reports.Remove(report);
            SaveReports();
        }
        public void UpdateReport(Report report, string newTitle, string newContent)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }
            report.Title = newTitle;
            report.Content = newContent;
            SaveReports();
        }
        private void SaveReports()
        {
            File.WriteAllLines("reports.txt", Reports.Select(r =>
            $"{r.Title}|{r.Content}|{r.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")}"));
        }
        private void LoadReports()
        {
            if (File.Exists("reports.txt"))
            {
                var lines = File.ReadAllLines("reports.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        DateTime creationDate;
                        if (DateTime.TryParse(parts[2], out creationDate))
                        {
                            Reports.Add(new Report(parts[0], parts[1], creationDate));
                        }
                    }
                }
            }
        }
    }
}
