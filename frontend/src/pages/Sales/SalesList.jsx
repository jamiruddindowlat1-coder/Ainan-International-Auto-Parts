import React, { useState, useEffect } from 'react';
import { FiShoppingBag, FiSearch, FiPrinter, FiEye, FiTrash2 } from 'react-icons/fi';
import { Link } from 'react-router-dom';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import { printDocument } from '../../utils/printUtils';
import branding from '../../config/branding';

export default function SalesList() {
  const [sales, setSales] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [selectedInvoice, setSelectedInvoice] = useState(null);

  useEffect(() => {
    fetchSales();
  }, []);

  const fetchSales = async () => {
    try {
      setLoading(true);
      const res = await api.get('/sales?pageSize=100');
      if (res.data?.data) {
        setSales(res.data.data);
      }
    } catch {
      setSales([]);
    } finally {
      setLoading(false);
    }
  };

  const handlePrintInvoice = (inv) => {
    printDocument({
      title: 'SALES INVOICE',
      columns: ['#', 'Part Description', 'Quantity', 'Unit Price', 'Total'],
      rows: (inv.items || []).map((it, i) => [
        i + 1,
        `${it.partNumber} - ${it.partName}`,
        it.quantity,
        it.unitPrice.toLocaleString(),
        it.totalPrice.toLocaleString()
      ]),
      extraInfo: {
        'Invoice Number': inv.invoiceNumber,
        'Date': new Date(inv.saleDate).toLocaleDateString('en-GB'),
        'Customer': inv.customerName,
        'Payment Method': inv.paymentMethod || 'Cash',
        'Total Amount': `${branding.defaultCurrency.symbol} ${inv.totalAmount.toLocaleString()}`,
        'Paid Amount': `${branding.defaultCurrency.symbol} ${inv.paidAmount.toLocaleString()}`,
        'Due Balance': `${branding.defaultCurrency.symbol} ${inv.dueAmount.toLocaleString()}`,
        'Status': inv.paymentStatus
      }
    });
  };

  const handleDelete = (id) => {
    if (!window.confirm('Are you sure you want to void / delete this sales invoice?')) return;
    setSales(sales.filter(s => s.id !== id));
  };

  const filtered = sales.filter(s => {
    const q = search.toLowerCase();
    return s.invoiceNumber.toLowerCase().includes(q) ||
           s.customerName.toLowerCase().includes(q);
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiShoppingBag /></span>
            Sales Invoices &amp; Billing
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Browse, inspect, void, and reprint sales invoices
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <Link to="/pos" className="btn btn-accent">
            <span>+ New POS Sale</span>
          </Link>
          <ExportToolbar
            title="Sales Invoices Report"
            columns={['Invoice #', 'Date', 'Customer', 'Amount', 'Paid', 'Due', 'Status']}
            rows={filtered.map(s => [
              s.invoiceNumber,
              new Date(s.saleDate).toLocaleDateString('en-GB'),
              s.customerName,
              s.totalAmount,
              s.paidAmount,
              s.dueAmount,
              s.paymentStatus,
            ])}
          />
        </div>
      </div>

      <div className="card mb-16">
        <div className="search-wrapper">
          <FiSearch className="search-icon" />
          <input
            type="text"
            className="search-input"
            placeholder="Search by Invoice # or Customer..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
      </div>

      <div className="card">
        <DataTable
          loading={loading}
          data={filtered}
          columns={[
            { header: 'Invoice #', accessor: 'invoiceNumber', render: (r) => <strong>{r.invoiceNumber}</strong> },
            { header: 'Customer', accessor: 'customerName' },
            { header: 'Date', accessor: 'saleDate', render: (r) => new Date(r.saleDate).toLocaleDateString('en-GB') },
            { header: 'Total Amount', accessor: 'totalAmount', render: (r) => `${currency} ${r.totalAmount.toLocaleString()}` },
            { header: 'Paid', accessor: 'paidAmount', render: (r) => `${currency} ${r.paidAmount.toLocaleString()}` },
            { header: 'Due', accessor: 'dueAmount', render: (r) => (
              <span style={{ color: r.dueAmount > 0 ? 'var(--danger)' : 'var(--text-muted)' }}>
                {currency} {r.dueAmount.toLocaleString()}
              </span>
            )},
            { header: 'Status', accessor: 'paymentStatus', render: (r) => (
              <span className={`badge ${r.paymentStatus === 'Paid' ? 'badge-success' : 'badge-warning'}`}>
                {r.paymentStatus}
              </span>
            )},
            {
              header: 'Actions',
              align: 'right',
              render: (r) => (
                <div className="td-actions" style={{ justifyContent: 'flex-end' }}>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    onClick={() => setSelectedInvoice(r)}
                    title="View Details"
                  >
                    <FiEye />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    onClick={() => handlePrintInvoice(r)}
                    title="Print Invoice"
                  >
                    <FiPrinter style={{ color: 'var(--primary-light)' }} />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDelete(r.id)}
                    title="Void / Delete Invoice"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* VIEW INVOICE MODAL */}
      <Modal
        isOpen={!!selectedInvoice}
        onClose={() => setSelectedInvoice(null)}
        title={`Invoice: ${selectedInvoice?.invoiceNumber}`}
        size="lg"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setSelectedInvoice(null)}>
              Close
            </button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={() => {
                handlePrintInvoice(selectedInvoice);
                setSelectedInvoice(null);
              }}
            >
              <FiPrinter /> Print Document
            </button>
          </>
        }
      >
        {selectedInvoice && (
          <div>
            <div className="flex-between mb-16" style={{ background: 'var(--bg-input)', padding: '12px', borderRadius: '8px' }}>
              <div>
                <div style={{ fontWeight: 700, color: '#fff' }}>Customer: {selectedInvoice.customerName}</div>
                <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Phone: {selectedInvoice.customerPhone || '-'}</div>
              </div>
              <div style={{ textAlign: 'right' }}>
                <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                  Date: {new Date(selectedInvoice.saleDate).toLocaleDateString('en-GB')}
                </div>
                <span className={`badge ${selectedInvoice.paymentStatus === 'Paid' ? 'badge-success' : 'badge-warning'}`}>
                  {selectedInvoice.paymentStatus}
                </span>
              </div>
            </div>

            <DataTable
              data={selectedInvoice.items || []}
              columns={[
                { header: 'Part #', accessor: 'partNumber' },
                { header: 'Description', accessor: 'partName' },
                { header: 'Quantity', accessor: 'quantity' },
                { header: 'Unit Price', accessor: 'unitPrice', render: (i) => `${currency} ${i.unitPrice?.toLocaleString()}` },
                { header: 'Total', accessor: 'totalPrice', render: (i) => `${currency} ${i.totalPrice?.toLocaleString()}` },
              ]}
            />

            <div style={{ textAlign: 'right', marginTop: '16px', fontSize: '1.1rem', fontWeight: 800 }}>
              Total: <span style={{ color: 'var(--accent)' }}>{currency} {selectedInvoice.totalAmount.toLocaleString()}</span>
            </div>
          </div>
        )}
      </Modal>
    </div>
  );
}
