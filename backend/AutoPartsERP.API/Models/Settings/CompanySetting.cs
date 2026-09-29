namespace AutoPartsERP.API.Models.Settings;

public class CompanySetting
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = "Ainan International Auto Parts System";
    public string ShortName { get; set; } = "AIAPS";
    public string Tagline { get; set; } = "Your Trusted Auto Parts Partner Worldwide";
    public string Email { get; set; } = "info@aiaps.com";
    public string Phone { get; set; } = "+880 1700-000000";
    public string Address { get; set; } = "Dhaka, Bangladesh";
    public string Website { get; set; } = "www.aiaps.com";
    public string? TaxId { get; set; }
    public string? TradeLicenseId { get; set; }
    public string CurrencyCode { get; set; } = "BDT";
    public string CurrencySymbol { get; set; } = "৳";
    public string? LogoUrl { get; set; }
    public string FooterText { get; set; } = "Thank you for your business | AIAPS";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
