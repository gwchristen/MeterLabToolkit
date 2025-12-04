using CommunityToolkit.Mvvm.ComponentModel;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.DataEntry;

public partial class CreatedHistoriesViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    public CreatedHistoriesViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public CreatedHistoriesViewModel() : this(new DataService())
    {
    }
}
