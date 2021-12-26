﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatOnline.Application.Common.Interfaces
{
    public interface IFileStore
    {
        public string SafeWriteFile(byte[] content, string sourceFileName, string path);
    }
}
