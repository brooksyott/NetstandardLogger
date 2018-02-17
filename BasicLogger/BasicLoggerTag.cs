using System;
using System.Collections.Generic;
using System.Text;

namespace Peamel.BasicLogger
{
    public class BasicLoggerTag
    {
        public BasicLoggerTag(String tag)
        {
            _tagName = tag;
        }

        public BasicLoggerTag()
        {
        }

        private String _tagName = "DEFAULT";
        public String TagName
        {
            get { return _tagName; }
        }
    }
}
