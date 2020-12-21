using delivery.Linq;
using delivery.Windows;
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
    /// Interaction logic for Movement.xaml
    /// </summary>
    public partial class Movement : UserControl
    {
        static public Movement frm;
        Flags.flags flag = new Flags.flags();
        DataTable Dt = new DataTable();
        DataTable DtState = new DataTable();
        DataTable DtDelegate = new DataTable();
        DataTable DtCity = new DataTable();
        DataTable DtSearch = new DataTable();
        SqlConnection Connection = new SqlConnection();

        public Movement()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                frm = this;

                if (MainWindow.frm.offline)
                {
                    Connection = flag.SubCon;
                }
                else
                {
                    Connection = flag.Con;
                }

                DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
                flag.Fill_ComboBox(DtState, cmbState, 1);

                Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Province]) AS RowNum FROM [dbo].[Provinces] where Exist = 'true'");
                flag.Fill_ComboBox(Dt, cmbProvince, 1);

                DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'", flag.SubCon);
                dgvDelegate.DataContext = DtDelegate;

                DtCity = flag.Fill_DataGrid_join("Select  CityName FROM[dbo].[Cities] where Exist = 'true'");
                dgvCity.DataContext = DtCity;

                txt_from_date.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
                txt_to_date.SelectedDate = txt_from_date.SelectedDate.Value.AddMonths(1).AddDays(-1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCity_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                txtCity.SelectAll();
                if (dgvCity.Visibility == Visibility.Visible)
                {
                    dgvCity.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgvCity.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DtCity.Clear();
                DtCity = flag.Fill_DataGrid_join("Select CityName FROM [dbo].[Cities] where Exist = 'true' and CityName like '%'+ N'" + txtCity.Text + "' +'%'");
                dgvCity.DataContext = DtCity;
                if (DtCity.Rows.Count > 0)
                {
                    dgvCity.Visibility = Visibility.Visible;

                }
                if (txtCity.Text == string.Empty)
                {
                    DtCity = flag.Fill_DataGrid_join("Select CityName FROM [dbo].[Cities] where Exist = 'true'");
                    dgvCity.DataContext = DtCity;

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dgvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtCity.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[0].ToString();
                dgvCity.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

        private void txtDelegate_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                txtDelegate.SelectAll();
                if (dgvDelegate.Visibility == Visibility.Visible)
                {
                    dgvDelegate.Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgvDelegate.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtDelegate_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DtDelegate.Clear();
                DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM [dbo].[Delegates] where Exist = 'true' and Name like '%'+ '" + txtDelegate.Text + "' +'%'",flag.SubCon);
                dgvDelegate.DataContext = DtDelegate;
                if (DtDelegate.Rows.Count > 0)
                {
                    dgvDelegate.Visibility = Visibility.Visible;

                }
                if (txtDelegate.Text == string.Empty)
                {
                    DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'",flag.SubCon);
                    dgvDelegate.DataContext = DtDelegate;

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dgvDelegate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtDelegate.Text = DtDelegate.Rows[dgvDelegate.SelectedIndex].ItemArray[0].ToString();
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbState.IsEnabled = (bool)cbxState.IsChecked;
                cmbState.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxDelegate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDelegate.IsEnabled = (bool)cbxDelegate.IsChecked;
                txtDelegate.Clear();
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxProvince_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbProvince.IsEnabled = (bool)cbxProvince.IsChecked;
                cmbProvince.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtCity.IsEnabled = (bool)cbxCity.IsChecked;
                txtCity.Text = "";
                dgvCity.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txt_from_date.IsEnabled = (bool)cbxDate.IsChecked;
                txt_to_date.IsEnabled = (bool)cbxDate.IsChecked;
                txt_from_date.Text = "";
                txt_to_date.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                bool Exist = true;
                if (((Button)sender).Name == "btn_Search") Exist = false;
                decimal sumDelivery = 0;
                decimal sumPackage = 0;
                string select;
                select = "SELECT Orders.ID_Order, [Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],Orders.[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date] ,Orders.[Note],[User],[City],[Delegate], Province,FORMAT (Orders.[DateState], 'yyyy-MM-dd') as [DateState] , ROW_NUMBER() OVER(ORDER BY[date]) AS RowNum,('" + Exist + "') as Exist,isnull((select Name from AspNetUsers where Code = Orders.Customer),'') as Name FROM [dbo].Orders   where Orders.Exist = 'true'";
                DtSearch.Clear();

                if (cbxDate.IsChecked == true && txt_from_date.Text!=string.Empty && txt_to_date.Text != string.Empty)
                {
                    DtSearch = flag.Fill_DataGrid_join(select + "  and State like '%'+N'" + cmbState.Text + "'+'%' and Delegate like '%'+N'" + txtDelegate.Text + "'+'%' and City like '%'+N'" + txtCity.Text + "'+'%' and Province like '%'+N'" + cmbProvince.Text + "'+'%' and Barcode like '%'+N'" + txtBarcode.Text + "'+'%' and Customer like '%'+N'" + txtCode.Text + "'+'%' and DateState Between '" + txt_from_date.Text + "' and '" + txt_to_date.Text + "'", Connection);
                }
                else
                {
                    DtSearch = flag.Fill_DataGrid_join(select + "  and State like '%'+N'" + cmbState.Text + "'+'%' and Delegate like '%'+N'" + txtDelegate.Text + "'+'%' and City like '%'+N'" + txtCity.Text + "'+'%' and Province like '%'+N'" + cmbProvince.Text + "'+'%' and Barcode like '%'+N'" + txtBarcode.Text + "'+'%' and Customer like '%'+N'" + txtCode.Text + "'+'%' ", Connection);
                }

                dgvMovement.DataContext = DtSearch;
                for (int i = 0; i < DtSearch.Rows.Count; i++)
                {
                    sumDelivery += Convert.ToDecimal(DtSearch.Rows[i].ItemArray[10]) ;

                }
                for (int i = 0; i < DtSearch.Rows.Count; i++)
                {
                    sumPackage += Convert.ToDecimal(DtSearch.Rows[i].ItemArray[8]);

                }

                txtTotalOrder.Text = DtSearch.Rows.Count.ToString();
                txtTotalDelivery.Text = sumDelivery.ToString();
                txtTotalPackage.Text = sumPackage.ToString();

                if (MainWindow.frm.offline && Dt.Rows.Count > 0)
                {
                    btnPublish.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxBarcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtBarcode.IsEnabled = (bool)cbxBarcode.IsChecked;
                txtBarcode.Text = "";
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtCode.IsEnabled = (bool)cbxCode.IsChecked;
                txtCode.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StateEdit edit = new StateEdit();
                
                flag.Create_Columns(edit.Dt, "BarCode", "RecipientPhone1", "RecipientPhone2", "TotalPrice","City","DateState","RowNum","Name");
                for (int i = 0; i < dgvMovement.Items.Count ; i++)
                {

                    if (dgvMovement.Columns[0].GetCellContent(dgvMovement.Items[i]).ToString() == "System.Windows.Controls.CheckBox Content: IsChecked:True")
                    {
                        edit.Dt.Rows.Add(DtSearch.Rows[i].ItemArray[1], DtSearch.Rows[i].ItemArray[5], DtSearch.Rows[i].ItemArray[6], DtSearch.Rows[i].ItemArray[11], DtSearch.Rows[i].ItemArray[16], DtSearch.Rows[i].ItemArray[19], DtSearch.Rows[i].ItemArray[20], DtSearch.Rows[i].ItemArray[22]);
                    }
                }
                grdEdit.Children.Clear();
                grdEdit.Children.Add(edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvMovement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
          
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [95.216.93.102].[Delivery_Mev_Db].[dbo].[Orders] ([Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[City],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[Delegate],[State],[Date],[User],[Exist],[Note],[DateState],[Province])    SELECT [Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[City],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[Delegate],[State],[Date],[User],[Exist],Note,DateState ,Province FROM [dbo].[Orders] where [Barcode] COLLATE DATABASE_DEFAULT not in (select [Barcode] COLLATE DATABASE_DEFAULT from [95.216.93.102].[Delivery_Mev_Db].[dbo].[Orders])", flag.SubCon) { CommandTimeout = 1200 }; ;
                flag.SubCon.Open();
                cmd.ExecuteNonQuery();
                flag.SubCon.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
