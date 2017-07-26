    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace MSAccessMigrationLibrary
    {
        public class DBEngineObject
        {
            public List<TableInfo> Tables { get; set; }
            public List<string> Queries { get; set; }
            public List<string> Forms { get; set; }
            public List<string> ExternalTables { get; set; }
            public List<string> Macros { get; set; }

            public List<string> Reports { get; set; }

            public List<string> Modules { get; set; }

            public List<AccessTableRelationInfo> TablesRelation { get; set; }

            public List<AccessTableIndexInfo> TableIndexs { get; set; }
        }
    }


