using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for city.xaml
    /// </summary>
    public partial class city : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable(); DataTable DtCity = new DataTable();
        Button[] btn = new Button[3]; Button[] btnCity = new Button[3];
        bool isnew = false;
        UIElement[] txt = new UIElement[4];

        void usc_Initialize()
        {
            city usc = new city();
            flag.Initialize_uscgrid(usc);
        }

        public city()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Province]) AS RowNum FROM [dbo].[Provinces] where Exist = 'true'");
                dgvProvince.DataContext = Dt;

                DtCity = flag.Fill_DataGrid_join("SELECT [ID_City],[CityName],[PriceMen], ProvinceName,[Days],ROW_NUMBER() OVER(ORDER BY[ID_City]) AS RowNum1 FROM [dbo].[Cities] where Cities.Exist = 'true' and Brunsh ='" + Properties.Settings.Default.Brunch + "'");
                dgvCity.DataContext = DtCity;

                flag.Fill_ComboBox(Dt, cmbProvince, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Province
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag.btn_New(btn, grdbtnProvince, txtProvince);
                isnew = true;
                dgvProvince.SelectedIndex = -1;
                txtProvince.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                return;
                flag.grd_SelectionChaneged(btn, grdbtnProvince, Dt, dgvProvince.SelectedIndex, txtProvince);
                if (dgvProvince.SelectedIndex == -1) return;
                isnew = false;
                txtProvince.Focus();
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
                Db = new Linq.DbDataContext(flag.Con);
                Linq.Province province = new Linq.Province();

                if (!isnew)
                {
                    if (dgvProvince.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        province = Db.Provinces.SingleOrDefault(item => item.Exist == true && item.ID_Province == Convert.ToInt32(Dt.Rows[dgvProvince.SelectedIndex].ItemArray[0]));
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                province.ProvinceName = txtProvince.Text;
                province.Exist = true;

                if (isnew)
                {
                    Db.Provinces.InsertOnSubmit(province);
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
                flag.Dellete("Provinces", "ID_Province", Dt, dgvProvince);
                usc_Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProvince_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtSearchProvince_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("SELECT *,ROW_NUMBER() OVER(ORDER BY[ID_Province]) AS RowNum FROM [dbo].[Provinces] where Exist = 'true' and ProvinceName like '%'+ '" + txtSearchProvince.Text + "' +'%'") ;

                dgvProvince.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion


        #region City
        private void btnNewCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdtxtCity.Children.CopyTo(txt, 0);
                flag.btn_New(btnCity, grdbtnCity, txt);
                isnew = true;
                dgvCity.SelectedIndex = -1;
                txtCity.Focus();
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
                grdtxtCity.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btnCity, grdbtnCity, DtCity, dgvCity.SelectedIndex, txt);
                if (dgvCity.SelectedIndex == -1) return;
                txtCity.Focus();
                isnew = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Db = new Linq.DbDataContext(flag.Con);
                Linq.DbDataContext subDb = new Linq.DbDataContext(flag.SubCon);

                Linq.City city = new Linq.City();
                Linq.City subcity = new Linq.City();

                if (!isnew)
                {
                    if (dgvCity.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        city = Db.Cities.SingleOrDefault(item => item.Exist == true && item.ID_City == Convert.ToInt32(DtCity.Rows[dgvCity.SelectedIndex].ItemArray[0]) && item.Brunsh == Properties.Settings.Default.Brunch);
                        subcity = subDb.Cities.SingleOrDefault(item => item.Exist == true && item.CityName == Convert.ToString(DtCity.Rows[dgvCity.SelectedIndex].ItemArray[1]) && item.Brunsh == Properties.Settings.Default.Brunch);

                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                city.CityName = subcity.CityName = txtCity.Text;
                city.PriceMen = subcity.PriceMen = Convert.ToDecimal(txtPriceMen.Text);
                city.ID_Province = subcity.ID_Province = Convert.ToInt32(Dt.Rows[cmbProvince.SelectedIndex].ItemArray[0]) ;
                city.Days = subcity.Days = txtDays.Text;
                city.Exist = subcity.Exist = true;
                city.Brunsh = subcity.Brunsh = Properties.Settings.Default.Brunch;
                city.ProvinceName = subcity.ProvinceName = cmbProvince.Text;

                if (isnew)
                {
                    Db.Cities.InsertOnSubmit(city);
                    subDb.Cities.InsertOnSubmit(subcity);
                }

                Db.SubmitChanges();

                subDb.SubmitChanges();

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

        private void btnDeleteCity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Db = new Linq.DbDataContext(flag.SubCon);
                Linq.City subcity = Db.Cities.SingleOrDefault(item => item.Exist == true && item.CityName == Convert.ToString(DtCity.Rows[dgvCity.SelectedIndex].ItemArray[1]) && item.Brunsh == Properties.Settings.Default.Brunch);
                subcity.Exist = false;
                Db.SubmitChanges();

                flag.Dellete("Cities", "ID_City", DtCity, dgvCity);

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
                DataTable ReDT = flag.Fill_DataGrid_join("SELECT [CityName],[PriceMen], ProvinceName,[Days],ROW_NUMBER() OVER(ORDER BY[ID_City]) AS RowNum1 FROM [dbo].[Cities] inner join Provinces on Provinces.ID_Province = Cities.ID_Province where Cities.Exist = 'true'");

                CReport.Cities Bill = new CReport.Cities();
                Bill.SetDataSource(ReDT);
                CReport.Report_Wnidow rewindow = new CReport.Report_Wnidow();
                rewindow.ReportVW.ViewerCore.ReportSource = Bill;
                rewindow.Show();
                Bill.PrintToPrinter(1, false, 0, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearchCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("SELECT [ID_City],[CityName],[PriceMen], ProvinceName,[Days],ROW_NUMBER() OVER(ORDER BY[ID_City]) AS RowNum1 FROM [dbo].[Cities] inner join Provinces on Provinces.ID_Province = Cities.ID_Province where Cities.Exist = 'true' and CityName like '%'+ '" + txtSearchCity.Text + "' +'%'");

                dgvCity.DataContext = Dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPriceMen_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;

                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}
