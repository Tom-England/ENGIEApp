// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.
// Modified by MO and BP 2021-01-27

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using ENGIE_App.iOS;
using UIKit;
using QuickLook;
using GettingStarted;

[assembly: Dependency(typeof(SaveiOS))]

/// <summary>
/// iOS implementation of ISave
/// </summary>
class SaveiOS : ISave
{
    //Method to save document as a file and view the saved document
    public async Task<string> Save(string filename, string contentType, MemoryStream stream)
    {
        //Get the root path in iOS device.
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string filePath = Path.Combine(path, filename);

        //Create a file and write the stream into it.
        FileStream fileStream = File.Open(filePath, FileMode.Create);
        stream.Position = 0;
        stream.CopyTo(fileStream);
        fileStream.Flush();
        fileStream.Close();

        // Modified by BP: return filepath so file can be emailed
        return filePath;
    }
}
