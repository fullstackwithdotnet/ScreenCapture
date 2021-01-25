# SCapture
Simple screen capture application created with C# and WPF

## Getting Started
Import SCapture library to your apppication from references.

Call Capture() method in ScreenCapture class in your application's any button click.

	private void button_Click(object sender, RoutedEventArgs e)
        {
            SCapture.ScreenCapture.Capture();

        }

##Change Settings
Set ScreenShot save path. 

Go to Properties > Settings file.
Change your destination

Set ScreenShot save file type.
You can choose 
.bmp - 0
.jpeg -1
.png - 2 

as yout file type. 

Set the desired value. 
