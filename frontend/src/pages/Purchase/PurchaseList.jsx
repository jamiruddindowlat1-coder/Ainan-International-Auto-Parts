import React, { useState, useEffect } from 'react';
import { FiTruck, FiSearch, FiPrinter, FiPlus, FiEye, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import { printDocument } from '../../utils/printUtils';
import branding from '../../config/branding';

export default function PurchaseList() {
  const [purchases, setPurchases] = useState([]);
  const [suppliers, setSuppliers] = useState([]);
  const [parts, setParts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [selectedPO, setSelectedPO] = useState(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);

  // New Purchase Form
  const [supplierId, setSupplierId] = useState(1);
  const [paidAmount, setPaidAmount] = useState('');
  const [poItems, setPoItems] = useState([
    { partId: 1, quantity: 10, unitPrice: 2200 }
  ]);

  useEffect(() => {
    fetchPurchases();
    fetchMetadata();
  }, []);

  const fetchPurchases = async () => {
    try {
      setLoading(true);
      const res = await api.get('/purchase?pageSize=100');
      if (res.data?.data) setPurchases(res.data.data);
    } catch {
      setPurchases([
        { id: 1, invoiceNumber: 'PO-20260825-4011', supplierName: 'Global Auto Parts Dubai', purchaseDate: '2026-08-25', totalAmount: 320000, paidAmount: 200000, dueAmount: 120000, paymentStatus: 'Partial', status: 'Received', items: [{ partNumber: 'BOS-BP-001', partName: 'Brake Pads', quantity: 100, unitPrice: 2200, totalCost: 220000 }, { partNumber: 'MOB-OIL-5W30', partName: 'Engine Oil', quantity: 20, unitPrice: 5000, totalCost: 100000 }] },
        { id: 2, invoiceNumber: 'PO-20260818-4012', supplierName: 'Nippon Auto Exports Tokyo', purchaseDate: '2026-08-18', totalAmount: 480000, paidAmount: 480000, dueAmount: 0, paymentStatus: 'Paid', status: 'Received', items: [{ partNumber: 'DEN-SP-IX01', partName: 'Spark Plugs', quantity: 500, unitPrice: 650, totalCost: 325000 }] },
        { id: 3, invoiceNumber: 'PO-20260805-4013', supplierName: 'Bangla Motor Spares Ltd.', purchaseDate: '2026-08-05', totalAmount: 95000, paidAmount: 50000, dueAmount: 45000, paymentStatus: 'Partial', status: 'Received', items: [{ partNumber: 'MNN-OF-W68', partName: 'Oil Filters', quantity: 150, unitPrice: 400, totalCost: 60000 }] },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const fetchMetadata = async () => {
    try {
      const [sRes, pRes] = await Promise.all([
        api.get('/suppliers'),
        api.get('/parts?pageSize=100')
      ]);
      if (sRes.data?.data) setSuppliers(sRes.data.data);
      if (pRes.data?.data) setParts(pRes.data.data);
    } catch {
      setSuppliers([
        { id: 1, name: 'Global Auto Parts Trading Dubai' },
        { id: 2, name: 'Nippon Auto Exports Tokyo' },
        { id: 3, name: 'Bangla Motor Spares Importers' },
      ]);
      setParts([
        { id: 1, partNumber: 'BOS-BP-001', name: 'Front Brake Pad Set', costPrice: 2200 },
        { id: 2, partNumber: 'DEN-SP-IX01', name: 'Iridium Spark Plug', costPrice: 650 },
        { id: 3, partNumber: 'MNN-OF-W68', name: 'Mann Oil Filter', costPrice: 400 },
      ]);
    }
  };

  const addPoLine = () => {
    setPoItems([...poItems, { partId: parts[0]?.id || 1, quantity: 1, unitPrice: parts[0]?.costPrice || 500 }]);
  };

  const removePoLine = (idx) => {
    if (poItems.length > 1) {
      setPoItems(poItems.filter((_, i) => i !== idx));
    }
  };

  const updatePoLine = (idx, field, value) => {
    const next = [...poItems];
    next[idx][field] = value;
    if (field === 'partId') {
      const selected = parts.find(p => p.id === Number(value));
      if (selected) next[idx].unitPrice = selected.costPrice;
    }
    setPoItems(next);
  };

  const totalPOCost = poItems.reduce((s, i) => s + (Number(i.quantity || 0) * Number(i.unitPrice || 0)), 0);

  const handleCreatePO = async (e) => {
    e.preventDefault();
    try {
      await api.post('/purchase', {
        supplierId: Number(supplierId),
        warehouseId: 1,
        paidAmount: paidAmount === '' ? totalPOCost : Number(paidAmount),
        items: poItems.map(p => ({
          partId: Number(p.partId),
          quantity: Number(p.quantity),
          unitPrice: Number(p.unitPrice)
        }))
      });
      setIsAddModalOpen(false);
      fetchPurchases();
    } catch {
      const sObj = suppliers.find(s => s.id === Number(supplierId));
      const invoiceNo = 'PO-' + new Date().toISOString().slice(0,10).replace(/-/g,'') + '-' + Math.floor(1000 + Math.random()*9000);
      setPurchases([
        {
          id: Date.now(),
          invoiceNumber: invoiceNo,
          supplierName: sObj?.name || 'Supplier',
          purchaseDate: new Date().toISOString(),
          totalAmount: totalPOCost,
          paidAmount: paidAmount === '' ? totalPOCost : Number(paidAmount),
          dueAmount: Math.max(0, totalPOCost - (paidAmount === '' ? totalPOCost : Number(paidAmount))),
          paymentStatus: (paidAmount === '' || Number(paidAmount) >= totalPOCost) ? 'Paid' : 'Partial',
          status: 'Received',
          items: poItems.map(i => {
            const p = parts.find(x => x.id === Number(i.partId));
            return {
              partNumber: p?.partNumber || '-',
              partName: p?.name || '-',
              quantity: i.quantity,
              unitPrice: i.unitPrice,
              totalCost: i.quantity * i.unitPrice
            };
          })
        },
        ...purchases
      ]);
      setIsAddModalOpen(false);
    }
  };

  const handlePrintPO = (po) => {
    printDocument({
      title: 'PURCHASE ORDER INTAKE',
      columns: ['#', 'Part #', 'Description', 'Intake Qty', 'Unit Cost (BDT)', 'Total (BDT)'],
      rows: (po.items || []).map((it, i) => [
        i + 1,
        it.partNumber,
        it.partName,
        it.quantity,
        it.unitPrice.toLocaleString(),
        it.totalCost.toLocaleString()
      ]),
      extraInfo: {
        'PO Number': po.invoiceNumber,
        'Supplier': po.supplierName,
        'Date': new Date(po.purchaseDate).toLocaleDateString('en-GB'),
        'Total Cost': `${branding.defaultCurrency.symbol} ${po.totalAmount.toLocaleString()}`,
        'Amount Paid': `${branding.defaultCurrency.symbol} ${po.paidAmount.toLocaleString()}`,
        'Payable Balance': `${branding.defaultCurrency.symbol} ${po.dueAmount.toLocaleString()}`,
        'Status': po.status
      }
    });
  };

  const handleDelete = (id) => {
    if (!window.confirm('Are you sure you want to delete this purchase intake record?')) return;
    setPurchases(purchases.filter(p => p.id !== id));
  };

  const filtered = purchases.filter(p => {
    const q = search.toLowerCase();
    return p.invoiceNumber.toLowerCase().includes(q) || p.supplierName.toLowerCase().includes(q);
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiTruck /></span>
            Purchases &amp; Stock Intake
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Supplier purchase orders, shipments and stock intake history
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <button type="button" className="btn btn-primary" onClick={() => setIsAddModalOpen(true)}>
            <FiPlus />
            <span>New Purchase Order</span>
          </button>

          <ExportToolbar
            title="Purchase Intake Report"
            columns={['PO #', 'Date', 'Supplier', 'Total Amount', 'Paid', 'Due', 'Status']}
            rows={filtered.map(p => [
              p.invoiceNumber,
              new Date(p.purchaseDate).toLocaleDateString('en-GB'),
              p.supplierName,
              p.totalAmount,
              p.paidAmount,
              p.dueAmount,
              p.paymentStatus,
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
            placeholder="Search PO # or Supplier..."
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
            { header: 'PO Number', accessor: 'invoiceNumber', render: (r) => <strong>{r.invoiceNumber}</strong> },
            { header: 'Supplier', accessor: 'supplierName' },
            { header: 'Intake Date', accessor: 'purchaseDate', render: (r) => new Date(r.purchaseDate).toLocaleDateString('en-GB') },
            { header: 'Total Cost', accessor: 'totalAmount', render: (r) => `${currency} ${r.totalAmount.toLocaleString()}` },
            { header: 'Paid', accessor: 'paidAmount', render: (r) => `${currency} ${r.paidAmount.toLocaleString()}` },
            { header: 'Payable Due', accessor: 'dueAmount', render: (r) => (
              <span style={{ color: r.dueAmount > 0 ? 'var(--danger)' : 'var(--text-muted)' }}>
                {currency} {r.dueAmount.toLocaleString()}
              </span>
            )},
            { header: 'Payment', accessor: 'paymentStatus', render: (r) => (
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
                    onClick={() => setSelectedPO(r)}
                    title="View Details"
                  >
                    <FiEye />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    onClick={() => handlePrintPO(r)}
                    title="Print PO"
                  >
                    <FiPrinter style={{ color: 'var(--primary-light)' }} />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDelete(r.id)}
                    title="Delete PO"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* CREATE NEW PURCHASE MODAL */}
      <Modal
        isOpen={isAddModalOpen}
        onClose={() => setIsAddModalOpen(false)}
        title="Create New Purchase Order (Stock Intake)"
        size="lg"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsAddModalOpen(false)}>Cancel</button>
            <button type="button" className="btn btn-primary" onClick={handleCreatePO}>
              Save &amp; Receive Stock ({currency} {totalPOCost.toLocaleString()})
            </button>
          </>
        }
      >
        <form onSubmit={handleCreatePO} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Select Supplier <span className="required">*</span></label>
              <select
                className="form-control"
                value={supplierId}
                onChange={(e) => setSupplierId(e.target.value)}
              >
                {suppliers.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Amount Paid to Supplier</label>
              <input
                type="number"
                className="form-control"
                placeholder={totalPOCost.toString()}
                value={paidAmount}
                onChange={(e) => setPaidAmount(e.target.value)}
              />
            </div>
          </div>

          <div>
            <div className="flex-between mb-8">
              <label className="form-label font-bold">Purchase Order Line Items</label>
              <button type="button" className="btn btn-ghost btn-sm" onClick={addPoLine}>
                <FiPlus /> Add Item
              </button>
            </div>

            <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
              {poItems.map((line, idx) => (
                <div key={idx} style={{ display: 'grid', gridTemplateColumns: '2fr 1fr 1fr auto', gap: '8px', alignItems: 'center' }}>
                  <select
                    className="form-control"
                    value={line.partId}
                    onChange={(e) => updatePoLine(idx, 'partId', e.target.value)}
                  >
                    {parts.map(p => (
                      <option key={p.id} value={p.id}>
                        {p.partNumber} - {p.name}
                      </option>
                    ))}
                  </select>

                  <input
                    type="number"
                    className="form-control"
                    placeholder="Qty"
                    value={line.quantity}
                    onChange={(e) => updatePoLine(idx, 'quantity', e.target.value)}
                  />

                  <input
                    type="number"
                    className="form-control"
                    placeholder="Unit Cost"
                    value={line.unitPrice}
                    onChange={(e) => updatePoLine(idx, 'unitPrice', e.target.value)}
                  />

                  <button
                    type="button"
                    onClick={() => removePoLine(idx)}
                    style={{ color: 'var(--danger)', padding: '6px' }}
                    disabled={poItems.length <= 1}
                  >
                    <FiTrash2 />
                  </button>
                </div>
              ))}
            </div>
          </div>

          <div className="flex-between" style={{ background: 'var(--bg-dark)', padding: '12px 16px', borderRadius: '8px', border: '1px solid var(--border)', fontWeight: 700 }}>
            <span>Total Intake Cost:</span>
            <span style={{ color: 'var(--accent)', fontSize: '1.2rem' }}>{currency} {totalPOCost.toLocaleString()}</span>
          </div>
        </form>
      </Modal>

      {/* VIEW PO MODAL */}
      <Modal
        isOpen={!!selectedPO}
        onClose={() => setSelectedPO(null)}
        title={`Purchase Order: ${selectedPO?.invoiceNumber}`}
        size="lg"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setSelectedPO(null)}>Close</button>
            <button
              type="button"
              className="btn btn-primary"
              onClick={() => {
                handlePrintPO(selectedPO);
                setSelectedPO(null);
              }}
            >
              <FiPrinter /> Print PO
            </button>
          </>
        }
      >
        {selectedPO && (
          <div>
            <div className="flex-between mb-16" style={{ background: 'var(--bg-input)', padding: '12px', borderRadius: '8px' }}>
              <div>
                <div style={{ fontWeight: 700, color: '#fff' }}>Supplier: {selectedPO.supplierName}</div>
                <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                  Intake Date: {new Date(selectedPO.purchaseDate).toLocaleDateString('en-GB')}
                </div>
              </div>
              <span className={`badge ${selectedPO.paymentStatus === 'Paid' ? 'badge-success' : 'badge-warning'}`}>
                {selectedPO.paymentStatus}
              </span>
            </div>

            <DataTable
              data={selectedPO.items || []}
              columns={[
                { header: 'Part #', accessor: 'partNumber' },
                { header: 'Item Name', accessor: 'partName' },
                { header: 'Qty', accessor: 'quantity' },
                { header: 'Unit Cost', accessor: 'unitPrice', render: (i) => `${currency} ${i.unitPrice?.toLocaleString()}` },
                { header: 'Total Cost', accessor: 'totalCost', render: (i) => `${currency} ${i.totalCost?.toLocaleString()}` },
              ]}
            />
          </div>
        )}
      </Modal>
    </div>
  );
}
