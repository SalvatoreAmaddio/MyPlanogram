using MyPlanogram.Controller;
using Testing.DB;

namespace MyPlanogram.View;

public partial class PlanogramPage : ContentPage
{
	public PlanogramListController Controller { get; }

	public PlanogramPage()
	{
		InitializeComponent();
		Controller = (PlanogramListController)BindingContext;
	}

    //        SemanticScreenReader.Announce("Done");

}

