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

        public List<string> Categories { get; private set; }

        public ReportManager()
        {
            Reports = new List<Report>();
            Categories = new List<string> { "Без категории" };
            LoadReports();
        }

        public void AddReport(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));
            Reports.Add(report);
            SaveReports();
        }

        public void RemoveReport(Report report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));
            Reports.Remove(report);
            SaveReports();
        }

        public void UpdateReport(Report report, string newTitle, string newContent, string newCategory = null)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));
            report.Title = newTitle;
            report.Content = newContent; if (newCategory != null)
                report.Category = newCategory;
            SaveReports();
        }

        public void AddCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new Exception("Название категории не может быть пустым");
            if (Categories.Contains(category))
                throw new Exception("Категория уже существует");
            Categories.Add(category);
        }

        public  List<Report> GetReportsByCategory(string category)
        {
            if (string.IsNullOrEmpty(category) || category == "Все")
                return Reports;
            return Reports.FindAll(x => x.Category == category);
        }

        private void SaveReports()
        {
            File.WriteAllLines("reports.txt", Reports.Select(r =>
            $"{r.Title}|{r.Content}|{r.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")}|{r.Category}"));
        }

        private void LoadReports()
        {
            if (File.Exists("reports.txt"))
            {
                var lines = File.ReadAllLines("reports.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        DateTime creationDate;
                        if (DateTime.TryParse(parts[2], out creationDate))
                        {
                            string category = parts.Length > 3 ? parts[3] : "Без категории";
                            Reports.Add(new Report(parts[0], parts[1], creationDate, category));
                            if (!Categories.Contains(category) && !string.IsNullOrEmpty(category))
                                Categories.Add(category);
                        }
                    }
                }
            }
        }
    }
}
