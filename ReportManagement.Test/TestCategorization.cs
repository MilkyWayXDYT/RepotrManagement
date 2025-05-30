using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ReportManagement.Test
{
    [TestClass]
    public class TestCategorization
    {
        private ReportManager reportManager;

        [TestMethod]
        public void AddCategory_ValidCategory()
        {
            string category = "Тестовая категория";
            reportManager = new ReportManager();

            reportManager.AddCategory(category);
            Assert.IsTrue(reportManager.Categories.Contains(category));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddCategory_InvalidCategory_EmptyCategory()
        {
            string category = "";
            reportManager = new ReportManager();
            reportManager.AddCategory(category);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddCategory_InvalidCategory_ExistCategory()
        {
            string category = "Существующая категория";
            reportManager = new ReportManager();
            reportManager.AddCategory(category);
            reportManager.AddCategory(category);
        }

        [TestMethod]
        public void GetReportsByCategory_AllCategory()
        {
            string title1 = "Report name";
            string content1 = "Report content";
            DateTime creationDate1 = new DateTime(2025, 5, 14);
            string category1 = "Первая категория";

            string title2 = "Report name2";
            string content2 = "Report content2";
            DateTime creationDate2 = new DateTime(2025, 5, 14);
            string category2 = "Вторая категория";

            var report = new Report(title1, content1, creationDate1, category1);
            var report2 = new Report(title2, content2, creationDate2, category2);

            reportManager = new ReportManager();
            reportManager.AddReport(report);
            reportManager.AddReport(report2);

            var allCategory = reportManager.GetReportsByCategory("All");
            Assert.IsTrue(allCategory.Contains(report));
            Assert.IsTrue(allCategory.Contains(report2));
            Assert.IsNotNull(allCategory.Count); // две добавленные категории + по умолчанию "Без категории"
        }

        [TestMethod]
        public void GetReportsByCategory_EmptyCategory()
        {
            string title1 = "Report name";
            string content1 = "Report content";
            DateTime creationDate1 = new DateTime(2025, 5, 14);
            string category1 = "Первая категория";

            string title2 = "Report name2";
            string content2 = "Report content2";
            DateTime creationDate2 = new DateTime(2025, 5, 14);
            string category2 = "Вторая категория";

            var report = new Report(title1, content1, creationDate1, category1);
            var report2 = new Report(title2, content2, creationDate2, category2);

            reportManager = new ReportManager();
            reportManager.AddReport(report);
            reportManager.AddReport(report2);

            var allCategory = reportManager.GetReportsByCategory("");
            Assert.IsTrue(allCategory.Contains(report));
            Assert.IsTrue(allCategory.Contains(report2));
            Assert.IsNotNull(allCategory.Count); // две добавленные категории + по умолчанию "Без категории"
        }

        [TestMethod]
        public void GetReportsByCategory_OneCategoryIsSelected()
        {
            string title1 = "Report name one category";
            string content1 = "Report content";
            DateTime creationDate1 = new DateTime(2025, 5, 14);
            string category1 = "Первая категория";

            string title2 = "Report name2 one category";
            string content2 = "Report content2";
            DateTime creationDate2 = new DateTime(2025, 5, 14);
            string category2 = "Вторая категория";

            var report = new Report(title1, content1, creationDate1, category1);
            var report2 = new Report(title2, content2, creationDate2, category2);

            reportManager = new ReportManager();
            reportManager.AddReport(report);
            reportManager.AddReport(report2);

            var oneCategory = reportManager.GetReportsByCategory(category1);
            Assert.IsTrue(oneCategory.Contains(report));
            Assert.IsFalse(oneCategory.Contains(report2));
        }

        [TestMethod]
        public void AddReport_AddReportAndSaveToFileWithCategory()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = new DateTime(2025, 5, 14);
            string category = "New category";

            var report = new Report(title, content, creationDate, category);
            var manager = new ReportManager();

            manager.AddReport(report);

            Assert.IsNotNull(manager.Reports.Count);

            Assert.IsTrue(File.Exists("reports.txt"));

            string contentFile = File.ReadAllText("reports.txt");
            string expectedString = $"{title}|{content}|{creationDate.ToString("yyyy-MM-dd HH:mm:ss")}|{category}";

            Assert.IsTrue(contentFile.Contains(expectedString));
        }
    }
}
