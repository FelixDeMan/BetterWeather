using Microsoft.Maui.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Diagnostics;

namespace BetterWeather
{
	public partial class App : Application
	{
		public App(MainPage mainPage)
		{
			try
			{
				Debug.WriteLine("=== App Constructor Start ===");
				InitializeComponent();
				Debug.WriteLine("App InitializeComponent completed");

				// Configure LiveCharts
				Debug.WriteLine("Configuring LiveCharts...");
				LiveCharts.Configure(config => 
				{
					Debug.WriteLine("Inside LiveCharts configuration");
					config.AddSkiaSharp().AddDefaultMappers();
				});
				Debug.WriteLine("LiveCharts configuration completed");

				MainPage = new NavigationPage(mainPage);
				Debug.WriteLine("Navigation page set");
				Debug.WriteLine("=== App Constructor Completed Successfully ===");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"ERROR in App constructor: {ex.Message}");
				Debug.WriteLine($"Stack trace: {ex.StackTrace}");
			}
		}

		protected override void OnStart()
		{
			try
			{
				Debug.WriteLine("=== App OnStart ===");
				base.OnStart();
				Debug.WriteLine("=== App OnStart Completed ===");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"ERROR in OnStart: {ex.Message}");
			}
		}
	}
}
