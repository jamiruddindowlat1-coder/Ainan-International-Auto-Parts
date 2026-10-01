using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IWarehouseService
{
    Task<ApiResponse<WarehouseDto>> CreateAsync(WarehouseDto dto);
    Task<ApiResponse<WarehouseDto>> UpdateAsync(int id, WarehouseDto dto);
}