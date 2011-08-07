using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class ValidateScript: Views
    {
        public override void InitView(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;
            GlobalVariables.g_ModelName = _tbl.Name;
            TblUsingFck = Utils.TableUsingFCK(_table);
            TblHaveImgAttr = Utils.TableHaveImageAttribute(_table);
        }

        public override string GenerateView(EnumView _type)
        {
            string Result = "";
            
            Result += "function Validate" + _table.Name + "() {" + END;

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (!_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsForeignKey ||
                    (_table.Attributes[i].IsPrimaryKey && !_table.Attributes[i].IsIdentify))
                {
                    if (_table.Attributes[i].Name != Utils.GetImageAttrName(_table))
                    {
                        Result += TAB + "var " + _table.Attributes[i].Name.ToLower() + " = document.getElementsByName(\"" + _table.Name + "_" + _table.Attributes[i].Name + "\").item(0);" + END;
                        Result += TAB + "if (" + _table.Attributes[i].Name.ToLower() + ".value == \"\") {" + END;
                        Result += TAB2 + "alert(\"Please input " + _table.Attributes[i].Name + "\");" + END;
                        Result += TAB2 + "return false;" + END;
                        Result += TAB + "}" + END;
                    }
                    else
                    {
                        Result += TAB + "var " + _table.Attributes[i].Name.ToLower() + " = document.getElementsByName(\"" + _table.Name + "_" + Utils.GetImageAttrName(_table) + "" + "\").item(0);" + END;
                        Result += TAB + "if (" + _table.Attributes[i].Name.ToLower() + ".value == \"\") {" + END;
                        Result += TAB2 + "alert(\"Please choose " + _table.Attributes[i].Name + "\");" + END;
                        Result += TAB2 + "return false;" + END;
                        Result += TAB + "}" + END;
                    }
                }
            }

            Result += TAB + "return true;" + END;
            Result += "}" + END;

            return Result;
        }

        public string GenerateValidateNguoiDung()
        {
            string Result = "";

            Result += "function Validate" + GlobalVariables.g_sTableNguoiDung + "() {" + END;

            Result += TAB + "var username = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_UserName" + "\").item(0);" + END;
            Result += TAB + "if (username.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input UserName\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "var password = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_Password" + "\").item(0);" + END;
            Result += TAB + "if (password.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input Password\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "var email = document.getElementsByName(\"" + GlobalVariables.g_sTableNguoiDung + "_Email" + "\").item(0);" + END;
            Result += TAB + "if (email.value == \"\") {" + END;
            Result += TAB2 + "alert(\"Please input Email\");" + END;
            Result += TAB2 + "return false;" + END;
            Result += TAB + "}" + END;

            Result += TAB + "return true;" + END;
            Result += "}" + END;

            return Result;
        }
    }
}
