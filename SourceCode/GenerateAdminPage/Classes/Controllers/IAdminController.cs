using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IAdminController
    {
        string GenerateSelectActionResult();
        string GenerateSelectByFKs();
        string GenerateUpdateActionResult();
        string GenerateDeleteActionResult();
        string GenerateInsertActionResult();
    }
}
