using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for LoginWindows.xaml
    /// </summary>
    public partial class LoginWindows : MetroWindow
    {
        int perem = 14;
        String TabNum;
        DataTable table;
        SqlDataAdapter adapter;
        public LoginWindows()
        {
            InitializeComponent();
            MouseWheel += MouseWheelSize;
        }
        private void initializeDB()
        {
            string connectionString = @"Data Source=MN02;Initial Catalog=OrionLight;Integrated Security=True";

            SqlConnection sqlconnection = new SqlConnection();
            try
            {
                sqlconnection = new SqlConnection(connectionString);
                //Выполнение хранимой процедуры
                SqlCommand command = new SqlCommand("GETPersonList @TabNum = '" + TabNum + "', @Workdate = '2019-08-06 00:00:00'", sqlconnection);

                adapter = new SqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlconnection.State == ConnectionState.Open)
                    sqlconnection.Close();
            }
        }
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridRow)sender;
            Authorize.userId = ((DataRowView)dataGrid.SelectedItem).Row["TabNum"].ToString();
            Authorize.selectedUsername = ((DataRowView)dataGrid.SelectedItem).Row["fio"].ToString();
            InfoWindow iw = new InfoWindow();
            iw.ShowDialog();
        }
        private void buttonTheme_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        private void dataGrid_Initialized(object sender, EventArgs e)
        {
            if (Environment.UserName == "intern.it")
            {
                TabNum = "01531";
                this.Title = "Атаматов Нодир Заиржанович, Таб.Номер 01608";
                initializeDB();
            }
        }
        private void ExportToExcel() // Импорт DataGrid в Excel
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook workBook = excel.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range cellRange;
                workSheet.Name = "Табель"; // Название страницы
                System.Data.DataTable tempDt = table;
                dataGrid.ItemsSource = tempDt.DefaultView;
                workSheet.Cells.Font.Size = 11;
                int rowcount = 1;
                for (int i = 1; i <= tempDt.Columns.Count; i++) //Имена заголовков
                {
                    workSheet.Cells[1, i] = dataGrid.Columns[i - 1].Header;
                }
                foreach (System.Data.DataRow row in tempDt.Rows) //Все строки
                {
                    rowcount += 1;
                    for (int i = 0; i < tempDt.Columns.Count; i++) //Каждая колонка
                    {
                        workSheet.Cells[rowcount, i + 1] = row[i].ToString();
                    }
                }
                cellRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[rowcount, tempDt.Columns.Count]];
                cellRange.EntireColumn.AutoFit();

                System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog(); // Диалог сохранения файла
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workBook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Успешно!");
                }

                workBook.Close(); excel.Quit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MouseWheelSize(object sender, MouseWheelEventArgs e)
        {

            if (e.Delta != 0)
            {
                if (e.Delta >= 0)
                {
                    perem++;
                    textBox.AppendText(perem.ToString());
                    dataGrid.FontSize = perem;
                }
                else if (e.Delta <= 0)
                {
                    perem--;
                    textBox.AppendText(perem.ToString());
                    dataGrid.FontSize = perem;
                }

            }
        }
    }
}
