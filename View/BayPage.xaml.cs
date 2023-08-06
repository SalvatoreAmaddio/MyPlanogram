using MyPlanogram.Controller;
using MyPlanogram.Model;

namespace MyPlanogram.View;

public partial class BayPage : ContentPage, ITitle
{
	public BayListController Controller { get; }

    public string TopTitle { get; set; }

    public BayPage()
	{
		InitializeComponent();
		Controller = (BayListController) BindingContext;
	}

	public BayPage(Section section) : this()
	{
		TopTitle = section.SectionName;
		Controller.Filter(section);
	}
}