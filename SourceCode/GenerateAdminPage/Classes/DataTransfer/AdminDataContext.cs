using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateAdminPage.Classes
{
    #region Using 
    using System.Data;
    using System.Data.SqlClient;
    #endregion

    public class ForeignKey
    {
        public List<string> Name { get; set; }
        public string ReferTo { get; set; }
    }

    public class PrimaryKey
    {
        public string Name { get; set; }
        public bool IsIdentify { get; set; }

        public PrimaryKey()
        {
            IsIdentify = false;
        }
    }

    public class AdminDataContext
    {
        public DBProvider DBProvider { get; set; }
        public DataTextProvider DataTextProvider { get; set; }

        public AdminDataContext()
        {
            DataTextProvider = new DataTextProvider();
            DBProvider = new DBProvider();
        }

        public void CreateConnection(string dataSource, string dataBase)
        {
            DBProvider.InitDBProvider(dataSource, dataBase);
        }

        public List<string> GetTables()
        {
            List<string> lstItem = new List<string>();
            try
            {
                var ds = new DataSet();
                DBProvider.SqlQuery = @"select table_name as Name from
                                INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE'";
                DBProvider.FillDataSet(ref ds, "ListTable");

                lstItem = ConvertDataTable2List(ds.Tables["ListTable"]);
                return lstItem;
            }
            catch (Exception ex)
            {
                return lstItem;
            } 
        }

        public void InitDB(ref DataBase DB)
        {
            var lstTable = GetTables();

            for (int i = 0; i < lstTable.Count; i++)
            {
                string[] tblComp = lstTable[i].Split(new char[] { '_' });
                if (tblComp[0] != "aspnet" || lstTable[i] == "aspnet_Users")
                {
                    var ds = new DataSet();
                    DBProvider.SqlQuery = "Select * From " + lstTable[i] + " ";
                    DBProvider.FillDataSet(ref ds, lstTable[i]);

                    Table tbl = new Table();
                    InitTable(ds.Tables[lstTable[i]], ref tbl);
                    DB.Tables.Add(tbl);
                }
            }
        }

        public List<PrimaryKey> GetListPK(Microsoft.SqlServer.Management.Smo.Table tbl)
        {
            var lst = new List<PrimaryKey>();
            var columns = tbl.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].InPrimaryKey)
                {
                    if(columns[i].Identity)
                        lst.Add(new PrimaryKey { Name = columns[i].Name, IsIdentify = true });
                    else
                        lst.Add(new PrimaryKey { Name = columns[i].Name, IsIdentify = false });
                }
            }
            return lst;
        }

        public void InitTable(DataTable dataTable, ref Table tbl)
        {
            tbl.Name = dataTable.TableName;
            DataColumn[] columns = dataTable.PrimaryKey;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var col = dataTable.Columns[i];
                var Attr = new Attribute
                {
                    Name = col.ColumnName,
                    Type = GetType(col.DataType)
                };
                tbl.Attributes.Add(Attr);
            }
        }

        public DataType GetType(Type type)
        {
            switch (type.ToString())
            {
                case "System.String":
                    return DataType.STRING;

                case "System.Float":
                    return DataType.FLOAT;
              
                case "System.Double":
                    return DataType.DOUBLE;

                case "System.Int32":
                    return DataType.INT;

                case "System.Int64":
                    return DataType.LONG;

                case "System.Guid":
                    return DataType.GUILD;

                case "System.Boolean":
                    return DataType.BOOL;

                case "System.DateTime":
                    return DataType.DATETIME;

                default:
                    return DataType.STRING;
            }
        }

        public void SetForeignKey(ref DataBase DB)
        {
            for (int i = 0; i < DB.Tables.Count; i++)
            {
                Table table = DB.Tables[i];

                Microsoft.SqlServer.Management.Smo.Database db;
                Microsoft.SqlServer.Management.Smo.Server server;

                //build a "serverConnection" with the information of the "sqlConnection"
                Microsoft.SqlServer.Management.Common.ServerConnection serverConnection =
                  new Microsoft.SqlServer.Management.Common.ServerConnection(DBProvider.ObjConnection);

                //The "serverConnection is used in the ctor of the Server.
                server = new Microsoft.SqlServer.Management.Smo.Server(serverConnection);

                db = server.Databases[DBProvider.DataBaseName];

                Microsoft.SqlServer.Management.Smo.Table tbl;
                //get foreign key list of corresponding table
                tbl = db.Tables[table.Name];
                
                for (int j = 0; j < table.Attributes.Count; j++)
                {
                    string referTo = "";
                    if (IsExistOn(ConvertSOMTable2List(tbl), table.Attributes[j].Name, ref referTo))
                    {
                        table.Attributes[j].IsForeignKey = true;
                        table.Attributes[j].ReferTo = referTo;
                    }
                }
                var lstPK = GetListPK(tbl);
                for (int j = 0; j < table.Attributes.Count; j++)
                {
                    for (int k = 0; k < lstPK.Count; k++)
                    {
                        if (lstPK[k].Name == table.Attributes[j].Name)
                        {
                            table.Attributes[j].IsPrimaryKey = true;
                            table.Attributes[j].IsIdentify = lstPK[k].IsIdentify;
                        }
                    }
                }
            }           
        }

        public bool IsExistOn(List<ForeignKey> lst, string item, ref string referto)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                foreach (string key in lst[i].Name)
                {
                    if (key == item)
                    {
                        referto = lst[i].ReferTo;
                        return true;
                    }
                }
            }
                    
            return false;
        }

        public List<string> GetFKs(Microsoft.SqlServer.Management.Smo.ForeignKey fk)
        {
            List<string> lstFK = new List<string>();
            foreach(Microsoft.SqlServer.Management.Smo.ForeignKeyColumn key in fk.Columns)
            {
                lstFK.Add(key.Name);
            }
            return lstFK;
        }

        public List<ForeignKey> ConvertSOMTable2List(Microsoft.SqlServer.Management.Smo.Table tbl)
        {
            List<ForeignKey> lstItem = new List<ForeignKey>();
            
            foreach (Microsoft.SqlServer.Management.Smo.ForeignKey fk in tbl.ForeignKeys)
            {
                var Item = new ForeignKey
                {
                    Name = GetFKs(fk),
                    ReferTo = fk.ReferencedTable
                };
                lstItem.Add(Item);
            }

            return lstItem;
        }

        public List<string> ConvertDataTable2List(DataTable dt)
        {
            List<string> lstItem = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstItem.Add(dt.Rows[i][0].ToString());
            }

            return lstItem;
        }
    }
}
