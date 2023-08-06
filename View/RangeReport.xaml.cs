using MyPlanogram.Controller;

namespace MyPlanogram.View;

public partial class RangeReport : ContentPage, ITitle
{
	public DepartmentListController Controller { get; }
	public string TopTitle { get; set; } = "Range Report";

    public RangeReport()
	{
		InitializeComponent();
		Controller = (DepartmentListController)BindingContext;
	}



}