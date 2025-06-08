using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FlaUI.Core;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReportManagement.Test
{
    [TestClass]
    public class AutoUploadingReports
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
        }

        [TestMethod]
        public void UploadFile_FileCompleted()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            var reportsInFile = File.ReadAllLines(dataPath);
            
            var report = reportsInFile[reportsInFile.Length - 1].Split('|');
            string resReport = $"{report[0]} | ({DateTime.Parse(report[2]).ToString("yyyy-MM-dd")}) | {report[3]}";

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Name == resReport);
            Assert.IsTrue(reportsListBox.Items.Length == reportsInFile.Length);
            File.WriteAllText(dataPath, string.Empty);
            app.Close();
        }

        [TestMethod]
        public void UploadFile_EmptyFile()
        {
            File.WriteAllText(dataPath, string.Empty);
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();
            var reportsInFile = File.ReadAllLines(dataPath);

            Assert.IsTrue(reportsListBox.Items.Length == reportsInFile.Length);
            Assert.IsTrue(reportsListBox.Items.Length == 0);
            Assert.IsTrue(reportsInFile.Length == 0);

            app.Close();
        }
    }
}
