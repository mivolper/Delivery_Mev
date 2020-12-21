using System;
using System.Collections.Generic;
using System.Data;
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

namespace delivery.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Flags.flags flag = new Flags.flags();
        DataTable Dt_users = new DataTable();
        DataTable Dt = new DataTable();

        public Login()
        {
            try
            {
                InitializeComponent();
                flag.Con.Open();
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show("خطاء في الاتصال بشبكة الانترنت");
                cbxOffline.IsEnabled = true;
                txt_Name.IsEnabled = false;
                txt_pass.IsEnabled = false;
            }
            try
            {
                flag.SubCon.Open();
            }
            catch (Exception ex)
            {
                flag.SubCon.Close();
                MessageBox.Show("خطاء في الاتصال بقاعدة البيانات");
                SUBUSC.Admin admin = new SUBUSC.Admin();
                admin.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Dt = flag.Fill_DataGrid_join("SELECT [ID_User] ,[Name],[pass],[OrderPrm],[MovementPrm],[CitieAndProvincePrm],[DelegatePrm],[CustomerPrm],[AddMovementNewPrm],[BtnDelete],[UsersPrm],[SittingsPrm],[Exist],ROW_NUMBER() OVER(ORDER BY[ID_User]) AS RowNum FROM [dbo].[Users] where Exist = 'true' and Name = '" + txt_Name.Text + "' and pass ='" + txt_pass.Password + "'",flag.SubCon);


                if (Dt.Rows.Count > 0)
                {
                    MainWindow frm = new MainWindow();
                    frm.btn_order.IsEnabled = (bool)Dt.Rows[0].ItemArray[3];
                    frm.btn_Movement.IsEnabled = (bool)Dt.Rows[0].ItemArray[4];
                    frm.btn_city.IsEnabled = (bool)Dt.Rows[0].ItemArray[5];
                    frm.btn_delegate.IsEnabled = (bool)Dt.Rows[0].ItemArray[6];
                    frm.btnCustomer.IsEnabled = (bool)Dt.Rows[0].ItemArray[7];
                    frm.btn_BranchsAndMovements.IsEnabled = (bool)Dt.Rows[0].ItemArray[8];
                    frm.btn_users.IsEnabled = (bool)Dt.Rows[0].ItemArray[10];
                    frm.btn_set.IsEnabled = (bool)Dt.Rows[0].ItemArray[11];
                    Properties.Settings.Default.delete = (bool)Dt.Rows[0].ItemArray[9];
                    frm.offline = (bool)cbxOffline.IsChecked;
                    frm.user = txt_Name.Text;
                    Close();
                    frm.ShowDialog();
                  
                }
                else
                {
                    MessageBox.Show(" كلمة المرور غير صحيحة");
                    txt_pass.Clear();
                    txt_pass.Focus();
                }
            }
            catch (Exception ex)
            {
                flag.SubCon.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dgvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txt_Name.Text = Dt_users.Rows[dgvUsers.SelectedIndex].ItemArray[0].ToString();
                dgvUsers.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

        private void txt_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Dt_users.Clear();
                Dt_users = flag.Fill_DataGrid_join("Select  Name FROM [dbo].[Users] where Exist = 'true' and Name like '%'+ '" + txt_Name.Text + "' +'%'",flag.SubCon);
                dgvUsers.DataContext = Dt_users;
                if (Dt_users.Rows.Count > 0)
                {
                    dgvUsers.Visibility = Visibility.Visible;

                }
                if (txt_Name.Text == string.Empty)
                {
                    Dt_users = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Users] where Exist = 'true'",flag.SubCon);
                    dgvUsers.DataContext = Dt_users;

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void txt_Name_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                txt_Name.SelectAll();
                if (dgvUsers.Visibility == Visibility.Visible)
                {
                    dgvUsers.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgvUsers.Visibility = Visibility.Visible;
                    Dt_users = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Users] where Exist = 'true'",flag.SubCon);
                    dgvUsers.DataContext = Dt_users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag.Con.Open();
                cbxOffline.IsEnabled = false;
                cbxOffline.IsChecked = false;
                txt_Name.IsEnabled = true;
                txt_pass.IsEnabled = true;
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show("خطاء في الاتصال بشبكة الانترنت");
                cbxOffline.IsEnabled = true;
                cbxOffline.IsChecked = false;
                txt_Name.IsEnabled = false;
                txt_pass.IsEnabled = false;
            }
        }

        private void cbxOffline_Click(object sender, RoutedEventArgs e)
        {
           txt_Name.IsEnabled = txt_pass.IsEnabled= (bool)((CheckBox)sender).IsChecked;
        }
    }
}
