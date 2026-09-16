using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IAccountingService
{
    Task<ApiResponse<List<IncomeDto>>> GetIncomesAsync();
    Task<ApiResponse<IncomeDto>> CreateIncomeAsync(IncomeDto dto);
    
    Task<ApiResponse<List<ExpenseDto>>> GetExpensesAsync();
    Task<ApiResponse<ExpenseDto>> CreateExpenseAsync(ExpenseDto dto);
    
    Task<ApiResponse<List<LiabilityDto>>> GetLiabilitiesAsync();
    Task<ApiResponse<LiabilityDto>> CreateLiabilityAsync(LiabilityDto dto);
    
    Task<ApiResponse<List<AssetDto>>> GetAssetsAsync();
    Task<ApiResponse<AssetDto>> CreateAssetAsync(AssetDto dto);
    
    Task<ApiResponse<List<JournalEntryDto>>> GetJournalEntriesAsync();
    Task<ApiResponse<JournalEntryDto>> CreateJournalEntryAsync(JournalEntryDto dto);
    
    Task<ApiResponse<object>> GetLedgerAsync();
    Task<ApiResponse<object>> GetSummaryReportAsync();
}
