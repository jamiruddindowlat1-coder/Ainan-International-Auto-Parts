// ============================================================
//  AIAPS — Centralized Branding Configuration
//  Change anything here → Updates EVERYWHERE in the system
// ============================================================
const branding = {
  company: {
    name: "Ainan International Auto Parts System",
    shortName: "AIAPS",
    tagline: "Your Trusted Auto Parts Partner Worldwide",
    email: "info@aiaps.com",
    phone: "+880 1700-000000",
    address: "Dhaka, Bangladesh",
    website: "www.aiaps.com",
    taxId: "",
    tradeId: "",
  },
  logo: {
    main: "/assets/logo/aiaps-logo.svg",
    icon: "/assets/logo/aiaps-icon.svg",
    printLogo: "/assets/logo/aiaps-logo.svg",
    width: 180,
    iconSize: 48,
  },
  colors: {
    primary: "#1B3A6B",
    primaryLight: "#2A52A0",
    primaryDark: "#0F2347",
    accent: "#E87722",
    accentLight: "#F4A261",
    accentDark: "#C85F10",
    success: "#10B981",
    warning: "#F59E0B",
    danger: "#EF4444",
    info: "#3B82F6",
    bgDark: "#0F1729",
    bgCard: "#1A2744",
    bgPage: "#111827",
    bgLight: "#F8FAFC",
    textPrimary: "#F1F5F9",
    textSecondary: "#94A3B8",
    textDark: "#1E293B",
    border: "#2D3F6B",
    borderLight: "#E2E8F0",
  },
  fonts: {
    heading: "'Outfit', 'Inter', sans-serif",
    body: "'Inter', 'Segoe UI', sans-serif",
    googleFontsUrl: "https://fonts.googleapis.com/css2?family=Outfit:wght@400;500;600;700;800&family=Inter:wght@300;400;500;600;700&display=swap",
  },
  sidebar: { width: "260px", collapsedWidth: "72px", showLogo: true, showCompanyName: true },
  defaultCurrency: { code: "BDT", symbol: "&#2547;", name: "Bangladeshi Taka" },
  document: {
    showLogo: true,
    showCompanyName: true,
    showCompanyAddress: true,
    showCompanyPhone: true,
    logoWidth: 120,
    primaryColor: "#1B3A6B",
    accentColor: "#E87722",
    footerText: "Thank you for your business | AIAPS",
  },
  app: { version: "1.0.0", dateFormat: "DD/MM/YYYY", timeFormat: "hh:mm A", timezone: "Asia/Dhaka", itemsPerPage: 20 },
};
export default branding;
