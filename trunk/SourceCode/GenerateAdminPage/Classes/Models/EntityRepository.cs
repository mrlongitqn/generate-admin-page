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

    public class EntityRepository : AbstractRepository
    {
        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.ViewModels;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public class " + Tbl.Name + "Repository" + END;
            Result += TAB + "{" + END;
            Result += GenerateSelectAll() + END;
            Result += GenerateSelectByID() + END;
            Result += GenerateSelectPaging() + END;
            Result += GenerateInsert() + END;
            Result += GenerateDelete() + END;
            Result += GenerateSave() + END;
            Result += GenerateGetTotalItem() + END;
            Result += GenerateGetTotalItemByFKs() + END;
            Result += GenerateGetTotalPage() + END;
            Result += GenerateGetTotalPageByFKs() + END;
            Result += GenerateRetrieveByID() + END;
            
            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    //generate select by foreign key
                    Result += GenerateSelectByForeignKey(Tbl.Attributes[i]) + END;
                }
            }
            Result += TAB + "}" + END;

            return Result;
        }

        public override string GenerateGetTotalPage()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalPage()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "int RecordCount = GetTotalItem();" + END;
            Result += TAB3 + "int PageSize = WebConfiguration.Num" + Tbl.Name + "PerPage;" + END;
            Result += TAB3 + "return (RecordCount / PageSize) + ((RecordCount % PageSize == 0) ? 0 : 1);" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateDelete()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            Result += TAB2 + "public bool Delete(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "var delitem = DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".FirstOrDefault(item =>" + Utils.BuildPKWhereClause(lst) + ");" + END;
            Result += TAB4 + "DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".DeleteObject(delitem);" + END;
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

        public override string GenerateInsert()
        {
            string Result = "";

            Result += TAB2 + "public bool Insert(" + Tbl.Name + " obj)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".AddObject(obj);" + END;
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

        public override string GenerateSelectAll()
        {
            string Result = "";

            Result += TAB2 + "public List<" + Tbl.Name + "> SelectAll()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".ToList();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectByID()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            Result += TAB2 + "public " + Tbl.Name + " SelectByID(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".FirstOrDefault(item =>" + Utils.BuildPKWhereClause(lst) + ");" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public override string GenerateSelectPaging()
        {
            string Result = "";

            Result += TAB2 + "public List<" + Tbl.Name + "> SelectPaging(int page, int pageSize)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".ToList();" + END;

            Result += GeneratePaging();

            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByForeignKey(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();

            // Select item by foreign key and paging
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += TAB2 + "public List<" + Tbl.Name + "> SelectBy" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page, int pageSize)" + END;
            }
            else
            {
                Result += TAB2 + "public List<" + Tbl.Name + "> SelectBy" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ")" + END;
            }

            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = (from item in DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + " where item." + FK.Name + " == " + idFK + " select item).ToList();" + END;

            // Generate paging items
            if (Utils.ContainsKey(GlobalVariables.g_colFKPaging.GetEnumerator(), Tbl.Name))
            {
                Result += GeneratePaging();
            }

            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByForeignKey(List<Attribute> FK)
        {
            string Result = "";
            Result += TAB2 + "public List<" + Tbl.Name + "> SelectBy" + FK[0].ReferTo + "(" + Utils.BuildFKParams(FK) + " int page, int pageSize)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = (from item in DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + " where " + Utils.BuildWhereClause(FK) + " select item).ToList();" + END;
            Result += GeneratePaging();

            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateRetrieveByID()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(Tbl);
            Result += TAB2 + "public List<" + Tbl.Name + "> RetrieveByID(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return (from item in DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + " where " + Utils.BuildPKWhereClause(lst) + " select item).ToList();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateGetTotalItem()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalItem()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + ".Count();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateGetTotalItemByFK(Attribute fk)
        {
            string Result = "";
            //generate select by foreign key
            Result += TAB2 + "public int GetTotalItemBy" + fk.Name + "(" + Utils.GetDataType(fk.Type) + " " + fk.Name.ToLower() + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return (from p in DataContext.Instance." + Utils.BuildModelName(DB, Tbl, Tbl.Name) + " where p." + fk.Name + " == " + fk.Name.ToLower() + " select p).Count();" + END;
            Result += TAB2 + "}" + END;
            return Result;
        }

        public string GenerateGetTotalItemByFKs()
        {
            string Result = "";

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += GenerateGetTotalItemByFK(Tbl.Attributes[i]) + END;
                }
            }

            return Result;
        }

        public string GenerateGetTotalPageByFK(Attribute fk)
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalPageBy" + fk.Name + "(" + Utils.GetDataType(fk.Type) + " " + fk.Name.ToLower() + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "int RecordCount = GetTotalItemBy" + fk.Name + "(" + fk.Name.ToLower() + ");" + END;
            Result += TAB3 + "int PageSize = WebConfiguration.Num" + Tbl.Name + "PerPage;" + END;
            Result += TAB3 + "return (RecordCount / PageSize) + ((RecordCount % PageSize == 0) ? 0 : 1);" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateGetTotalPageByFKs()
        {
            string Result = "";

            for (int i = 0; i < Tbl.Attributes.Count; i++)
            {
                if (Tbl.Attributes[i].IsForeignKey)
                {
                    Result += GenerateGetTotalPageByFK(Tbl.Attributes[i]) + END;
                }
            }

            return Result;
        }
    }
}
