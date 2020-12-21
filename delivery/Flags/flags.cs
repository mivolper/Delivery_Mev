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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using delivery.USC;
using delivery.Properties;
using delivery.Windows;
using ZXing;
using System.IO;

//using PharmaMev.Properties;
namespace delivery.Flags
{
    
    class flags
    {
        public SqlConnection SubCon = new SqlConnection(@"server  =" + Settings.Default.ServerName + "; Database =" + Settings.Default.DataBase +
                                                      "; Integrated Security = false;" + "User ID =" + Settings.Default.User +
                                                      ";PassWord =" + Settings.Default.PassWord + "");

        public SqlConnection Con = new SqlConnection(Properties.Settings.Default.Delivery_MevConnectionString1);

        //public SqlConnection Con = new SqlConnection(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=DeliveryWeb;Integrated Security=True");

        Linq.DbDataContext Db;
        
        SqlDataAdapter da;
        Thickness th = new Thickness(25); Thickness Bth = new Thickness(6);
        BrushConverter bc =new BrushConverter();
        SqlCommand cmd;

        public void Fill_DataGrid(DataGrid Dg,IQueryable Dt)
        {
            Dg.AutoGenerateColumns = false;
            Dg.DataContext = Dt;
        }

        public DataTable Fill_DataGrid_join(string procedur)
        {
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(procedur, Con);
            da.Fill(dt);
            Con.Close();
            return dt;
        }

        public DataTable Fill_DataGrid_join(string procedur,SqlConnection con)
        {
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(procedur, con);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public bool Null_Checker(TextBox txt)
        {
            if (txt.Text == "")
            {
                MessageBox.Show(" !!! لا يمكن ترك هذا العنصر فارغا ");
                txt.Focus();
            }

            return (txt.Text == "") ?  true: false;
        }

        public void Edit_Dt(DataTable dt,int index,params object[] temp)
        {
            dt.Rows[index].BeginEdit();
            dt.Rows[index].ItemArray = temp;
            dt.Rows[index].EndEdit();

        }
        public void Fill_ComboBox(DataTable Dt, ComboBox Cmb,int Dtindex)
        {
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                Cmb.Items.Insert(i, Dt.Rows[i].ItemArray[Dtindex].ToString());
            }
           
        }

        public void Create_Columns(DataTable Dt, params string[] names)
        {
            for (int i = 0; i < names.Length;i++)
            {
                Dt.Columns.Add(names[i]);

            }
        }

        public Button[] Add_Button(DataTable Dt,WrapPanel wrp)
        {
            Button[] btn1 = new Button[Dt.Rows.Count];
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                Button btn = new Button();

                btn.Width = 200;
                btn.Height = 50;
                btn.Margin = th;
                btn.BorderThickness = Bth;
                btn.BorderBrush = Brushes.Black;
                btn.Background = (Brush)bc.ConvertFrom("#FF218280");
                btn.Content = Dt.Rows[i].ItemArray[0].ToString();
                btn.Name = "btn" + i.ToString();
                btn.TabIndex = i;
                btn1[i] = btn;
                wrp.Children.Add(btn);
                if ( i % 3 == 0)
                {
                    wrp.Height += 76;
                }
            }
            return btn1;
        }

        public void BackUp(string path,string filename)
        {
            //string txt = path + "\\" + filename + "  " + DateTime.Now.ToShortDateString().Replace('/', '-') + "  " + DateTime.Now.ToShortTimeString().Replace(':', '-');
            //string txtquery = "Backup DataBase " + "[" + Settings.Default.DataBase + "]" + "  to Disk ='" + txt + ".bak'";
            //cmd = new SqlCommand(txtquery, Con);
            //Con.Open();
            //cmd.ExecuteNonQuery();
            //Con.Close();
        }

        public void Restore(string path)
        {
            //string txtquery = " Alter  DataBase " + "[" + Settings.Default.DataBase + "]" + " set Offline with Rollback immediate; Use master  Restore DataBase " + "[" + Settings.Default.DataBase + "]" + "  from Disk ='" + path + "' WITH REPLACE";
            //cmd = new SqlCommand(txtquery, Con);
            //Con.Open();
            //cmd.ExecuteNonQuery();
            //Con.Close();
        }

        public void Focus_txt(params TextBox[] txt)
        {
            foreach (TextBox t in txt)
            {
                //t.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(MainWindow.GetMainForm.TxtSelected_PreviewMouseLeftButtonUp);
            }
        }

