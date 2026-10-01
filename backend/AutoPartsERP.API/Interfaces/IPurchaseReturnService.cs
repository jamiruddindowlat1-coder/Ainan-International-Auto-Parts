using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IPurchaseReturnService
{
    Task<ApiResponse<PurchaseReturnResultDto>> CreateAsync(CreatePurchaseReturnDto dto, int? userId);
}