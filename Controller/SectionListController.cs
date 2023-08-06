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
    public class SectionListController : AbstractDataListController<Section>
    {
        public SectionListController() 
        {
            RecordSource.ReplaceRange(MainDB.DBSection.RecordSource);
            SelectedRecord = RecordSource.FirstOrDefault();
            AfterPropChanged += SectionListController_AfterPropChanged;
        }

        private void SectionListController_AfterPropChanged(object sender, Testing.Model.Abstracts.PropChangedEvtArgs e)
        {
            if (e.PropIs(nameof(SelectedRecord)))
            {
                OpenPage(e.GetValue<Section>());
                return;
            }

            if (e.PropIs(nameof(Search)))
            {
                RecordSource.ReplaceRange(MainDB.DBSection.RecordSource.Where(s=>s.SectionName.ToLower().Contains(e.GetValue<string>().ToLower())));
                return;
            }
        }

        public override void GoBack()
        {
        }

        public async override void OpenPage(Section record)
        {
            if (record == null) return;
            if (Shell.Current == null) return;
            if (Shell.Current.Navigation == null) return;
            await Shell.Current.Navigation.PushAsync(new BayPage(record));
            SelectedRecord = null;
        }
    }
}
