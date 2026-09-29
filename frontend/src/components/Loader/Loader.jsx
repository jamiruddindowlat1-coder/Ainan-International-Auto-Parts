import React from 'react';
import branding from '../../config/branding';

export default function Loader({ message = 'Loading AIAPS Enterprise System...' }) {
  return (
    <div className="loader-fullscreen">
      <div
        style={{
          width: '64px',
          height: '64px',
          borderRadius: '16px',
          background: 'linear-gradient(135deg, var(--primary), var(--accent))',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          fontWeight: 800,
          color: '#fff',
          fontSize: '1.8rem',
          boxShadow: '0 0 30px rgba(232,119,34,0.5)',
          marginBottom: '16px',
          animation: 'pulse 1.5s infinite',
        }}
      >
        AI
      </div>
      <div className="spinner" />
      <h3 style={{ fontFamily: 'var(--font-heading)', color: '#fff', marginTop: '8px' }}>
        {branding.company.name}
      </h3>
      <p className="text-muted">{message}</p>
    </div>
  );
}
