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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow frm;
        DataTable Dt = new DataTable();
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        public bool offline = false;
        public string user = "";
        static void frm_Closed(object sender, EventArgs e)
        {
            frm = null;
        }
        public static MainWindow GetMainForm
        {
            get
            {
                if (frm == null)
                {
                    frm = new MainWindow();
                    frm.Closed += new EventHandler(frm_Closed);
                }
                return frm;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (frm == null)
                frm = this;
           
        }

        private void btn_city_Selected(object sender, RoutedEventArgs e)
        {
            USC.city city = new USC.city();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(city);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

       

        private void grdUSC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GridMenu.Width == 260)
                {
                    btnCloseMenu.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnShutDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_Minimized_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn.Name == "btnCloseMenu")
                {
                    btnCloseMenu.Visibility = Visibility.Collapsed;
                    btnOpenMenu.Visibility = Visibility.Visible;

                }
                else
                {
                    btnCloseMenu.Visibility = Visibility.Visible;
                    btnOpenMenu.Visibility = Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCustomer_Selected(object sender, RoutedEventArgs e)
        {
            USC.customer customer = new USC.customer();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(customer);
        }

        private void btn_delegate_Selected(object sender, RoutedEventArgs e)
        {
            USC.@delegate @delegate = new USC.@delegate();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(@delegate);
        }

        private void btn_order_Selected(object sender, RoutedEventArgs e)
        {
            USC.Order order = new USC.Order();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(order);
        }

        private void btn_Movement_Selected(object sender, RoutedEventArgs e)
        {
            USC.Movement movement = new USC.Movement();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(movement);
        }

        private void btn_BranchsAndMovements_Selected(object sender, RoutedEventArgs e)
        {
            USC.BranchsAndMovements branchs = new USC.BranchsAndMovements();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(branchs);
        }

        private void btn_users_Selected(object sender, RoutedEventArgs e)
        {
            USC.users users = new USC.users();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(users);

        }

        private void btn_set_Selected(object sender, RoutedEventArgs e)
        {
            USC.setting setting = new USC.setting();
            grdUSC.Children.Clear();
            grdUSC.Children.Add(setting);
        }
    }
}
