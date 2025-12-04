using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MeterLabToolkit.Data;
using MeterLabToolkit.Models;

namespace MeterLabToolkit.Services;

public class DataService : IDataService
{
    private readonly AppDbContext _context;

    public DataService()
    {
        _context = new AppDbContext();
    }

    public async Task InitializeDatabaseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    // Created Histories
    public async Task<List<CreatedHistory>> GetCreatedHistoriesAsync()
    {
        return await _context.CreatedHistories.ToListAsync();
    }

    public async Task<CreatedHistory?> GetCreatedHistoryAsync(int id)
    {
        return await _context.CreatedHistories.FindAsync(id);
    }

    public async Task AddCreatedHistoryAsync(CreatedHistory history)
    {
        _context.CreatedHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCreatedHistoryAsync(CreatedHistory history)
    {
        _context.CreatedHistories.Update(history);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCreatedHistoryAsync(int id)
    {
        var history = await _context.CreatedHistories.FindAsync(id);
        if (history != null)
        {
            _context.CreatedHistories.Remove(history);
            await _context.SaveChangesAsync();
        }
    }

    // Cellular Registrations
    public async Task<List<CellularRegistration>> GetCellularRegistrationsAsync()
    {
        return await _context.CellularRegistrations.ToListAsync();
    }

    public async Task<CellularRegistration?> GetCellularRegistrationAsync(int id)
    {
        return await _context.CellularRegistrations.FindAsync(id);
    }

    public async Task AddCellularRegistrationAsync(CellularRegistration registration)
    {
        _context.CellularRegistrations.Add(registration);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCellularRegistrationAsync(CellularRegistration registration)
    {
        _context.CellularRegistrations.Update(registration);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCellularRegistrationAsync(int id)
    {
        var registration = await _context.CellularRegistrations.FindAsync(id);
        if (registration != null)
        {
            _context.CellularRegistrations.Remove(registration);
            await _context.SaveChangesAsync();
        }
    }

    // RMA Entries
    public async Task<List<RmaEntry>> GetRmaEntriesAsync()
    {
        return await _context.RmaEntries.ToListAsync();
    }

    public async Task<RmaEntry?> GetRmaEntryAsync(int id)
    {
        return await _context.RmaEntries.FindAsync(id);
    }

    public async Task AddRmaEntryAsync(RmaEntry entry)
    {
        _context.RmaEntries.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRmaEntryAsync(RmaEntry entry)
    {
        _context.RmaEntries.Update(entry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRmaEntryAsync(int id)
    {
        var entry = await _context.RmaEntries.FindAsync(id);
        if (entry != null)
        {
            _context.RmaEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }

    // Lookup Codes
    public async Task<List<LookupCode>> GetLookupCodesAsync()
    {
        return await _context.LookupCodes.ToListAsync();
    }

    public async Task<LookupCode?> GetLookupCodeAsync(int id)
    {
        return await _context.LookupCodes.FindAsync(id);
    }

    public async Task AddLookupCodeAsync(LookupCode code)
    {
        _context.LookupCodes.Add(code);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLookupCodeAsync(LookupCode code)
    {
        _context.LookupCodes.Update(code);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLookupCodeAsync(int id)
    {
        var code = await _context.LookupCodes.FindAsync(id);
        if (code != null)
        {
            _context.LookupCodes.Remove(code);
            await _context.SaveChangesAsync();
        }
    }

    // Manufacturer Codes
    public async Task<List<ManufacturerCode>> GetManufacturerCodesAsync()
    {
        return await _context.ManufacturerCodes.ToListAsync();
    }

    public async Task<ManufacturerCode?> GetManufacturerCodeAsync(int id)
    {
        return await _context.ManufacturerCodes.FindAsync(id);
    }

    public async Task AddManufacturerCodeAsync(ManufacturerCode code)
    {
        _context.ManufacturerCodes.Add(code);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateManufacturerCodeAsync(ManufacturerCode code)
    {
        _context.ManufacturerCodes.Update(code);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteManufacturerCodeAsync(int id)
    {
        var code = await _context.ManufacturerCodes.FindAsync(id);
        if (code != null)
        {
            _context.ManufacturerCodes.Remove(code);
            await _context.SaveChangesAsync();
        }
    }

    // Device Codes
    public async Task<List<DeviceCode>> GetDeviceCodesAsync()
    {
        return await _context.DeviceCodes.ToListAsync();
    }

    public async Task<DeviceCode?> GetDeviceCodeAsync(int id)
    {
        return await _context.DeviceCodes.FindAsync(id);
    }

    public async Task AddDeviceCodeAsync(DeviceCode code)
    {
        _context.DeviceCodes.Add(code);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDeviceCodeAsync(DeviceCode code)
    {
        _context.DeviceCodes.Update(code);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDeviceCodeAsync(int id)
    {
        var code = await _context.DeviceCodes.FindAsync(id);
        if (code != null)
        {
            _context.DeviceCodes.Remove(code);
            await _context.SaveChangesAsync();
        }
    }
}
