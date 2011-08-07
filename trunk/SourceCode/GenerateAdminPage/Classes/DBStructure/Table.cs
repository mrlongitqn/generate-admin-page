using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class Table
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; }

        public Table()
        {
            Name = "";
            Attributes = new List<Attribute>();
        }
    }
}
