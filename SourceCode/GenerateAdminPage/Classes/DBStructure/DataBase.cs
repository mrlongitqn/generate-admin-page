using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class DataBase
    {
        public List<Table> Tables { get; set; }

        public DataBase()
        {
            Tables = new List<Table>();
        }
    }
}
