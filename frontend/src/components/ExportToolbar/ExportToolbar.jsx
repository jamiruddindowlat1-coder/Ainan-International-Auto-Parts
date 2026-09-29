import React from 'react';
import { FiPrinter, FiFileText, FiDownload, FiShare2, FiCheck } from 'react-icons/fi';
import { useExport } from '../../hooks/useExport';

/**
 * Centralized Export Toolbar for all modules.
 * Standardizes Print, Excel, PDF and Share across the entire ERP.
 */
export default function ExportToolbar({
  title,
  columns,
  rows,
  extraInfo,
  footerNote,
  filename,
  shareUrl,
  shareText,
  className = '',
}) {
  const {
    handlePrint,
    handleExcel,
    handlePDF,
    handleShare,
    isSharing,
    shareResult,
  } = useExport({
    title,
    columns,
    rows,
    extraInfo,
    footerNote,
    filename,
    shareUrl,
    shareText,
  });

  return (
    <div className={`export-toolbar ${className}`}>
      {/* PRINT BUTTON */}
      <button
        type="button"
        onClick={handlePrint}
        className="btn btn-ghost btn-sm"
        title="Print document with company branding"
      >
        <FiPrinter style={{ color: 'var(--primary-light)' }} />
        <span>Print</span>
      </button>

      {/* EXCEL BUTTON */}
      <button
        type="button"
        onClick={handleExcel}
        className="btn btn-ghost btn-sm"
        title="Export to formatted Excel file (.xlsx)"
      >
        <FiDownload style={{ color: 'var(--success)' }} />
        <span>Excel</span>
      </button>

      {/* PDF BUTTON */}
      <button
        type="button"
        onClick={handlePDF}
        className="btn btn-ghost btn-sm"
        title="Export / Save as PDF"
      >
        <FiFileText style={{ color: 'var(--danger)' }} />
        <span>PDF</span>
      </button>

      {/* SHARE BUTTON */}
      <button
        type="button"
        onClick={handleShare}
        disabled={isSharing}
        className="btn btn-ghost btn-sm"
        title="Share link or copy to clipboard"
      >
        {shareResult?.success ? (
          <>
            <FiCheck style={{ color: 'var(--success)' }} />
            <span>Copied!</span>
          </>
        ) : (
          <>
            <FiShare2 style={{ color: 'var(--accent)' }} />
            <span>{isSharing ? 'Sharing...' : 'Share'}</span>
          </>
        )}
      </button>
    </div>
  );
}
