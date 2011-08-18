using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class DataTransferViewModel: ClassGenerate
    {
        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public List<Attribute> GetListOfForeignKey()
        {
            var lst = new List<Attribute>();
            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != "aspnet_Users")
                {
                    for (int j = 0; j < _db.Tables[i].Attributes.Count; j++)
                    {
                        if (_db.Tables[i].Attributes[j].IsForeignKey)
                        {
                            if (!IsExist(lst, _db.Tables[i].Attributes[j].Name))
                            {
                                lst.Add(_db.Tables[i].Attributes[j]);
                            }
                        }
                    }
                }
            }
            return lst;
        }

        public bool IsExist(List<Attribute> lst, string key)
        {
            foreach (Attribute attr in lst)
            {
                if (attr.Name == key)
                    return true;
            }
            return false;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += GenerateEnumViewModel() + END;

            Result += TAB + "public class DataTransferViewModel" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public int CurrentPage { get; set; }" + END;
            Result += TAB2 + "public EnumViewModel EnumViewModelType { get; set; }" + END;
            Result += TAB2 + "public string StrID { get; set; }" + END;
            Result += TAB2 + "public int IntID { get; set; }" + END;
            Result += TAB2 + "public Guid GuidID { get; set; }" + END;
            Result += TAB2 + "public string UserName { get; set; }" + END;
            Result += TAB2 + "public string Role { get; set; }" + END;
            Result += TAB2 + "public string InfoText { get; set; }" + END;

            var lst = GetListOfForeignKey();
            for (int i = 0; i < lst.Count; i++)
            {
                Result += TAB2 + "public " + Utils.GetDataType(lst[i].Type) + " " + lst[i].Name + " { get; set; }" + END;
            }

            Result += TAB2 + "public DataTransferViewModel()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "CurrentPage = 1;" + END;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].Type != DataType.GUILD)
                {
                    if (lst[i].Type == DataType.STRING)
                    {
                        Result += TAB3 + lst[i].Name + " = \"\";" + END;
                    }
                    else if(lst[i].Type == DataType.BOOL)
                    {
                        Result += TAB3 + lst[i].Name + " = false;" + END;
                    }
                    else if (lst[i].Type == DataType.DATETIME)
                    {
                        Result += TAB3 + lst[i].Name + " = 1/1/1;" + END;
                    }
                    else
                    {
                        Result += TAB3 + lst[i].Name + " = -1;" + END;
                    }
                }
                else
                    Result += TAB3 + lst[i].Name + " = Guid.Parse(\"" + GlobalVariables.g_DefaultGuid.ToString() + "\");" + END;
            }
            Result += TAB2 + "}" + END;

            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateEnumViewModel()
        {
            string Result = "";
            bool isExistNguoiDung = false;

            Result += TAB + "public enum EnumViewModel" + END;
            Result += TAB + "{" + END;

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != "aspnet_Users")
                {
                    if (Utils.TableUsingFCK(_db.Tables[i]) || Utils.TableHaveImageAttribute(_db.Tables[i]))
                    {
                        Result += TAB2 + "ADMIN_DETAILOF_" + _db.Tables[i].Name.ToUpper() + "," + END;
                        GlobalVariables.g_colEnumViewModel.Add("ADMIN_DETAILOF_" + _db.Tables[i].Name.ToUpper());
                    }
                    else if(_db.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                    {
                        Result += TAB2 + "ADMIN_DETAILOF_" + _db.Tables[i].Name.ToUpper() + "," + END;
                        GlobalVariables.g_colEnumViewModel.Add("ADMIN_DETAILOF_" + _db.Tables[i].Name.ToUpper());
                    }
                }
            }

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name != "aspnet_Users")
                {
                    Result += TAB2 + "ADMIN_" + _db.Tables[i].Name.ToUpper() + "," + END;
                    GlobalVariables.g_colEnumViewModel.Add("ADMIN_" + _db.Tables[i].Name.ToUpper());
                }

                if (_db.Tables[i].Name == GlobalVariables.g_sTableNguoiDung)
                    isExistNguoiDung = true;
            }

            if (!isExistNguoiDung)
            {
                Result += TAB2 + "ADMIN_NGUOIDUNG," + END;
                GlobalVariables.g_colEnumViewModel.Add("ADMIN_NGUOIDUNG");
            }


            Result += TAB2 + "HOME_REGISTER," + END;
            Result += TAB2 + "HOME_DETAILOF_NGUOIDUNG" + END;
            GlobalVariables.g_colEnumViewModel.Add("HOME_REGISTER");
            GlobalVariables.g_colEnumViewModel.Add("HOME_DETAILOF_NGUOIDUNG");
            Result += TAB + "}" + END;

            return Result;
        }
    }
}
