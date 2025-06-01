using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System.Linq;
using FlaUI.Core;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReportManagement.Test
{
    [TestClass]
    public class AutoDeleteReports
    {
        private Application app;
        private UIA3Automation automation;
        private Window mainWindow;

        [TestInitialize]
        public void TestInitialize()
        {
            app = Application.Launch(@"H:\Учеба\Тестирование\Лабораторные\ReportManagement\bin\Debug\ReportManagement.exe");
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
        }

        [TestMethod]
        public void DeleteReport_ReportIsSelect()
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

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Name == report);

            reportsListBox.Items[reportsListBox.Items.Length - 1].AsButton().Click();
            removeReportButton.Click();
            Assert.IsTrue(reportsListBox.Items.Length == 0);

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void DeleteReport_ReportNotSelect()
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

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Name == report);

            removeReportButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Выберите отчёт для удаления!", "Выберите отчёт для удаления!");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

    }
}
