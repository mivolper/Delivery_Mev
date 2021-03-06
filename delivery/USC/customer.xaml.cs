﻿using delivery.Windows;
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
    /// Interaction logic for customer.xaml
    /// </summary>
    public partial class customer : UserControl
    {
        SqlDataAdapter da = new SqlDataAdapter();
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable(); DataTable DtCode = new DataTable();
        Button[] btn = new Button[3];
        bool isnew = false;
        UIElement[] txt = new UIElement[6];
        SqlConnection Connection = new SqlConnection();

        void usc_Initialize()
        {
            customer usc = new customer();
            flag.Initialize_uscgrid(usc);
        }
        public customer()
        {
            InitializeComponent();
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
                Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Customer]) AS RowNum FROM [dbo].[Customers] where Exist = 'true'",Connection);
                dgvCustomer.DataContext = Dt;
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
                dgvCustomer.SelectedIndex = -1;
                DtCode = flag.Fill_DataGrid_join("SELECT  isnull(RIGHT('00000' + CONVERT(VARCHAR, SUBSTRING(Max([CustomerCode]), 2, 6) + 1), 6),'000001') FROM [dbo].[Codes] ",flag.SubCon);
                txtCode.Text = Properties.Settings.Default.Brunch + DtCode.Rows[0].ItemArray[0].ToString();
                txtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                grdtxt.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btn, grdbtn, Dt, dgvCustomer.SelectedIndex, txt);
                if (dgvCustomer.SelectedIndex == -1) return;
                txtName.Focus();
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
                Db = new Linq.DbDataContext(Connection);
                Linq.AspNetUser customer = new Linq.AspNetUser();
                Linq.DbDataContext CodeDb = new Linq.DbDataContext(flag.SubCon);
                Linq.Code code = CodeDb.Codes.FirstOrDefault();

                if (!isnew)
                {
                    if (dgvCustomer.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        customer = Db.AspNetUsers.SingleOrDefault(item => item.Exist == true && item.Id == Convert.ToString(Dt.Rows[dgvCustomer.SelectedIndex].ItemArray[0]));
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                customer.Name = txtName.Text;
                customer.Code = txtCode.Text;
                customer.Phone1 = txtPhone1.Text;
                customer.Phone2 = txtPhone2.Text;
                customer.Address = txtAddress.Text;
                //customer.Note = txtNote.Text;
                customer.Exist = true;

                if (isnew)
                {
                    Db.AspNetUsers.InsertOnSubmit(customer);
                    code.CustomerCode = customer.Code;
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
                flag.Dellete("Customers", "ID_Customer", Dt, dgvCustomer,Connection);
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
                if (cmbSerch.SelectedIndex == 0)
                {
                    da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Customer]) AS RowNum FROM [dbo].[Customers] where Exist = 'true' and Code like '%'+'" + txtSerch.Text + "'+'%' ", Connection);


                }
                if (cmbSerch.SelectedIndex == 1)
                {
                    da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Customer]) AS RowNum FROM [dbo].[Customers] where Exist = 'true' and Name like '%'+'" + txtSerch.Text + "'+'%' ", Connection);


                }
                if (cmbSerch.SelectedIndex == 2)
                {
                    da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Customer]) AS RowNum FROM [dbo].[Customers]  where ( Phone1 like '%'+'" + txtSerch.Text + "'+'%' or Phone2 like '%'+'" + txtSerch.Text + "'+'%')and Exist = 'true'  ",Connection);
                }
                da.Fill(Dt);
                dgvCustomer.DataContext = Dt;

            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);


            }
        }

        private void txtName_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                ((TextBox)sender).SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPhone1_PreviewTextInput(object sender, TextCompositionEventArgs e)
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


    }
}
