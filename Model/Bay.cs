using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.DB;
using Testing.Model.Abstracts;
#nullable enable

namespace MyPlanogram.Model
{
    public class Bay : AbstractModel, IDB<Bay>
    {
        #region backprops
        Int64? _bayid;
        Section? _section;
        int? _baynum;
        string? _baytitle;
        string? _pictureurl;
        #endregion

        #region Props
        public string? PictureURL 
        {
            get => _pictureurl;
            set => Set<string?>(ref value,ref _pictureurl,nameof(PictureURL));
        }

        public Int64? BayID 
        {
            get => _bayid;
            set => Set<Int64?>(ref value, ref _bayid,nameof(BayID));
        }

        public Section? Section
        {
            get => _section;
            set => Set<Section?>(ref value, ref _section,nameof(Section));
        }

        public int? BayNum
        {
            get => _baynum;
            set => Set<int?>(ref value, ref _baynum,nameof(BayNum));
        }

        public string? BayTitle 
        {
            get => _baytitle;
            set => Set<string?>(ref value, ref _baytitle,nameof(BayTitle));
        }
        #endregion

        #region Constructors
        public Bay()
        {
            Section = new();
        }

        public Bay(Int64 BayID)=>this.BayID = BayID;

        public Bay(MySqlDataReader reader)
        {
            BayID = reader.GetInt64(0);
            Section = new(reader.GetInt64(1));
            BayNum = reader.GetInt32(2);
            BayTitle = reader.GetString(3);
            PictureURL=reader.GetString(4);
        }
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => BayID==0;

        public override void SetForeignKeys()
        {
        }
        #endregion

        #region EqualsString
        public override bool Equals(object? obj) =>
        obj is Bay bay &&
        BayID == bay.BayID;

        public override int GetHashCode() => HashCode.Combine(BayID);

        public override string? ToString() => (BayID==null) ? $"BAY: N/A" : $"BAY: {BayNum} ({BayTitle})";
        #endregion

        #region IDB
        public string SQLQuery(QueryType Query)
        {
            switch (Query)
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Bay;";
                case QueryType.DELETE:
                    return string.Empty;
                case QueryType.UPDATE:
                    return string.Empty;
                case QueryType.INSERT:
                    return string.Empty;
                default: return string.Empty;
            }
        }

        public Bay GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
        }

        public void SetPrimaryKey(ulong ID)
        {
        }
        #endregion
    }
}
