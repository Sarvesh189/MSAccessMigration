using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessMigrationLibrary
{
  public  class AccessTableRelationInfo
    {
        public string Table { get; set; }
        public string ForeignTable { get; set; }
        public string RelationName { get; set; }

        
        public List<AccessFieldInfo> FieldsInfo { get; set; } = new List<AccessFieldInfo>();

        public override string ToString()
        {
            var fieldsInfo = FieldsInfo.Select(f => f.ToString());
            var fieldinfo = string.Join(",", fieldsInfo);
            return $"Relation: {RelationName} between primary table: {Table} and foreign table: {ForeignTable}; Field information {fieldinfo}";
        }
    }

    public class AccessFieldInfo
    {
        public string Name { get; set; }
        public string ForeignName { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} ForeignName: {ForeignName}";
        }
    }

    public class AccessTableIndexInfo
    {
        public string Table { get; set; }
        public string Name { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsUniqueKey { get; set; }

        public bool IsForeignKey { get; set; }

        public bool IsRequired { get; set; }

        public bool IsClustered { get; set; }

        public List<AccessFieldInfo> Fields { get; set; } = new List<AccessFieldInfo>();

        public override string ToString()
        {
            var fieldsInfo = Fields.Select(f => f.ToString());
            var fieldinfo = string.Join(",", fieldsInfo);
            return $"Table : {Table} Indexname: {Name} isprimarykey: {IsPrimaryKey} isForeignKey {IsForeignKey} fields: {fieldinfo}";
        }

    }
}
