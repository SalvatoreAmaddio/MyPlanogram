using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using MyPlanogram.Controller;
using MyPlanogram.Model;

#nullable enable
namespace MyPlanogram.View;

public partial class BayPlanogramPage : ContentPage, ITitle
{
    public PlanogramListController? Controller { get; }
    public string TopTitle { get; set; } = string.Empty;

    public BayPlanogramPage()
	{
		InitializeComponent();
        Controller = (PlanogramListController)BindingContext;
    }

    public BayPlanogramPage(Planogram planogram) : this()
    {
        TopTitle=planogram.Shelf.Bay.Section.SectionName;
        Controller?.Filter(planogram);
    }

    protected override void OnAppearing()
    {
        Lista.ScrollTo(
            Controller?.SelectedRecord,
            Controller?.SelectedGroup,
            ScrollToPosition.Start,
            true);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var obj = (Planogram)((ImageButton)sender).BindingContext;
        await Shell.Current.DisplayAlert("LOCATION:",$"{obj.Shelf.ShelfAndNotch}", "OK");
    }
}