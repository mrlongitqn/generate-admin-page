using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes.Base
{
    #region USING
    using GenerateAdminPage.Classes.DBStructure;
    #endregion

    /// <summary>
    /// Using factory method pattern to organize structure of project
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// Init the components of generate class
        /// </summary>
        /// <param name="db">Database loaded from mechanims DB</param>
        /// <param name="tblOne">Using for mode single</param>
        void InitClassGenerate(DataBase db, Table tbl);

        /// <summary>
        /// Generate name space of a class
        /// </summary>
        /// <param name="nameSpace">Namespace input</param>
        /// <returns></returns>
        string GenerateNameSpace(string nameSpace);
        string GenerateView(EnumView _type);
    }
}
