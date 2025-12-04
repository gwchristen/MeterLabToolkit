using CommunityToolkit.Mvvm.ComponentModel;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.DataEntry;

public partial class RmaEntriesViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    public RmaEntriesViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public RmaEntriesViewModel() : this(new DataService())
    {
    }
}
