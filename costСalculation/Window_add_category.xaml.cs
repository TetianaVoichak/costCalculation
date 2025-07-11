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
    /// Interaction logic for Window_add_category.xaml
    /// </summary>
    public partial class Window_add_category : Window
    {
        List<Category> listCategory = new List<Category>();

        //CATEGORYLIST stores all categories from the database
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

        //button to save a new category
        private void button_save_new_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    string mes = null;
                    Category c = new Category(textBox_new_category.Text);
                    WorkingWithCategories workingWith = new WorkingWithCategories(c);
                    string error = null;
                    if (!workingWith.ValidateCategory(c, out error))//checking for data correctness
                    {
                        MessageBox.Show(error);
                        return;
                    }

                    Data.AddCategory(c, out mes);
                    if (mes != null) MessageBox.Show(mes);
                    else
                    {
                        LoadCategoryInCombobox();
                        textBox_new_category.Text = "";
                        MessageBox.Show("The item has been added successfully.");
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
            try
            {
                if(CustomMessageBox.Show("Do you want to delete the selected category?", "Delete"))
                {
                    WorkingWithCategories workingCateg = new WorkingWithCategories(CATEGORYLIST);
                    bool check = false;
                    workingCateg.ReturnCategoryMethod(comboBoxList.SelectedItem.ToString(), out check);
                    string msg = null;
                    Data.DeleteCategory(workingCateg.ReturnCategory, out msg);
                    if(msg=="")
                    {
                        LoadCategoryInCombobox();
                        MessageBox.Show("Item deleted successfully");    
                    }
                    else
                    {
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(CustomMessageBox.Show("Do you want to edit the selected category?", "Edit"))
                {
                    WorkingWithCategories categories = new WorkingWithCategories(CATEGORYLIST);
                    bool check = true;
                    string error = null;

                    categories.ReturnCategoryMethod(comboBoxList.SelectedItem.ToString(), out check);

                    if (!categories.ValidateTextBoxCategory(textBox_new_category.Text, out error))//checking for data correctness
                    {
                        MessageBox.Show(error);
                        return;
                    }
                    if (categories.CheckCategory(textBox_new_category.Text))
                    {
                        MessageBox.Show("Element exist");
                        return;
                    }
                    string mes = null;

                    Data.EditCategory(categories.ReturnCategory, textBox_new_category.Text, out mes);
                    if (mes == null)
                    {
                        LoadCategoryInCombobox();
                        textBox_new_category.Text = "";
                        MessageBox.Show("The item has been edited successfully.");
                    }
                    else MessageBox.Show(mes);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
