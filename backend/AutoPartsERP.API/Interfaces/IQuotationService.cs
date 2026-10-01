using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IQuotationService
{
    Task<ApiResponse<QuotationResultDto>> CreateAsync(CreateQuotationDto dto);
}