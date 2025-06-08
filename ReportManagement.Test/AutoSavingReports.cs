using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FlaUI.Core;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReportManagement.Test
{
    [TestClass]
    public class AutoSavingReports
    {
        private Application app;
        private UIA3Automation automation;
        private Window mainWindow;
        private string dataPath = "reports.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            app = Application.Launch(@"H:\Учеба\Тестирование\Лабораторные\ReportManagement\bin\Debug\ReportManagement.exe");
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
            File.WriteAllText(dataPath, string.Empty);
        }

        [TestMethod]
        public void SaveReport_AfterAddingReport()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Now;

            string report = $"{title}|{content}|{creationDate.ToString("yyyy-MM-dd HH:mm:ss")}|Без категории";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            var reportsInFile = File.ReadAllLines(dataPath);

            Assert.AreEqual(reportsInFile[0], report);
            File.WriteAllText(dataPath, string.Empty);
            app.Close();
        }

        [TestMethod]
        public void SaveReport_AfterDeletingReport()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();
            var removeReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("removeReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            reportsListBox.Items[reportsListBox.Items.Length - 1].AsButton().Click();
            removeReportButton.Click();

            var reportsInFile = File.ReadAllLines(dataPath);

            Assert.IsTrue(reportsInFile.Length == 0);
            File.WriteAllText(dataPath, string.Empty);
            app.Close();
        }

        [TestMethod]
        public void SaveReport_AfterEditingReport()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Now;

            string updateReport = $"{title} update|{content} update|{creationDate.ToString("yyyy-MM-dd HH:mm:ss")}|Без категории";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();
            var updateReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("updateReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            reportsListBox.Items[reportsListBox.Items.Length - 1].AsButton().Click();

            titleTextBox.Text = title + " update";
            contentTextBox.Text = content + " update";

            updateReportButton.Click();

            var reportsInFile = File.ReadAllLines(dataPath);

            Assert.IsTrue(reportsInFile[0] == updateReport);

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }
    }
}
