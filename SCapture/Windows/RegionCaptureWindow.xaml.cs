using Hardcodet.Wpf.TaskbarNotification;
using SCapture.Classes;
using SCapture.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCapture.Windows
{
    /// <summary>
    /// Interaction logic for RegionCaptureWindow.xaml
    /// </summary>
    public partial class RegionCaptureWindow : Window
    {
        public static TaskbarIcon notification = new TaskbarIcon();
        /// <summary>
        /// The start position of the Region Rectangle
        /// </summary>
        private Point Start;

        /// <summary>
        /// The end position of the Region Rectangle
        /// </summary>
        private Point Current;

        /// <summary>
        /// Determines weather the user is drawing region or not
        /// </summary>
        private bool isDrawing = false;

        /// <summary>
        /// Rectangle
        /// </summary>
        private double X, Y, W, H;



        public RegionCaptureWindow()
        {
            InitializeComponent();
        }

        #region Functions
        private Rectangle _rect;
        private void Grid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;

            Start = Mouse.GetPosition(Canvas1);

            _rect = new Rectangle
            {
                Fill = FillColor
            };

            Mouse.Capture(Canvas1);

            Canvas.SetLeft(_rect, Start.X);
            Canvas.SetTop(_rect, Start.Y);
            Canvas1.Children.Add(_rect);
        }

        private void Grid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            isDrawing = false;

            // Calculate rectangle cords/size
            BitmapSource bSource = ScreenCapturer.CaptureRegion((int)X, (int)Y, (int)W, (int)H);



            if (Settings.Default.AlwaysCopyToClipboard)
                Clipboard.SetImage(bSource);

            else
            {
                if (ScreenCapturer.Save(bSource))
                    notification.ShowBalloonTip(this.Title, "File saved!", BalloonIcon.Info);
                else
                    MessageBox.Show("Oops! We couldn't save this file. Please check permissions.");
            }

            this.Close();
        }

        private void Grid1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // Get new position
                Current = Mouse.GetPosition(Canvas1);

                // Calculate rectangle cords/size
                X = Math.Min(Current.X, Start.X);
                Y = Math.Min(Current.Y, Start.Y);
                W = Math.Max(Current.X, Start.X) - X;
                H = Math.Max(Current.Y, Start.Y) - Y;

                Canvas.SetLeft(_rect, X);
                Canvas.SetTop(_rect, Y);

                // Update rectangle
                _rect.Width = W;
                _rect.Height = H;
                _rect.SetValue(Canvas.LeftProperty, X);
                _rect.SetValue(Canvas.TopProperty, Y);

                // Toogle visibility
                if (_rect.Visibility != Visibility.Visible)
                    _rect.Visibility = Visibility.Visible;
            }
        }

        private Brush _fillColor;
        Brush FillColor
        {
            get { return _fillColor ?? (_fillColor = new SolidColorBrush(Color.FromArgb(125, 0, 0, 125))); }
        }
        #endregion
    }
}
