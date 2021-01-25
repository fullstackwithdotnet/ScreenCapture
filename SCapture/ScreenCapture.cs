using SCapture.Windows;

namespace SCapture
{
    public static class ScreenCapture
    {
        public static void Capture()
        {
            var xx = new RegionCaptureWindow();
            xx.ShowDialog();


        }
    }
}
