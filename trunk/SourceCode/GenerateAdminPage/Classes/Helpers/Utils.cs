using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BK.Util;

namespace GenerateAdminPage.Classes.Helpers
{
    #region USING
    using GenerateAdminPage.Classes.DBStructure;
    #endregion

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

        public static string BuildCastingPK(Table _tbl)
        {
            string Result = "";
            var pk = GetPK(_tbl);

            if (pk.Type == DataType.STRING)
            {
                Result += pk.Name;
            }
            else
            {
                Result += GetDataType(pk.Type) + ".Parse(txt" + pk.Name + ".Text)";
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
                    if (lst[i].Type != DataType.STRING)
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
                if (_db.Tables[i].Name != _table.Name)
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
                if (_db.Tables[i].Name != _table.Name)
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
                    Result += tab + "Lst" + lst[i] + "ReferObjModel = objItem." + lst[i] + ".ToList(), " + END;
                }
                else
                {
                    Result += tab + "Lst" + lst[i] + "ReferObjModel = objItem." + lst[i] + ".ToList()";
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

                case DataType.IMAGE:
                    return "byte[]";
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
            return ModelName;
        }

        public static bool ContainsKey(MultimapBK<string, string>.MultimapEnum enumMultiMapItems, string key)
        {
            enumMultiMapItems.Reset();
            while (enumMultiMapItems.MoveNext()) // Line 6
            {
                var item = enumMultiMapItems.Current.Value.First;
                while (item != null)
                {
                    if (enumMultiMapItems.Current.Key.ToUpper() == key.ToUpper())
                    {
                        return true;
                    }
                    item = enumMultiMapItems.Current.Value.Next;
                }
            }
            return false;
        }

        public static bool ContainsValue(MultimapBK<string, string>.MultimapEnum enumMultiMapItems, string key, string value)
        {
            while (enumMultiMapItems.MoveNext())
            {
                if (enumMultiMapItems.Current.Key.ToUpper() == key.ToUpper())
                {
                    if (enumMultiMapItems.Current.Value.Current.ToUpper() == value.ToUpper())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GetStrAttributeStartWithTen(Table _tbl)
        {
            var result = "";
            foreach (var item in _tbl.Attributes)
            {
                if (item.Name.ToUpper().StartsWith("TEN"))
                {
                    result = item.Name;
                    break;
                }
            }
            return result;
        }

        public static Attribute GetAttributeStartWithTen(Table _tbl)
        {
            Attribute result = null;
            foreach (var item in _tbl.Attributes)
            {
                if (item.Name.ToUpper().StartsWith("TEN"))
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        public static Table GetTableByName(DataBase db, string name)
        {
            for (int i = 0; i < db.Tables.Count; i++)
            {
                if (db.Tables[i].Name.ToUpper() == name.ToUpper())
                {
                    return db.Tables[i];
                }
            }
            return null;
        }

        public static string BuildListColumnData(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        Result += "String.Format(\"{0:dd/MM/yyyy}\", lstItem[i]." + lst[i].Name + "), ";
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        Result += "lstItem[i]." + lst[i].Name + " == null ? \"\" : (lstItem[i]." + lst[i].Name + " == true ? \"Nam\" : \"Nữ\"), ";
                    }
                    else
                    {
                        Result += "lstItem[i]." + lst[i].Name + ", ";
                    }
                }
                else
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        Result += "String.Format(\"{0:dd/MM/yyyy}\", lstItem[i]." + lst[i].Name + ")";
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        Result += "lstItem[i]." + lst[i].Name + " == null ? \"\" : (lstItem[i]." + lst[i].Name + " == true ? \"Nam\" : \"Nữ\")";
                    }
                    else
                    {
                        Result += "lstItem[i]." + lst[i].Name;
                    }
                }
            }

            return Result;
        }

        public static string BuildListColumnData2(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].Name + "), ";
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        Result += "item." + lst[i].Name + " = 1, ";
                    }
                    else
                    {
                        Result += "item." + lst[i].Name + ", ";
                    }
                }
                else
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].Name + ")";
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        Result += "item." + lst[i].Name + " = 1";
                    }
                    else
                    {
                        Result += "item." + lst[i].Name;
                    }
                }
            }

            return Result;
        }

        public static string BuildListColumnData3(List<Attribute> lst)
        {
            string Result = "";

            for (int i = 0; i < lst.Count; i++)
            {
                if (i < lst.Count - 1)
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].ReferTo + "." + lst[i].Name + "), ";
                        }
                        else
                        {
                            Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].Name + "), ";
                        }
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "item." + lst[i].ReferTo + "." + lst[i].Name + " = 1, ";
                        }
                        else
                        {
                            Result += "item." + lst[i].Name + " = 1, ";
                        }
                    }
                    else
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "item." + lst[i].ReferTo + "." + lst[i].Name + ", ";
                        }
                        else
                        {
                            Result += "item." + lst[i].Name + ", ";
                        }

                    }
                }
                else
                {
                    if (lst[i].Type == DataType.DATETIME)
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].ReferTo + "." + lst[i].Name + ")";
                        }
                        else
                        {
                            Result += "String.Format(\"{0:dd/MM/yyyy}\", item." + lst[i].Name + ")";
                        }
                    }
                    else if (lst[i].Type == DataType.BOOL)
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "item." + lst[i].ReferTo + "." + lst[i].Name + " = 1";
                        }
                        else
                        {
                            Result += "item." + lst[i].Name + " = 1";
                        }
                    }
                    else
                    {
                        if (lst[i].ReferTo != "" && !lst[i].IsForeignKey)
                        {
                            Result += "item." + lst[i].ReferTo + "." + lst[i].Name;
                        }
                        else
                        {
                            Result += "item." + lst[i].Name;
                        }

                    }
                }
            }

            return Result;
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
    }
}
