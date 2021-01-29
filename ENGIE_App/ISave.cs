// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.
// Modified by MO and BP 2021-01-27

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted
{
    /// <summary>
    /// Interface for handling file saving across both iOS and Android
    /// </summary>
    public interface ISave
    {
        Task<string> Save(string filename, string contentType, MemoryStream stream);
    }
}
