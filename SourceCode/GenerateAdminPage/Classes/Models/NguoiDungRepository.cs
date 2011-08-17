using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class NguoiDungRepository : Repository
    {
        public Table _BaseInfo { get; set; }
        public Table _table { get; set; }

        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;

            if (_tbl != null)
            {
                GlobalVariables.g_ModelName = _tbl.Name;
            }
            else
            {
                GlobalVariables.g_ModelName = GlobalVariables.g_sTableNguoiDung;
            }

            string Result = "";

            Result += GenerateNameSpace("Models.Repositories");

            return Result;
        }

        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "using System.Web.Security;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.ViewModels;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "Repository" + END;
            Result += TAB + "{" + END;
            Result += TAB2 + "public int TotalItem { get; set; }" + END;
            Result += GenerateSelectAll() + END;
            Result += GenerateSelectPaging() + END;
            Result += GenerateSelectByUserName() + END;
            if (_table != null)
            {
                Result += GenerateSelectByID() + END;
            }
            Result += GenerateDelete() + END;
            Result += GenerateSave() + END;
            Result += GenerateInsert() + END;
            Result += GenerateGetTotalItem() + END;
            Result += GenerateGetTotalPage() + END;
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateSelectAll()
        {
            string Result = "";

            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "Info> SelectAll(string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "List<" + GlobalVariables.g_ModelName + "Info> lstItem = new List<" + GlobalVariables.g_ModelName + "Info>();" + END;
            Result += TAB3 + "MembershipUserCollection userCollection = Membership.GetAllUsers();" + END;
            Result += TAB3 + "foreach (MembershipUser user in userCollection)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "var roleForUser = Roles.GetRolesForUser(user.UserName)[0];" + END;

            Result += TAB4 + "if (role.ToUpper() == \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB5 + "{" + END;
            
            Result += TAB6 + GlobalVariables.g_ModelName + "Info userInfo = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB6 + "{" + END;

            if (_table == null)
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
            }
            else
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
                Result += TAB7 + "ExtraInfo = SelectByID((Guid)user.ProviderUserKey)" + END;
            }
            
            Result += TAB6 + "};" + END;

            Result += TAB6 + "lstItem.Add(userInfo);" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;

            Result += TAB4 + "else if (role.ToUpper() == \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\" && roleForUser.ToUpper() != \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB5 + "{" + END;

            Result += TAB6 + GlobalVariables.g_ModelName + "Info userInfo = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB6 + "{" + END;

            if (_table == null)
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
            }
            else
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
                Result += TAB7 + "ExtraInfo = SelectByID((Guid)user.ProviderUserKey)" + END;
            }

            Result += TAB6 + "};" + END;

            Result += TAB6 + "lstItem.Add(userInfo);" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;

            Result += TAB3 + "}" + END;
            Result += TAB3 + "TotalItem = lstItem.Count;" + END;
            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectPaging()
        {
            string Result = "";

            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "Info> SelectPaging(int page, int pageSize, string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "List<" + GlobalVariables.g_ModelName + "Info> lstItem = new List<" + GlobalVariables.g_ModelName + "Info>();" + END;
            Result += TAB3 + "MembershipUserCollection userCollection = Membership.GetAllUsers();" + END;
            Result += TAB3 + "foreach (MembershipUser user in userCollection)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "var roleForUser = Roles.GetRolesForUser(user.UserName)[0];" + END;

            Result += TAB4 + "if (role.ToUpper() == \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB5 + "{" + END;

            Result += TAB6 + GlobalVariables.g_ModelName + "Info userInfo = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB6 + "{" + END;
            if (_table == null)
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
            }
            else
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
                Result += TAB7 + "ExtraInfo = SelectByID((Guid)user.ProviderUserKey)" + END;
            }
            Result += TAB6 + "};" + END;

            Result += TAB6 + "lstItem.Add(userInfo);" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;

            Result += TAB4 + "else if (role.ToUpper() == \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\" && roleForUser.ToUpper() != \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB5 + "{" + END;

            Result += TAB6 + GlobalVariables.g_ModelName + "Info userInfo = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB6 + "{" + END;
            if (_table == null)
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
            }
            else
            {
                Result += TAB7 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB7 + "{" + END;
                Result += TAB8 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB8 + "UserName = user.UserName," + END;
                Result += TAB8 + "Email = user.Email," + END;
                Result += TAB8 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB7 + "}," + END;
                Result += TAB7 + "ExtraInfo = SelectByID((Guid)user.ProviderUserKey)" + END;
            }
            Result += TAB6 + "};" + END;

            Result += TAB6 + "lstItem.Add(userInfo);" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;

            Result += TAB3 + "}" + END;
            Result += TAB3 + "TotalItem = lstItem.Count;" + END;
            Result += GeneratePaging() + END;
            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByUserName()
        {
            string Result = "";

            Result += TAB2 + "public " + GlobalVariables.g_ModelName + "Info SelectByUserName(string username)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var user = Membership.GetUser(username);" + END;
            Result += TAB3 + GlobalVariables.g_ModelName + "Info userInfo = new " + GlobalVariables.g_ModelName + "Info" + END;
            Result += TAB3 + "{" + END;
            if (_table == null)
            {
                Result += TAB4 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB4 + "{" + END;
                Result += TAB5 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB5 + "UserName = user.UserName," + END;
                Result += TAB5 + "Email = user.Email," + END;
                Result += TAB5 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB4 + "}," + END;
            }
            else
            {
                Result += TAB4 + "BaseInfo = new " + GlobalVariables.g_ModelName + "BaseInfo" + END;
                Result += TAB4 + "{" + END;
                Result += TAB5 + "ID = (Guid)user.ProviderUserKey," + END;
                Result += TAB5 + "UserName = user.UserName," + END;
                Result += TAB5 + "Email = user.Email," + END;
                Result += TAB5 + "Role = Roles.GetRolesForUser(user.UserName)[0]" + END;
                Result += TAB4 + "}," + END;
                Result += TAB4 + "ExtraInfo = SelectByID((Guid)user.ProviderUserKey)" + END;
            }
            Result += TAB3 + "};" + END;
            Result += TAB3 + "TotalItem = 1;" + END;
            Result += TAB3 + "return userInfo;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectByID()
        {
            string Result = "";

            Result += TAB2 + "public " + GlobalVariables.g_ModelName + " SelectByID(Guid id)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var item = DataContext.Instance." + GlobalVariables.g_ModelName + "s.SingleOrDefault(dmw => dmw." + Utils.GetPKWith1Attr(_table) + " == id);" + END;
            Result += TAB3 + "if (item != null)" + END;
            Result += TAB4 + "return item;" + END;
            Result += TAB3 + "return new " + GlobalVariables.g_ModelName + "();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string BuildExtraInfo()
        {
            string Result = "";

            if (_table != null)
            {
                Result += TAB6 + "var extraInfo = new " + GlobalVariables.g_sTableNguoiDung + END;
                Result += TAB6 + "{" + END;
                for (int i = 0; i < _table.Attributes.Count; i++)
                {
                    if (_table.Attributes[i].Name == Utils.GetPKWith1Attr(_table))
                    {
                        Result += TAB7 + _table.Attributes[i].Name + " = (Guid)newUser.ProviderUserKey " + END;
                        break;
                    }
                }
                Result += TAB6 + "};" + END;
                Result += TAB6 + "DataContext.Instance." + GlobalVariables.g_ModelName + "s.AddObject(extraInfo);" + END;
                Result += TAB6 + "DataContext.Instance.SaveChanges();" + END;
            }

            return Result;
        }

        public override string GenerateInsert()
        {
            string Result = "";

            Result += TAB2 + "public bool Insert(Add" + GlobalVariables.g_ModelName + "ViewModel obj, ref string errorText)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "errorText = \"\";" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;

            Result += TAB4 + "MembershipCreateStatus createStatus;" + END;
            Result += TAB4 + "MembershipUser newUser = Membership.CreateUser(obj.Info.BaseInfo.UserName, obj.Info.BaseInfo.Password, obj.Info.BaseInfo.Email, \"What is your favouritr color?\", \"Black Or White\", true, out createStatus);" + END;
            Result += TAB4 + "switch (createStatus)" + END;
            Result += TAB4 + "{" + END;

            Result += TAB5 + "case MembershipCreateStatus.Success:" + END;
            Result += TAB6 + "Roles.AddUserToRole(obj.Info.BaseInfo.UserName, obj.Info.BaseInfo.Role);" + END;
            Result += BuildExtraInfo();
            Result += TAB6 + "break;" + END;

            Result += TAB5 + "case MembershipCreateStatus.DuplicateUserName:" + END;
            Result += TAB6 + "errorText = \"Người dùng này đã tồn tại, vui lòng nhập vào tên người dùng khác!\";" + END;
            Result += TAB6 + "break;" + END;

            Result += TAB5 + "case MembershipCreateStatus.DuplicateEmail:" + END;
            Result += TAB6 + "errorText = \"Địa chỉ Email bạn vừa nhập vào đã tồn tại, vui lòng nhập địa chỉ email khác!\";" + END;
            Result += TAB6 + "break;" + END;

            Result += TAB5 + "case MembershipCreateStatus.InvalidEmail:" + END;
            Result += TAB6 + "errorText = \"Địa chỉ Email không hợp lệ, vui lòng nhập lại.\";" + END;
            Result += TAB6 + "break;" + END;

            Result += TAB5 + "case MembershipCreateStatus.InvalidPassword:" + END;
            Result += TAB6 + "errorText = \"Mật khẩu phải có độ dài ít nhất 7 kí tự. Trong đó có 1 kí tự Alpha (~@#$%^&*).\";" + END;
            Result += TAB6 + "break;" + END;

            Result += TAB5 + "default:" + END;
            Result += TAB6 + "errorText = \"Có 1 lỗi gì đó, tài khoản của bạn chưa được khởi tạo.\";" + END;
            Result += TAB6 + "break;" + END;

            Result += TAB4 + "}" + END;
            Result += TAB4 + "if (errorText != \"\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "return false;" + END;
            Result += TAB4 + "}" + END;

            Result += TAB4 + "return true;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "catch" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "errorText = \"Có 1 lỗi gì đó, tài khoản của bạn chưa được khởi tạo.\";" + END;
            Result += TAB4 + "return false;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateDelete()
        {
            string Result = "";

            Result += TAB2 + "public bool Delete(Guid id)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            if (_table != null)
            {
                Result += TAB4 + "var item = DataContext.Instance." + GlobalVariables.g_ModelName + "s.FirstOrDefault(p => p." + Utils.GetPKWith1Attr(_table) + " == id);" + END;
                Result += TAB4 + "DataContext.Instance." + GlobalVariables.g_ModelName + "s.DeleteObject(item);" + END;
                Result += TAB4 + "DataContext.Instance.SaveChanges();" + END;
                Result += TAB4 + "Membership.DeleteUser(Membership.GetUser(id).UserName, true);" + END;
            }
            else
            {
                Result += TAB4 + "Membership.DeleteUser(Membership.GetUser(id).UserName, true);" + END;
            }
            Result += TAB4 + "return true;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "catch" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "return false;" + END;
            Result += TAB3 + "}" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateGetTotalItem()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalItem(string role)" + END;
            Result += TAB2 + "{" + END;

            Result += TAB3 + "int count = 0;" + END;
            Result += TAB3 + "MembershipUserCollection userCollection = Membership.GetAllUsers();" + END;
            Result += TAB3 + "foreach (MembershipUser user in userCollection)" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "var roleForUser = Roles.GetRolesForUser(user.UserName)[0];" + END;

            Result += TAB4 + "if (role.ToUpper() == \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\")" + END;
            Result += TAB5 + "{" + END;

            Result += TAB6 + "count++;" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;

            Result += TAB4 + "else if (role.ToUpper() == \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB4 + "{" + END;
            Result += TAB5 + "if (roleForUser.ToUpper() != \"" + GlobalVariables.g_sSuperAdmin + "\" && roleForUser.ToUpper() != \"" + GlobalVariables.g_sAdmin + "\")" + END;
            Result += TAB5 + "{" + END;

            Result += TAB6 + "count++;" + END;
            Result += TAB5 + "}" + END;
            Result += TAB4 + "}" + END;
            Result += TAB3 + "}" + END;
            Result += TAB3 + "return count;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateGetTotalPage()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalPage(string role)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "int RecordCount = GetTotalItem(role);" + END;
            Result += TAB3 + "int PageSize = WebConfiguration.Num" + GlobalVariables.g_ModelName + "PerPage;" + END;
            Result += TAB3 + "return (RecordCount / PageSize) + ((RecordCount % PageSize == 0) ? 0 : 1);" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }
    }
}
