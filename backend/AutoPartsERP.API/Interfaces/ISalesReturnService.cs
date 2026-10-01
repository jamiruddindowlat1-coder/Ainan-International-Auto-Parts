using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface ISalesReturnService
{
    Task<ApiResponse<SalesReturnResultDto>> CreateAsync(CreateSalesReturnDto dto, int? userId);
}