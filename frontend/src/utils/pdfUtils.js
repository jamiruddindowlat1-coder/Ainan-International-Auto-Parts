// ============================================================
//  pdfUtils.js — Centralized PDF Export Utility
//  Uses branding.js for company name, colors, logo, etc.
//  Strategy: Uses printUtils with a hidden iframe + PDF dialog
//            (no extra library needed — uses browser's Save as PDF)
//
//  For server-side or true PDF blob generation, swap the body
//  with a jsPDF/html2canvas implementation following the same API.
// ============================================================

import { printDocument } from './printUtils';

/**
 * Opens the browser's "Save as PDF" dialog with branded content.
 * The API is identical to printDocument so you can swap freely.
 *
 * @param {Object} options  - Same options as printDocument
 */
export function exportToPDF(options) {
  // We reuse printDocument — the user just clicks "Save as PDF"
  // in the browser print dialog instead of a physical printer.
  printDocument(options);
}

// ============================================================
//  shareUtils.js — Centralized Share Utility
//  Tries Web Share API, falls back to clipboard copy.
// ============================================================

import branding from '../config/branding';

/**
 * Shares a document link or text using the Web Share API (mobile/modern browsers)
 * Falls back to copying a URL to clipboard on desktop.
 *
 * @param {Object} options
 * @param {string} options.title   - Share title
 * @param {string} [options.text]  - Share body text
 * @param {string} [options.url]   - URL to share (defaults to current page)
 */
export async function shareDocument({ title, text, url }) {
  const company = branding.company;
  const shareUrl = url || window.location.href;
  const shareText = text || `${title} — ${company.name}`;
  const shareTitle = `${title} | ${company.shortName}`;

  // Web Share API (mobile, PWA, modern Edge/Chrome)
  if (navigator.share) {
    try {
      await navigator.share({ title: shareTitle, text: shareText, url: shareUrl });
      return { success: true, method: 'native' };
    } catch (err) {
      if (err.name !== 'AbortError') {
        console.warn('Share API failed, falling back to clipboard:', err);
      } else {
        // User cancelled
        return { success: false, method: 'cancelled' };
      }
    }
  }

  // Clipboard fallback
  try {
    await navigator.clipboard.writeText(`${shareTitle}\n${shareText}\n${shareUrl}`);
    return { success: true, method: 'clipboard' };
  } catch {
    // Last resort — prompt
    window.prompt('Copy this link:', shareUrl);
    return { success: true, method: 'prompt' };
  }
}
