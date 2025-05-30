using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportManagement
{
    public partial class ReportForm : Form
    {
        private ReportManager reportManager;
        private TextBox titleTextBox;
        private TextBox contentTextBox;
        private Button addReportButton;
        private Button removeReportButton;
        private Button updateReportButton;
        private ListBox reportsListBox;
        private ComboBox categoryComboBox;
        private TextBox newCategoryTextBox;
        private Button addCategoryButton;
        private ComboBox filterComboBox;

        public ReportForm()
        {
            this.Text = "Управление отчётами";
            this.Width = 600;
            this.Height = 500;
            titleTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Width = 200,
                Text = "Название"
            };
            titleTextBox.GotFocus += TitleFocusEnter;
            titleTextBox.LostFocus += TitleFocusLeave;

            contentTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Width = 200,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Text = "Содержание"
            };
            contentTextBox.GotFocus += ContentFocusEnter;
            contentTextBox.LostFocus += ContentFocusLeave;

            categoryComboBox = new ComboBox
            {
                Location = new Point(220, 10),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            newCategoryTextBox = new TextBox
            {
                Location = new Point(220, 40),
                Width = 150,
                Text = "Новая категория"
            };
            newCategoryTextBox.GotFocus += NewCategoryFocusEnter;
            newCategoryTextBox.LostFocus += NewCategoryFocusLeave;

            addCategoryButton = new Button
            {
                Location = new Point(380, 40),
                Text = "Добавить категорию",
                Width = 150
            };
            addCategoryButton.Click += AddCategoryButton_Click;

            filterComboBox = new ComboBox
            {
                Location = new Point(10, 150),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;

            addReportButton = new Button
            {
                Location = new System.Drawing.Point(10, 180),
                Text = "Добавить",
                Width = 100
            };
            addReportButton.Click += AddReportButton_Click;
            removeReportButton = new Button
            {
                Location = new System.Drawing.Point(120, 180),
                Text = "Удалить",
                Width = 100
            };
            removeReportButton.Click += RemoveReportButton_Click;
            updateReportButton = new Button
            {
                Location = new System.Drawing.Point(220, 180),
                Text = "Обновить",
                Width = 100
            };
            updateReportButton.Click += UpdateReportButton_Click;
            reportsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 210),
                Width = 560,
                Height = 200
            };
            this.Controls.Add(titleTextBox);
            this.Controls.Add(contentTextBox); 
            this.Controls.Add(categoryComboBox);
            this.Controls.Add(newCategoryTextBox);
            this.Controls.Add(addCategoryButton);
            this.Controls.Add(filterComboBox);
            this.Controls.Add(addReportButton);
            this.Controls.Add(removeReportButton);
            this.Controls.Add(updateReportButton);
            this.Controls.Add(reportsListBox);
            reportManager = new ReportManager();
            UpdateReportsList(); 
            UpdateCategoryComboBox();
            UpdateFilterComboBox();
        }

        private void TitleFocusEnter(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "Название")
                ((TextBox)sender).Text = "";
        }

        private void TitleFocusLeave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "Название";
        }

        private void ContentFocusEnter(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "Содержание")
                ((TextBox)sender).Text = "";
        }

        private void ContentFocusLeave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "Содержание";
        }

        private void NewCategoryFocusEnter(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "Новая категория")
                ((TextBox)sender).Text = "";
        }

        private void NewCategoryFocusLeave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "Новая категория";
        }

        private void UpdateCategoryComboBox()
        {
            categoryComboBox.Items.Clear();
            foreach (var category in reportManager.Categories)
            {
                categoryComboBox.Items.Add(category);
            }
            categoryComboBox.SelectedIndex = categoryComboBox.Items.IndexOf("Без категории");
        }

        private void UpdateFilterComboBox()
        {
            filterComboBox.Items.Clear();
            filterComboBox.Items.Add("All");
            foreach (var category in reportManager.Categories)
            {
                filterComboBox.Items.Add(category);
            }
            filterComboBox.SelectedIndex = 0;
        }

        private void UpdateReportsList(string categoryFilter = null)
        {
            reportsListBox.Items.Clear();
            foreach (var report in reportManager.GetReportsByCategory(categoryFilter))
            {
                reportsListBox.Items.Add($"{report.Title} | ({report.CreationDate.ToString("yyyy-MM-dd")}) | {report.Category}");
            }
        }

        private void AddReportButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(titleTextBox.Text) || string.IsNullOrEmpty(contentTextBox.Text) || titleTextBox.Text == "Название" || contentTextBox.Text == "Содержание")
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            string category = categoryComboBox.SelectedItem?.ToString() ?? "Без категории";
            Report newReport = new Report(titleTextBox.Text, contentTextBox.Text,
            DateTime.Now, category);
            try
            {
                reportManager.AddReport(newReport);
                titleTextBox.Clear();
                contentTextBox.Clear();
                UpdateReportsList(filterComboBox.SelectedItem?.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            titleTextBox.Text = "Название";
            contentTextBox.Text = "Содержание";
        }

        private void RemoveReportButton_Click(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите отчёт для удаления!");
                return;
            }
            string selectedItem = reportsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { " | " }, StringSplitOptions.None);
            if (parts.Length >= 3)
            {
                string title = parts[0];
                DateTime date;
                if (DateTime.TryParse(parts[1].Trim(new char[] { '(', ')' }), out date))
                {
                    var reportToRemove = reportManager.Reports.Find(r => r.Title == title &&
                    r.CreationDate.Date == date.Date);
                    if (reportToRemove != null)
                    {
                        try
                        {
                            reportManager.RemoveReport(reportToRemove);
                            UpdateReportsList(filterComboBox.SelectedItem?.ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void UpdateReportButton_Click(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите отчёт для обновления!");
                return;
            }
            string selectedItem = reportsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { " | " }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string title = parts[0].Trim();
                DateTime date;
                if (DateTime.TryParse(parts[1].Trim(new char[] { '(', ')' }), out date))
                {
                    var reportToUpdate = reportManager.Reports.Find(r => r.Title == title && r.CreationDate.Date == date.Date);
                    if (reportToUpdate != null)
                    {
                        if (string.IsNullOrEmpty(titleTextBox.Text) ||
                        string.IsNullOrEmpty(contentTextBox.Text) ||
                         titleTextBox.Text == "Название" || 
                         contentTextBox.Text == "Содержание")
                        {
                            MessageBox.Show("Заполните все поля!");
                            return;
                        }

                        string category = categoryComboBox.SelectedItem?.ToString() ?? reportToUpdate.Category;
                        try
                        {
                            reportManager.UpdateReport(reportToUpdate, titleTextBox.Text, contentTextBox.Text, category);
                            UpdateReportsList(filterComboBox.SelectedItem?.ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(newCategoryTextBox.Text) || newCategoryTextBox.Text == "Новая категория")
            {
                MessageBox.Show("Введите название категории!");
                return;
            }

            try
            {
                reportManager.AddCategory(newCategoryTextBox.Text);
                newCategoryTextBox.Text = "Новая категория";
                UpdateCategoryComboBox();
                UpdateFilterComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReportsList(filterComboBox.SelectedItem?.ToString());
        }

        private void ReportsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportsListBox.SelectedIndex != -1)
            {
                string selectedItem = reportsListBox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { " | " }, StringSplitOptions.None);
                if (parts.Length >= 3)
                {
                    string title = parts[0];
                    DateTime date;
                    if (DateTime.TryParse(parts[1], out date))
                    {
                        var report = reportManager.Reports.Find(r => r.Title == title && r.CreationDate.Date == date.Date);
                        if (report != null)
                        {
                            titleTextBox.Text = report.Title;
                            contentTextBox.Text = report.Content;
                            categoryComboBox.SelectedIndex = categoryComboBox.Items.IndexOf(report.Category);
                        }
                    }
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
