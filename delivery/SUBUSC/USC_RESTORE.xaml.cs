using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace delivery.SUBUSC
{
    /// <summary>
    /// Interaction logic for USC_RESTORE.xaml
    /// </summary>
    public partial class USC_RESTORE : UserControl
    {
        OpenFileDialog ofd = new OpenFileDialog();
        Flags.flags flag = new Flags.flags();

        public USC_RESTORE()
        {
            try
            {
                InitializeComponent();
                txtServer.Text = Properties.Settings.Default.ServerName;
                txtDataBase.Text = Properties.Settings.Default.DataBase;
                txtUser.Text = Properties.Settings.Default.User;
                txtPassWoard.Text = Properties.Settings.Default.PassWord;
                txtBrunch.Text = Properties.Settings.Default.Brunch;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnShutDown_Click(object sender, RoutedEventArgs e)
        {
            SUBUSC.Admin.frm.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ServerName = txtServer.Text;
                Properties.Settings.Default.DataBase = txtDataBase.Text;
                Properties.Settings.Default.User = txtUser.Text;
                Properties.Settings.Default.PassWord = txtPassWoard.Text;
                Properties.Settings.Default.Brunch = txtBrunch.Text;
                Properties.Settings.Default.Save();
                MessageBox.Show("تم الحفظ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    flag.Restore(ofd.FileName);
                    MessageBox.Show("تم استعادة النسحة بنجاح");
                }
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show(ex.Message);

            }
        }
    }
}
