using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private bool _isBottomPanelVisible;

    public MainWindowViewModel(IDataService dataService)
    {
        _dataService = dataService;
        
        // Initialize database on startup - fire and forget is acceptable here
        // as this is startup initialization
        _ = InitializeDatabaseAsync();
    }

    public MainWindowViewModel() : this(new DataService())
    {
    }

    private async Task InitializeDatabaseAsync()
    {
        try
        {
            await _dataService.InitializeDatabaseAsync();
        }
        catch (Exception ex)
        {
            // Log or handle database initialization errors
            // In a production app, you might want to show an error dialog
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
        }
    }

    [RelayCommand]
    private void NavigateToView(string viewName)
    {
        CurrentView = viewName switch
        {
            "CreatedHistories" => new DataEntry.CreatedHistoriesViewModel(_dataService),
            "CellularRegistration" => new DataEntry.CellularRegistrationViewModel(_dataService),
            "RmaEntries" => new DataEntry.RmaEntriesViewModel(_dataService),
            "CsvDateUpdater" => new Tools.CsvDateUpdaterViewModel(),
            "SerialRangeGenerator" => new Tools.SerialRangeGeneratorViewModel(),
            "RangeSplitter" => new Tools.RangeSplitterViewModel(),
            "AepBarcodeGenerator" => new Tools.AepBarcodeGeneratorViewModel(),
            _ => CurrentView
        };
    }

    [RelayCommand]
    private void ToggleBottomPanel()
    {
        IsBottomPanelVisible = !IsBottomPanelVisible;
    }

    public void Dispose()
    {
        if (_dataService is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
