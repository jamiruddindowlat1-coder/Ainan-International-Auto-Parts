using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using AutoPartsERP.API.Data;
using AutoPartsERP.API.DTOs;
using AutoPartsERP.API.DTOs.Common;
using AutoPartsERP.API.Interfaces;
using AutoPartsERP.API.Models.Partners;

namespace AutoPartsERP.API.Services;

public class PartnerService : IPartnerService
{
    private static readonly string[] CustomerTypes = { "Retail", "Wholesale", "Garage", "Corporate" };

    private readonly AppDbContext _context;

    public PartnerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CustomerDto dto)
    {
        var error = ValidateCustomer(dto, out var type);
        if (error != null) return ApiResponse<CustomerDto>.Fail(error);

        var customer = new Customer
        {
            Name = dto.Name.Trim(),
            CustomerType = type!,
            Phone = dto.Phone.Trim(),
            Email = Clean(dto.Email),
            Address = Clean(dto.Address),
            CreditLimit = dto.CreditLimit,
            OpeningBalance = 0,
            CurrentBalance = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return ApiResponse<CustomerDto>.Ok(ToDto(customer), "Customer created successfully");
    }

    public async Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, CustomerDto dto)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customer == null) return ApiResponse<CustomerDto>.Fail("Customer not found");

        var error = ValidateCustomer(dto, out var type);
        if (error != null) return ApiResponse<CustomerDto>.Fail(error);

        if (!dto.IsActive && customer.IsActive && customer.CurrentBalance != 0)
            return ApiResponse<CustomerDto>.Fail("Cannot deactivate a customer with an outstanding balance");

        customer.Name = dto.Name.Trim();
        customer.CustomerType = type!;
        customer.Phone = dto.Phone.Trim();
        customer.Email = Clean(dto.Email);
        customer.Address = Clean(dto.Address);
        customer.CreditLimit = dto.CreditLimit;
        customer.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return ApiResponse<CustomerDto>.Ok(ToDto(customer), "Customer updated successfully");
    }

    public async Task<ApiResponse<SupplierDto>> CreateSupplierAsync(SupplierDto dto)
    {
        var error = ValidateCommon(dto.Name, dto.Phone, dto.Email);
        if (error != null) return ApiResponse<SupplierDto>.Fail(error);

        var supplier = new Supplier
        {
            Name = dto.Name.Trim(),
            Company = Clean(dto.Company),
            Phone = dto.Phone.Trim(),
            Email = Clean(dto.Email),
            Address = Clean(dto.Address),
            TaxNumber = Clean(dto.TaxNumber),
            OpeningBalance = 0,
            CurrentBalance = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();

        return ApiResponse<SupplierDto>.Ok(ToDto(supplier), "Supplier created successfully");
    }

    public async Task<ApiResponse<SupplierDto>> UpdateSupplierAsync(int id, SupplierDto dto)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        if (supplier == null) return ApiResponse<SupplierDto>.Fail("Supplier not found");

        var error = ValidateCommon(dto.Name, dto.Phone, dto.Email);
        if (error != null) return ApiResponse<SupplierDto>.Fail(error);

        if (!dto.IsActive && supplier.IsActive && supplier.CurrentBalance != 0)
            return ApiResponse<SupplierDto>.Fail("Cannot deactivate a supplier with an outstanding balance");

        supplier.Name = dto.Name.Trim();
        supplier.Company = Clean(dto.Company);
        supplier.Phone = dto.Phone.Trim();
        supplier.Email = Clean(dto.Email);
        supplier.Address = Clean(dto.Address);
        supplier.TaxNumber = Clean(dto.TaxNumber);
        supplier.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return ApiResponse<SupplierDto>.Ok(ToDto(supplier), "Supplier updated successfully");
    }

    private static string? ValidateCustomer(CustomerDto dto, out string? type)
    {
        type = null;

        var error = ValidateCommon(dto.Name, dto.Phone, dto.Email);
        if (error != null) return error;

        if (dto.CreditLimit < 0) return "Credit limit cannot be negative";

        type = string.IsNullOrWhiteSpace(dto.CustomerType)
            ? "Retail"
            : CustomerTypes.FirstOrDefault(t => t.Equals(dto.CustomerType.Trim(), StringComparison.OrdinalIgnoreCase));

        return type == null ? "Customer type must be Retail, Wholesale, Garage or Corporate" : null;
    }

    private static string? ValidateCommon(string? name, string? phone, string? email)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Name is required";
        if (string.IsNullOrWhiteSpace(phone)) return "Phone is required";

        if (!string.IsNullOrWhiteSpace(email))
        {
            try { _ = new MailAddress(email.Trim()); }
            catch (FormatException) { return "Email address is not valid"; }
        }

        return null;
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static CustomerDto ToDto(Customer c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        CustomerType = c.CustomerType,
        Phone = c.Phone,
        Email = c.Email,
        Address = c.Address,
        CreditLimit = c.CreditLimit,
        CurrentBalance = c.CurrentBalance,
        IsActive = c.IsActive
    };

    private static SupplierDto ToDto(Supplier s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Company = s.Company,
        Phone = s.Phone,
        Email = s.Email,
        Address = s.Address,
        TaxNumber = s.TaxNumber,
        CurrentBalance = s.CurrentBalance,
        IsActive = s.IsActive
    };
}