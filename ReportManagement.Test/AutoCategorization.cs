using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using System.Linq;
using System;
using System.IO;

namespace ReportManagement.Test
{
    [TestClass]
    public class AutoCategorization
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
        public void AddCategory_ValidCategory()
        {
            string newCategory = "Финансы";

            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            Assert.IsTrue(filterCategoryComboBox.Items.Any(item => item.Text == newCategory));
            Assert.IsTrue(categoryComboBox.Items.Any(item => item.Text == newCategory));

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void AddCategory_EmptyName()
        {
            string newCategory = "";

            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Введите название категории!", "Введите название категории!");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void AddCategory_DoubleCategory()
        {
            string newCategory = "Финансы";

            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            var messageBox = mainWindow.ModalWindows.FirstOrDefault();
            var msgText = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("65535"));
            var msgButton = messageBox.FindFirstDescendant(cf => cf.ByAutomationId("2")).AsButton();

            string mText = msgText.Name;

            StringAssert.Contains(mText, "Категория уже существует", "Категория уже существует");

            msgButton.Click();

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void FilterCategory_AllCategories()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;
            string newCategory = "Финансы";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            titleTextBox.Text = title;
            contentTextBox.Text = content;
            categoryComboBox.Items[categoryComboBox.Items.Length - 1].AsButton().Click();

            addReportButton.Click();

            filterCategoryComboBox.Items[0].AsButton().Click();

            Assert.IsTrue(reportsListBox.Items.Length == 2);

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }

        [TestMethod]
        public void FilterCategory_SelectAddedCategory()
        {
            string title = "Report name";
            string content = "Report content";
            DateTime creationDate = DateTime.Today;
            string newCategory = "Финансы";

            var titleTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("titleTextBox")).AsTextBox();
            var contentTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("contentTextBox")).AsTextBox();
            var newCategoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("newCategoryTextBox")).AsTextBox();
            var addCategoryButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addCategoryButton")).AsButton();
            var filterCategoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("filterComboBox")).AsComboBox();
            var categoryComboBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryComboBox")).AsComboBox();
            var addReportButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addReportButton")).AsButton();
            var reportsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("reportsListBox")).AsListBox();

            titleTextBox.Text = title;
            contentTextBox.Text = content;

            addReportButton.Click();

            newCategoryTextBox.Text = newCategory;
            addCategoryButton.Click();

            titleTextBox.Text = title;
            contentTextBox.Text = content;
            categoryComboBox.Items[categoryComboBox.Items.Length - 1].AsButton().Click();

            addReportButton.Click();

            filterCategoryComboBox.Items[filterCategoryComboBox.Items.Length - 1].AsButton().Click();

            Assert.AreEqual(1, reportsListBox.Items.Length);

            File.WriteAllText("reports.txt", string.Empty);
            app.Close();
        }
    }
}
