using AccessDAO = Microsoft.Office.Interop.Access.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Forms.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Forms[index], Access.AcObjectType.acForm, _dbEngineObject.Forms[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Forms.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Forms[index], ItemType = "Form" });

                }
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                Utility.ExceptionList.Add(ex);
            }
        }

        public List<string> TransferInternalTables(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            List<string> _prps = new List<string>();
            try
            {
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
                }
                _application.CloseCurrentDatabase();
            }
            catch(Exception ex) { Utility.ExceptionList.Add(ex); }
            return _prps;
        }

        public void TransferQueries(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Queries.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Queries[index], Access.AcObjectType.acQuery, _dbEngineObject.Queries[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Queries.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Queries[index], ItemType = "Query" });

                }
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            { Utility.ExceptionList.Add(ex); }
        }

        public void TransferReport(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try { 
            _application.OpenCurrentDatabase(sourceDBFile, false, "");
            Access.DoCmd _docmd = _application.DoCmd;
            for (int index = 0; index < _dbEngineObject.Reports.Count; index++)
            {
                _docmd.CopyObject(destinationDBFile, _dbEngineObject.Reports[index], Access.AcObjectType.acReport, _dbEngineObject.Reports[index]);
                progress.Report((index + 1) * 100 / _dbEngineObject.Reports.Count);
                OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Reports[index], ItemType = "Report" });
            }
            _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            { Utility.ExceptionList.Add(ex); }
        }

        public List<string> TransferObjectToSQL(List<string> tables, string sourceAccessfile)
        {
            List<string> _prps = new List<string>();
            try
            {
               
                string strConnect = ConfigurationManager.AppSettings["SQLConnection"].ToString();
                Access.DoCmd _docmd = _application.DoCmd;
                foreach (var table in tables)
                {

                    _application.OpenCurrentDatabase(sourceAccessfile, false, "");

                    _docmd.TransferDatabase(Access.AcDataTransferType.acExport, "ODBC Database", strConnect, Access.AcObjectType.acTable, table, table);// "","testdb",false,"sa","Password",true);
                    _application.CloseCurrentDatabase();

                    _prps.Add(table);
                }

                foreach (var table in _prps)
                {
                    AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
                    AccessDAO.Database _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                    _db.TableDefs.Delete(table);
                    _db.TableDefs.Refresh();
                    _db.Close();

                }

                foreach (var table in _prps)
                {
                    AccessDAO.DBEngine _dbEngine = new AccessDAO.DBEngine();
                    AccessDAO.Database _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                    _db.TableDefs.Refresh();

                    var newtbl = _db.CreateTableDef(table, 0, table, strConnect);

                    _db.TableDefs.Append(newtbl);

                    _db.Close();
                }
            }
            catch (Exception ex)
            {
                Utility.ExceptionList.Add(ex);
            }

            return _prps;
        }

        public void TransferMacros(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Macros.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Macros[index], Access.AcObjectType.acMacro, _dbEngineObject.Macros[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Macros.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Macros[index], ItemType = "Macro" });
                }
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                Utility.ExceptionList.Add(ex);
            }
        }

        public void TransferModules(string sourceDBFile, string destinationDBFile, IProgress<int> progress)
        {
            try
            {
                _application.OpenCurrentDatabase(sourceDBFile, false, "");
                Access.DoCmd _docmd = _application.DoCmd;
                for (int index = 0; index < _dbEngineObject.Modules.Count; index++)
                {
                    _docmd.CopyObject(destinationDBFile, _dbEngineObject.Modules[index], Access.AcObjectType.acModule, _dbEngineObject.Modules[index]);
                    progress.Report((index + 1) * 100 / _dbEngineObject.Modules.Count);
                    OnItemTransferred(new ItemTransferEventArg() { ItemName = _dbEngineObject.Modules[index], ItemType = "Module" });
                }
                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            {
                Utility.ExceptionList.Add(ex);
            }
        }
    }

    public class ItemTransferEventArg : EventArgs
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
    }
}
