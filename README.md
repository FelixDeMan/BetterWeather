# BetterWeather

A modern weather comparison application built with .NET MAUI, allowing users to compare weather conditions and forecasts between different cities with intuitive visualizations.

## Features

- **City Weather Comparison**: Compare current weather conditions between any two cities worldwide
- **5-Day Forecast**: View and compare 5-day weather forecasts with daily data points
- **Interactive Charts**:
  - Temperature trend line chart showing forecast comparisons
  - Weather distribution pie charts showing weather condition patterns
  - Color-coded legends for easy interpretation
- **Clean, Modern UI**: Theme-independent design that looks great in both light and dark modes

## Tech Stack

- **.NET MAUI**: Cross-platform UI framework (.NET 8.0)
- **LiveCharts2**: Modern data visualization library for .NET
- **OpenWeatherMap API**: Weather data provider
- **C# 12**: Programming language
- **MVVM Architecture**: For clean separation of concerns


## Supported Platforms

- macOS 13.0 or later (Mac Catalyst)


## Building and Running

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/BetterWeather.git
   cd BetterWeather
   ```

2. Add your OpenWeatherMap API key:
   - Get a free API key from [OpenWeatherMap](https://openweathermap.org/api)
   - Replace the API_KEY constant in `Services/WeatherService.cs`

3. Build and run:
   ```bash
   dotnet build
   dotnet run --framework net8.0-maccatalyst
   ```

   Or open the solution in Visual Studio and press F5.


## Project Structure

- `Services/`: API and data services
- `ViewModels/`: MVVM view models
- `Models/`: Data models
- `Views/`: XAML views and pages

## Future Improvements

1. **Enhanced Weather Data**:
   - Historical weather data comparison
   - Hourly forecast views
   - Weather alerts and notifications
   - Air quality information

2. **Additional Visualizations**:
   - Wind direction compass
   - Precipitation probability charts
   - Humidity and pressure trends
   - Temperature heat maps

3. **User Experience**:
   - Favorite locations
   - Location auto-complete
   - Geolocation support
   - Custom themes and color schemes
   - Widgets for quick access

4. **Data Export**:
   - Export comparisons as PDF
   - Share weather reports
   - Data export in various formats

5. **Advanced Features**:
   - Weather pattern analysis
   - Multiple city comparison (>2)
   - Offline mode support
   - Custom comparison metrics

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- OpenWeatherMap for weather data
- LiveCharts2 for visualization components
- .NET MAUI team for the framework 