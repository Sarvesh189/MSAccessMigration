using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access = Microsoft.Office.Interop.Access;
using Microsoft.Office.Interop.Access.Dao;
using System.ComponentModel;
using System.Configuration;


namespace MSAccessMigrationLibrary
{
    public class MigrationManager : IMigrationManager
    {

        DBEngineObject _dbEngineobject;
        IMSAccessAnalysis _msaccessAnalysis;
        IMSAccessTransfer _msaccessTransfer;
        Access.Application _application;

        public MigrationManager(IMSAccessAnalysis msaccessAnalysis, IMSAccessTransfer msaccessTransfer)
        {
            _dbEngineobject = new DBEngineObject();
            _msaccessAnalysis = msaccessAnalysis;
            _msaccessTransfer = msaccessTransfer;
            _application = new Access.Application();

        }
        public IMSAccessTransfer MSAccessTransfer { get { return _msaccessTransfer; } }

        public DBEngineObject AnalyseAccessDB(string accessDBfileName)
        {
            AppLogManager.LogInfo(string.Format("Analysis of {0}", accessDBfileName));

            _dbEngineobject.Forms = _msaccessAnalysis.GetFormNames(accessDBfileName);
            _dbEngineobject.Tables = _msaccessAnalysis.GetTablesName(accessDBfileName);
            _dbEngineobject.Reports = _msaccessAnalysis.GetReport(accessDBfileName);
            _dbEngineobject.Queries = _msaccessAnalysis.GetQueries(accessDBfileName);
            _dbEngineobject.Macros = _msaccessAnalysis.GetMacros(accessDBfileName);
            _dbEngineobject.Modules = _msaccessAnalysis.GetModules(accessDBfileName);
            return _dbEngineobject;
        }



        public bool TransferAccessDB(string sourceDBFile, string destinationDBFile, List<string> tables, IProgress<int> progress)
        {
            AppLogManager.LogInfo(string.Format("Transformation started from {0} to {1}",sourceDBFile,destinationDBFile));
            _msaccessTransfer.DBEngineObject = _dbEngineobject;
            _msaccessTransfer.TransferInternalTables(sourceDBFile, destinationDBFile, progress);
            _msaccessTransfer.TransferForm(sourceDBFile, destinationDBFile, progress);
            if (tables != null && tables.Count > 0)
                _msaccessTransfer.TransferObjectToSQL(tables, destinationDBFile);
            _msaccessTransfer.TransferReport(sourceDBFile, destinationDBFile, progress);
            _msaccessTransfer.TransferQueries(sourceDBFile, destinationDBFile, progress);
            _msaccessTransfer.TransferMacros(sourceDBFile, destinationDBFile, progress);
            _msaccessTransfer.TransferModules(sourceDBFile, destinationDBFile, progress);
            AppLogManager.LogInfo("Transformation Done");
            return true;
        }



        public List<string> TransferObjectToSQL(List<string> tables, string sourceAccessfile)
        {
            
            List<string> _prps = new List<string>();
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
                DBEngine _dbEngine = new DBEngine();
                Database _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                _db.TableDefs.Delete(table);
                _db.TableDefs.Refresh();
                _db.Close();

            }

            foreach (var table in _prps)
            {
                DBEngine _dbEngine = new DBEngine();
                Database _db = _dbEngine.Workspaces[0].OpenDatabase(sourceAccessfile, false, false, "");
                _db.TableDefs.Refresh();

                var newtbl = _db.CreateTableDef(table, 0, table, strConnect);

                _db.TableDefs.Append(newtbl);

                _db.Close();
            }

            return _prps;
        }

    }
}
