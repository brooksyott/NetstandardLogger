using System;
using System.Collections.Generic;
using System.Text;

namespace Peamel.BasicLogger
{
    public interface IBasicLoggerTag
    {
        void SetName(String tag);
        String GetName();
    }
}
