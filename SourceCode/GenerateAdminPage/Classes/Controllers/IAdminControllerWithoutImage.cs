﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public interface IAdminControllerWithoutImage
    {
        string GenerateEditActionResult();
        string GenerateCancelActionResult();
    }
}
