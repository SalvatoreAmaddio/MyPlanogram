using MyPlanogram.Controller.AbstractControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Testing.Model.Abstracts;
#nullable enable
namespace Testing.Controller.AbstractControllers
{
    abstract public class AbstractDataListController<M> : AbstractDataController<M>, IDataListController where M : AbstractModel, new()
    {

        public ICommand OpenPageCMD { get; }
        public ICommand GoBackCMD { get; }

        M? _selectedrecord;
        public M? SelectedRecord
        {
            get => _selectedrecord;
            set => Set<M?>(ref value, ref _selectedrecord, nameof(SelectedRecord));
        }

        public AbstractDataListController() 
        {
            OpenPageCMD = new Command<M>(OpenPage);
            GoBackCMD = new Command(GoBack);
            BeforePropChanged += AbstractDataListController_BeforePropChanged;
            AfterPropChanged += AbstractDataListController_AfterPropChanged;
        }

        private void AbstractDataListController_BeforePropChanged(object? sender, PropChangedEvtArgs e)
        {
            if (!e.PropIs(nameof(SelectedRecord))) return;
            if (e.OldValueIsNull()) return;
            e.GetOldValue<M>().SelectedColor = Colors.Transparent;
        }

        private void AbstractDataListController_AfterPropChanged(object? sender, PropChangedEvtArgs e)
        {
            if (!e.PropIs(nameof(SelectedRecord))) return;
            if (e.ValueIsNull()) return;
            e.GetValue<M>().SelectedColor = Colors.Orange;
        }

        public abstract void GoBack();
        public abstract void OpenPage(M record);

    }
}
