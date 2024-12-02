using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BetterWeather.Services;
using System.Diagnostics;

namespace BetterWeather.ViewModels;

public class PieSeriesViewModel : PieSeries<double>
{
    public Color ColorForLegend { get; set; }
}

public class WeatherChartViewModel : INotifyPropertyChanged
{
    private ISeries[] _series;
    private ISeries[] _pieSeries1;
    private ISeries[] _pieSeries2;
    private Axis[] _xAxes;
    private Axis[] _yAxes;
    private string _city1Name = "City 1";
    private string _city2Name = "City 2";
    private readonly Random _random = new Random();
    private readonly List<float> _usedHues = new();

    public WeatherChartViewModel()
    {
        // Initialize empty series arrays
        _series = Array.Empty<ISeries>();
        _xAxes = Array.Empty<Axis>();
        _yAxes = Array.Empty<Axis>();

        // Initialize pie charts with dummy data
        var dummySeries = new ISeries[]
        {
            new PieSeriesViewModel
            {
                Values = new double[] { 1 },
                Name = "Loading...",
                Fill = new SolidColorPaint(SKColors.Gray),
                ColorForLegend = Colors.Gray
            }
        };

        _pieSeries1 = dummySeries;
        _pieSeries2 = dummySeries;
    }

    private SKColor GenerateRandomColor(string key)
    {
        float hue;
        if (_usedHues.Count == 0)
        {
            // First color - pick a random starting hue
            hue = _random.Next(360);
        }
        else
        {
            // Calculate the largest gap in the used hues
            _usedHues.Sort();
            float largestGap = 0;
            float gapStart = 0;
            
            // Check gap between last and first (wrapping around 360)
            float gap = (360 - _usedHues[^1]) + _usedHues[0];
            if (gap > largestGap)
            {
                largestGap = gap;
                gapStart = _usedHues[^1];
            }

            // Check gaps between consecutive hues
            for (int i = 0; i < _usedHues.Count - 1; i++)
            {
                gap = _usedHues[i + 1] - _usedHues[i];
                if (gap > largestGap)
                {
                    largestGap = gap;
                    gapStart = _usedHues[i];
                }
            }

            // Place new hue in middle of largest gap
            hue = (gapStart + (largestGap / 2)) % 360;
        }

        _usedHues.Add(hue);

        // Generate a vibrant color with good saturation and brightness
        var saturation = 85f; // Fixed high saturation for vibrant colors
        var brightness = 75f; // Fixed medium-high brightness for visibility

        var color = SKColor.FromHsv(
            hue,
            saturation,
            brightness
        );

        Debug.WriteLine($"Generated color for {key}: H:{hue} S:{saturation} V:{brightness}");
        return color;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ISeries[] Series
    {
        get => _series;
        set
        {
            if (_series != value)
            {
                _series = value;
                OnPropertyChanged();
            }
        }
    }

    public ISeries[] PieSeries1
    {
        get => _pieSeries1;
        set
        {
            if (_pieSeries1 != value)
            {
                _pieSeries1 = value;
                OnPropertyChanged();
            }
        }
    }

    public ISeries[] PieSeries2
    {
        get => _pieSeries2;
        set
        {
            if (_pieSeries2 != value)
            {
                _pieSeries2 = value;
                OnPropertyChanged();
            }
        }
    }

    public string City1Name
    {
        get => _city1Name;
        set
        {
            if (_city1Name != value)
            {
                _city1Name = value;
                OnPropertyChanged();
            }
        }
    }

    public string City2Name
    {
        get => _city2Name;
        set
        {
            if (_city2Name != value)
            {
                _city2Name = value;
                OnPropertyChanged();
            }
        }
    }

    public Axis[] XAxes
    {
        get => _xAxes;
        set
        {
            if (_xAxes != value)
            {
                _xAxes = value;
                OnPropertyChanged();
            }
        }
    }

    public Axis[] YAxes
    {
        get => _yAxes;
        set
        {
            if (_yAxes != value)
            {
                _yAxes = value;
                OnPropertyChanged();
            }
        }
    }

    public void UpdateForecastData(string city1, List<ForecastData> forecast1, string city2, List<ForecastData> forecast2)
    {
        Debug.WriteLine($"Updating forecast data for {city1} and {city2}");
        
        City1Name = city1;
        City2Name = city2;

        // Clear color cache and used hues when updating data
        _usedHues.Clear();

        var newSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = forecast1.Take(5).Select(f => f.Temperature).ToArray(),
                Name = city1,
                Fill = null,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.DodgerBlue, 2)
            },
            new LineSeries<double>
            {
                Values = forecast2.Take(5).Select(f => f.Temperature).ToArray(),
                Name = city2,
                Fill = null,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.OrangeRed, 2)
            }
        };

        var xLabels = forecast1.Take(5).Select(f => f.DateTime.ToString("MM/dd")).ToArray();

        Series = newSeries;
        XAxes = new[]
        {
            new Axis
            {
                Labels = xLabels,
                NamePaint = new SolidColorPaint(SKColors.DarkSlateGray),
                LabelsPaint = new SolidColorPaint(SKColors.DarkSlateGray)
            }
        };
        YAxes = new[]
        {
            new Axis
            {
                Name = "Temperature (°C)",
                NamePaint = new SolidColorPaint(SKColors.DarkSlateGray),
                LabelsPaint = new SolidColorPaint(SKColors.DarkSlateGray)
            }
        };

        UpdatePieChartData(forecast1, forecast2);
    }

    public void UpdatePieChartData(List<ForecastData> forecast1, List<ForecastData> forecast2)
    {
        Debug.WriteLine("Updating pie chart data");
        Debug.WriteLine($"Forecast 1 count: {forecast1.Count}");
        Debug.WriteLine($"Forecast 2 count: {forecast2.Count}");

        var series1 = CreatePieSeriesForForecast(forecast1);
        var series2 = CreatePieSeriesForForecast(forecast2);

        Debug.WriteLine($"Created pie series 1 with {series1.Length} segments");
        Debug.WriteLine($"Created pie series 2 with {series2.Length} segments");

        PieSeries1 = series1;
        PieSeries2 = series2;

        // Force refresh
        OnPropertyChanged(nameof(PieSeries1));
        OnPropertyChanged(nameof(PieSeries2));
    }

    private ISeries[] CreatePieSeriesForForecast(List<ForecastData> forecast)
    {
        var weatherDistribution = forecast
            .GroupBy(f => f.Description.ToLower())
            .Select(g => new { Description = g.Key, Count = g.Count() })
            .ToList();

        Debug.WriteLine($"Weather distribution has {weatherDistribution.Count} unique conditions:");
        foreach (var item in weatherDistribution)
        {
            Debug.WriteLine($"- {item.Description}: {item.Count}");
        }

        var series = weatherDistribution.Select(w =>
        {
            var skColor = GenerateRandomColor(w.Description);
            var color = new Color((float)skColor.Red / 255f, 
                                (float)skColor.Green / 255f, 
                                (float)skColor.Blue / 255f, 
                                (float)skColor.Alpha / 255f);
            
            return new PieSeriesViewModel
            {
                Values = new[] { (double)w.Count },
                Name = char.ToUpper(w.Description[0]) + w.Description.Substring(1),
                Fill = new SolidColorPaint(skColor),
                ColorForLegend = color,
                IsVisible = true
            };
        }).ToArray();

        return series;
    }
} 