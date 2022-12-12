using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace SmartHome
{
    /// <summary>
    /// Interaction logic for House.xaml
    /// </summary>
    public partial class House : Window
    {
        MainWindow mainWindow;
        
        public House(MainWindow mainWindow)
        {
            InitializeComponent();
            
            this.mainWindow = mainWindow;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Show();
           

        }

        private void ToiletRoom_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(ToiletRoom,ToiletRoom,DragDropEffects.Move);
            }
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {

            Border clonedCanvas = Clone<Border>(ToiletRoom);
            //clonedCanvas.Width = ToiletRoom.Width;
            //clonedCanvas.Height = ToiletRoom.Height;
            //set the position of the cloned canvas
            //Point p = e.GetPosition(borderTest);
            //borderTest.Children.Add(clonedCanvas);
            //clonedCanvas.Margin = new Thickness(ToiletRoom.Margin.Left, ToiletRoom.Margin.Top, ToiletRoom.Margin.Right, ToiletRoom.Margin.Bottom);
            //get the position of the mouse and set the position of the cloned canvas
            Point p = e.GetPosition(borderTest);
            clonedCanvas.Width = ToiletRoom.ActualWidth;
            clonedCanvas.Height = ToiletRoom.ActualHeight;
            clonedCanvas.Margin = new Thickness(p.X-190, p.Y-190, 710 - p.X, 480 - p.Y);

            borderTest.Children.Add(clonedCanvas);
        


        //710,490
        //clonedCanvas.Margin = new Thickness { Left = p.X, Top = p.Y, Right = 190 + p.X, Bottom = 190 + p.Y };




    }


        public static T Clone<T>(T source)
        {
            string objXaml = XamlWriter.Save(source);
            StringReader stringReader = new StringReader(objXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            T t = (T)XamlReader.Load(xmlReader);
            return t;
        }

        private void btnProfil_Click(object sender, RoutedEventArgs e)
        {
            profilPage profil = new profilPage();
            profil.Show();

        }

        //public static Canvas CloneXaml(Canvas source)
        //{
        //    string xaml = XamlWriter.Save(source);
        //    StringReader sr = new StringReader(xaml);
        //    XmlReader xr = XmlReader.Create(sr);
        //    return (Canvas)XamlReader.Load(xr);
        //}





    }
}
