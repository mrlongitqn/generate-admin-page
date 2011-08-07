using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IViewModel: IBaseModel
    {
        string GenerateGetViewModel();
        string GenerateEditViewModel();
        string GenerateAddViewModel();
        string GenerateObjectInfo();
    }
}
