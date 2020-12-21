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
    /// Interaction logic for addAdmin.xaml
    /// </summary>
    public partial class addAdmin : UserControl
    {
        public addAdmin()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtUserName.Text == "eslam" && txtPassWard.Password == "12341234")

                {
                    //Properties.Settings.Default.IsNew = Dns.GetHostName();
                    USC_RESTORE usc = new USC_RESTORE();
                    SUBUSC.Admin.frm.grdusc.Children.Clear();
                    SUBUSC.Admin.frm.grdusc.Children.Add(usc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SUBUSC.Admin.frm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    }

