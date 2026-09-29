import React from 'react';
import { FiSearch, FiBell, FiUser, FiLogOut } from 'react-icons/fi';
import { useAuth } from '../../context/AuthContext';
import branding from '../../config/branding';

export default function Navbar({ collapsed }) {
  const { user, logout } = useAuth();
  const company = branding.company;

  return (
    <header
      style={{
        height: 'var(--navbar-height)',
        position: 'fixed',
        top: 0,
        right: 0,
        left: collapsed ? branding.sidebar.collapsedWidth : branding.sidebar.width,
        background: 'rgba(15, 23, 41, 0.85)',
        backdropFilter: 'blur(12px)',
        borderBottom: '1px solid var(--border)',
        zIndex: 90,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '0 24px',
        transition: 'left 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
      }}
    >
      {/* LEFT: COMPANY & SEARCH */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '20px' }}>
        <div style={{ display: 'none', md: 'block' }}>
          <span style={{ fontSize: '0.85rem', fontWeight: 600, color: 'var(--text-secondary)' }}>
            📍 {company.name}
          </span>
        </div>

        <div className="search-wrapper" style={{ minWidth: '260px' }}>
          <FiSearch className="search-icon" />
          <input
            type="text"
            className="search-input"
            placeholder="Quick search parts (Part #, OEM, Barcode)..."
          />
        </div>
      </div>

      {/* RIGHT: NOTIFICATIONS & USER PROFILE */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
        {/* Currency indicator */}
        <div
          style={{
            background: 'var(--bg-input)',
            border: '1px solid var(--border)',
            padding: '4px 10px',
            borderRadius: 'var(--radius-md)',
            fontSize: '0.78rem',
            fontWeight: 600,
            color: 'var(--accent-light)',
          }}
        >
          {branding.defaultCurrency.code} ({branding.defaultCurrency.symbol})
        </div>

        {/* User Card */}
        <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
          <div
            style={{
              width: '36px',
              height: '36px',
              borderRadius: '50%',
              background: 'linear-gradient(135deg, var(--primary), var(--primary-light))',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              color: '#fff',
              fontWeight: 700,
              fontSize: '0.85rem',
            }}
          >
            <FiUser />
          </div>
          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <span style={{ fontSize: '0.85rem', fontWeight: 600, color: '#fff', lineHeight: 1.2 }}>
              {user?.fullName || 'Administrator'}
            </span>
            <span style={{ fontSize: '0.72rem', color: 'var(--text-muted)' }}>
              {user?.roles?.[0] || 'SuperAdmin'}
            </span>
          </div>
        </div>

        {/* Logout Button */}
        <button
          type="button"
          onClick={logout}
          className="btn btn-ghost btn-icon"
          title="Sign out of system"
          style={{ width: '36px', height: '36px' }}
        >
          <FiLogOut style={{ color: 'var(--danger)' }} />
        </button>
      </div>
    </header>
  );
}
