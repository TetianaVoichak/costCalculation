using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace costСalculation
{
    /// <summary>
    /// Логика взаимодействия для Window_add_category.xaml
    /// </summary>
    public partial class Window_add_category : Window
    {
        List<Category> listCategory = new List<Category>();
        public List<Category> CATEGORYLIST { get; set; } = Data.GetCategories();
        public Window_add_category()
        {
            InitializeComponent();
            LoadCategoryInCombobox();
        }

       void LoadCategoryInCombobox()
        {
            CATEGORYLIST  = Data.GetCategories();
            comboBoxList.Items.Clear();
            foreach (var a in CATEGORYLIST)
                comboBoxList.Items.Add(a.NameCategory);
            comboBoxList.SelectedIndex = 0;
        }
       
        private void textBox_new_category_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void button_save_new_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string mes = "";
                if (textBox_new_category.Text != "")
                {
                    Category c = new Category(textBox_new_category.Text);
                    Data.AddCategory(c, out mes);
                    if (mes != "") MessageBox.Show(mes);
                    else
                    {
                        LoadCategoryInCombobox();
                        textBox_new_category.Text = "";
                    }
                }
                else 
                {
                    MessageBox.Show("cannot be added"); 
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
          
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindows = this.Owner as MainWindow;
            if (mainWindows != null)
            {
                mainWindows.UpdateInfoInComboboxCategory();
            }
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            Category c = new Category(comboBoxList.SelectedItem.ToString());
            Data.DeleteCategory(c);
            LoadCategoryInCombobox();
            MessageBox.Show("item deleted successfully");
        }
    }
}
