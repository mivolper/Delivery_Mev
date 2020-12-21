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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace delivery.SUBUSC
{
    /// <summary>
    /// Interaction logic for CustomerCode.xaml
    /// </summary>
    public partial class CustomerCode : UserControl
    {
        bool selectindx = false;
        Flags.flags flag = new Flags.flags();
        //System.Data.Linq.DataContext Db;
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable();
        DataTable DtCustomer = new DataTable();
        DataTable DtCity = new DataTable();

        public CustomerCode()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("SELECT [ID_SubOrder],[CustomerCode],[Recipient],[RecipientPhone1],[RecipientPhone2],[CustomerOrders].[Address],CityName,[PackagePrice],[PackageNumber],[CustomerOrders].[ID_City],PriceMen,ProvinceName,ROW_NUMBER() OVER(ORDER BY[ID_SubOrder]) AS RowNum  FROM [dbo].[CustomerOrders] inner join AspNetUsers on AspNetUsers.Code = CustomerOrders.CustomerCode inner join Cities on Cities.ID_City=CustomerOrders.ID_City inner join Provinces on Provinces.ID_Province = Cities.ID_Province where CustomerCode = '" + Flags.StaticFlag.CustomerCode + "'");
                dgvCustomerOrder.DataContext = Dt;

                DtCustomer = flag.Fill_DataGrid_join("Select Name,Phone1,Phone2 FROM [dbo].[AspNetUsers]  where Code = '" + Flags.StaticFlag.CustomerCode + "'");

                txtCode.Text = Flags.StaticFlag.CustomerCode;
                txtName.Text = DtCustomer.Rows[0].ItemArray[0].ToString();
                txtPhone1.Text = DtCustomer.Rows[0].ItemArray[1].ToString();
                txtPhone2.Text = DtCustomer.Rows[0].ItemArray[2].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCustomerOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvCustomerOrder.SelectedIndex != -1) btnAdd.IsEnabled = true;

            else btnAdd.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Flags.StaticFlag.Order.txtCustomerPhone.Text = txtPhone1.Text;
                Flags.StaticFlag.Order.txtRecipient.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[2].ToString();
                Flags.StaticFlag.Order.txtRecipientPhone1.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[3].ToString();
                Flags.StaticFlag.Order.txtRecipientPhone2.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[4].ToString();
                Flags.StaticFlag.Order.txtAddress.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[5].ToString();
                Flags.StaticFlag.Order.txtCity.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[6].ToString();
                Flags.StaticFlag.Order.txtPackagePrice.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[7].ToString();
                Flags.StaticFlag.Order.txtPackageNumber.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[8].ToString();
                Flags.StaticFlag.Order.txtDeliveryPrice.Text = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[10].ToString();
                Flags.StaticFlag.Order.provine = Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[11].ToString();
                Flags.StaticFlag.Order.selectindx = true;
                //DtCity = flag.Fill_DataGrid_join("Select CityName,ProvinceName FROM [dbo].[Cities] inner join Provinces on Provinces.ID_Province=Cities.ID_Province  where ID_City = '" + Dt.Rows[dgvCustomerOrder.SelectedIndex].ItemArray[6] + "'");

                this.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
