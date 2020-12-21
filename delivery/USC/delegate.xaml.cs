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
    /// Interaction logic for @delegate.xaml
    /// </summary>
    public partial class @delegate : UserControl
    {
        SqlDataAdapter da = new SqlDataAdapter();
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        
        DataTable Dt = new DataTable();
        Button[] btn = new Button[3];
        bool isnew = false;
        UIElement[] txt = new UIElement[5];
        void usc_Initialize()
        {
            @delegate usc = new @delegate();
            flag.Initialize_uscgrid(usc);
        }
        public @delegate()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Delegate]) AS RowNum FROM [dbo].[Delegates] where Exist = 'true'",flag.SubCon);
                dgvDelegate.DataContext = Dt;

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
                dgvDelegate.SelectedIndex = -1;
                txtName.Focus();
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
                grdtxt.Children.CopyTo(txt, 0);
                flag.grd_SelectionChaneged(btn, grdbtn, Dt, dgvDelegate.SelectedIndex, txt);
                if (dgvDelegate.SelectedIndex == -1) return;
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
                Db = new Linq.DbDataContext(flag.SubCon);
                Linq.Delegate @delegate = new Linq.Delegate();


                if (!isnew)
                {
                    if (dgvDelegate.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        @delegate = Db.Delegates.SingleOrDefault(item => item.Exist == true && item.ID_Delegate == Convert.ToInt32(Dt.Rows[dgvDelegate.SelectedIndex].ItemArray[0]));
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                @delegate.Name = txtName.Text;
                @delegate.Phone1 = txtPhone1.Text;
                @delegate.Phone2 = txtPhone2.Text;
                if (txtSalary.Text != string.Empty)
                {
                    @delegate.Salary = Convert.ToDecimal(txtSalary.Text);
                  
                }
                else
                {
                    @delegate.Salary  = Convert.ToDecimal(0);
                }
                @delegate.Note = txtNote.Text;
                @delegate.Exist = true;

                if (isnew)
                {
                    Db.Delegates.InsertOnSubmit(@delegate);
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
                flag.Dellete("Delegates", "ID_Delegate", Dt, dgvDelegate,flag.SubCon);
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
                    da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_delegate]) AS RowNum FROM [dbo].[Delegates] where Exist = 'true' and Name like '%'+'" + txtSerch.Text + "'+'%' ", flag.SubCon);
                  
                   
                }
                if (cmbSerch.SelectedIndex == 1)
                {
                    da = new SqlDataAdapter("Select *,ROW_NUMBER() OVER(ORDER BY[ID_delegate]) AS RowNum FROM [dbo].[Delegates] where ( Phone1 like '%'+'" + txtSerch.Text + "'+'%' or Phone2 like '%'+'" + txtSerch.Text + "'+'%')and Exist = 'true'  ", flag.SubCon);
                }
                da.Fill(Dt);
                dgvDelegate.DataContext = Dt;

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