        public bool Copmarer(DataTable Dt,string txt,int index)
        {
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (txt == Dt.Rows[i].ItemArray[index].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        public void btn_New(Button[] btn,Grid grd,params UIElement[] txt)
        {
            grd.Children.CopyTo(btn, 0);
            btn[0].IsEnabled = true;

            for(int i = 1; i < btn.Count(); i++)
            {
                btn[i].IsEnabled = false;
            }

            for (int i=0;i<txt.Count();i++)
            {
                txt[i].IsEnabled = true;

                if ((txt[i] as TextBox) != null)
                {
                    ((TextBox)txt[i]).Clear();
                    continue;
                }
                else if ((txt[i] as ComboBox) != null)
                {
                    ((ComboBox)txt[i]).SelectedIndex = 0;
                    continue;

                }
                else if ((txt[i] as DatePicker) != null)
                {
                    ((DatePicker)txt[i]).SelectedDate = DateTime.Now;
                    continue;
                }
                else if ((txt[i] as CheckBox) != null)
                {
                    ((CheckBox)txt[i]).IsChecked = false;
                    continue;
                }
            }
        }

        public void grd_SelectionChaneged(Button[] btn, Grid grd,DataTable Dt,int dgv_Selectedindex, params UIElement[] txt)
        {
            grd.Children.CopyTo(btn, 0);
            if (dgv_Selectedindex == -1 && !btn[1].IsEnabled)
            {
                btn[0].IsEnabled = true;
                btn[2].IsEnabled = false;
                return;
            }
            else if (dgv_Selectedindex == -1 && btn[1].IsEnabled)
            {
                btn[0].IsEnabled = false;
                btn[2].IsEnabled = false;
                return;
            }
            btn[0].IsEnabled = true;
            btn[1].IsEnabled = true;
            if (Properties.Settings.Default.delete==true) {
                btn[2].IsEnabled = true; 
            }
            //for (int i = 0; i < btn.Count(); i++)
            //{
            //    btn[i].IsEnabled = true;
            //}

            for (int i = 0; i < txt.Count(); i++)
            {
                txt[i].IsEnabled = true;


                if ((txt[i] as TextBox) != null)
                {
                    ((TextBox)txt[i]).Text = Dt.Rows[dgv_Selectedindex].ItemArray[i + 1].ToString();
                    continue;

                }
                else if ((txt[i] as ComboBox) != null)
                {
                    ((ComboBox)txt[i]).Text = Dt.Rows[dgv_Selectedindex].ItemArray[i + 1].ToString();
                    continue;

                }
                else if ((txt[i] as DatePicker) != null)
                {
                    ((DatePicker)txt[i]).Text = Dt.Rows[dgv_Selectedindex].ItemArray[i + 1].ToString();
                    continue;
                }
                else if ((txt[i] as CheckBox) != null)
                {
                    ((CheckBox)txt[i]).IsChecked = Convert.ToBoolean(Dt.Rows[dgv_Selectedindex].ItemArray[i + 1]);
                    continue;
                }

            }
        }

        public void Initialize_uscgrid(UserControl usc)
        {            
            MainWindow.GetMainForm.grdUSC.Children.Clear();
            MainWindow.GetMainForm.grdUSC.Children.Add(usc);
        }

        public void Dellete(string TableName,string IDColumn,DataTable DT_ID,DataGrid dgv)
        {
            if (dgv.SelectedIndex != -1)
            {

                DataTable dt = new DataTable();
                if (MessageBox.Show("هل تريد حذف العنصر المحدد؟", "حذف", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    da = new SqlDataAdapter("Update " + TableName + " Set Exist = 'false' Where " + IDColumn + " = '" + DT_ID.Rows[dgv.SelectedIndex].ItemArray[0].ToString() + "'", Con);
                    da.Fill(dt);
                }
            }
            else
            {
                MessageBox.Show("الرجاء اختيار عنصر من القائمة");
            }
        }

        public void Dellete(string TableName, string IDColumn, DataTable DT_ID, DataGrid dgv,SqlConnection con)
        {
            if (dgv.SelectedIndex != -1)
            {

                DataTable dt = new DataTable();
                if (MessageBox.Show("هل تريد حذف العنصر المحدد؟", "حذف", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    da = new SqlDataAdapter("Update " + TableName + " Set Exist = 'false' Where " + IDColumn + " = '" + DT_ID.Rows[dgv.SelectedIndex].ItemArray[0].ToString() + "'", con);
                    da.Fill(dt);
                }
            }
            else
            {
                MessageBox.Show("الرجاء اختيار عنصر من القائمة");
            }
        }
        public decimal Defrence_Time(DateTime firstTime, DateTime SecondTime,string firstPeriod,string secondPeriod)
        {
            decimal ttx = 0;
            int minute = 0;

            int firsthour = firstTime.Hour, secondhour = SecondTime.Hour;
            if (firstPeriod == "Pm")
            {                  
                firsthour += + 12;
            }
            if (secondPeriod == "Pm")
            {
                secondhour += + 12;
            }

            if (firstTime.Minute > SecondTime.Minute)
            {
                minute =  firstTime.Minute - SecondTime.Minute;
                ttx = Math.Abs(secondhour - firsthour) - 1;
                ttx += Convert.ToDecimal(0.60 - (minute * 0.01));
            }
            else
            {
                minute = SecondTime.Minute - firstTime.Minute;
                ttx = Math.Abs(secondhour - firsthour);
                ttx += Convert.ToDecimal(minute * 0.01);
            }


            return Math.Abs(ttx);
        }

        public void TryParse_double(object sender)
        {
            double ot = 0;
            if (!double.TryParse(((TextBox)sender).Text, out ot))
            {
                if(((TextBox)sender).Text.Length == 1)
                {
                    return;
                }
                ((TextBox)sender).Text = ((TextBox)sender).Text.Remove(((TextBox)sender).Text.Length - 1);
            }
        }

    }
}
