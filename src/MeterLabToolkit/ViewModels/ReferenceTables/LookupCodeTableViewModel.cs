using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeterLabToolkit.Models;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.ReferenceTables;

public partial class LookupCodeTableViewModel : ReferenceTableViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<LookupCode> _items = new();

    [ObservableProperty]
    private LookupCode? _selectedItem;

    [ObservableProperty]
    private bool _isLoading;

    public LookupCodeTableViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private void AddNew()
    {
        var newItem = new LookupCode { Code = "", Description = "" };
        Items.Add(newItem);
        SelectedItem = newItem;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;

            foreach (var item in Items)
            {
                if (item.Id == 0)
                {
                    await _dataService.AddLookupCodeAsync(item);
                }
                else
                {
                    await _dataService.UpdateLookupCodeAsync(item);
                }
            }

            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving Lookup Codes: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedItem == null) return;

        try
        {
            IsLoading = true;

            if (SelectedItem.Id > 0)
            {
                await _dataService.DeleteLookupCodeAsync(SelectedItem.Id);
            }

            Items.Remove(SelectedItem);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting Lookup Code: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ExportCsvAsync()
    {
        try
        {
            var csv = "Code,Description\n";
            foreach (var item in Items)
            {
                csv += $"\"{item.Code}\",\"{item.Description}\"\n";
            }

            var fileName = $"LookupCodes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            
            await File.WriteAllTextAsync(filePath, csv);
            System.Diagnostics.Debug.WriteLine($"Exported to: {filePath}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error exporting CSV: {ex.Message}");
        }
    }

    public async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            var items = await _dataService.GetLookupCodesAsync();
            Items = new ObservableCollection<LookupCode>(items);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading Lookup Codes: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
