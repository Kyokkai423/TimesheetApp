using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApplication4;

namespace TimesheetApplication
{
    /// <summary>
    /// Interaction logic for PastMonth.xaml
    /// </summary>
    public partial class PastMonth : MetroWindow
    {
        String TabNum;
        DataTable table;
        SqlDataAdapter adapter;
        SqlConnection sqlconnection;
        public PastMonth()
        {
            InitializeComponent();
            this.Title = Authorize.selectedUsername + ", " + "Таб. Номер " + Authorize.userId;
        }

        private void initializeDB()
        {
            string connectionString = @"Data Source=MN02;Initial Catalog=OrionLight;Integrated Security=True";

            string sql = @"declare @PastDate datetime,
                         @CurrentDate datetime
                         SET @CurrentDate = '2019.07.01'
                         SET @PastDate = DATEADD(mm, -1, @CurrentDate)
                         execute GetTabelInfo @TabNum = '" + TabNum + "', @CurrDate = @PastDate";

            sqlconnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, sqlconnection);
            adapter = new SqlDataAdapter(command);

            table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.DefaultView;
        }

        private void dataGrid_Initialized(object sender, EventArgs e)
        {
            if (Environment.UserName == "intern.it")
            {
                TabNum = Authorize.userId;
                initializeDB();
            }
        }
    }
}
