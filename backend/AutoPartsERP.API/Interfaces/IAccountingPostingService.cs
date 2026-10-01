using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IAccountingPostingService
{
    Task<ApiResponse<JournalPostResultDto>> PostJournalEntryAsync(PostJournalEntryDto dto, int? userId);
    Task<ApiResponse<ExpenseResultDto>> RecordExpenseAsync(RecordExpenseDto dto, int? userId);
}