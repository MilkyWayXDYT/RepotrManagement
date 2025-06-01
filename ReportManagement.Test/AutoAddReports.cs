using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FlaUI.Core;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ReportManagement.Test
{
    [TestClass]
    public class AutoAddReports
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
        public void AddReport_ValidReportWithoutCategory()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} | ({creationDate.ToString("yyyy-MM-dd")}) | {"Без категории"}";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Name == report);
            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void AddReport_ValidReportWithCategory()
        {
            string newCategory = "Финансы";

            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();
            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();
            categoryComboBox.Select(categoryComboBox.Items.Length - 1);

            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            string report = $"{title} | ({creationDate.ToString("yyyy-MM-dd")}) | {newCategory}";

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            Assert.IsTrue(reportsListBox.Items[reportsListBox.Items.Length - 1].Name == report);
            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void AddReport_EmptyName()
        {
            string title = "";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

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
        public void AddReport_EmptyContent()
        {
            string title = "Report name";
            string content = "";
            DateTime creationDate = DateTime.Today;

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

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
