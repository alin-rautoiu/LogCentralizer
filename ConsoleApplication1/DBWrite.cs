﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection
{
    public interface DBWrite
    {
        void Write(LogTable log);
    }
}
