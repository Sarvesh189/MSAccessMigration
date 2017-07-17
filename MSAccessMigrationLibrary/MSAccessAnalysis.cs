using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access = Microsoft.Office.Interop.Access;
using Microsoft.Office.Interop.Access.Dao;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;


namespace MSAccessMigrationLibrary
{
    public class MSAccessAnalysis : IMSAccessAnalysis
    {
        public List<string> GetFormNames(string accessDBfileName)
        {
           
            var frmNames = new List<string>();
            try
            {
                Access.Application _application = new Access.Application();

                _application.OpenCurrentDatabase(accessDBfileName, true, "");

                Access.AllObjects forms = _application.CurrentProject.AllForms;

                foreach (var frm in forms)
                {
                    try
                    {
                        var typedesc = TypeDescriptor.GetProperties(frm).Find("Name", true);

                        frmNames.Add(typedesc.GetValue(frm).ToString());
                    }
                    catch (Exception ex)
                    {
                        Utility.ExceptionList.Add(ex);

                    }

                }

                _application.CloseCurrentDatabase();
            }
            catch (Exception ex)
            { Utility.ExceptionList.Add(ex); }
            return frmNames;
        }

        public List<string> GetQueries(string accessDBfileName)
        {
          
            var queries = new List<string>();
            try
            {
                DBEngine _dbEngine = new DBEngine();
                Database _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");
                var _queries = _db.QueryDefs;

                for (int index = 0; index < _queries.Count; index++)
                {
                    var _query = _queries[index];
                    queries.Add(_query.Name);
                }

                _db.Close();
            }
            catch (Exception ex)
            {
                Utility.ExceptionList.Add(ex);
            }
            return queries;
        }

        public List<string> GetReport(string accessDBfileName)
        {
            var reportsName = new List<string>();
            Access.Application _application = new Access.Application();

            _application.OpenCurrentDatabase(accessDBfileName, true, "");

            Access.AllObjects reports = _application.CurrentProject.AllReports;

            foreach (var rpt in reports)
            {
                var typedesc = TypeDescriptor.GetProperties(rpt).Find("Name", true);

                reportsName.Add(typedesc.GetValue(rpt).ToString());
            }

            _application.CloseCurrentDatabase();

            return reportsName;
        }

        public List<string> GetMacros(string accessDBfileName)
        {
            var macrosname = new List<string>();
            Access.Application _application = new Access.Application();

            _application.OpenCurrentDatabase(accessDBfileName, true, "");

            Access.AllObjects macros = _application.CurrentProject.AllMacros;

            foreach (var rpt in macros)
            {
                var typedesc = TypeDescriptor.GetProperties(rpt).Find("Name", true);

                macrosname.Add(typedesc.GetValue(rpt).ToString());
            }

            _application.CloseCurrentDatabase();

            return macrosname;
        }

        public List<TableInfo> GetTablesName(string accessDBfileName)
        {
            var tables = new List<TableInfo>();

            DBEngine _dbEngine = new DBEngine();
            Database _db = _dbEngine.OpenDatabase(accessDBfileName, false, false, "");

            var debugtables = new List<string>();

            for (int i = 0; i < _db.TableDefs.Count; i++)
            {
                var tbdl = _db.TableDefs[i];
                if (tbdl.Attributes != 2 && !tbdl.Name.StartsWith("MSys"))
                {
                    if (!string.IsNullOrEmpty(tbdl.Connect))
                        tables.Add(new TableInfo(tbdl.Name, "External"));
                    else
                        tables.Add(new TableInfo(tbdl.Name, "Internal"));
                }
                //non system table
                // debugtables.Add(tbdl.Name);
            }


            //     Recordset _rs = _db.ListTables();

            //_rs.MoveFirst();
            ////   string str = "";
            //while (!_rs.EOF)
            //{
            //    for (int i = 0; i < _rs.Fields.Count; i++)
            //        debugtables.Add(_rs.Fields[i].Name + ' ' + _rs.Fields[i].Value);




            //    if (_rs.Fields["TableType"].Value == 1 && _rs.Fields["Attributes"].Value == 0)
            //        tables.Add(new TableInfo(_rs.Fields[0].Value,"Internal"));
            //    if (_rs.Fields["TableType"].Value == 4 && _rs.Fields["Attributes"].Value == 0)
            //        tables.Add(new TableInfo(_rs.Fields[0].Value, "External"));
            //    _rs.MoveNext();
            //}

            _db.Close();

            return tables;
        }
    }
}
