import React from 'react';
import branding from '../../config/branding';

/**
 * Footer — App footer shown at the bottom of the main layout.
 * Displays company name, version, copyright.
 */
export default function Footer() {
  const company = branding.company;
  const year = new Date().getFullYear();

  return (
    <footer
      style={{
        borderTop: '1px solid var(--border)',
        padding: '12px 24px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
        gap: '8px',
        background: 'var(--bg-dark)',
        fontSize: '0.75rem',
        color: 'var(--text-muted)',
      }}
    >
      <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
        <span style={{ fontWeight: 600, color: 'var(--text-secondary)' }}>{company.name}</span>
        <span style={{
          background: 'var(--primary)',
          color: '#fff',
          fontSize: '0.65rem',
          padding: '2px 7px',
          borderRadius: '999px',
          fontWeight: 700,
          letterSpacing: '0.3px',
        }}>
          v{branding.app.version}
        </span>
      </div>

      <div style={{ textAlign: 'center' }}>
        {company.tagline}
      </div>

      <div style={{ textAlign: 'right' }}>
        © {year} {company.shortName} &mdash; All rights reserved
      </div>
    </footer>
  );
}
