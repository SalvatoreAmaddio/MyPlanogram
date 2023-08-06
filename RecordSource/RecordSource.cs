using MvvmHelpers;
using MyPlanogram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Model.Abstracts;

namespace Testing.RecordSource
{
    public class RecordSource<M> : ObservableRangeCollection<M> where M : AbstractModel
    {

        public RecordSource() { }

        public RecordSource(IEnumerable<M> source) : base(source)
        {
        }
    }
}
