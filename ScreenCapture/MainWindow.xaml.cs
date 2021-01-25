using System.Windows;

namespace ScreenCapture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //MyWindow.WindowState = WindowState.Minimized;
            MyWindow.Visibility = Visibility.Collapsed;
            SCapture.ScreenCapture.Capture();
            MyWindow.Visibility = Visibility.Visible;


        }
    }
}
