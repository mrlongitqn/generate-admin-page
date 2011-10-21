using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    #region USING
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    public class ValidateScript: AbstractBase
    {
        public override string GenerateUsingRegion()
        {
            return string.Empty;
        }

        public override string GenerateClass()
        {
            return string.Empty;
        }

        public override string GenerateView(EnumView _type)
        {
            string Result = "";
            
            Result += "function Validate" + Tbl.Name + "() {" + END;

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (!Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsForeignKey ||
                    (Tbl.Attributes[i].IsPrimaryKey && !Tbl.Attributes[i].IsIdentify))
                {
                    if (Tbl.Attributes[i].Name != Utils.GetImageAttrName(Tbl))
                    {
                        Result += TAB + "var " + Tbl.Attributes[i].Name.ToLower() + " = document.getElementsByName(\"" + Tbl.Name + "_" + Tbl.Attributes[i].Name + "\").item(0);" + END;
                        Result += TAB + "if (" + Tbl.Attributes[i].Name.ToLower() + ".value == \"\") {" + END;
                        Result += TAB2 + "alert(\"Please input " + Tbl.Attributes[i].Name + "\");" + END;
                        Result += TAB2 + "return false;" + END;
                        Result += TAB + "}" + END;
                    }
                    else
                    {
                        Result += TAB + "var " + Tbl.Attributes[i].Name.ToLower() + " = document.getElementsByName(\"" + Tbl.Name + "_" + Utils.GetImageAttrName(Tbl) + "" + "\").item(0);" + END;
                        Result += TAB + "if (" + Tbl.Attributes[i].Name.ToLower() + ".value == \"\") {" + END;
                        Result += TAB2 + "alert(\"Please choose " + Tbl.Attributes[i].Name + "\");" + END;
                        Result += TAB2 + "return false;" + END;
                        Result += TAB + "}" + END;
                    }
                }
            }

            Result += TAB + "return true;" + END;
            Result += "}" + END + END;

            //Result += GenerateValidateNguoiDung() + END;

            return Result;
        }
    }
}
