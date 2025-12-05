using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

public partial class ManufacturerTableViewModel : ReferenceTableViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<ManufacturerCode> _items = new();

    [ObservableProperty]
    private ManufacturerCode? _selectedItem;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

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
            ErrorMessage = null;
            var items = await _dataService.GetManufacturerCodesAsync();
            Items = new ObservableCollection<ManufacturerCode>(items);
            
            if (Items.Count == 0)
            {
                ErrorMessage = "No data found. Use 'Add New' or 'Import CSV' to add records.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Error loading Manufacturer Codes: {ex.Message}");
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
            // Use Avalonia's file picker
            var topLevel = TopLevel.GetTopLevel(App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop 
                ? desktop.MainWindow : null);
            
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Import CSV",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("CSV Files") { Patterns = new[] { "*.csv" } } }
            });

            if (files.Count == 0) return;

            var file = files[0];
            using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream);
            
            var content = await reader.ReadToEndAsync();
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                var values = ParseCsvLine(lines[i]);
                if (values.Length >= 3)
                {
                    var item = new ManufacturerCode 
                    { 
                        Code = values[0].Trim('"'), 
                        IndustryCode = values[1].Trim('"'),
                        Name = values[2].Trim('"')
                    };
                    Items.Add(item);
                }
            }
            
            ErrorMessage = null;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error importing CSV: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Error importing CSV: {ex.Message}");
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
