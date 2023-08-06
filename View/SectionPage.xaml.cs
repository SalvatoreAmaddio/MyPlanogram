using MyPlanogram.Controller;

namespace MyPlanogram.View;

public partial class SectionPage : ContentPage
{
	public SectionListController Controller { get; }

	public SectionPage()
	{
		InitializeComponent();
		Controller = (SectionListController) BindingContext;
	}

}