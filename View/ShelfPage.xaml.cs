using MyPlanogram.Controller;
using MyPlanogram.Model;

namespace MyPlanogram.View;

public partial class ShelfPage : ContentPage, ITitle
{
    public ShelfListController Controller { get; }
    public string TopTitle { get; set ; }

    public ShelfPage()
	{
		InitializeComponent();
        Controller= (ShelfListController) BindingContext;
    }

    public ShelfPage(Bay bay) : this()
    {
        TopTitle = bay.Section.SectionName;
        Controller.Filter(bay);
    }
}