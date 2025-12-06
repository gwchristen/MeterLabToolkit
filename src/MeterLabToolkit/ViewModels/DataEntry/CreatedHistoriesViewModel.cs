using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeterLabToolkit.Models;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.DataEntry;

public partial class CreatedHistoriesViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    // Tab selection
    [ObservableProperty]
    private int _selectedTabIndex;

    // Collections
    [ObservableProperty]
    private ObservableCollection<CreatedHistory> _histories = new();

    [ObservableProperty]
    private ObservableCollection<Status> _statuses = new();

    [ObservableProperty]
    private ObservableCollection<ManufacturerCode> _manufacturers = new();

    [ObservableProperty]
    private ObservableCollection<DeviceCode> _deviceCodes = new();

    [ObservableProperty]
    private ObservableCollection<PurchaseCode> _purchaseCodes = new();

    // Selected item
    [ObservableProperty]
    private CreatedHistory? _selectedHistory;

    // Form fields
    [ObservableProperty]
    private string _selectedOpCo = "Ohio";

    [ObservableProperty]
    private string _selectedDeviceType = "Meter";

    [ObservableProperty]
    private Status? _selectedStatus;

    [ObservableProperty]
    private ManufacturerCode? _selectedManufacturer;

    [ObservableProperty]
    private DeviceCode? _selectedDevCode;

    [ObservableProperty]
    private string _begSer = string.Empty;

    [ObservableProperty]
    private string _endSer = string.Empty;

    [ObservableProperty]
    private string _oor = string.Empty;

    [ObservableProperty]
    private int _qty;

    [ObservableProperty]
    private DateTimeOffset? _poDate = DateTimeOffset.Now;

    [ObservableProperty]
    private string _poNumber = string.Empty;

    [ObservableProperty]
    private DateTimeOffset? _recvDate = DateTimeOffset.Now;

    [ObservableProperty]
    private decimal _unitCost;

    [ObservableProperty]
    private string _cid = string.Empty;

    [ObservableProperty]
    private string _meNumber = string.Empty;

    [ObservableProperty]
    private PurchaseCode? _selectedPurCode;

    [ObservableProperty]
    private DateTimeOffset? _established = DateTimeOffset.Now;

    [ObservableProperty]
    private string _notes = string.Empty;

    // Search
    [ObservableProperty]
    private string _searchText = string.Empty;

    // Status message for validation and save feedback
    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private bool _isError;

    private List<CreatedHistory> _allHistories = new();

    public CreatedHistoriesViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = InitializeAsync();
    }

    public CreatedHistoriesViewModel() : this(new DataService())
    {
    }

    private async Task InitializeAsync()
    {
        try
        {
            await LoadReferenceDataAsync();
            await LoadHistoriesAsync();
        }
        catch (Exception ex)
        {
            // Log initialization errors
            System.Diagnostics.Debug.WriteLine($"Initialization error: {ex.Message}");
        }
    }

    private async Task LoadReferenceDataAsync()
    {
        var statuses = await _dataService.GetStatusesAsync();
        Statuses = new ObservableCollection<Status>(statuses);

        var manufacturers = await _dataService.GetManufacturerCodesAsync();
        Manufacturers = new ObservableCollection<ManufacturerCode>(manufacturers);

        var purchaseCodes = await _dataService.GetPurchaseCodesAsync();
        PurchaseCodes = new ObservableCollection<PurchaseCode>(purchaseCodes);

        await UpdateDeviceCodesForCurrentTab();
    }

    private async Task UpdateDeviceCodesForCurrentTab()
    {
        var deviceCodes = await _dataService.GetDeviceCodesAsync(SelectedDeviceType);
        DeviceCodes = new ObservableCollection<DeviceCode>(deviceCodes);
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        // Map tab index to OpCo and DeviceType
        switch (value)
        {
            case 0: // Ohio - Meters
                SelectedOpCo = "Ohio";
                SelectedDeviceType = "Meter";
                break;
            case 1: // I&M - Meters
                SelectedOpCo = "I&M";
                SelectedDeviceType = "Meter";
                break;
            case 2: // Ohio - Transformers
                SelectedOpCo = "Ohio";
                SelectedDeviceType = "Transformer";
                break;
            case 3: // I&M - Transformers
                SelectedOpCo = "I&M";
                SelectedDeviceType = "Transformer";
                break;
        }

        _ = UpdateDeviceCodesForCurrentTab();
        _ = LoadHistoriesAsync();
    }

    private async Task LoadHistoriesAsync()
    {
        var histories = await _dataService.GetCreatedHistoriesAsync(SelectedOpCo, SelectedDeviceType);
        _allHistories = histories;
        FilterHistories();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterHistories();
    }

    private void FilterHistories()
    {
        var filtered = _allHistories.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower();
            filtered = filtered.Where(h =>
                h.DevCode.ToLower().Contains(searchLower) ||
                (h.BegSer?.ToLower().Contains(searchLower) ?? false) ||
                (h.EndSer?.ToLower().Contains(searchLower) ?? false) ||
                (h.OOR?.ToLower().Contains(searchLower) ?? false) ||
                h.PONumber.ToLower().Contains(searchLower) ||
                h.CID.ToLower().Contains(searchLower) ||
                h.MENumber.ToLower().Contains(searchLower));
        }

        Histories = new ObservableCollection<CreatedHistory>(filtered);
    }

    partial void OnSelectedHistoryChanged(CreatedHistory? value)
    {
        if (value != null)
        {
            PopulateFormFromHistory(value);
        }
    }

    private void PopulateFormFromHistory(CreatedHistory history)
    {
        SelectedStatus = Statuses.FirstOrDefault(s => s.Code == history.Status);
        SelectedManufacturer = Manufacturers.FirstOrDefault(m => m.Code == history.MFR);
        SelectedDevCode = DeviceCodes.FirstOrDefault(d => d.Code == history.DevCode);
        BegSer = history.BegSer ?? string.Empty;
        EndSer = history.EndSer ?? string.Empty;
        Oor = history.OOR ?? string.Empty;
        Qty = history.Qty;
        PoDate = new DateTimeOffset(history.PODate);
        PoNumber = history.PONumber;
        RecvDate = new DateTimeOffset(history.RecvDate);
        UnitCost = history.UnitCost;
        Cid = history.CID;
        MeNumber = history.MENumber;
        SelectedPurCode = PurchaseCodes.FirstOrDefault(p => p.Code == history.PurCode);
        Established = new DateTimeOffset(history.Established);
        Notes = history.Notes ?? string.Empty;
    }

    partial void OnBegSerChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Oor = string.Empty;
        }
        CalculateQty();
    }

    partial void OnEndSerChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Oor = string.Empty;
        }
        CalculateQty();
    }

    partial void OnOorChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            BegSer = string.Empty;
            EndSer = string.Empty;
        }
        CalculateQty();
    }

    private void CalculateQty()
    {
        Qty = 0;

        // Calculate from BegSer and EndSer
        if (!string.IsNullOrWhiteSpace(BegSer) && !string.IsNullOrWhiteSpace(EndSer))
        {
            if (long.TryParse(BegSer, out long begValue) && long.TryParse(EndSer, out long endValue))
            {
                // Validate that BegSer <= EndSer
                if (begValue <= endValue)
                {
                    Qty = (int)(endValue - begValue + 1);
                }
                else
                {
                    // Invalid range - EndSer is less than BegSer
                    Qty = 0;
                }
            }
            return;
        }

        // Calculate from OOR
        if (!string.IsNullOrWhiteSpace(Oor))
        {
            var segments = Oor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var segment in segments)
            {
                if (segment.Contains('-'))
                {
                    var parts = segment.Split('-', StringSplitOptions.TrimEntries);
                    if (parts.Length == 2 &&
                        long.TryParse(parts[0], out long start) &&
                        long.TryParse(parts[1], out long end))
                    {
                        // Validate range
                        if (start <= end)
                        {
                            Qty += (int)(end - start + 1);
                        }
                    }
                }
                else
                {
                    if (long.TryParse(segment, out _))
                    {
                        Qty++;
                    }
                }
            }
        }
    }

    [RelayCommand]
    private void AddNew()
    {
        SelectedHistory = null;
        ClearForm();
    }

    private void ClearForm()
    {
        SelectedStatus = null;
        SelectedManufacturer = null;
        SelectedDevCode = null;
        BegSer = string.Empty;
        EndSer = string.Empty;
        Oor = string.Empty;
        Qty = 0;
        PoDate = DateTimeOffset.Now;
        PoNumber = string.Empty;
        RecvDate = DateTimeOffset.Now;
        UnitCost = 0;
        Cid = string.Empty;
        MeNumber = string.Empty;
        SelectedPurCode = null;
        Established = DateTimeOffset.Now;
        Notes = string.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        StatusMessage = null;
        IsError = false;
        
        if (!ValidateForm())
        {
            return;
        }

        try
        {
            var history = SelectedHistory ?? new CreatedHistory();

            // Populate from form
            history.OpCo = SelectedOpCo;
            history.DeviceType = SelectedDeviceType;
            history.Status = SelectedStatus?.Code ?? string.Empty;
            history.MFR = SelectedManufacturer?.Code ?? string.Empty;
            history.DevCode = SelectedDevCode?.Code ?? string.Empty;
            history.BegSer = string.IsNullOrWhiteSpace(BegSer) ? null : BegSer;
            history.EndSer = string.IsNullOrWhiteSpace(EndSer) ? null : EndSer;
            history.OOR = string.IsNullOrWhiteSpace(Oor) ? null : Oor;
            history.Qty = Qty;
            history.PODate = PoDate?.DateTime ?? DateTime.Now;
            history.PONumber = PoNumber;
            history.RecvDate = RecvDate?.DateTime ?? DateTime.Now;
            history.UnitCost = UnitCost;
            history.CID = Cid;
            history.MENumber = MeNumber;
            history.PurCode = SelectedPurCode?.Code ?? string.Empty;
            history.Established = Established?.DateTime ?? DateTime.Now;
            history.Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes;

            if (SelectedHistory == null)
            {
                await _dataService.AddCreatedHistoryAsync(history);
                StatusMessage = "Record saved successfully!";
            }
            else
            {
                await _dataService.UpdateCreatedHistoryAsync(history);
                StatusMessage = "Record updated successfully!";
            }
            
            IsError = false;
            await LoadHistoriesAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving record: {ex.Message}";
            IsError = true;
            System.Diagnostics.Debug.WriteLine($"Save error: {ex}");
        }
    }

    private bool ValidateForm()
    {
        // Required field validation
        if (SelectedStatus == null)
        {
            StatusMessage = "Please select a Status.";
            IsError = true;
            return false;
        }
        
        if (SelectedManufacturer == null)
        {
            StatusMessage = "Please select a Manufacturer.";
            IsError = true;
            return false;
        }
        
        if (SelectedDevCode == null)
        {
            StatusMessage = "Please select a Device Code.";
            IsError = true;
            return false;
        }
        
        if (SelectedPurCode == null)
        {
            StatusMessage = "Please select a Purchase Code.";
            IsError = true;
            return false;
        }

        if (string.IsNullOrWhiteSpace(PoNumber))
        {
            StatusMessage = "PO Number is required.";
            IsError = true;
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(Cid))
        {
            StatusMessage = "CID is required.";
            IsError = true;
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(MeNumber))
        {
            StatusMessage = "ME Number is required.";
            IsError = true;
            return false;
        }

        // Date validation
        if (!PoDate.HasValue)
        {
            StatusMessage = "PO Date is required.";
            IsError = true;
            return false;
        }
        
        if (!RecvDate.HasValue)
        {
            StatusMessage = "Received Date is required.";
            IsError = true;
            return false;
        }
        
        if (!Established.HasValue)
        {
            StatusMessage = "Established Date is required.";
            IsError = true;
            return false;
        }

        // Serial validation
        var hasBegEnd = !string.IsNullOrWhiteSpace(BegSer) && !string.IsNullOrWhiteSpace(EndSer);
        var hasOor = !string.IsNullOrWhiteSpace(Oor);

        if (!hasBegEnd && !hasOor)
        {
            StatusMessage = "Enter either Beginning/Ending Serial OR Out of Range serials.";
            IsError = true;
            return false;
        }

        if (hasBegEnd && hasOor)
        {
            StatusMessage = "Cannot have both Beginning/Ending Serial AND Out of Range serials.";
            IsError = true;
            return false;
        }

        if (Qty <= 0)
        {
            StatusMessage = "Quantity must be greater than zero.";
            IsError = true;
            return false;
        }

        return true;
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedHistory == null)
        {
            return;
        }

        // In a real app, you'd show a confirmation dialog here
        await _dataService.DeleteCreatedHistoryAsync(SelectedHistory.Id);
        await LoadHistoriesAsync();
        ClearForm();
    }

    [RelayCommand]
    private void ExportCsv()
    {
        // Export current tab's data to CSV
        // This would require file dialog and CSV writing logic
        // Placeholder for now
    }
}
