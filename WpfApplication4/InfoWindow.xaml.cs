using System;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using MahApps.Metro.Controls;
using TimesheetApplication;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : MetroWindow
    {
        String TabNum;
        String CurrDate;
        DataTable table;
        SqlDataAdapter adapter;
        
        public InfoWindow()
        {
            InitializeComponent();
            this.Title = Authorize.selectedUsername + ", " + "Таб. Номер " + Authorize.userId;     
        }
        private void initializeDB()
        {
            CurrDate = "2019.07.31";
            string connectionString = @"Data Source=MN02;Initial Catalog=OrionLight;Integrated Security=True";
            SqlConnection sqlconnection = new SqlConnection();
            try
            {
                sqlconnection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("GetTabelInfo @TabNum = '" + TabNum + "', @CurrDate = '" + CurrDate + "'", sqlconnection);
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
        private void FieldDataGridChecked(object sender, RoutedEventArgs e)
        { }
        private void FieldDataGridUnchecked(object sender, RoutedEventArgs e)
        { }
		private void dataGrid_Initialized(object sender, EventArgs e)
		{
			if (Environment.UserName == "intern.it")
			{
				TabNum = Authorize.userId;
				initializeDB();
			}

		}
        private void acceptsChangesClick(object sender, EventArgs e)
        {
            //MessageBoxResult result = new MessageBoxResult();
            //result = MessageBox.Show("Вы уверены? Данное действие нельзя будет отменить", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Information);

            //if(result == MessageBoxResult.Yes)
            //{
            //    MessageBox.Show("Данные успешно внесены");
            //    /// <>
            //    ///insert to database and other..
            //    /// <>
            //}
            //else if(result == MessageBoxResult.No)
            //{
            //    MessageBox.Show("Действие отменено");
            //}

            PastMonth pm = new PastMonth();
            pm.ShowDialog();
           
            
        }
    }
}
