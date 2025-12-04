using CommunityToolkit.Mvvm.ComponentModel;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.DataEntry;

public partial class CellularRegistrationViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    public CellularRegistrationViewModel(IDataService dataService)
    {
        _dataService = dataService;
    }

    public CellularRegistrationViewModel() : this(new DataService())
    {
    }
}
