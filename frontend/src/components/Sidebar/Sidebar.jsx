import React from 'react';
import { NavLink } from 'react-router-dom';
import {
  FiGrid,
  FiBox,
  FiShoppingCart,
  FiShoppingBag,
  FiUsers,
  FiTruck,
  FiDatabase,
  FiDollarSign,
  FiBarChart2,
  FiSettings,
  FiChevronLeft,
  FiChevronRight,
  FiTag,
  FiLayers
} from 'react-icons/fi';
import branding from '../../config/branding';

const navItems = [
  { path: '/dashboard', label: 'Dashboard', icon: FiGrid },
  { path: '/pos', label: 'POS Counter', icon: FiShoppingCart, badge: 'Live' },
  { path: '/parts', label: 'Parts Catalog', icon: FiBox },
  { path: '/brands', label: 'Brands', icon: FiTag },
  { path: '/categories', label: 'Categories', icon: FiLayers },
  { path: '/sales', label: 'Sales & Invoices', icon: FiShoppingBag },
  { path: '/purchase', label: 'Purchases', icon: FiTruck },
  { path: '/inventory', label: 'Inventory & Stock', icon: FiDatabase },
  { path: '/customers', label: 'Customers', icon: FiUsers },
  { path: '/suppliers', label: 'Suppliers', icon: FiTruck },
  { path: '/accounting', label: 'Accounting', icon: FiDollarSign },
  { path: '/reports', label: 'Reports & Export', icon: FiBarChart2 },
  { path: '/settings', label: 'Settings & Brand', icon: FiSettings },
];

export default function Sidebar({ collapsed, setCollapsed }) {
  const company = branding.company;

  return (
    <aside
      style={{
        width: collapsed ? branding.sidebar.collapsedWidth : branding.sidebar.width,
        transition: 'width 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
        position: 'fixed',
        top: 0,
        left: 0,
        bottom: 0,
        background: 'var(--bg-dark)',
        borderRight: '1px solid var(--border)',
        zIndex: 100,
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      {/* BRANDING HEADER */}
      <div
        style={{
          height: 'var(--navbar-height)',
          display: 'flex',
          alignItems: 'center',
          padding: collapsed ? '0 16px' : '0 20px',
          borderBottom: '1px solid var(--border)',
          justifyContent: collapsed ? 'center' : 'space-between',
        }}
      >
        {!collapsed && (
          <div style={{ display: 'flex', alignItems: 'center', gap: '10px', overflow: 'hidden' }}>
            <img
              src={branding.logo.icon}
              alt="Logo"
              style={{
                width: '36px',
                height: '36px',
                minWidth: '36px',
                borderRadius: '8px',
                objectFit: 'contain',
                boxShadow: '0 0 12px rgba(232,119,34,0.4)',
              }}
            />
            <div style={{ overflow: 'hidden' }}>
              <div
                style={{
                  fontFamily: 'var(--font-heading)',
                  fontWeight: 800,
                  fontSize: '1rem',
                  color: '#fff',
                  lineHeight: 1.2,
                  whiteSpace: 'nowrap',
                }}
              >
                {company.shortName}
              </div>
              <div
                style={{
                  fontSize: '0.68rem',
                  color: 'var(--text-muted)',
                  whiteSpace: 'nowrap',
                  overflow: 'hidden',
                  textOverflow: 'ellipsis',
                  maxWidth: '150px',
                }}
              >
                Auto Parts ERP
              </div>
            </div>
          </div>
        )}

        {collapsed && (
          <div
            style={{
              width: '36px',
              height: '36px',
              borderRadius: '8px',
              background: 'linear-gradient(135deg, var(--primary), var(--accent))',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontWeight: 800,
              color: '#fff',
            }}
          >
            AI
          </div>
        )}

        <button
          type="button"
          onClick={() => setCollapsed(!collapsed)}
          style={{
            color: 'var(--text-muted)',
            padding: '6px',
            borderRadius: '6px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            background: 'rgba(255,255,255,0.04)',
          }}
          title={collapsed ? 'Expand Sidebar' : 'Collapse Sidebar'}
        >
          {collapsed ? <FiChevronRight /> : <FiChevronLeft />}
        </button>
      </div>

      {/* NAVIGATION LINKS */}
      <nav
        style={{
          flex: 1,
          padding: '16px 10px',
          overflowY: 'auto',
          display: 'flex',
          flexDirection: 'column',
          gap: '4px',
        }}
      >
        {navItems.map((item) => {
          const Icon = item.icon;
          return (
            <NavLink
              key={item.path}
              to={item.path}
              className={({ isActive }) =>
                `nav-link ${isActive ? 'active' : ''}`
              }
              style={({ isActive }) => ({
                display: 'flex',
                alignItems: 'center',
                gap: '12px',
                padding: '10px 14px',
                borderRadius: 'var(--radius-md)',
                color: isActive ? '#fff' : 'var(--text-secondary)',
                background: isActive
                  ? 'linear-gradient(90deg, var(--primary), var(--primary-light))'
                  : 'transparent',
                fontWeight: isActive ? 600 : 500,
                fontSize: '0.875rem',
                transition: 'all var(--transition)',
                textDecoration: 'none',
                position: 'relative',
                justifyContent: collapsed ? 'center' : 'flex-start',
              })}
              title={collapsed ? item.label : undefined}
            >
              <Icon style={{ fontSize: '1.2rem', minWidth: '1.2rem' }} />
              {!collapsed && (
                <span style={{ flex: 1, whiteSpace: 'nowrap' }}>{item.label}</span>
              )}
              {!collapsed && item.badge && (
                <span
                  style={{
                    fontSize: '0.65rem',
                    background: 'var(--accent)',
                    color: '#fff',
                    padding: '2px 6px',
                    borderRadius: '999px',
                    fontWeight: 700,
                  }}
                >
                  {item.badge}
                </span>
              )}
            </NavLink>
          );
        })}
      </nav>

      {/* BOTTOM BRANDING FOOTER */}
      {!collapsed && (
        <div
          style={{
            padding: '14px 18px',
            borderTop: '1px solid var(--border)',
            fontSize: '0.75rem',
            color: 'var(--text-muted)',
            textAlign: 'center',
          }}
        >
          <div style={{ fontWeight: 600, color: 'var(--text-secondary)' }}>{company.name}</div>
          <div style={{ marginTop: '2px' }}>v{branding.app.version} &bull; Enterprise ERP</div>
        </div>
      )}
    </aside>
  );
}
