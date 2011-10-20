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

    public abstract class AbstractViewModel : AbstractBase
    {
        /// <summary>
        /// Method is responsible for select generate type such as: view, model or repository
        /// </summary>
        /// <returns>Correspding generate class</returns>
        public static AbstractBase SelectGenerateType(Table tbl)
        {
            if (tbl == null)
            {
                return new NguoiDungViewModel();
            }
            else
            {
                if (tbl.Name.ToUpper() == GlobalVariables.g_sTableNguoiDung.ToUpper())
                {
                    return new NguoiDungViewModel();
                }
                else
                {
                    return new EntityViewModel();
                }
            }
        }

        public abstract string GenerateGetViewModel();
        public abstract string GenerateEditViewModel();
        public abstract string GenerateAddViewModel();
    }
}
