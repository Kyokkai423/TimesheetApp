using MahApps.Metro.Controls;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial  class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            txtLog.Focus(); 
        }
        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string name, string domain, string pass, int logType, int logpv, ref IntPtr pht);
        private void button_Click(object sender, RoutedEventArgs e)
        {
            IntPtr th = IntPtr.Zero;
            Authorize.username = txtLog.Text;
            Authorize.log = LogonUser(txtLog.Text, "SARBAST", txtPass.Password, 2, 0, ref th);
            if (Authorize.log == true)
            {
                LoginWindows wd = new LoginWindows();
                wd.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

    }
}
