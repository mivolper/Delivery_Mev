using delivery.Windows;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for StateEdit.xaml
    /// </summary>
    public partial class StateEdit : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        public DataTable Dt = new DataTable();
        DataTable DtState = new DataTable();
        DataTable DtDelegate = new DataTable();
        public StateEdit()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DtState = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
                flag.Fill_ComboBox(DtState, cmbState, 1);

                DtDelegate = flag.Fill_DataGrid_join("Select  Name FROM[dbo].[Delegates] where Exist = 'true'", flag.SubCon);
                dgvDelegate.DataContext = DtDelegate;

                txtDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnShutDown_Click(object sender, RoutedEventArgs e)
        {
            Movement.frm.grdEdit.Children.Clear();
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

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            Movement.frm.grdEdit.Children.Clear();
        }

        private void btnEditAndPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnEdit_Click(sender, e);
                CReport.DelegateBill Bill = new CReport.DelegateBill();
                Bill.SetDataSource(Dt);
                Bill.SetParameterValue(0, txtDelegate.Text);
                Bill.SetParameterValue(1, txtDate.SelectedDate.Value.ToString("dddd", new System.Globalization.CultureInfo("ar-AE"))); 
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Db = new Linq.DbDataContext(flag.Con);
                Linq.Order order = new Linq.Order();

                for(int i = 0; i < Dt.Rows.Count; i++)
                {
                    order = Db.Orders.SingleOrDefault(item => item.Exist == true & item.Barcode == Dt.Rows[i].ItemArray[0].ToString());
                    if (cmbState.Text != string.Empty)
                    {
                        order.State = cmbState.Text;
                    }
                    if (txtDelegate.Text != string.Empty)
                    {
                        order.Delegate = txtDelegate.Text;
                    }
                    order.DateState = (DateTime)txtDate.SelectedDate;
                }
                Db.SubmitChanges();
                Movement.frm.grdEdit.Children.Clear();
                MessageBox.Show("تم التعديل");
                Movement usc = new Movement();

                MainWindow.GetMainForm.grdUSC.Children.Clear();
                MainWindow.GetMainForm.grdUSC.Children.Add(usc);
                this.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
