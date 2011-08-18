using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public static class Utils
    {
        public static string END = Environment.NewLine;

        public static bool IsForeignKeyReferTo(Table _tbl, Table _currTbl)
        {
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsForeignKey)
                {
                    if (_tbl.Attributes[i].ReferTo == _currTbl.Name)
                        return true;
                }
            }
            return false;
        }

        public static string GetImageAttrName(Table _tbl)
        {
            try
            {
                return GlobalVariables.g_colTableHaveImage[_tbl.Name];
            }
            catch
            {
                return "";
            }
        }

        public static bool TableHaveImageAttribute(Table _tbl)
        {
            try
            {
                return GlobalVariables.g_colTableHaveImage.ContainsKey(_tbl.Name);
            }
            catch
            {
                return false;
            }
        }

        public static bool TableUsingFCK(Table _tbl)
        {
            try
            {
                return GlobalVariables.g_colUsingFCK.ContainsKey(_tbl.Name);
            }
            catch
            {
                return false;
            }
        }

        public static bool TableHaveForeignKey(Table _tbl)
        {
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsForeignKey)
                    return true;
            }
            return false;
        }

        public static bool IsPKWithMoreThan1Attr(Table _tbl)
        {
            int count = 0;
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsPrimaryKey)
                {
                    count++;
                    if (count > 1)
                        return true;
                }
            }
            return false;
        }

        public static string BuildWhereClause(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    Result += (" item." + lst[i].Name + " == " + lst[i].Name.ToLower() + " && ");
                }
                else
                {
                    Result += (" item." + lst[i].Name + " == " + lst[i].Name.ToLower() + " ");
                }
            }

            return Result;
        }

        public static string BuildPKWhereClause(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    Result += (" item." + lst[i].Name + " == " + lst[i].Name.ToLower() + " && ");
                }
                else
                {
                    Result += (" item." + lst[i].Name + " == " + lst[i].Name.ToLower() + " ");
                }
            }

            return Result;
        }

        public static List<Attribute> PKHaveMoreThan1Attribute(Table _tbl)
        {
            List<Attribute> KeySameRefer = new List<Attribute>();

            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsPrimaryKey)
                    KeySameRefer.Add(_tbl.Attributes[i]);
            }

            return KeySameRefer;
        }

        public static Attribute GetPK(Table _tbl)
        {
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsPrimaryKey)
                    return _tbl.Attributes[i];
            }

            return null;
        }

        public static string GetPKWith1Attr(Table _tbl)
        {
            string Result = "";
            var keys = PKHaveMoreThan1Attribute(_tbl);
            if (keys.Count == 1)
            {
                Result = keys[0].Name;
            }
            return Result;
        }

        public static string GetPKWith1Attr(DataBase db, string tblName)
        {
            string Result = "";
            for (int i = 0; i < db.Tables.Count; i++)
            {
                if (db.Tables[i].Name.ToUpper() == tblName.ToUpper())
                {
                    var keys = PKHaveMoreThan1Attribute(db.Tables[i]);
                    if (keys.Count == 1)
                    {
                        Result = keys[0].Name;
                    }
                }
            }
            
            return Result;
        }

        public static string BuildFKParams(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                Result += GetDataType(lst[i].Type) + " " + lst[i].Name.ToLower() + ", ";
            }

            return Result;
        }

        public static string BuildPKParams(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                    Result += GetDataType(lst[i].Type) + " " + lst[i].Name.ToLower() + ", ";
                else
                    Result += GetDataType(lst[i].Type) + " " + lst[i].Name.ToLower();
            }

            return Result;
        }

        public static string BuildPKParamsWithoutDataType(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                    Result += lst[i].Name.ToLower() + ", ";
                else
                    Result += lst[i].Name.ToLower();
            }

            return Result;
        }

        public static string BuildCastingPKParams(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    if(lst[i].Type != DataType.STRING)
                        Result += Utils.GetDataType(lst[i].Type) + ".Parse(" + lst[i].Name.ToLower() + "), ";
                    else
                        Result += lst[i].Name.ToLower() + ", ";
                }
                else
                {
                    if (lst[i].Type != DataType.STRING)
                        Result += Utils.GetDataType(lst[i].Type) + ".Parse(" + lst[i].Name.ToLower() + ")";
                    else
                        Result += lst[i].Name.ToLower();
                }
            }

            return Result;
        }

        public static bool HaveFKReferTo(DataBase _db, Table _table)
        {
            for (int i = 0; i < _db.Tables.Count; i++)
            {
                //Consider other table
                if (_db.Tables[i].Name != GlobalVariables.g_ModelName)
                {
                    if (Utils.IsForeignKeyReferTo(_db.Tables[i], _table))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string BuildFKReferTo(DataBase _db, Table _table, string tab)
        {
            string Result = "";
            
            var lst = new List<string>();
            
            for (int i = 0; i < _db.Tables.Count; i++)
            {
                //Consider other table
                if (_db.Tables[i].Name != GlobalVariables.g_ModelName)
                {
                    if (Utils.IsForeignKeyReferTo(_db.Tables[i], _table))
                    {
                        lst.Add(_db.Tables[i].Name);
                    }
                }
            }

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    Result += tab + "Lst" + lst[i] + "ReferObjModel = objItem." + lst[i] + "s.ToList(), " + END; 
                }
                else
                {
                    Result += tab + "Lst" + lst[i] + "ReferObjModel = objItem." + lst[i] + "s.ToList()"; 
                }
            }

            return Result;
        }
        public static string BuildPKParams2(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                    Result += lst[i].Name.ToLower() + ", ";
                else
                    Result += lst[i].Name.ToLower();
            }

            return Result;
        }

        public static string GetDataType(DataType type)
        {
            switch (type)
            {
                case DataType.INT:
                    return "int";

                case DataType.STRING:
                    return "string";

                case DataType.GUILD:
                    return "Guid";

                case DataType.BOOL:
                    return "bool";

                case DataType.DATETIME:
                    return "DateTime";

                case DataType.DOUBLE:
                    return "double";

                case DataType.FLOAT:
                    return "float";

                case DataType.LONG:
                    return "long";
            }

            return "string";
        }

        public static List<Attribute> GetForeighKeyList(Table _tbl)
        {
            var lst = new List<Attribute>();
            for (int i = 0; i < _tbl.Attributes.Count; i++)
            {
                if (_tbl.Attributes[i].IsForeignKey)
                {
                    lst.Add(_tbl.Attributes[i]);
                }
            }
            return lst;
        }

        public static string BuildModelName(DataBase db, Table tbl, string ModelName)
        {
            string Result = "";

            var TwoLastChars = ModelName.Trim().ToUpper().Substring(ModelName.Length - 2);
            var LastChars = ModelName.Trim().ToUpper().Substring(ModelName.Length - 1);
            string modifiedModelName = "";

            if (TwoLastChars.Contains("CE") ||
                TwoLastChars.Contains("CH") ||
                TwoLastChars.Contains("GE") ||
                TwoLastChars.Contains("SH") ||
                TwoLastChars.Contains("SS") ||
                LastChars.Contains("S") ||
                LastChars.Contains("X") ||
                LastChars.Contains("Z"))
            {
                Result = ModelName + "es";
            }
            else if (LastChars.Contains("Y"))
            {
                var beforeY = ModelName.Trim().ToUpper().Substring(ModelName.Length - 2, 1);
                if (beforeY == "E" || beforeY == "U" || beforeY == "O" || beforeY == "A" || beforeY == "I")
                {
                    Result = ModelName + "s";
                }
                else
                {
                    modifiedModelName = ModelName.Substring(0, ModelName.Length - 1);
                    Result = modifiedModelName + "ies";
                }
            }
            else if (LastChars.Contains("O"))
            {
                var beforeY = ModelName.Trim().ToUpper().Substring(ModelName.Length - 2, 1);
                if (beforeY == "E" || beforeY == "U" || beforeY == "O" || beforeY == "A" || beforeY == "I")
                {
                    Result = ModelName + "s";
                }
                else
                {
                    Result = ModelName + "es";
                }
            }
            else if (LastChars.Contains("U"))
            {
                var beforeY = ModelName.Trim().ToUpper().Substring(ModelName.Length - 2, 1);
                if (beforeY == "E" || beforeY == "U" || beforeY == "O" || beforeY == "A" || beforeY == "I")
                {
                    Result = ModelName + "x";
                }
                else
                {
                    Result = ModelName + "s";
                }
            }
            else
            {
                Result = ModelName + "s";
            }

            return Result;
        }
    }
}
