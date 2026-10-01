using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;

namespace AutoPartsERP.API.Interfaces;

public interface IPartnerService
{
    Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CustomerDto dto);
    Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, CustomerDto dto);
    Task<ApiResponse<SupplierDto>> CreateSupplierAsync(SupplierDto dto);
    Task<ApiResponse<SupplierDto>> UpdateSupplierAsync(int id, SupplierDto dto);
}