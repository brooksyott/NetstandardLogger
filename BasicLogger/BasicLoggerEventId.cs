using System;
using System.Collections.Generic;
using System.Text;

namespace Peamel.BasicLogger
{
    public class BasicLoggerEventId
    {
        public BasicLoggerEventId(int id, String name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public String Name { get; }
    }
}
