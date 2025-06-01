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
    public class AutoUpdateReports
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
        public void UpdateReport_ValidUpdate()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} update | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

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

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Text == report);

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void UpdateReport_ReportNotSelected()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} update | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();
            var updateReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("updateReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            updateReportButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Выберите отчёт для обновления!", "Выберите отчёт для обновления!");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void UpdateReport_EmptyName()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} update | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();
            var updateReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("updateReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            reportsListBox.Items[reportsListBox.Items.Length - 1].AsButton().Click();

            titleTextBox.Text = "";
            contentTextBox.Text = content + " update";

            updateReportButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Заполните все поля!", "Заполните все поля!");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void UpdateReport_EmptyContent()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} update | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

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
            contentTextBox.Text = "";

            updateReportButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Заполните все поля!", "Заполните все поля!");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

    }
}
