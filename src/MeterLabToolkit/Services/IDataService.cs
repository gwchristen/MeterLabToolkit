using System.Collections.Generic;
using System.Threading.Tasks;
using MeterLabToolkit.Models;

namespace MeterLabToolkit.Services;

public interface IDataService
{
    // Created Histories
    Task<List<CreatedHistory>> GetCreatedHistoriesAsync();
    Task<List<CreatedHistory>> GetCreatedHistoriesAsync(string opCo, string deviceType);
    Task<CreatedHistory?> GetCreatedHistoryAsync(int id);
    Task AddCreatedHistoryAsync(CreatedHistory history);
    Task UpdateCreatedHistoryAsync(CreatedHistory history);
    Task DeleteCreatedHistoryAsync(int id);

    // Cellular Registrations
    Task<List<CellularRegistration>> GetCellularRegistrationsAsync();
    Task<CellularRegistration?> GetCellularRegistrationAsync(int id);
    Task AddCellularRegistrationAsync(CellularRegistration registration);
    Task UpdateCellularRegistrationAsync(CellularRegistration registration);
    Task DeleteCellularRegistrationAsync(int id);

    // RMA Entries
    Task<List<RmaEntry>> GetRmaEntriesAsync();
    Task<RmaEntry?> GetRmaEntryAsync(int id);
    Task AddRmaEntryAsync(RmaEntry entry);
    Task UpdateRmaEntryAsync(RmaEntry entry);
    Task DeleteRmaEntryAsync(int id);

    // Lookup Codes
    Task<List<LookupCode>> GetLookupCodesAsync();
    Task<LookupCode?> GetLookupCodeAsync(int id);
    Task AddLookupCodeAsync(LookupCode code);
    Task UpdateLookupCodeAsync(LookupCode code);
    Task DeleteLookupCodeAsync(int id);

    // Manufacturer Codes
    Task<List<ManufacturerCode>> GetManufacturerCodesAsync();
    Task<ManufacturerCode?> GetManufacturerCodeAsync(int id);
    Task AddManufacturerCodeAsync(ManufacturerCode code);
    Task UpdateManufacturerCodeAsync(ManufacturerCode code);
    Task DeleteManufacturerCodeAsync(int id);

    // Device Codes
    Task<List<DeviceCode>> GetDeviceCodesAsync();
    Task<List<DeviceCode>> GetDeviceCodesAsync(string? deviceType);
    Task<DeviceCode?> GetDeviceCodeAsync(int id);
    Task AddDeviceCodeAsync(DeviceCode code);
    Task UpdateDeviceCodeAsync(DeviceCode code);
    Task DeleteDeviceCodeAsync(int id);

    // OpCos
    Task<List<OpCo>> GetOpCosAsync();
    Task<OpCo?> GetOpCoAsync(int id);
    Task AddOpCoAsync(OpCo opCo);
    Task UpdateOpCoAsync(OpCo opCo);
    Task DeleteOpCoAsync(int id);

    // Statuses
    Task<List<Status>> GetStatusesAsync();
    Task<Status?> GetStatusAsync(int id);
    Task AddStatusAsync(Status status);
    Task UpdateStatusAsync(Status status);
    Task DeleteStatusAsync(int id);

    // Purchase Codes
    Task<List<PurchaseCode>> GetPurchaseCodesAsync();
    Task<PurchaseCode?> GetPurchaseCodeAsync(int id);
    Task AddPurchaseCodeAsync(PurchaseCode code);
    Task UpdatePurchaseCodeAsync(PurchaseCode code);
    Task DeletePurchaseCodeAsync(int id);

    // Database initialization
    Task InitializeDatabaseAsync();
}
