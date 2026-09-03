import React, { useState } from 'react';
import { FiSettings, FiSave, FiCheck, FiPrinter } from 'react-icons/fi';
import branding from '../../config/branding';
import { printDocument } from '../../utils/printUtils';

export default function SettingsPage() {
  const [company, setCompany] = useState({ ...branding.company });
  const [docConfig, setDocConfig] = useState({ ...branding.document });
  const [saved, setSaved] = useState(false);

  const handleSave = (e) => {
    e.preventDefault();
    Object.assign(branding.company, company);
    Object.assign(branding.document, docConfig);
    setSaved(true);
    setTimeout(() => setSaved(false), 3000);
  };

  const handleTestPrint = () => {
    printDocument({
      title: 'TEST BRANDING PRINT PREVIEW',
      columns: ['#', 'Item Test Description', 'Sample Qty', 'Unit Rate', 'Total Amount'],
      rows: [
        [1, 'Front Ceramic Brake Pad Set (Bosch)', 2, '3,200', '6,400'],
        [2, 'Iridium Power Spark Plug (Denso)', 8, '950', '7,600'],
        [3, 'Mann Spin-On Engine Oil Filter', 2, '650', '1,300']
      ],
      extraInfo: {
        'Test Reference': 'AIAPS-BRAND-TEST-001',
        'Date': new Date().toLocaleDateString('en-GB'),
        'Branding Status': 'Active & Configured'
      }
    });
  };

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiSettings /></span>
            Enterprise Settings &amp; Branding
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Configure company information, currency, and branded print/PDF templates
          </p>
        </div>

        <button type="button" className="btn btn-ghost" onClick={handleTestPrint}>
          <FiPrinter style={{ color: 'var(--primary-light)' }} />
          <span>Test Print Preview</span>
        </button>
      </div>

      {saved && (
        <div style={{ background: 'rgba(16,185,129,0.15)', border: '1px solid var(--success)', color: '#34D399', padding: '12px 16px', borderRadius: '8px', marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
          <FiCheck />
          <span>Settings and Branding updated across the entire system!</span>
        </div>
      )}

      <form onSubmit={handleSave} style={{ display: 'grid', gridTemplateColumns: '1.2fr 1fr', gap: '20px' }}>
        {/* COMPANY DETAILS */}
        <div className="card">
          <h3 className="mb-16">Company Profile &amp; Contact</h3>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
            <div className="form-group">
              <label className="form-label">Full Company Name</label>
              <input
                type="text"
                className="form-control"
                value={company.name}
                onChange={(e) => setCompany({ ...company, name: e.target.value })}
              />
            </div>

            <div className="form-grid-2">
              <div className="form-group">
                <label className="form-label">Short Name (Acronym)</label>
                <input
                  type="text"
                  className="form-control"
                  value={company.shortName}
                  onChange={(e) => setCompany({ ...company, shortName: e.target.value })}
                />
              </div>

              <div className="form-group">
                <label className="form-label">Company Phone</label>
                <input
                  type="text"
                  className="form-control"
                  value={company.phone}
                  onChange={(e) => setCompany({ ...company, phone: e.target.value })}
                />
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">Company Tagline / Slogan</label>
              <input
                type="text"
                className="form-control"
                value={company.tagline}
                onChange={(e) => setCompany({ ...company, tagline: e.target.value })}
              />
            </div>

            <div className="form-grid-2">
              <div className="form-group">
                <label className="form-label">Email Address</label>
                <input
                  type="email"
                  className="form-control"
                  value={company.email}
                  onChange={(e) => setCompany({ ...company, email: e.target.value })}
                />
              </div>

              <div className="form-group">
                <label className="form-label">Website</label>
                <input
                  type="text"
                  className="form-control"
                  value={company.website}
                  onChange={(e) => setCompany({ ...company, website: e.target.value })}
                />
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">Physical Address</label>
              <textarea
                className="form-control"
                value={company.address}
                onChange={(e) => setCompany({ ...company, address: e.target.value })}
              />
            </div>
          </div>
        </div>

        {/* PRINT & DOCUMENT TEMPLATE SETTINGS */}
        <div className="card" style={{ height: 'fit-content' }}>
          <h3 className="mb-16">Print, Invoice &amp; PDF Branding</h3>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
            <div className="form-group">
              <label className="form-label">Document Footer Message</label>
              <input
                type="text"
                className="form-control"
                value={docConfig.footerText}
                onChange={(e) => setDocConfig({ ...docConfig, footerText: e.target.value })}
              />
            </div>

            <div className="form-grid-2">
              <div className="form-group">
                <label className="form-label">Primary Brand Color</label>
                <input
                  type="color"
                  className="form-control"
                  style={{ height: '42px', padding: '2px 6px' }}
                  value={docConfig.primaryColor || '#1B3A6B'}
                  onChange={(e) => setDocConfig({ ...docConfig, primaryColor: e.target.value })}
                />
              </div>

              <div className="form-group">
                <label className="form-label">Accent Brand Color</label>
                <input
                  type="color"
                  className="form-control"
                  style={{ height: '42px', padding: '2px 6px' }}
                  value={docConfig.accentColor || '#E87722'}
                  onChange={(e) => setDocConfig({ ...docConfig, accentColor: e.target.value })}
                />
              </div>
            </div>

            <div style={{ borderTop: '1px solid var(--border)', paddingTop: '14px', marginTop: '6px' }}>
              <button type="submit" className="btn btn-primary btn-lg" style={{ width: '100%', justifyContent: 'center' }}>
                <FiSave />
                <span>Save All Settings</span>
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
}
