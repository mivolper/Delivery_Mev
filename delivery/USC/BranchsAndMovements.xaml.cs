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

namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for BranchsAndMovements.xaml
    /// </summary>
    public partial class BranchsAndMovements : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable();
        DataTable DtState = new DataTable();
        Button[] btn = new Button[3];
        bool isnew = false;
        UIElement[] txt = new UIElement[1];

        void usc_Initialize()
        {
            BranchsAndMovements  usc = new  BranchsAndMovements();
            flag.Initialize_uscgrid(usc);
        }
        public BranchsAndMovements()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dt = flag.Fill_DataGrid_join("Select *,ROW_NUMBER() OVER(ORDER BY[ID_Movement]) AS RowNum FROM [dbo].[Movements] where Exist = 'true'");
                dgvMovement.DataContext = Dt;

               
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
                flag.btn_New(btn, grdbtnMovement, txtMovement);
                isnew = true;
                dgvMovement.SelectedIndex = -1;
                txtMovement.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvMovement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                flag.grd_SelectionChaneged(btn, grdbtnMovement, Dt, dgvMovement.SelectedIndex, txtMovement);
                if (dgvMovement.SelectedIndex == -1) return;
                isnew = false;
                txtMovement.Focus();
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
                Linq.Movement move = new Linq.Movement();

                if (!isnew)
                {
                    if (dgvMovement.SelectedIndex != -1)
                    {
                        if (MessageBox.Show("هل تريد حفظ التعديلات؟", "تعديل", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                        move = Db.Movements.SingleOrDefault(item => item.Exist == true && item.ID_Movement == Convert.ToInt32(Dt.Rows[dgvMovement.SelectedIndex].ItemArray[0]));
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار عنصر من القائمة");
                        return;
                    }
                }

                move.Name = txtMovement.Text;
                move.Exist = true;

                if (isnew)
                {
                    Db.Movements.InsertOnSubmit(move);
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
                flag.Dellete("Movements", "ID_Movement", Dt, dgvMovement);
                usc_Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtMovement_GotMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                ((TextBox)sender).SelectAll();
            }
            catch (Exception ex)
            {

            }
        }

      
    }
}
