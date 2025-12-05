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

public partial class PurchaseCodeTableViewModel : ReferenceTableViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<PurchaseCode> _items = new();

    [ObservableProperty]
    private PurchaseCode? _selectedItem;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public PurchaseCodeTableViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    private void AddNew()
    {
        try
        {
            var newItem = new PurchaseCode { Code = "", Description = "" };
            Items.Add(newItem);
            SelectedItem = newItem;
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
                    await _dataService.AddPurchaseCodeAsync(item);
                    savedCount++;
                }
                else
                {
                    await _dataService.UpdatePurchaseCodeAsync(item);
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
        if (SelectedItem == null)
        {
            ErrorMessage = "Please select a row to delete.";
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            if (SelectedItem.Id > 0)
            {
                await _dataService.DeletePurchaseCodeAsync(SelectedItem.Id);
            }

            Items.Remove(SelectedItem);
            SelectedItem = null;
            ErrorMessage = "Record deleted successfully.";
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
            csv.AppendLine("Code,Description");
            
            foreach (var item in Items)
            {
                csv.AppendLine($"\"{item.Code}\",\"{item.Description}\"");
            }

            var fileName = $"PurchaseCodes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
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
            var items = await _dataService.GetPurchaseCodesAsync();
            Items = new ObservableCollection<PurchaseCode>(items);
            
            if (Items.Count == 0)
            {
                ErrorMessage = "No data found. Use 'Add New' or 'Import CSV' to add records.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Error loading Purchase Codes: {ex.Message}");
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
            // Skip header row
            for (int i = 1; i < lines.Length; i++)
            {
                var values = ParseCsvLine(lines[i]);
                if (values.Length >= 2)
                {
                    var item = new PurchaseCode 
                    { 
                        Code = values[0].Trim('"').Trim(), 
                        Description = values[1].Trim('"').Trim() 
                    };
                    Items.Add(item);
                    importedCount++;
                }
            }
            
            ErrorMessage = $"Successfully imported {importedCount} records. Click Save to persist.";
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
