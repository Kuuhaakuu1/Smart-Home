using MySql.Data.MySqlClient;
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

namespace SmartHome
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MySqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
             connection = new MySqlConnection("server=localhost;user id=root;database=library");

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM user where username='" + TxtUsername.Text + "' and password='" + TxtPassword.Password.ToString() + "';", connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {

                    if (TxtUsername.Text == (string)reader[3] && TxtPassword.Password.ToString() == (string)reader[4])
                    {
                        MessageBox.Show("Welcome back " + reader[2] + " " + reader[1], "Congrats", MessageBoxButton.OK, MessageBoxImage.Information);
                        House house = new House(this);
                        house.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
                else
                {
                    MessageBox.Show("Username or Password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
           
            

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                //code for any other type of exception
            }

        }

        private void btxExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
