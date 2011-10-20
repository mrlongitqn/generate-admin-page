using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Models
{
    #region USING
    using GenerateAdminPage.Classes.Base;
    using GenerateAdminPage.Classes.DBStructure;
    using GenerateAdminPage.Classes.Helpers;
    #endregion

    public abstract class AbstractRepository : AbstractBase
    {
        /// <summary>
        /// Method is responsible for select generate type such as: view, model or repository
        /// </summary>
        /// <returns>Correspding generate class</returns>
        public static AbstractBase SelectGenerateType(Table tbl)
        {
            if (tbl == null)
            {
                return new NguoiDungRepository();
            }
            else
            {
                if (tbl.Name.ToUpper() == GlobalVariables.g_sTableNguoiDung.ToUpper())
                {
                    return new NguoiDungRepository();
                }
                else
                {
                    return new EntityRepository();
                }
            }
        }

        public string GenerateSave()
        {
            string Result = "";

            Result += TAB2 + "public bool Save()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "DataContext.Instance.SaveChanges();" + END;
            Result += TAB4 + "return true;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "catch" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "return false;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GeneratePaging()
        {
            string Result = "";

            Result += TAB3 + "if (pageSize > 0)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "int startIndex, itemCount;" + END;
            Result += TAB4 + "startIndex = (page - 1) * pageSize;" + END;
            Result += TAB4 + "itemCount = pageSize;" + END;
            Result += TAB4 + "if (startIndex + itemCount > lstItem.Count)" + END;
            Result += TAB5 + "itemCount = lstItem.Count - startIndex;" + END;
            Result += TAB4 + "lstItem = lstItem.GetRange(startIndex, itemCount);" + END;
            Result += TAB3 + "}" + END;

            return Result;
        }

        public abstract string GenerateSelectAll();
        public abstract string GenerateSelectPaging();
        public abstract string GenerateSelectByID();
        public abstract string GenerateInsert();
        public abstract string GenerateDelete();
        public abstract string GenerateGetTotalPage();
    }
}
