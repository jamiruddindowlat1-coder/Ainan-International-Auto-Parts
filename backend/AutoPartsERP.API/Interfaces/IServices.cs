using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<UserDto>> RegisterAsync(RegisterUserDto request);
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
}

public interface IPartsService
{
    Task<PagedResponse<PartDto>> GetPartsAsync(string? search, int? categoryId, int? brandId, int page = 1, int pageSize = 20);
    Task<ApiResponse<PartDto>> GetPartByIdAsync(int id);
    Task<ApiResponse<PartDto>> CreatePartAsync(CreatePartDto dto);
    Task<ApiResponse<PartDto>> UpdatePartAsync(int id, CreatePartDto dto);
    Task<ApiResponse<bool>> DeletePartAsync(int id);
    Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync();
    Task<ApiResponse<List<BrandDto>>> GetBrandsAsync();
    Task<ApiResponse<List<UnitDto>>> GetUnitsAsync();
}

public interface ISalesService
{
    Task<PagedResponse<SalesInvoiceDto>> GetSalesAsync(string? search, string? status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 20);
    Task<ApiResponse<SalesInvoiceDto>> GetSaleByIdAsync(int id);
    Task<ApiResponse<SalesInvoiceDto>> CreateSaleAsync(CreateSalesInvoiceDto dto, int? userId);
}

public interface IPurchaseService
{
    Task<PagedResponse<PurchaseInvoiceDto>> GetPurchasesAsync(string? search, string? status, DateTime? fromDate, DateTime? toDate, int page = 1, int pageSize = 20);
    Task<ApiResponse<PurchaseInvoiceDto>> GetPurchaseByIdAsync(int id);
    Task<ApiResponse<PurchaseInvoiceDto>> CreatePurchaseAsync(CreatePurchaseInvoiceDto dto, int? userId);
}

public interface IDashboardService
{
    Task<ApiResponse<DashboardSummaryDto>> GetDashboardSummaryAsync();
}

public interface IExportService
{
    byte[] ExportPartsToExcel(IEnumerable<PartDto> parts);
    byte[] ExportSalesToExcel(IEnumerable<SalesInvoiceDto> sales);
}
