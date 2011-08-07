using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IRepository: IBaseModel
    {
        string GenerateSelectAll();
        string GenerateSelectPaging();
        string GenerateSelectByID();
        string GenerateInsert();
        string GenerateDelete();
        string GenerateSave();
        string GenerateGetTotalPage();
    }
}
