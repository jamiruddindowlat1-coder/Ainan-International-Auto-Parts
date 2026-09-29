// ============================================================
//  printUtils.js — Centralized Print Utility
//  Uses branding.js for company name, logo, colors, etc.
//  Usage: printDocument({ title, columns, rows, extraInfo })
// ============================================================

import branding from '../config/branding';

/**
 * Opens a styled print window with company branding.
 *
 * @param {Object} options
 * @param {string}   options.title        - Document title (e.g. "Sales Report")
 * @param {string[]} options.columns      - Table column headers
 * @param {Array[]}  options.rows         - 2D array of row values (matches columns order)
 * @param {Object}   [options.extraInfo]  - Key-value pairs shown below the header (e.g. { "Invoice No": "INV-001" })
 * @param {string}   [options.footerNote] - Optional footer note (overrides branding default)
 */
export function printDocument({ title, columns = [], rows = [], extraInfo = {}, footerNote }) {
  const b = branding;
  const doc = b.document;
  const company = b.company;
  const colors = b.colors;

  const logoSrc = doc.showLogo && b.logo.printLogo ? b.logo.printLogo : null;
  const footer = footerNote || doc.footerText;
  const primary = doc.primaryColor || colors.primary;
  const accent = doc.accentColor || colors.accent;
  const now = new Date();
  const printedAt = now.toLocaleDateString('en-GB') + ' ' + now.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });

  // Build extra-info rows
  const extraInfoHtml = Object.entries(extraInfo)
    .map(([k, v]) => `<div class="info-item"><span class="info-label">${k}:</span> <span class="info-value">${v}</span></div>`)
    .join('');

  // Build table headers
  const thHtml = columns.map(col => `<th>${col}</th>`).join('');

  // Build table rows
  const trHtml = rows.map((row, i) =>
    `<tr class="${i % 2 === 0 ? 'even' : 'odd'}">${
      (Array.isArray(row) ? row : Object.values(row)).map(cell => `<td>${cell ?? ''}</td>`).join('')
    }</tr>`
  ).join('');

  const html = `
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8" />
  <title>${title} — ${company.shortName}</title>
  <style>
    @import url('${b.fonts.googleFontsUrl}');
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body {
      font-family: ${b.fonts.body};
      color: #1E293B;
      background: #fff;
      font-size: 13px;
      padding: 24px 32px;
    }
    /* ---- Header ---- */
    .print-header {
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
      border-bottom: 3px solid ${primary};
      padding-bottom: 16px;
      margin-bottom: 16px;
    }
    .header-left { display: flex; align-items: center; gap: 14px; }
    .header-logo { width: ${doc.logoWidth || 100}px; object-fit: contain; }
    .company-name {
      font-family: ${b.fonts.heading};
      font-size: 20px;
      font-weight: 800;
      color: ${primary};
      line-height: 1.2;
    }
    .company-tagline { font-size: 11px; color: #64748B; margin-top: 2px; }
    .company-contact { font-size: 11px; color: #475569; margin-top: 6px; line-height: 1.6; }
    .header-right { text-align: right; }
    .doc-title {
      font-family: ${b.fonts.heading};
      font-size: 22px;
      font-weight: 700;
      color: ${accent};
    }
    .doc-meta { font-size: 11px; color: #64748B; margin-top: 4px; }

    /* ---- Extra Info ---- */
    .extra-info {
      display: flex;
      flex-wrap: wrap;
      gap: 6px 24px;
      margin-bottom: 14px;
      background: #F8FAFC;
      border: 1px solid #E2E8F0;
      border-radius: 6px;
      padding: 10px 14px;
    }
    .info-item { font-size: 12px; }
    .info-label { font-weight: 600; color: ${primary}; }
    .info-value { color: #334155; }

    /* ---- Table ---- */
    table {
      width: 100%;
      border-collapse: collapse;
      margin-bottom: 20px;
    }
    thead tr {
      background: ${primary};
      color: #fff;
    }
    thead th {
      padding: 9px 10px;
      text-align: left;
      font-family: ${b.fonts.heading};
      font-size: 12px;
      font-weight: 600;
      letter-spacing: 0.4px;
    }
    tbody tr.even { background: #F8FAFC; }
    tbody tr.odd  { background: #FFFFFF; }
    tbody td {
      padding: 8px 10px;
      border-bottom: 1px solid #E2E8F0;
      font-size: 12px;
      color: #334155;
    }
    tbody tr:last-child td { border-bottom: 2px solid ${primary}; }

    /* ---- Footer ---- */
    .print-footer {
      text-align: center;
      font-size: 11px;
      color: #94A3B8;
      border-top: 1px solid #E2E8F0;
      padding-top: 10px;
      margin-top: 10px;
    }
    .print-footer strong { color: ${primary}; }

    @media print {
      body { padding: 10px 16px; }
      @page { margin: 12mm 10mm; }
    }
  </style>
</head>
<body>

  <!-- HEADER -->
  <div class="print-header">
    <div class="header-left">
      ${logoSrc ? `<img src="${logoSrc}" alt="Logo" class="header-logo" />` : ''}
      <div>
        ${doc.showCompanyName ? `<div class="company-name">${company.name}</div>` : ''}
        <div class="company-tagline">${company.tagline}</div>
        <div class="company-contact">
          ${doc.showCompanyPhone && company.phone ? `📞 ${company.phone}` : ''}
          ${doc.showCompanyAddress && company.address ? ` &nbsp;|&nbsp; 📍 ${company.address}` : ''}
          ${company.email ? ` &nbsp;|&nbsp; ✉ ${company.email}` : ''}
        </div>
      </div>
    </div>
    <div class="header-right">
      <div class="doc-title">${title}</div>
      <div class="doc-meta">Printed: ${printedAt}</div>
      ${company.taxId ? `<div class="doc-meta">Tax ID: ${company.taxId}</div>` : ''}
    </div>
  </div>

  <!-- EXTRA INFO -->
  ${extraInfoHtml ? `<div class="extra-info">${extraInfoHtml}</div>` : ''}

  <!-- TABLE -->
  ${columns.length > 0 ? `
  <table>
    <thead><tr>${thHtml}</tr></thead>
    <tbody>${trHtml}</tbody>
  </table>` : ''}

  <!-- FOOTER -->
  <div class="print-footer">
    <strong>${company.shortName}</strong> &mdash; ${footer}
    &nbsp;|&nbsp; ${company.website || ''}
  </div>

  <script>
    window.onload = function() {
      window.print();
      setTimeout(() => window.close(), 500);
    };
  <\/script>
</body>
</html>`;

  const win = window.open('', '_blank', 'width=900,height=700');
  if (!win) {
    alert('Pop-up blocked! Please allow pop-ups for this site to use the Print feature.');
    return;
  }
  win.document.write(html);
  win.document.close();
}
