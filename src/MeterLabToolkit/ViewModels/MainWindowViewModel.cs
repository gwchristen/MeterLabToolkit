using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    [ObservableProperty]
    private bool _isBottomPanelVisible;

    public MainWindowViewModel(IDataService dataService)
    {
        _dataService = dataService;
        
        // Initialize database on startup
        InitializeDatabaseAsync();
    }

    public MainWindowViewModel() : this(new DataService())
    {
    }

    private async void InitializeDatabaseAsync()
    {
        await _dataService.InitializeDatabaseAsync();
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
}
