using AccessDAO = Microsoft.Office.Interop.Access.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using Access = Microsoft.Office.Interop.Access;

namespace MSAccessMigrationLibrary
{
    public class MSAccessTransfer : IMSAccessTransfer
    {
        public event EventHandler<ItemTransferEventArg> ItemTransferred;
        Access.Application _application;
        DBEngineObject _dbEngineObject;
        public MSAccessTransfer()
        {
            _application = new Access.Application();

        }

        protected virtual void OnItemTransferred(ItemTransferEventArg e)
        {
            EventHandler<ItemTransferEventArg> handler = ItemTransferred;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public DBEngineObject DBEngineObject
        {
            get { return _dbEngineObject; }
            set { _dbEngineObject = value; }
        }

        public void TransferForm(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                AppLogManager.LogInfo("Form transfer started");
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Forms.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Forms[index], Access.AcObjectType.acForm, _dbEngineObject.Forms[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Forms.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Forms[index], ItemType = "Form" });
                    AppLogManager.LogInfo(string.Format("{0} transferred", _dbEngineObject.Forms[index]));
                }
                AppLogManager.LogInfo("Form transfer done");
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex); ;
            }
        }

        public List<string> TransferInternalTables(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            List<string> _prps = new List<string>();
            try
            {
                AppLogManager.LogInfo("Local Table transfer started");
                Access.Application _application = new Access.Application();
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                var internalTables = _dbEngineObject.Tables.FindAll(t => t.TableType == "Internal");


                for (int index = 0; index < internalTables.Count; index++)
                {
                    _docmd.TransferDatabase(Access.AcDataTransferType.acExport, "Microsoft Access", destinationDBFile, Access.AcObjectType.acTable, internalTables[index].TableName, internalTables[index].TableName, false);
                    progress.Report((index + 1) * 100 / internalTables.Count);
                    _prps.Add(internalTables[index].TableName);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = internalTables[index].TableName, ItemType = "Table" });
                    AppLogManager.LogInfo(string.Format("{0} transferred", internalTables[index].TableName));
                }
                AppLogManager.LogInfo("Local Table transfer done");
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex) { AppLogManager.LogError(ex); }
            return _prps;
        }

        public void TransferQueries(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                AppLogManager.LogInfo("Queries transfer started");
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Queries.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Queries[index], Access.AcObjectType.acQuery, _dbEngineObject.Queries[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Queries.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Queries[index], ItemType = "Query" });
                    AppLogManager.LogInfo(_dbEngineObject.Queries[index] + " transferred");
                }
                AppLogManager.LogInfo("Queries transfer done");
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            { AppLogManager.LogError(ex); }
        }

        public void TransferReport(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                AppLogManager.LogInfo("Reports transfer started");
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Reports.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Reports[index], Access.AcObjectType.acReport, _dbEngineObject.Reports[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Reports.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Reports[index], ItemType = "Report" });
                    AppLogManager.LogInfo(_dbEngineObject.Reports[index] + " transferred");
                }
                _application.CloseCurrentDatabase();
                AppLogManager.LogInfo("Reports transfer done");
            }
            catch (Exception ex)
            { AppLogManager.LogError(ex); }
        }

        public void SetIdentityOff(string table)
        {
            ADODB.Connection con = null;
            try
            {
                con = new ADODB.Connection();
                object recordsAffected;
                var cmd = new ADODB.Command();
                 
                con.Provider = "MSDASQL";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.Substring(4);
                con.Open();

                cmd.ActiveConnection = con;
                AppLogManager.LogInfo($"Set Identity_insert [{table}] Off");
                cmd.CommandText = $"Set Identity_insert [{table}] Off";
                cmd.Execute(out recordsAffected);
            }
            catch (Exception ex)
            { AppLogManager.LogError(ex); }
            finally
            { con.Close(); }
            

        }

        public void EstablishRelationsInSql(AccessTableRelationInfo tableInfo)
        {

            object recordsAffected;
            var cmd = new ADODB.Command();
            var con = new ADODB.Connection();
            con.Provider = "MSDASQL";
            con.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.Substring(4);


            con.Open();

            cmd.ActiveConnection = con;
            try
            {
                foreach (var accessfieldInfo in tableInfo.FieldsInfo)
                {
                    cmd.CommandText = $"ALTER TABLE [{tableInfo.ForeignTable}] ADD CONSTRAINT {tableInfo.RelationName} FOREIGN KEY ({accessfieldInfo.ForeignName}) references [{tableInfo.Table}]({accessfieldInfo.Name}) ";
                    cmd.Execute(out recordsAffected);

                   // progress.Report((index + 1) * 100 / _dbEngineObject.TablesRelation.Count);

                  //  OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.TablesRelation[i].RelationName, ItemType = "TablesRelation" });

                }
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                con.Close();
            }
        }

        public List<string> TransferObjectToSQL(List<string> tables, string sourceAccessfile)
        {
            List<string> _prps = new List<string>();
            try
            {
                AppLogManager.LogInfo("Table transfer to SQL started");
                string strConnect = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.ToString();
                Access.DoCmd _docmd = _application.DoCmd;
                foreach (var table in tables)
                {
                    try
                    {
                        _application.OpenCurrentDatabase(sourceAccessfile, false, "");

                        _docmd.TransferDatabase(Access.AcDataTransferType.acExport, "ODBC Database", strConnect, Access.AcObjectType.acTable, table, table);// "","testdb",false,"sa","Password",true);
                                     
                    }
                    catch (Exception ex)
                    {
                        AppLogManager.LogError(ex);

                    }
                    finally
                    {
                        _application.CloseCurrentDatabase();
                    }
                    SetIdentityOff(table);
                    _prps.Add(table);
                }

                foreach (var table in _prps)
                {
                    AccessDAO.Database _db = null;
                    try
                    {
                        AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
                        _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                        _db.TableDefs.Delete(table);
                        _db.TableDefs.Refresh();
                        
                    }
                    catch (Exception ex)
                    {
                        AppLogManager.LogError(ex);
                    }
                    finally
                    {
                        _db.Close();
                        _db = null;
                    }

                }

                foreach (var table in _prps)
                {
                    AccessDAO.Database _db = null;
                    try
                    {
                        AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
                        _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                        _db.TableDefs.Refresh();

                        var newtbl = _db.CreateTableDef(table, 0, table, strConnect);

                        _db.TableDefs.Append(newtbl);

                        
                        AppLogManager.LogInfo(table + " transferred");
                    }
                    catch (Exception ex)
                    {
                        AppLogManager.LogError(ex);
                    }
                    finally {
                        _db.Close();
                        _db = null;
                    }
                }
                AppLogManager.LogInfo("Table transfer to SQL done");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }

            return _prps;
        }

        public void TransferMacros(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                AppLogManager.LogInfo("Macros transfer started");
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Macros.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Macros[index], Access.AcObjectType.acMacro, _dbEngineObject.Macros[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Macros.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Macros[index], ItemType = "Macro" });
                    AppLogManager.LogInfo(_dbEngineObject.Macros[index] + " transferred");
                }
                _application.CloseCurrentDatabase();
                AppLogManager.LogInfo("Macros transfer done");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
        }

        public void TransferModules(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                AppLogManager.LogInfo("Module transfer started");

                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Modules.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Modules[index], Access.AcObjectType.acModule, _dbEngineObject.Modules[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Modules.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Modules[index], ItemType = "Module" });
                    AppLogManager.LogInfo(_dbEngineObject.Modules[index] + "transferred");
                }
                _application.CloseCurrentDatabase();
                AppLogManager.LogInfo("Module transfer done");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
        }
        public void EstablishRelations(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
            AccessDAO.Database _db = null;
            try
            {
                AppLogManager.LogInfo("Relations transfer started");

                _db = _dbEngine.OpenDatabase(destinationDBFile, false, false, "");


                for (int index = 0; index < _dbEngineObject.TablesRelation.Count; index++)
                {

                    var newRelation = _db.CreateRelation(_dbEngineObject.TablesRelation[index].RelationName, _dbEngineObject.TablesRelation[index].Table, _dbEngineObject.TablesRelation[index].ForeignTable);
                    //var newRelation = _db.CreateRelation(_dbEngineObject.TablesRelation[index].RelationName, _db.TableDefs[_dbEngineObject.TablesRelation[index].Table], _db.TableDefs[_dbEngineObject.TablesRelation[index].ForeignTable]);
                    var fields = _db.TableDefs[_dbEngineObject.TablesRelation[index].Table].Fields;
                    for (int i = 0; i < fields.Count; i++)
                    {
                        if (fields[i].Name == _dbEngineObject.TablesRelation[index].FieldsInfo[0].Name)
                        {
                            var field = newRelation.CreateField(_dbEngineObject.TablesRelation[index].FieldsInfo[0].Name);
                            field.Name = fields[i].Name;
                            field.ForeignName = _dbEngineObject.TablesRelation[index].FieldsInfo[0].ForeignName;
                            newRelation.Fields.Append(field);
                            _db.Relations.Append(newRelation);
                            progress.Report((index + 1) * 100 / _dbEngineObject.TablesRelation.Count);

                            OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.TablesRelation[i].RelationName, ItemType = "TablesRelation" });
                        }

                    }


                   

                    AppLogManager.LogInfo(_dbEngineObject.TablesRelation[index] + " established");
                }


                AppLogManager.LogInfo("Relations done");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {

                _db.Close();
            }
        }

        public void EstablishRelationsInSql(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            int index = 0;
            foreach (var tableInfo in _dbEngineObject.TablesRelation)
            {
                EstablishRelationsInSql(tableInfo);
                progress.Report((++index) * 100 / _dbEngineObject.TablesRelation.Count);
            }
        }

        public void CreateIndexes(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {


        }
        public void CreateIndexes(string destinationDBFile,IProgress<int> progress)
        {
            AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
            AccessDAO.Database _db = null;
            try
            {
                AppLogManager.LogInfo("Index creation started");

                _db = _dbEngine.OpenDatabase(destinationDBFile, false, false, "");

            for(int counter=0; counter < _db.TableDefs.Count;counter++)
                {
                    var tableName = _db.TableDefs[counter].Name;
                    AppLogManager.LogInfo(tableName);

                    var tableIndexesInfo = _dbEngineObject.TableIndexs.FindAll(t => t.Table == tableName);
                    foreach (var tblIndex in tableIndexesInfo)
                    {
                        if (_db.TableDefs[counter].Indexes[tblIndex.Name] == null)
                        {
                            var index = _db.TableDefs[counter].CreateIndex(tblIndex.Name);
                            index.Primary = tblIndex.IsPrimaryKey;
                            index.Required = tblIndex.IsRequired;
                            index.Unique = tblIndex.IsUniqueKey;
                            //  index.IgnoreNulls = tblIndex.ignore
                            //index.Foreign = tblIndex.IsForeignKey;
                            foreach (var field in tblIndex.Fields)
                                index.Fields.Append(index.CreateField(field.Name));
                            _db.TableDefs[counter].Indexes.Append(index);

                            //  progress.Report((index + 1) * 100 / _dbEngineObject.TablesRelation.Count);
                        }
                        OnItemTransferred(new ItemTransferEventArg() { ItemName = tblIndex.Name, ItemType = "Index" });
                    }

                   // AppLogManager.LogInfo(_dbEngineObject.TableIndexs[counter] + " established");
                 }


                AppLogManager.LogInfo("indexes done");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
                
            }
            finally
            {

                _db.Close();
            }
        }

        public void CreateSqlIndexes(string destinationDBFile)
        {
            object recordsAffected;
            var cmd = new ADODB.Command();
            var con = new ADODB.Connection();
            con.Provider = "MSDASQL";
            con.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString.Substring(4);

            //alter table Persion add primary key (persionId,Pname,PMID)
            con.Open();

            cmd.ActiveConnection = con;
            var primaryKeyIndexes = _dbEngineObject.TableIndexs.FindAll(t => t.IsPrimaryKey);
            try
            {
                foreach (var index in primaryKeyIndexes)
                {
                    cmd.CommandText = $"ALTER TABLE [{index.Table}] ADD primary key ({index.Fields[0].Name}) ";
                    cmd.Execute(out recordsAffected);
                }
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            finally
            {
                con.Close();
            }

        }
    }

    public class ItemTransferEventArg : EventArgs
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
    }
}
