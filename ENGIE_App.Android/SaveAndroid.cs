using System;
using System.IO;
using Android.Content;
using Java.IO;
using Xamarin.Forms;
using System.Threading.Tasks;
using GettingStarted;

///<summary>
/// Implementation of ISave for android
/// </summary>
[assembly: Dependency(typeof(SaveAndroid))]
class SaveAndroid : ISave
{
    //Method to save document as a file in Android and view the saved document
    public async Task<string> Save(string fileName, String contentType, MemoryStream stream)
    {

        // TODO this line is testing whether I was initially right about emulation
        string root = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        //var folderPath = Path.Combine(root, "Syncfusion");
        var filePath = Path.Combine(root, fileName);

        //Modification made by BP: Create a file and write the stream into it (using code from the iOS solution)
        FileStream fileStream = System.IO.File.Open(filePath, FileMode.Create);
        stream.Position = 0;
        stream.CopyTo(fileStream);
        fileStream.Flush();
        fileStream.Close();

        //Modification made by BP: Return filePath so that it can be emailed onwards
        return filePath;

    }
}
