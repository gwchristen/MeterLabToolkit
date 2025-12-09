using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeterLabToolkit.Models;
using MeterLabToolkit.Services;

namespace MeterLabToolkit.ViewModels.ReferenceTables;

public partial class DeviceCodeTableViewModel : ReferenceTableViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<DeviceCode> _items = new();

    [ObservableProperty]
    private System.Collections.IList? _selectedItems;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public DeviceCodeTableViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private void AddNew()
    {
        try
        {
            var newItem = new DeviceCode { Code = "", Description = "", DeviceType = "Meter" };
            Items.Add(newItem);
            ErrorMessage = "New row added. Edit the values and click Save.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error adding new item: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            int savedCount = 0;

            foreach (var item in Items)
            {
                if (string.IsNullOrWhiteSpace(item.Code))
                {
                    continue; // Skip items without a code
                }
                
                if (item.Id == 0)
                {
                    await _dataService.AddDeviceCodeAsync(item);
                    savedCount++;
                }
                else
                {
                    await _dataService.UpdateDeviceCodeAsync(item);
                    savedCount++;
                }
            }

            await LoadDataAsync();
            ErrorMessage = $"Successfully saved {savedCount} records.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedItems == null || SelectedItems.Count == 0)
        {
            ErrorMessage = "Please select one or more rows to delete.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var itemsToDelete = SelectedItems.OfType<DeviceCode>().ToList();
            int deletedCount = 0;

            foreach (var item in itemsToDelete)
            {
                if (item.Id > 0)
                {
                    await _dataService.DeleteDeviceCodeAsync(item.Id);
                }
                Items.Remove(item);
                deletedCount++;
            }

            ErrorMessage = $"{deletedCount} record(s) deleted successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error deleting: {ex.Message}";
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
            if (Items.Count == 0)
            {
                ErrorMessage = "No data to export.";
                return;
            }

            var csv = new StringBuilder();
            csv.AppendLine("Code,Description,DeviceType");
            
            foreach (var item in Items)
            {
                csv.AppendLine($"\"{item.Code}\",\"{item.Description}\",\"{item.DeviceType}\"");
            }

            var fileName = $"DeviceCodes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            
            await File.WriteAllTextAsync(filePath, csv.ToString());
            ErrorMessage = $"Exported to: {filePath}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error exporting CSV: {ex.Message}";
        }
    }

    public async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            var items = await _dataService.GetDeviceCodesAsync();
            Items = new ObservableCollection<DeviceCode>(items);
            
            if (Items.Count == 0)
            {
                ErrorMessage = "No data found. Use 'Add New' or 'Import CSV' to add records.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Error loading Device Codes: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ImportCsvAsync()
    {
        try
        {
            ErrorMessage = null;
            
            // Get the main window directly from Application
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                ErrorMessage = "Cannot access file picker";
                return;
            }

            var mainWindow = desktop.MainWindow;
            if (mainWindow == null)
            {
                ErrorMessage = "Cannot access main window";
                return;
            }

            var files = await mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Import CSV",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("CSV Files") { Patterns = new[] { "*.csv" } } }
            });

            if (files.Count == 0) return;

            var file = files[0];
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            
            var content = await reader.ReadToEndAsync();
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            int importedCount = 0;
            int duplicatesSkipped = 0;
            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                var values = ParseCsvLine(lines[i]);
                if (values.Length >= 3)
                {
                    var item = new DeviceCode 
                    { 
                        Code = values[0].Trim('"').Trim(), 
                        Description = values[1].Trim('"').Trim(),
                        DeviceType = values[2].Trim('"').Trim()
                    };
                    
                    // Check for duplicates
                    bool isDuplicate = Items.Any(existing => 
                        existing.Code.Equals(item.Code, StringComparison.OrdinalIgnoreCase) &&
                        existing.Description.Equals(item.Description, StringComparison.OrdinalIgnoreCase) &&
                        existing.DeviceType.Equals(item.DeviceType, StringComparison.OrdinalIgnoreCase));
                    
                    if (!isDuplicate)
                    {
                        Items.Add(item);
                        importedCount++;
                    }
                    else
                    {
                        duplicatesSkipped++;
                    }
                }
            }
            
            ErrorMessage = $"Successfully imported {importedCount} record(s) ({duplicatesSkipped} duplicate(s) skipped). Click Save to persist.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error importing CSV: {ex.Message}";
        }
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;
        
        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        result.Add(current.ToString());
        
        return result.ToArray();
    }
}
