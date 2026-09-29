// ============================================================
//  useExport.js — Master Export Hook
//  Combines Print, Excel, PDF, Share in one hook.
//  Usage:
//    const { handlePrint, handleExcel, handlePDF, handleShare, isSharing } = useExport({
//      title: 'Sales Report',
//      columns: ['#', 'Customer', 'Amount'],
//      rows: data.map((d, i) => [i+1, d.customerName, d.total]),
//      extraInfo: { 'Date Range': '01/08/2026 – 31/08/2026' },
//    });
// ============================================================

import { useState, useCallback } from 'react';
import { printDocument } from '../utils/printUtils';
import { exportToExcel } from '../utils/exportUtils';
import { exportToPDF } from '../utils/pdfUtils';
import { shareDocument } from '../utils/pdfUtils';

/**
 * @param {Object} exportConfig
 * @param {string}   exportConfig.title       - Document title
 * @param {string[]} exportConfig.columns     - Column headers
 * @param {Array}    exportConfig.rows        - Data rows (2D array or array of objects)
 * @param {Object}   [exportConfig.extraInfo] - Key-value metadata (invoice no, date range, etc.)
 * @param {string}   [exportConfig.footerNote]- Custom footer override
 * @param {string}   [exportConfig.filename]  - Custom Excel filename
 * @param {string}   [exportConfig.shareUrl]  - URL to share (defaults to current page)
 * @param {string}   [exportConfig.shareText] - Text body for sharing
 */
export function useExport({
  title = '',
  columns = [],
  rows = [],
  extraInfo = {},
  footerNote,
  filename,
  shareUrl,
  shareText,
} = {}) {
  const [isSharing, setIsSharing] = useState(false);
  const [shareResult, setShareResult] = useState(null);

  const config = { title, columns, rows, extraInfo, footerNote };

  const handlePrint = useCallback(() => {
    printDocument(config);
  }, [title, columns, rows, extraInfo, footerNote]); // eslint-disable-line

  const handleExcel = useCallback(() => {
    exportToExcel({ ...config, filename });
  }, [title, columns, rows, extraInfo, filename]); // eslint-disable-line

  const handlePDF = useCallback(() => {
    exportToPDF(config);
  }, [title, columns, rows, extraInfo, footerNote]); // eslint-disable-line

  const handleShare = useCallback(async () => {
    setIsSharing(true);
    try {
      const result = await shareDocument({ title, text: shareText, url: shareUrl });
      setShareResult(result);
    } finally {
      setIsSharing(false);
    }
  }, [title, shareText, shareUrl]);

  return {
    handlePrint,
    handleExcel,
    handlePDF,
    handleShare,
    isSharing,
    shareResult,
  };
}
