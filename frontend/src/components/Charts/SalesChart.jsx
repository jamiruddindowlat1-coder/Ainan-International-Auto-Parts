import React from 'react';
import {
  ResponsiveContainer,
  AreaChart,
  Area,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  Legend,
} from 'recharts';

/**
 * SalesChart — Reusable chart component for revenue & purchase trends.
 * @param {Object} props
 * @param {'area'|'bar'} props.type     - Chart type (default: 'area')
 * @param {Array}        props.data     - Array of data objects
 * @param {number}       [props.height] - Chart height in px (default: 280)
 */
export default function SalesChart({ type = 'area', data = [], height = 280 }) {
  const CustomTooltip = ({ active, payload, label }) => {
    if (active && payload && payload.length) {
      return (
        <div style={{
          background: 'var(--bg-card)',
          border: '1px solid var(--border)',
          borderRadius: '8px',
          padding: '10px 14px',
          fontSize: '12px',
        }}>
          <div style={{ fontWeight: 700, color: '#fff', marginBottom: '6px' }}>{label}</div>
          {payload.map((entry, i) => (
            <div key={i} style={{ color: entry.color, marginBottom: '2px' }}>
              {entry.name}: <strong>{typeof entry.value === 'number' ? entry.value.toLocaleString() : entry.value}</strong>
            </div>
          ))}
        </div>
      );
    }
    return null;
  };

  if (type === 'bar') {
    return (
      <ResponsiveContainer width="100%" height={height}>
        <BarChart data={data} margin={{ top: 4, right: 8, left: 0, bottom: 0 }}>
          <CartesianGrid strokeDasharray="3 3" stroke="rgba(45,63,107,0.4)" />
          <XAxis dataKey="month" stroke="var(--text-muted)" fontSize={11} />
          <YAxis stroke="var(--text-muted)" fontSize={11} tickFormatter={(v) => `${(v / 1000).toFixed(0)}k`} />
          <Tooltip content={<CustomTooltip />} />
          <Legend wrapperStyle={{ fontSize: '12px', color: 'var(--text-secondary)' }} />
          <Bar dataKey="sales" name="Sales Revenue" fill="var(--accent)" radius={[4, 4, 0, 0]} />
          <Bar dataKey="purchases" name="Purchases" fill="var(--primary-light)" radius={[4, 4, 0, 0]} />
        </BarChart>
      </ResponsiveContainer>
    );
  }

  return (
    <ResponsiveContainer width="100%" height={height}>
      <AreaChart data={data} margin={{ top: 4, right: 8, left: 0, bottom: 0 }}>
        <defs>
          <linearGradient id="gradSales" x1="0" y1="0" x2="0" y2="1">
            <stop offset="5%" stopColor="var(--accent)" stopOpacity={0.8} />
            <stop offset="95%" stopColor="var(--accent)" stopOpacity={0} />
          </linearGradient>
          <linearGradient id="gradPurchases" x1="0" y1="0" x2="0" y2="1">
            <stop offset="5%" stopColor="var(--primary-light)" stopOpacity={0.8} />
            <stop offset="95%" stopColor="var(--primary-light)" stopOpacity={0} />
          </linearGradient>
          <linearGradient id="gradExpenses" x1="0" y1="0" x2="0" y2="1">
            <stop offset="5%" stopColor="var(--danger)" stopOpacity={0.6} />
            <stop offset="95%" stopColor="var(--danger)" stopOpacity={0} />
          </linearGradient>
        </defs>
        <CartesianGrid strokeDasharray="3 3" stroke="rgba(45,63,107,0.4)" />
        <XAxis dataKey="month" stroke="var(--text-muted)" fontSize={11} />
        <YAxis stroke="var(--text-muted)" fontSize={11} tickFormatter={(v) => `${(v / 1000).toFixed(0)}k`} />
        <Tooltip content={<CustomTooltip />} />
        <Legend wrapperStyle={{ fontSize: '12px', color: 'var(--text-secondary)' }} />
        <Area type="monotone" dataKey="sales" name="Sales Revenue" stroke="var(--accent)" fillOpacity={1} fill="url(#gradSales)" />
        <Area type="monotone" dataKey="purchases" name="Purchases" stroke="var(--primary-light)" fillOpacity={1} fill="url(#gradPurchases)" />
        {data[0]?.expenses !== undefined && (
          <Area type="monotone" dataKey="expenses" name="Expenses" stroke="var(--danger)" fillOpacity={1} fill="url(#gradExpenses)" />
        )}
      </AreaChart>
    </ResponsiveContainer>
  );
}
