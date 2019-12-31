using System;
using System.Collections.Generic;
using System.Text;

namespace Peamel.BasicLogger
{
    public class BasicLoggerEventId
    {
        public BasicLoggerEventId(int? id, String name)
        {
            if (id == null)
                Id = 0;
            else
                Id = id.Value;

            Name = name;
        }

        public int Id { get; }
        public String Name { get; }
    }
}
