using System;
using System.Collections.Generic;
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

namespace delivery.SUBUSC
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        static public Admin frm;
        public Admin()
        {
            try { 
            InitializeComponent();
            frm = this;

            SUBUSC.addAdmin usc = new SUBUSC.addAdmin();
            grdusc.Children.Clear();
            grdusc.Children.Add(usc);
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}
    }
}
