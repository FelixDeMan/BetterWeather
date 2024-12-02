using Microsoft.Maui.Controls;
using BetterWeather.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BetterWeather.ViewModels;

namespace BetterWeather;

public partial class MainPage : ContentPage
{
	private readonly WeatherService _weatherService;
	private readonly WeatherChartViewModel _chartViewModel;

	public MainPage()
	{
		InitializeComponent();
		_weatherService = new WeatherService();
		_chartViewModel = new WeatherChartViewModel();
		BindingContext = _chartViewModel;
	}

	private async void CompareWeatherClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(locationEntry1.Text) || string.IsNullOrWhiteSpace(locationEntry2.Text))
		{
			await DisplayAlert("Error", "Please enter both locations", "OK");
			return;
		}

		loadingIndicator.IsVisible = true;
		loadingIndicator.IsRunning = true;

		try
		{
			var weather1 = await _weatherService.GetWeatherAsync(locationEntry1.Text);
			var weather2 = await _weatherService.GetWeatherAsync(locationEntry2.Text);

			if (weather1 == null || weather2 == null)
			{
				await DisplayAlert("Error", "Could not fetch weather data for one or both locations", "OK");
				return;
			}

			weatherInfoLabel1.Text = FormatWeatherInfo(weather1);
			weatherInfoLabel2.Text = FormatWeatherInfo(weather2);

			weatherInfoLabel1.IsVisible = true;
			weatherInfoLabel2.IsVisible = true;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			loadingIndicator.IsVisible = false;
			loadingIndicator.IsRunning = false;
		}
	}

	private async void CompareForecastClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(locationEntry1.Text) || string.IsNullOrWhiteSpace(locationEntry2.Text))
		{
				await DisplayAlert("Error", "Please enter both locations", "OK");
				return;
		}

		loadingIndicator.IsVisible = true;
		loadingIndicator.IsRunning = true;

		try
		{
			var forecast1 = await _weatherService.GetForecastAsync(locationEntry1.Text);
			var forecast2 = await _weatherService.GetForecastAsync(locationEntry2.Text);

			if (forecast1 == null || forecast2 == null || !forecast1.Any() || !forecast2.Any())
			{
				await DisplayAlert("Error", "Could not fetch forecast data for one or both locations", "OK");
				return;
			}

			_chartViewModel.UpdateForecastData(locationEntry1.Text, forecast1, locationEntry2.Text, forecast2);
			_chartViewModel.UpdatePieChartData(forecast1, forecast2);

			var comparisonText1 = FormatForecastComparison(locationEntry1.Text, forecast1);
			var comparisonText2 = FormatForecastComparison(locationEntry2.Text, forecast2);

			weatherInfoLabel1.Text = comparisonText1;
			weatherInfoLabel2.Text = comparisonText2;

			weatherInfoLabel1.IsVisible = true;
			weatherInfoLabel2.IsVisible = true;
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
			loadingIndicator.IsVisible = false;
			loadingIndicator.IsRunning = false;
		}
	}

	private string FormatWeatherInfo(WeatherData weather)
	{
		return $"{weather.CityName}\n" +
			   $"{weather.Temperature:F1}°C\n" +
			   $"{weather.Description}\n" +
			   $"Humidity: {weather.Humidity}%\n" +
			   $"Wind: {weather.WindSpeed:F1} m/s";
	}

	private string FormatForecastComparison(string city, List<ForecastData> forecast)
	{
		var avgTemp = forecast.Average(h => h.Temperature);
		var avgHumidity = forecast.Average(h => h.Humidity);
		var avgWind = forecast.Average(h => h.WindSpeed);

		return $"{city}\n" +
			   $"5-Day Averages:\n" +
			   $"Temp: {avgTemp:F1}°C\n" +
			   $"Humidity: {avgHumidity:F0}%\n" +
			   $"Wind: {avgWind:F1} m/s";
	}
}

