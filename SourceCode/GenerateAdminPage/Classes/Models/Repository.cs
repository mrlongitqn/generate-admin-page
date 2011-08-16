using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    public class Repository : ClassGenerate, IRepository
    {
        public override string GenerateClasses(DataBase _database, Table _tbl)
        {
            _db = _database;
            _table = _tbl;

            GlobalVariables.g_ModelName = _table.Name;

            string Result = "";

            Result += GenerateNameSpace("Models.Repositories");

            return Result;
        }

        public override string GenerateUsingRegion()
        {
            string Result = "";

            Result += TAB + "#region using" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Helpers;" + END;
            Result += TAB + "using " + GlobalVariables.g_sNameSpace + ".Models.ViewModels;" + END;
            Result += TAB + "#endregion" + END;

            return Result;
        }

        public override string GenerateClass()
        {
            string Result = "";

            Result += TAB + "public class " + GlobalVariables.g_ModelName + "Repository" + END;
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
            
            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    //generate select by foreign key
                    Result += GenerateSelectByForeignKey(_table.Attributes[i]) + END;
                }
            }

            var lst = Utils.FKHaveMoreThan1Attribute(_table);
            if (lst.Count > 0)
            {
                //find items have same refer to
                while (lst.Count > 0)
                {
                    var item = lst[0];
                    List<Attribute> lstItem = new List<Attribute>();
                    lstItem.Add(item);

                    for (int i = 1; i < lst.Count; i++)
                    {
                        if (lst[i].ReferTo == item.ReferTo)
                        {
                            lstItem.Add(lst[i]);
                        }
                    }

                    Result += GenerateSelectByForeignKey(lstItem) + END;
                    for (int i = 0; i < lstItem.Count; i++)
                    {
                        lst.Remove(lstItem[i]);
                    }
                }
            }
            Result += TAB + "}" + END;

            return Result;
        }

        public string GenerateSelectByForeignKey(Attribute FK)
        {
            string Result = "";
            var idFK = FK.Name.ToLower();
            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "> SelectBy" + FK.Name + "(" + Utils.GetDataType(FK.Type) + " " + idFK + ", int page, int pageSize)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = (from item in DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + " where item." + FK.Name + " == " + idFK + " select item).ToList();" + END;
            Result += GeneratePaging();

            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateSelectByForeignKey(List<Attribute> FK)
        {
            string Result = "";
            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "> SelectBy" + FK[0].ReferTo + "(" + Utils.BuildFKParams(FK) + " int page, int pageSize)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = (from item in DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + " where " + Utils.BuildWhereClause(FK) + " select item).ToList();" + END;
            Result += GeneratePaging();

            Result += TAB3 + "return lstItem;" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public string GenerateRetrieveByID()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);
            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "> RetrieveByID(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return (from item in DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + " where " + Utils.BuildPKWhereClause(lst) + " select item).ToList();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateGetTotalItem()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalItem()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".Count();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateGetTotalItemByFK(Attribute fk)
        {
            string Result = "";
            //generate select by foreign key
            Result += TAB2 + "public int GetTotalItemBy" + fk.Name + "(" + Utils.GetDataType(fk.Type) + " " + fk.Name.ToLower() + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return (from p in DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + " where p." + fk.Name + " == " + fk.Name.ToLower() + " select p).Count();" + END;
            Result += TAB2 + "}" + END;
            return Result;
        }

        public virtual string GenerateGetTotalItemByFKs()
        {
            string Result = "";

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += GenerateGetTotalItemByFK(_table.Attributes[i]) + END;
                }
            }

            return Result;
        }

        public virtual string GenerateGetTotalPage()
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalPage()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "int RecordCount = GetTotalItem();" + END;
            Result += TAB3 + "int PageSize = WebConfiguration.ProductsPerPage;" + END;
            Result += TAB3 + "return (RecordCount / PageSize) + ((RecordCount % PageSize == 0) ? 0 : 1);" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateGetTotalPageByFK(Attribute fk)
        {
            string Result = "";

            Result += TAB2 + "public int GetTotalPageBy" + fk.Name + "(" + Utils.GetDataType(fk.Type) + " " + fk.Name.ToLower() + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "int RecordCount = GetTotalItemBy" + fk.Name + "(" + fk.Name.ToLower() + ");" + END;
            Result += TAB3 + "int PageSize = WebConfiguration.ProductsPerPage;" + END;
            Result += TAB3 + "return (RecordCount / PageSize) + ((RecordCount % PageSize == 0) ? 0 : 1);" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateGetTotalPageByFKs()
        {
            string Result = "";

            for (int i = 0; i < _table.Attributes.Count; i++)
            {
                if (_table.Attributes[i].IsForeignKey)
                {
                    Result += GenerateGetTotalPageByFK(_table.Attributes[i]) + END;
                }
            }

            return Result;
        }

        public virtual string GenerateDelete()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);
            Result += TAB2 + "public bool Delete(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "var delitem = DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".FirstOrDefault(item =>" + Utils.BuildPKWhereClause(lst) + ");" + END;
            Result += TAB4 + "DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".DeleteObject(delitem);" + END;
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

        public virtual string GenerateInsert()
        {
            string Result = "";

            Result += TAB2 + "public bool Insert(" + GlobalVariables.g_ModelName + " obj)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "try" + END;
            Result += TAB3 + "{" + END;
            Result += TAB4 + "DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".AddObject(obj);" + END;
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

        public virtual string GenerateSelectAll()
        {
            string Result = "";

            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName  +"> SelectAll()" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".ToList();" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateSelectByID()
        {
            string Result = "";
            var lst = Utils.PKHaveMoreThan1Attribute(_table);
            Result += TAB2 + "public " + GlobalVariables.g_ModelName + " SelectByID(" + Utils.BuildPKParams(lst) + ")" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "return DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".FirstOrDefault(item =>" + Utils.BuildPKWhereClause(lst) + ");" + END;
            Result += TAB2 + "}" + END;

            return Result;
        }

        public virtual string GenerateSelectPaging()
        {
            string Result = "";

            Result += TAB2 + "public List<" + GlobalVariables.g_ModelName + "> SelectPaging(int page, int pageSize)" + END;
            Result += TAB2 + "{" + END;
            Result += TAB3 + "var lstItem = DataContext.Instance." + Utils.BuildModelName(_db, _table, GlobalVariables.g_ModelName) + ".ToList();" + END;
            
            Result += GeneratePaging();

            Result += TAB3 + "return lstItem;" + END;
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
    }
}
