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
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;


namespace delivery.USC
{
    /// <summary>
    /// Interaction logic for setting.xaml
    /// </summary>
    public partial class setting : UserControl
    {
        Flags.flags flag = new Flags.flags();
        Linq.DbDataContext Db;
        DataTable Dt = new DataTable(); DataTable DtEmployee = new DataTable();
        bool isnew = false;
        VistaFolderBrowserDialog test = new VistaFolderBrowserDialog();
        OpenFileDialog ofd = new OpenFileDialog();

        public static setting frm;
        public setting()
        {
            try
            {
                InitializeComponent();
                frm = this;
            }
            catch(Exception ex)
            {

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ServerName = txtServer.Text;
                Properties.Settings.Default.DataBase = txtDataBase.Text;
                Properties.Settings.Default.User = txtUser.Text;
                Properties.Settings.Default.PassWord = txtPassWoard.Text;
                Properties.Settings.Default.Brunch = txtBrunch.Text;
                Properties.Settings.Default.Save();
                MessageBox.Show("تم الحفظ");
                Windows.MainWindow.GetMainForm.Close();
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void full_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void use_setting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)use_setting.IsChecked)
                {
                    SUBUSC.add add = new SUBUSC.add();
                    hd.Children.Clear();
                    hd.Children.Add(add);
                    
                }
                else
                {
                    txtServer.Clear();
                    txtDataBase.Clear();
                    txtUser.Clear();
                    txtPassWoard.Clear();
                    txtBrunch.Clear();
                    txtServer.IsEnabled = false;
                    txtDataBase.IsEnabled = false;
                    txtUser.IsEnabled = false;
                    txtPassWoard.IsEnabled = false;
                    txtBrunch.IsEnabled = false;
                    btnSave.IsEnabled = false;
                    full.IsEnabled = false;
                    hd.Children.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_path_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (test.ShowDialog() == true)
                {
                    txt_path.Text = test.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_copysave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_path.Text == "" || txt_name.Text == "")
                {
                    MessageBox.Show("يجب اختيار اسم و مسار الملف");
                }
                else
                {
                    flag.BackUp(txt_path.Text, txt_name.Text);
                    MessageBox.Show("تم انشاء النسحة بنجاح");

                }
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void restore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    flag.Restore(ofd.FileName);
                    MessageBox.Show("تم استعادة النسحة بنجاح");
                }
            }
            catch (Exception ex)
            {
                flag.Con.Close();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
