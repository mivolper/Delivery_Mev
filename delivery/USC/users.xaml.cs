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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for users.xaml
    /// </summary>
    public partial class users : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable(); DataTable DtEmployee = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        Button[] btn = new Button[3];
        bool isnew = false;
        UIElement[] txt = new UIElement[11];
        void usc_Initialize()
        {
             users usc = new users();
             flag.Initialize_uscgrid(usc);
        }
        public users()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("SELECT [ID_User] ,[Name],[pass],[OrderPrm],[MovementPrm],[CitieAndProvincePrm],[DelegatePrm],[CustomerPrm],[AddMovementNewPrm],[BtnDelete],[UsersPrm],[SittingsPrm],[Exist],ROW_NUMBER() OVER(ORDER BY[ID_User]) AS RowNum FROM [dbo].[Users] where Exist='true' ", flag.SubCon);
                dgvUsers.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.btn_New(btn, grdbtn, txt);
                txt_Name.Focus();
                isnew = true;
                dgvUsers.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btn, grdbtn, Dt, dgvUsers.SelectedIndex, txt);
                if (dgvUsers.SelectedIndex == -1) return;
                txt_Name.Focus();
                isnew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Db = new Linq.DbDataContext(flag.SubCon);
                Linq.User users = new Linq.User();

                if (!isnew)
                {
                    if (dgvUsers.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        users = Db.Users.SingleOrDefault(item => item.Exist == true && item.ID_User == Convert.ToInt32(Dt.Rows[dgvUsers.SelectedIndex].ItemArray[0]));
                        
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                users.Name = txt_Name.Text;
                users.pass = txt_pass.Text;
                users.OrderPrm = cbxOrder.IsChecked;
                users.MovementPrm = cbxMovement.IsChecked;
                users.CitieAndProvincePrm = cbxCitieAndProvince.IsChecked;
                users.DelegatePrm = cbxDelegate.IsChecked;
                users.CustomerPrm = cbxCustomer.IsChecked;
                users.AddMovementNewPrm = cbxAddMovementNewPrm.IsChecked;
                users.BtnDelete = cbxBtnDelete.IsChecked;
                users.UsersPrm= cbxUsers.IsChecked;
                users.SittingsPrm = cbxSettings.IsChecked;
                users.Exist = true;

               
                if (isnew)
                {
                    Db.Users.InsertOnSubmit(users);
                }

                Db.SubmitChanges();

                if (isnew)
                {
                    MessageBox.Show("تم الحفظ");
                }
                usc_Initialize();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag.Dellete("Users", "ID_User", Dt, dgvUsers,flag.SubCon);
                usc_Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSerch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                Dt.Clear();
                da = new SqlDataAdapter("SELECT [ID_User] ,[Name],[pass],[OrderPrm],[MovementPrm],[CitieAndProvincePrm],[DelegatePrm],[CustomerPrm],[AddMovementNewPrm],[BtnDelete],[UsersPrm],[SittingsPrm],[Exist],ROW_NUMBER() OVER(ORDER BY[ID_User]) AS RowNum FROM [dbo].[Users] where Exist='true' and Name like '%'+'" + txtSerch.Text + "'+'%'", flag.Con);
                da.Fill(Dt);
                dgvUsers.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
