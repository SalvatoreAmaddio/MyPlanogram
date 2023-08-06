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
    public class Offer : AbstractModel, IDB<Offer>
    {
        #region backprops
        Int64 _offerid;
        string _offertitle;
        #endregion

        #region Props
        public Int64 OfferID { get => _offerid; set => Set<Int64>(ref value, ref _offerid,nameof(OfferID)); }
        public string OfferTitle { get => _offertitle; set => Set<string>(ref value, ref _offertitle, nameof(OfferTitle)); }
        #endregion

        #region constructors
        public Offer()
        {

        }

        public Offer(Int64 OfferID)=>this.OfferID=OfferID;

        public Offer(MySqlDataReader reader)
        {
            OfferID = reader.GetInt64(0);
            OfferTitle = reader.GetString(1);
        }
        #endregion

        #region AbstractModel
        public override bool IsNewRecord => OfferID == 0;

        public override void SetForeignKeys()
        {
        }
        #endregion

        #region EqualsToString
        public override bool Equals(object obj)=>
        obj is Offer offer && OfferID == offer.OfferID;
        
        public override int GetHashCode()=>HashCode.Combine(OfferID);

        public override string ToString() => OfferTitle;
        #endregion

        #region IDB
        public string SQLQuery(QueryType Query)
        {
            switch (Query)
            {
                case QueryType.SELECT:
                    return "SELECT * FROM Offer;";
                case QueryType.DELETE:
                    return string.Empty;
                case QueryType.UPDATE:
                    return string.Empty;
                case QueryType.INSERT:
                    return string.Empty;
                default: return string.Empty;
            }
        }

        public Offer GetRecord(MySqlDataReader reader) => new(reader);

        public void Params(MySqlParameterCollection param)
        {
            
        }

        public void SetPrimaryKey(ulong ID)
        {
        }
        #endregion

    }
}
