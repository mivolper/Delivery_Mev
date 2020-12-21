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
    /// Interaction logic for add.xaml
    /// </summary>
    public partial class add : UserControl
    {
        public add()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {

            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (txtUserName.Text == "eslam" && txtPassWard.Password == "12341234")

                {
                   USC.setting.frm.full.IsEnabled = true;
                   USC.setting.frm.txtServer.IsEnabled = true;
                   USC.setting.frm.txtDataBase.IsEnabled = true;
                   USC.setting.frm.txtUser.IsEnabled = true;
                   USC.setting.frm.txtPassWoard.IsEnabled = true;
                   USC.setting.frm.txt_path.IsEnabled = true;
                   USC.setting.frm.btnSave.IsEnabled = true;
                   USC.setting.frm.txtBrunch.IsEnabled = true;
                   USC.setting.frm.txtServer.Text = Properties.Settings.Default.ServerName;
                   USC.setting.frm.txtDataBase.Text = Properties.Settings.Default.DataBase;
                   USC.setting.frm.txtUser.Text = Properties.Settings.Default.User;
                   USC.setting.frm.txtPassWoard.Text = Properties.Settings.Default.PassWord;
                   USC.setting.frm.txtBrunch.Text = Properties.Settings.Default.Brunch;
                   USC.setting.frm.hd.Children.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
             USC.setting.frm.hd.Children.Clear();
             USC.setting.frm.use_setting.IsChecked = false;

        }
    }
}
