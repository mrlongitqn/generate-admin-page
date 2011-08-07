using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class BaseGenerate
    {
        public string END = Environment.NewLine;
        public string TAB = "\t";
        public string TAB2 = "\t\t";
        public string TAB3 = "\t\t\t";
        public string TAB4 = "\t\t\t\t";
        public string TAB5 = "\t\t\t\t\t";
        public string TAB6 = "\t\t\t\t\t\t";
        public string TAB7 = "\t\t\t\t\t\t\t";
        public string TAB8 = "\t\t\t\t\t\t\t\t";
        public string TAB9 = "\t\t\t\t\t\t\t\t\t";

        public Table _table { get; set; }
        public DataBase _db { get; set; }
        public bool TblHaveImgAttr { get; set; }
        public bool TblUsingFck { get; set; }
    }
}
