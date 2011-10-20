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

    public enum EnumView
    {
        SELECT,
        TEMPLATE_LIST,
        TEMPLATE_EDIT_OR_DETAILOF
    }

    public abstract class AbstractView : AbstractBase
    {
        /// <summary>
        /// Method is responsible for select generate type such as: view, model or repository
        /// </summary>
        /// <returns>Correspding generate class</returns>
        public static AbstractBase SelectGenerateType(Table tbl)
        {
            if (tbl == null)
            {
                return new NguoiDungView();
            }
            else
            {
                if (tbl.Name.ToUpper() == GlobalVariables.g_sTableNguoiDung.ToUpper())
                {
                    return new NguoiDungView();
                }
                else
                {
                    return new EntityView();
                }
            }
        }

        public override string GenerateUsingRegion()
        {
            return string.Empty;
        }

        /// <summary>
        /// Method is overrided by child classes
        /// </summary>
        /// <returns>Return a string that present body of class</returns>
        public override string GenerateClass()
        {
            return string.Empty;
        }
    }
}
