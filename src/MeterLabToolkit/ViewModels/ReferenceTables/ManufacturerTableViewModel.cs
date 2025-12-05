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

public partial class ManufacturerTableViewModel : ReferenceTableViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<ManufacturerCode> _items = new();

    [ObservableProperty]
    private ManufacturerCode? _selectedItem;

    [ObservableProperty]
    private bool _isLoading;

    public ManufacturerTableViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private void AddNew()
    {
        var newItem = new ManufacturerCode { Code = "", IndustryCode = "", Name = "" };
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
                    await _dataService.AddManufacturerCodeAsync(item);
                }
                else
                {
                    await _dataService.UpdateManufacturerCodeAsync(item);
                }
            }

            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving Manufacturer Codes: {ex.Message}");
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
                await _dataService.DeleteManufacturerCodeAsync(SelectedItem.Id);
            }

            Items.Remove(SelectedItem);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deleting Manufacturer Code: {ex.Message}");
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
            var csv = "Code,IndustryCode,Name\n";
            foreach (var item in Items)
            {
                csv += $"\"{item.Code}\",\"{item.IndustryCode}\",\"{item.Name}\"\n";
            }

            var fileName = $"ManufacturerCodes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
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
            var items = await _dataService.GetManufacturerCodesAsync();
            Items = new ObservableCollection<ManufacturerCode>(items);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading Manufacturer Codes: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
