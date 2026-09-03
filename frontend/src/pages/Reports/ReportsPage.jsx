import React, { useState } from 'react';
import { FiBarChart2, FiCalendar } from 'react-icons/fi';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import DataTable from '../../components/Table/DataTable';
import branding from '../../config/branding';

export default function ReportsPage() {
  const [reportType, setReportType] = useState('sales_summary');
  const [dateRange, setDateRange] = useState('This Month');

  // Sample dynamic report datasets
  const reportsData = {
    sales_summary: {
      title: 'Sales & Profitability Summary Report',
      columns: ['#', 'Date / Month', 'Invoices', 'Gross Revenue (BDT)', 'Discount (BDT)', 'Net Revenue (BDT)', 'Estimated Profit (BDT)'],
      rows: [
        [1, 'August 2026', 420, '2,840,000', '45,000', '2,795,000', '680,000'],
        [2, 'July 2026', 395, '2,650,000', '38,000', '2,612,000', '640,000'],
        [3, 'June 2026', 360, '2,480,000', '32,000', '2,448,000', '590,000'],
        [4, 'May 2026', 340, '2,290,000', '28,000', '2,262,000', '540,000'],
      ]
    },
    top_selling_parts: {
      title: 'Top Fast-Moving Spare Parts Report',
      columns: ['#', 'Part Number', 'Item Description', 'Brand', 'Units Sold', 'Total Revenue (BDT)'],
      rows: [
        [1, 'DEN-SP-IX01', 'Iridium Spark Plug (SK20R11)', 'Denso', 240, '228,000'],
        [2, 'BOS-BP-001', 'Front Ceramic Brake Pad Set', 'Bosch', 85, '272,000'],
        [3, 'MNN-OF-W68', 'Mann Spin-On Oil Filter', 'Mann-Filter', 190, '123,500'],
        [4, 'MOB-OIL-5W30', 'Mobil 1 Fully Synthetic 5W-30 (4L)', 'Mobil 1', 65, '338,000'],
        [5, 'KYB-SA-33331', 'KYB Excel-G Front Strut Assembly', 'KYB', 24, '220,800'],
      ]
    },
    customer_dues: {
      title: 'Customer Outstanding Balance & Aging Report',
      columns: ['#', 'Customer Name', 'Type', 'Phone', 'Credit Limit (BDT)', 'Total Due Balance (BDT)'],
      rows: [
        [1, 'Rahim Motors Wholesale', 'Wholesale', '+880 1913-987654', '500,000', '85,000'],
        [2, 'Apex Auto Garage', 'Garage', '+880 1812-345678', '150,000', '24,500'],
        [3, 'Dhaka City Motors', 'Wholesale', '+880 1711-223344', '300,000', '42,000'],
      ]
    }
  };

  const current = reportsData[reportType];

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiBarChart2 /></span>
            Business Intelligence &amp; Reports Center
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Instant analytical reports with single-click Print, Excel, PDF and Share
          </p>
        </div>

        <ExportToolbar
          title={current.title}
          columns={current.columns}
          rows={current.rows}
          extraInfo={{
            'Company': branding.company.name,
            'Generated On': new Date().toLocaleDateString('en-GB'),
            'Period': dateRange,
          }}
        />
      </div>

      {/* FILTER BAR */}
      <div className="card mb-16">
        <div style={{ display: 'flex', gap: '16px', flexWrap: 'wrap', alignItems: 'center' }}>
          <div className="form-group" style={{ minWidth: '240px' }}>
            <label className="form-label">Select Report Type</label>
            <select
              className="form-control"
              value={reportType}
              onChange={(e) => setReportType(e.target.value)}
            >
              <option value="sales_summary">Sales &amp; Profitability Summary</option>
              <option value="top_selling_parts">Top Fast-Moving Spare Parts</option>
              <option value="customer_dues">Customer Outstanding Due Ledger</option>
            </select>
          </div>

          <div className="form-group" style={{ minWidth: '180px' }}>
            <label className="form-label">Period Range</label>
            <select
              className="form-control"
              value={dateRange}
              onChange={(e) => setDateRange(e.target.value)}
            >
              <option value="This Month">This Month</option>
              <option value="Last Month">Last Month</option>
              <option value="Last 3 Months">Last 3 Months</option>
              <option value="This Year">This Year</option>
            </select>
          </div>
        </div>
      </div>

      {/* REPORT TABLE */}
      <div className="card">
        <div className="flex-between mb-16">
          <h3 style={{ color: 'var(--accent-light)' }}>{current.title}</h3>
          <span className="badge badge-primary">{dateRange}</span>
        </div>

        <DataTable
          columns={current.columns.map((col, idx) => ({
            header: col,
            accessor: idx.toString(),
            render: (r) => <span>{r[idx]}</span>
          }))}
          data={current.rows}
        />
      </div>
    </div>
  );
}
