using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportManagement;

namespace ReportManagement.Test
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public void CheckConstructor()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = new DateTime(2025, 5, 14);

            var report = new Report(title, content, creationDate);
            Assert.IsNotNull(report);
        }
    }

    [TestClass]
    public class ReportManagerTest
    {
        string filePath = $"reports.txt";

        [TestMethod]
        public void FileExist()
        {
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        public void AddReport_AddReportAndSaveToFile()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = new DateTime(2025, 5, 14);

            var report = new Report(title, content, creationDate);
            var manager = new ReportManager();

            manager.AddReport(report);

            Assert.IsNotNull(manager.Reports.Count);

            string contentFile = File.ReadAllText(filePath);
            string expectedString = $"{title}|{content}|{creationDate.ToString("yyyy-MM-dd HH:mm:ss")}";

            Assert.IsTrue(contentFile.Contains(expectedString));
        }

        [TestMethod]
        public void AddReport_NullReport()
        {
            var manager = new ReportManager();
            Assert.ThrowsException<ArgumentNullException>(() => manager.AddReport(null));
        }

        [TestMethod]
        public void RemoveReport_RemoveReportAndSave()
        {
            string title = "Report name1";
            string content = "Report content1";
            DateTime creationDate = new DateTime(2025, 5, 14);

            var report = new Report(title, content, creationDate);
            var manager = new ReportManager();

            manager.AddReport(report);

            manager.RemoveReport(report);

            Assert.IsFalse(manager.Reports.Contains(report));
        }

        [TestMethod]
        public void RemoveReport_NullReport()
        {
            var manager = new ReportManager();
            Assert.ThrowsException<ArgumentNullException>(() => manager.RemoveReport(null));
        }

        [TestMethod]
        public void UpdateReport_ChangeReport()
        {
            string title = "Old report name";
            string content = "Old report content";
            DateTime creationDate = new DateTime(2025, 5, 14);

            var report = new Report(title, content, creationDate);
            var manager = new ReportManager();

            manager.AddReport(report);

            string newTitle = "New report name";
            string newContent = "New report content";

            manager.UpdateReport(report, newTitle, newContent);

            Assert.AreEqual(newTitle, report.Title);
            Assert.AreEqual(newContent, report.Content);
        }

        [TestMethod]
        public void UpdateReport_NullReport()
        {
            var manager = new ReportManager();
            Assert.ThrowsException<ArgumentNullException>(() => manager.UpdateReport(null, "Title", "Content"));
        }
    }
}
