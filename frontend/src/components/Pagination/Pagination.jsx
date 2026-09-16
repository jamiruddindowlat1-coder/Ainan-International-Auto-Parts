import React from 'react';
import { FiChevronLeft, FiChevronRight } from 'react-icons/fi';

/**
 * Pagination — Reusable pagination component for all ERP list pages.
 *
 * @param {Object}   props
 * @param {number}   props.currentPage   - Current active page (1-indexed)
 * @param {number}   props.totalPages    - Total number of pages
 * @param {Function} props.onPageChange  - Called with new page number when user clicks
 * @param {number}   [props.totalItems]  - Optional total item count to display
 * @param {number}   [props.itemsPerPage]- Items per page (for display)
 */
export default function Pagination({ currentPage, totalPages, onPageChange, totalItems, itemsPerPage }) {
  if (totalPages <= 1) return null;

  const getPages = () => {
    const pages = [];
    const delta = 2;
    const range = [];
    for (let i = Math.max(2, currentPage - delta); i <= Math.min(totalPages - 1, currentPage + delta); i++) {
      range.push(i);
    }
    if (currentPage - delta > 2) range.unshift('...');
    if (currentPage + delta < totalPages - 1) range.push('...');
    pages.push(1);
    pages.push(...range);
    if (totalPages > 1) pages.push(totalPages);
    return pages;
  };

  const startItem = totalItems ? (currentPage - 1) * (itemsPerPage || 20) + 1 : null;
  const endItem = totalItems ? Math.min(currentPage * (itemsPerPage || 20), totalItems) : null;

  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      flexWrap: 'wrap',
      gap: '12px',
      marginTop: '16px',
      paddingTop: '16px',
      borderTop: '1px solid var(--border)',
    }}>
      {totalItems != null && (
        <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
          Showing <strong style={{ color: 'var(--text-secondary)' }}>{startItem}–{endItem}</strong> of{' '}
          <strong style={{ color: 'var(--text-secondary)' }}>{totalItems}</strong> records
        </div>
      )}

      <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
        {/* Prev */}
        <button
          type="button"
          onClick={() => onPageChange(currentPage - 1)}
          disabled={currentPage === 1}
          style={{
            display: 'flex', alignItems: 'center', gap: '4px',
            padding: '6px 10px', borderRadius: '6px', fontSize: '0.8rem',
            background: 'var(--bg-input)', border: '1px solid var(--border)',
            color: currentPage === 1 ? 'var(--text-muted)' : 'var(--text-secondary)',
            cursor: currentPage === 1 ? 'not-allowed' : 'pointer',
            transition: 'all var(--transition)',
          }}
        >
          <FiChevronLeft fontSize={14} /> Prev
        </button>

        {/* Page Numbers */}
        {getPages().map((page, i) =>
          page === '...' ? (
            <span key={`ellipsis-${i}`} style={{ padding: '6px 8px', color: 'var(--text-muted)', fontSize: '0.8rem' }}>…</span>
          ) : (
            <button
              key={page}
              type="button"
              onClick={() => onPageChange(page)}
              style={{
                minWidth: '34px', height: '34px', borderRadius: '6px',
                fontSize: '0.8rem', fontWeight: page === currentPage ? 700 : 400,
                background: page === currentPage ? 'var(--primary)' : 'var(--bg-input)',
                border: `1px solid ${page === currentPage ? 'var(--primary)' : 'var(--border)'}`,
                color: page === currentPage ? '#fff' : 'var(--text-secondary)',
                cursor: 'pointer', transition: 'all var(--transition)',
              }}
            >
              {page}
            </button>
          )
        )}

        {/* Next */}
        <button
          type="button"
          onClick={() => onPageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
          style={{
            display: 'flex', alignItems: 'center', gap: '4px',
            padding: '6px 10px', borderRadius: '6px', fontSize: '0.8rem',
            background: 'var(--bg-input)', border: '1px solid var(--border)',
            color: currentPage === totalPages ? 'var(--text-muted)' : 'var(--text-secondary)',
            cursor: currentPage === totalPages ? 'not-allowed' : 'pointer',
            transition: 'all var(--transition)',
          }}
        >
          Next <FiChevronRight fontSize={14} />
        </button>
      </div>
    </div>
  );
}
