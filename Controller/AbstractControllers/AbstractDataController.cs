using MyPlanogram.Controller;
using MyPlanogram.Controller.AbstractControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Model.Abstracts;
using Testing.RecordSource;
#nullable enable

namespace Testing.Controller.AbstractControllers
{
    abstract public class AbstractDataController<M> : AbstractNotifier, IDataController where M : AbstractModel,new()
    {
        string? _search=string.Empty;
        public string? Search { get => _search; set => Set<string>(ref value, ref _search,nameof(Search)); }

        M? _record;
        public M? Record 
        {
            get=>_record;
            set => Set<M?>(ref value, ref _record,nameof(Record));
        }

        public RecordSource<M> RecordSource { get; set; } = new();

        public AbstractDataController() { }

    }
}
