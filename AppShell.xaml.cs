using System.Diagnostics;

namespace BetterWeather;

public partial class AppShell : Shell
{
	public AppShell()
	{
		try
		{
			Debug.WriteLine("AppShell constructor - Starting initialization");
			InitializeComponent();
			
			// Register routes
			Routing.RegisterRoute("mainpage", typeof(MainPage));
			
			// Set the default route
			Current.GoToAsync("///mainpage");
			
			Debug.WriteLine("AppShell constructor - Completed successfully");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error in AppShell constructor: {ex}");
			throw;
		}
	}
}
