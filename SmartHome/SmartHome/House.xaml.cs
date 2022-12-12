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
        private static Border? draggedItem;
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
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                draggedItem = (Border)sender;
                DragDrop.DoDragDrop(draggedItem,draggedItem,DragDropEffects.Move);
            }
        }
        private void Border_Drop(object sender, DragEventArgs e)
        {
            Border clonedCanvas = Clone<Border>(draggedItem);
            Point p = e.GetPosition(borderTest);
            clonedCanvas.Width = draggedItem.ActualWidth;
            clonedCanvas.Height = draggedItem.ActualHeight;
            Canvas.SetLeft(clonedCanvas, p.X - 40);
            Canvas.SetTop(clonedCanvas, p.Y - 40);
            borderTest.Children.Add(clonedCanvas);
            Cursor = Cursors.Arrow;
        }

        public static T Clone<T>(T source)
        {
            string objXaml = XamlWriter.Save(source);
            StringReader stringReader = new StringReader(objXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            T t = (T)XamlReader.Load(xmlReader);
            return t;
        }
        public Cursor ConvertToCursor(UIElement control, Point hotSpot)
        {
            if (control is null)
            {
                throw new ArgumentNullException(nameof(control));
            }
            //MessageBox.Show("ConvertToCursor");
            // convert FrameworkElement to PNG stream
            var pngStream = new MemoryStream();
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Rect rect = new Rect(0, 0, control.DesiredSize.Width, control.DesiredSize.Height);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);

            control.Arrange(rect);
            rtb.Render(control);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            png.Save(pngStream);

            // write cursor header info
            var cursorStream = new MemoryStream();
            cursorStream.Write(new byte[2] { 0x00, 0x00 }, 0, 2);                               // ICONDIR: Reserved. Must always be 0.
            cursorStream.Write(new byte[2] { 0x02, 0x00 }, 0, 2);                               // ICONDIR: Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image. Other values are invalid
            cursorStream.Write(new byte[2] { 0x01, 0x00 }, 0, 2);                               // ICONDIR: Specifies number of images in the file.
            cursorStream.Write(new byte[1] { (byte)control.DesiredSize.Width }, 0, 1);          // ICONDIRENTRY: Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
            cursorStream.Write(new byte[1] { (byte)control.DesiredSize.Height }, 0, 1);         // ICONDIRENTRY: Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
            cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                                     // ICONDIRENTRY: Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
            cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                                     // ICONDIRENTRY: Reserved. Should be 0.
            cursorStream.Write(new byte[2] { (byte)hotSpot.X, 0x00 }, 0, 2);                    // ICONDIRENTRY: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
            cursorStream.Write(new byte[2] { (byte)hotSpot.Y, 0x00 }, 0, 2);                    // ICONDIRENTRY: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
            cursorStream.Write(new byte[4] {                                                    // ICONDIRENTRY: Specifies the size of the image's data in bytes
                                          (byte)((pngStream.Length & 0x000000FF)),
                                          (byte)((pngStream.Length & 0x0000FF00) >> 0x08),
                                          (byte)((pngStream.Length & 0x00FF0000) >> 0x10),
                                          (byte)((pngStream.Length & 0xFF000000) >> 0x18)
                                       }, 0, 4);
            cursorStream.Write(new byte[4] {                                                    // ICONDIRENTRY: Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
                                          (byte)0x16,
                                          (byte)0x00,
                                          (byte)0x00,
                                          (byte)0x00,
                                       }, 0, 4);

            // copy PNG stream to cursor stream
            pngStream.Seek(0, SeekOrigin.Begin);
            pngStream.CopyTo(cursorStream);

            // return cursor stream
            cursorStream.Seek(0, SeekOrigin.Begin);
            return new Cursor(cursorStream);
        }

        private void ToiletRoom_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Mouse.SetCursor(ConvertToCursor(customPointer, new Point(2, 2)));
            }
            else
                e.UseDefaultCursors = true;

            e.Handled = true;
        }
    }
}
