using delivery.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : UserControl
    {
    
        SqlDataAdapter da = new SqlDataAdapter();
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        SqlConnection Connection = new SqlConnection();

        DataTable Dt = new DataTable();
        DataTable DtDelegate = new DataTable();
        DataTable DtCity = new DataTable();
        DataTable DtState = new DataTable();
        DataTable DtCode = new DataTable();

        Button[] btn = new Button[4];
        UIElement[] txt = new UIElement[14];

        bool isnew = false;
        public bool selectindx = false;
        string sql;
        public string provine = "";

        void usc_Initialize()
        {
            Order usc = new Order();
            flag.Initialize_uscgrid(usc);
        }

        public Order()
        {
            try
            {    
               InitializeComponent();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.frm.offline)
                {
                    Connection = flag.SubCon;
                }
                else
                {
                    Connection = flag.Con;
                }
                Dt = flag.Fill_DataGrid_join("SELECT Orders.ID_Order, [Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],Orders.[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date] ,Orders.[Note],[User],[City],[Delegate], Province,FORMAT (Orders.[DateState], 'yyyy-MM-dd') as [DateState] , ROW_NUMBER() OVER(ORDER BY[date]) AS RowNum,isnull((select Name from AspNetUsers where Code = Orders.Customer),'') as Name FROM [dbo].Orders    where Orders.Exist = 'true' and Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'", Connection);
                dgvOrder.DataContext = Dt;

                DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen,ProvinceName FROM [dbo].[Cities] where Cities.Exist = 'true' and Brunsh ='" + Properties.Settings.Default.Brunch + "'", Connection);
                dgvCity.DataContext = DtCity;

                DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'", flag.SubCon);
                dgvDelegate.DataContext = DtDelegate;

                DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
                flag.Fill_ComboBox(DtState, cmbState, 1);

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
                isnew = true;
                dgvOrder.SelectedIndex = -1;
                btnPrint.IsEnabled = true;
                txtCity.IsEnabled = true;
                txtDelegate.IsEnabled = true;
                btn_Serch.IsEnabled = true;
                DtCode = flag.Fill_DataGrid_join("SELECT  isnull(RIGHT('00000' + CONVERT(VARCHAR, SUBSTRING(Max([Barcode]), 2, 6) + 1), 6),'000001') FROM [dbo].[Codes] ",flag.SubCon);
                txtBarcode.Text = Properties.Settings.Default.Brunch + DtCode.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btn, grdbtn, Dt, dgvOrder.SelectedIndex, txt);
                if (dgvOrder.SelectedIndex == -1) return;
                isnew = false;
                btnPrint.IsEnabled = true;
                txtCity.IsEnabled = true;
                txtDelegate.IsEnabled = true;
                txtCity.Clear();
                txtDelegate.Clear();
                txtCity.Text = Dt.Rows[dgvOrder.SelectedIndex].ItemArray[16].ToString();
                txtDelegate.Text = Dt.Rows[dgvOrder.SelectedIndex].ItemArray[17].ToString();
                dgvCity.Visibility = Visibility.Collapsed;
                dgvDelegate.Visibility = Visibility.Collapsed;
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
                string note = txtNote.Text;
                if (MainWindow.frm.offline)
                {
                    Db = new Linq.DbDataContext(flag.SubCon);
                    note += " لم يتم الرفع";
                }
                else
                {
                    Db = new Linq.DbDataContext(flag.Con);
                }
                Linq.Order order = new Linq.Order();
                Linq.DbDataContext CodeDb = new Linq.DbDataContext(flag.SubCon);
                Linq.Code code = CodeDb.Codes.FirstOrDefault();

                if (!isnew)
                {
                    if (dgvOrder.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        order = Db.Orders.SingleOrDefault(item => item.Exist == true && item.ID_Order == Convert.ToInt32(Dt.Rows[dgvOrder.SelectedIndex].ItemArray[0]));
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                order.Barcode = txtBarcode.Text;
                order.Customer = txtCustomer.Text;
                order.CustomerPhone = txtCustomerPhone.Text;
                order.Recipient = txtRecipient.Text;
                order.RecipientPhone1 = txtRecipientPhone1.Text;
                order.RecipientPhone2 = txtRecipientPhone2.Text;
                order.Address = txtAddress.Text;
                order.City = txtCity.Text;
                order.PackagePrice = Convert.ToDecimal(txtPackagePrice.Text);
                order.PackageNumber = Convert.ToInt32(txtPackageNumber.Text);
                order.DeliveryPrice = Convert.ToDecimal(txtDeliveryPrice.Text);
                order.TotalPrice = Convert.ToDecimal(txtTotalPrice.Text);
                order.Delegate = txtDelegate.Text;
                order.State = cmbState.Text;
                order.Date = Convert.ToDateTime(txtDate.Text);
                order.DateState= Convert.ToDateTime(txtDate.Text);
                order.Note = note;
                order.Exist = true;
                order.User = MainWindow.frm.user;

                if (selectindx == true)
                {
                    order.Province = provine;
                    selectindx = false;
                }
                if (isnew)
                {
                    Db.Orders.InsertOnSubmit(order);
                    code.Barcode = order.Barcode;
                }

                Db.SubmitChanges();
                CodeDb.SubmitChanges();

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
                flag.Dellete("Orders", "ID_Order", Dt, dgvOrder,Connection);
                usc_Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //flag.Barcode_Maker(txtBarcode.Text);
                CReport.Bill bill = new CReport.Bill();
                bill.SetParameterValue(0, txtCustomer.Text);
                bill.SetParameterValue(1, txtCustomer.Text);
                bill.SetParameterValue(2, txtCustomerPhone.Text);
                bill.SetParameterValue(3, txtPackagePrice.Text);
                bill.SetParameterValue(4, txtRecipient.Text);
                bill.SetParameterValue(5, txtRecipientPhone1.Text);
                bill.SetParameterValue(6, txtRecipientPhone2.Text);
                bill.SetParameterValue(7, txtAddress.Text);
                bill.SetParameterValue(8, ( "*" + txtBarcode.Text + "*"));

                CReport.Report_Wnidow rewindow = new CReport.Report_Wnidow();
                rewindow.ReportVW.ViewerCore.ReportSource = bill;
                rewindow.Show();
                bill.PrintToPrinter(1, false, 0, 0);

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
                if (cmbSerch.SelectedIndex == 0)
                {
                    sql = "Barcode";
                }
                if (cmbSerch.SelectedIndex == 1)
                {
                    sql = "Customer";
                }
                if (cmbSerch.SelectedIndex == 2)
                {
                    sql = "CustomerPhone";
                }
                Dt.Clear();
                da = new SqlDataAdapter("SELECT Orders.ID_Order, [Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date] ,[Note],[users],[HasEdit],[WhoIsEdit],[City],[Delegate], ProvinceName , ROW_NUMBER() OVER(ORDER BY[date]) AS RowNum FROM [dbo].[Cities] inner join Provinces on Provinces.ID_Province = Cities.ID_Province  inner join Orders on Orders.ID_City = Cities.ID_City   where Orders.Exist = 'true' and " + sql + " like '%'+'" + txtSerch.Text + "'+'%'", Connection);
                da.Fill(Dt);
                dgvOrder.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtBarcode_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                ((TextBox)sender).SelectAll();
                dgvCity.Visibility = Visibility.Collapsed;
                dgvDelegate.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

            }
        }

        private void txtCustomerPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     
        private void txtCity_TextChanged(object sender, TextChangedEventArgs e)
       {
            try
            {        
                if(dgvCity.SelectedIndex == -1)
                {
                    DtCity.Clear();
                    DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen, ProvinceName FROM [dbo].[Cities] where [Cities].Exist = 'true' and CityName like '%'+ N'" + txtCity.Text + "' +'%' and Brunsh ='" + Properties.Settings.Default.Brunch + "'", Connection);
                    dgvCity.DataContext = DtCity;
                    if (DtCity.Rows.Count > 0)
                    {
                        dgvCity.Visibility = Visibility.Visible;

                    }
                    if (txtCity.Text == string.Empty)
                    {

                        dgvCity.Visibility = Visibility.Visible;
                        DtCity = flag.Fill_DataGrid_join("Select CityName,PriceMen, ProvinceName FROM [dbo].[Cities]  where [Cities].Exist = 'true' and Brunsh ='" + Properties.Settings.Default.Brunch + "'", Connection);
                        dgvCity.DataContext = DtCity;
                    }
                }
            }
            catch (Exception ex)
            {

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
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgvCity.SelectedIndex == -1) return;
                txtCity.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[0].ToString();
                txtDeliveryPrice.Text = DtCity.Rows[dgvCity.SelectedIndex].ItemArray[1].ToString();
                provine = Convert.ToString(DtCity.Rows[dgvCity.SelectedIndex].ItemArray[2]);
                dgvCity.Visibility = Visibility.Collapsed;
                selectindx = true;

                dgvCity.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
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

            }
        }
   
        private void txtPackagePrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal packageprice = 0, deliveryprice = 0;
                if (!decimal.TryParse(txtPackagePrice.Text, out packageprice)) packageprice = 0;
                if (!decimal.TryParse(txtDeliveryPrice.Text, out deliveryprice)) deliveryprice = 0;
                txtTotalPrice.Text = (packageprice + deliveryprice).ToString();
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
                    DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'",Connection);
                    dgvDelegate.DataContext = DtDelegate;
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
                    DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'", flag.SubCon);
                    dgvDelegate.DataContext = DtDelegate;

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void btnPrintBarcode_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DataTable ReDT = flag.Fill_DataGrid_join("Select  ( '*' + Barcode + '*' ) as Barcode  FROM [dbo].[Orders] where Barcode = 'A000002' ",Connection);

                //flag.Barcode_Maker(txtBarcode.Text);
                CReport.Barcode bill = new CReport.Barcode();
                bill.SetDataSource(ReDT);
                //bill.SetParameterValue(0, ReDT);
                CReport.Report_Wnidow rewindow = new CReport.Report_Wnidow();
                rewindow.ReportVW.ViewerCore.ReportSource = bill;
                rewindow.Show();
                bill.PrintToPrinter(1, false, 0, 0);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Serch_Click(object sender, RoutedEventArgs e)
        {
            SUBUSC.CustomerCode code = new SUBUSC.CustomerCode();
            Flags.StaticFlag.CustomerCode = txtCustomer.Text;
            Flags.StaticFlag.Order = this;

            GrdCustomerCode.Children.Clear();
            GrdCustomerCode.Children.Add(code);
        }

        private void txtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("SELECT Orders.ID_Order, [Barcode],[Customer],[CustomerPhone],[Recipient],[RecipientPhone1],[RecipientPhone2],Orders.[Address],[PackagePrice],[PackageNumber],[DeliveryPrice],[TotalPrice],[State],FORMAT (Orders.[Date], 'yyyy-MM-dd') as [Date] ,Orders.[Note],[User],[City],[Delegate], Province,FORMAT (Orders.[DateState], 'yyyy-MM-dd') as [DateState] , ROW_NUMBER() OVER(ORDER BY[date]) AS RowNum,isnull((select Name from AspNetUsers where Code = Orders.Customer),'') as Name FROM [dbo].Orders    where Orders.Exist = 'true' and Date ='" + txtDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'", Connection);
                dgvOrder.DataContext = Dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
