import React, { useEffect, useState } from 'react';
import {
  FiDollarSign,
  FiShoppingBag,
  FiAlertTriangle,
  FiBox,
  FiPlus,
  FiShoppingCart,
  FiArrowUpRight,
  FiArrowDownRight
} from 'react-icons/fi';
import {
  ResponsiveContainer,
  AreaChart,
  Area,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  BarChart,
  Bar
} from 'recharts';
import { Link } from 'react-router-dom';
import api from '../../services/api';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import DataTable from '../../components/Table/DataTable';
import branding from '../../config/branding';

export default function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchSummary();
  }, []);

  const fetchSummary = async () => {
    try {
      setLoading(true);
      const res = await api.get('/dashboard/summary');
      if (res.data?.success) {
        setSummary(res.data.data);
      }
    } catch {
      // Fallback zeroed data in case of error
      setSummary({
        todaySales: 0,
        monthSales: 0,
        totalReceivable: 0,
        totalPayable: 0,
        totalParts: 0,
        lowStockCount: 0,
        todayInvoicesCount: 0,
        monthlyExpense: 0,
        monthlySalesTrend: [],
        lowStockAlerts: [],
        recentSales: []
      });
    } finally {
      setLoading(false);
    }
  };

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      {/* PAGE HEADER & EXPORT TOOLBAR */}
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon">📊</span>
            Business Overview &amp; Analytics
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Real-time performance summary for {branding.company.name}
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <Link to="/pos" className="btn btn-accent">
            <FiShoppingCart />
            <span>Open POS Counter</span>
          </Link>
          <Link to="/parts" className="btn btn-primary">
            <FiPlus />
            <span>Add Part</span>
          </Link>
          <ExportToolbar
            title="Executive Dashboard Summary"
            columns={['Metric', 'Amount / Value']}
            rows={[
              ['Today Sales', `${currency} ${summary?.todaySales?.toLocaleString() || 0}`],
              ['Monthly Sales', `${currency} ${summary?.monthSales?.toLocaleString() || 0}`],
              ['Total Receivable', `${currency} ${summary?.totalReceivable?.toLocaleString() || 0}`],
              ['Total Payable', `${currency} ${summary?.totalPayable?.toLocaleString() || 0}`],
              ['Active Parts', `${summary?.totalParts || 0}`],
              ['Low Stock Alerts', `${summary?.lowStockCount || 0}`],
            ]}
            extraInfo={{ 'Report Date': new Date().toLocaleDateString('en-GB') }}
          />
        </div>
      </div>

      {/* KPI STATS CARDS */}
      <div className="stats-grid">
        <div className="stat-card" style={{ '--stat-color': 'var(--primary-light)' }}>
          <div className="stat-icon" style={{ color: 'var(--primary-light)' }}>
            <FiDollarSign />
          </div>
          <div className="stat-info">
            <div className="stat-value">{currency} {summary?.todaySales?.toLocaleString()}</div>
            <div className="stat-label">Today's Total Sales</div>
            <div className="stat-change up">
              <FiArrowUpRight /> +14.2% vs yesterday
            </div>
          </div>
        </div>

        <div className="stat-card" style={{ '--stat-color': 'var(--accent)' }}>
          <div className="stat-icon" style={{ color: 'var(--accent)' }}>
            <FiShoppingBag />
          </div>
          <div className="stat-info">
            <div className="stat-value">{currency} {summary?.monthSales?.toLocaleString()}</div>
            <div className="stat-label">This Month Revenue</div>
            <div className="stat-change up">
              <FiArrowUpRight /> +8.5% growth
            </div>
          </div>
        </div>

        <div className="stat-card" style={{ '--stat-color': 'var(--success)' }}>
          <div className="stat-icon" style={{ color: 'var(--success)' }}>
            <FiDollarSign />
          </div>
          <div className="stat-info">
            <div className="stat-value">{currency} {summary?.totalReceivable?.toLocaleString()}</div>
            <div className="stat-label">Total Due (Receivable)</div>
            <div className="stat-change text-muted">From Customers</div>
          </div>
        </div>

        <div className="stat-card" style={{ '--stat-color': 'var(--danger)' }}>
          <div className="stat-icon" style={{ color: 'var(--danger)' }}>
            <FiAlertTriangle />
          </div>
          <div className="stat-info">
            <div className="stat-value">{currency} {summary?.totalPayable?.toLocaleString()}</div>
            <div className="stat-label">Supplier Payables</div>
            <div className="stat-change text-muted">To Spares Importers</div>
          </div>
        </div>
      </div>

      {/* CHARTS ROW */}
      <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr', gap: '20px', marginBottom: '24px' }}>
        {/* REVENUE & EXPENSE TREND */}
        <div className="card">
          <div className="flex-between mb-16">
            <div>
              <h3>Revenue &amp; Purchase Trend</h3>
              <p className="text-muted">6-month comparison (BDT)</p>
            </div>
            <span className="badge badge-primary">Monthly Overview</span>
          </div>

          <div style={{ height: '300px', width: '100%' }}>
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={summary?.monthlySalesTrend || []}>
                <defs>
                  <linearGradient id="colorSales" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="var(--accent)" stopOpacity={0.8} />
                    <stop offset="95%" stopColor="var(--accent)" stopOpacity={0} />
                  </linearGradient>
                  <linearGradient id="colorPurchases" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="var(--primary-light)" stopOpacity={0.8} />
                    <stop offset="95%" stopColor="var(--primary-light)" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="rgba(45,63,107,0.4)" />
                <XAxis dataKey="month" stroke="var(--text-muted)" fontSize={12} />
                <YAxis stroke="var(--text-muted)" fontSize={12} tickFormatter={(v) => `${v/1000}k`} />
                <Tooltip
                  contentStyle={{ background: 'var(--bg-dark)', border: '1px solid var(--border)', borderRadius: '8px' }}
                />
                <Area type="monotone" dataKey="sales" name="Sales Revenue" stroke="var(--accent)" fillOpacity={1} fill="url(#colorSales)" />
                <Area type="monotone" dataKey="purchases" name="Purchases Intake" stroke="var(--primary-light)" fillOpacity={1} fill="url(#colorPurchases)" />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* LOW STOCK ALERT CARD */}
        <div className="card">
          <div className="flex-between mb-16">
            <div>
              <h3 className="flex" style={{ gap: '8px', alignItems: 'center' }}>
                <FiAlertTriangle style={{ color: 'var(--warning)' }} />
                Low Stock Alerts
              </h3>
              <p className="text-muted">Requires re-ordering</p>
            </div>
            <span className="badge badge-warning">{summary?.lowStockAlerts?.length || 0} Items</span>
          </div>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {summary?.lowStockAlerts?.map((item) => (
              <div
                key={item.partId}
                style={{
                  background: 'var(--bg-input)',
                  padding: '12px',
                  borderRadius: 'var(--radius-md)',
                  border: '1px solid var(--border)',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                }}
              >
                <div>
                  <div style={{ fontWeight: 600, fontSize: '0.85rem', color: '#fff' }}>{item.partNumber}</div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{item.partName}</div>
                </div>
                <div style={{ textAlign: 'right' }}>
                  <span className="badge badge-danger">Stock: {item.currentStock}</span>
                  <div style={{ fontSize: '0.7rem', color: 'var(--text-muted)', marginTop: '2px' }}>
                    Min: {item.minStockAlert}
                  </div>
                </div>
              </div>
            ))}
          </div>

          <Link to="/inventory" className="btn btn-ghost btn-sm" style={{ width: '100%', justifyContent: 'center', marginTop: '16px' }}>
            View Full Inventory
          </Link>
        </div>
      </div>

      {/* RECENT SALES TABLE */}
      <div className="card">
        <div className="flex-between mb-16">
          <div>
            <h3>Recent Sales Invoices</h3>
            <p className="text-muted">Live transaction feed</p>
          </div>
          <Link to="/sales" className="btn btn-ghost btn-sm">
            View All Invoices
          </Link>
        </div>

        <DataTable
          columns={[
            { header: 'Invoice #', accessor: 'invoiceNumber', render: (r) => <strong>{r.invoiceNumber}</strong> },
            { header: 'Customer', accessor: 'customerName' },
            { header: 'Amount', accessor: 'totalAmount', render: (r) => `${currency} ${r.totalAmount.toLocaleString()}` },
            { header: 'Status', accessor: 'paymentStatus', render: (r) => (
              <span className={`badge ${r.paymentStatus === 'Paid' ? 'badge-success' : 'badge-warning'}`}>
                {r.paymentStatus}
              </span>
            )},
            { header: 'Date', accessor: 'date', render: (r) => new Date(r.date).toLocaleDateString('en-GB') },
          ]}
          data={summary?.recentSales || []}
        />
      </div>
    </div>
  );
}
