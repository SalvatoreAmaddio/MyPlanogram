using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.DB;
using Testing.Model.Abstracts;

namespace MyPlanogram.Model
{
#nullable enable

    public class Section : AbstractModel, IDB<Section>
    {
        #region backprops
        Int64? _sectionid;
        string? _sectioname;
        DateTime? _planodate;
        #endregion

        #region Props 
        public Int64? SectionID 
        {
            get => _sectionid;
            set => Set<Int64?>(ref value, ref _sectionid,nameof(SectionID));
        }

        public string? SectionName 
        {
            get => _sectioname;
            set => Set<string?>(ref value, ref _sectioname,nameof(SectionName));        
        }

        public DateTime? PlanoDate 
        { 
            get=> _planodate;
            set => Set<DateTime?>(ref value, ref _planodate,nameof(PlanoDate));
        }
        #endregion

        #region Constructors
        public Section()
        {

        }

        public Section(Int64 SectionID) => this.SectionID = SectionID;

        public Section(MySqlDataReader reader)
        {
            SectionID = reader.GetInt64(0);
            SectionName = reader.GetString(1);
            PlanoDate = reader.GetDateTime(2);
        }
        #endregion

        #region EqualsString
        public override bool Equals(object? obj) =>
        obj is Section section && SectionID == section.SectionID;
        public override int GetHashCode() => HashCode.Combine(SectionID);
        public override string? ToString() => (SectionID==null) ? "NOT RANGED": $"In: {SectionName}";
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => SectionID==0;

        public override void SetForeignKeys()
        {
        }
        #endregion

        #region IDB
        public string SQLQuery(QueryType Query)
        {
            switch (Query)
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Section;";
                case QueryType.DELETE:
                    return string.Empty;
                case QueryType.UPDATE:
                    return string.Empty;
                case QueryType.INSERT:
                    return string.Empty;
                default: return string.Empty;
            }
        }

        public Section GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
        }

        public void SetPrimaryKey(ulong ID)
        {
        }
        #endregion

    }
}
