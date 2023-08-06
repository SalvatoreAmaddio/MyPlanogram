using MyPlanogram.Model;
using MyPlanogram.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testing.Controller.AbstractControllers;
using Testing.DB;

namespace MyPlanogram.Controller
{
    public class BayListController : AbstractDataListController<Bay>
    {
        private Section Section { get; set; }

        public BayListController() {

        }

        public async override void OpenPage(Bay record)
        {
            if (record == null) return;
            record.Section = Section;
            await Shell.Current.Navigation.PushAsync(new ShelfPage(record));
            SelectedRecord = null;
        }

        public void Filter(Section section)
        {
            Section = section;
            RecordSource.ReplaceRange(MainDB.DBBay.RecordSource.Where(s=>s.Section.Equals(section)).OrderBy(s=>s.BayNum));
            SelectedRecord = RecordSource.FirstOrDefault();
            AfterPropChanged += BayListController_AfterPropChanged;
        }

        private void BayListController_AfterPropChanged(object sender, Testing.Model.Abstracts.PropChangedEvtArgs e)
        {
            if (e.PropIs(nameof(SelectedRecord)))
            {
                OpenPage(e.GetValue<Bay>());
            }
        }

        public override void GoBack()
        {
        }
    }
}
