﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted
{
    public interface ISave
    {
        Task SaveAndView(string filename, string contentType, MemoryStream stream);
    }
}
