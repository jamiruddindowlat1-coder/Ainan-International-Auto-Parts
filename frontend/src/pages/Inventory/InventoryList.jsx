import React, { useState, useEffect } from 'react';
import { FiDatabase, FiSearch, FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';

export default function InventoryList() {
  const [stocks, setStocks] = useState([]);
  const [warehouses, setWarehouses] = useState([]);
  const [selectedWh, setSelectedWh] = useState('');
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  // Modal States
  const [isAdjustModalOpen, setIsAdjustModalOpen] = useState(false);
  const [isWhModalOpen, setIsWhModalOpen] = useState(false);

  // Stock Adjustment Form
  const [selectedPartId, setSelectedPartId] = useState(1);
  const [adjustQty, setAdjustQty] = useState('');
  const [adjustType, setAdjustType] = useState('Add'); // 'Add' or 'Subtract'
  const [adjustReason, setAdjustReason] = useState('Stock Count Correction');

  // Warehouse Form
  const [whName, setWhName] = useState('');
  const [whCode, setWhCode] = useState('');
  const [whLocation, setWhLocation] = useState('');

  useEffect(() => {
    fetchInventory();
  }, []);

  const fetchInventory = async () => {
    try {
      setLoading(true);
      const [stockRes, whRes] = await Promise.all([
        api.get('/inventory/stocks'),
        api.get('/inventory/warehouses')
      ]);

      if (stockRes.data?.data) setStocks(stockRes.data.data);
      if (whRes.data?.data) setWarehouses(whRes.data.data);
    } catch {
      setStocks([
        { id: 1, warehouseName: 'Main Central Warehouse', partId: 1, partNumber: 'BOS-BP-001', partName: 'Front Ceramic Brake Pad Set', quantity: 45, rackLocation: 'Rack-A1', binLocation: 'Bin-04' },
        { id: 2, warehouseName: 'Main Central Warehouse', partId: 2, partNumber: 'DEN-SP-IX01', partName: 'Iridium Spark Plug', quantity: 120, rackLocation: 'Rack-B2', binLocation: 'Bin-12' },
        { id: 3, warehouseName: 'Main Central Warehouse', partId: 3, partNumber: 'MNN-OF-W68', partName: 'Mann Engine Oil Filter', quantity: 95, rackLocation: 'Rack-C1', binLocation: 'Bin-02' },
        { id: 4, warehouseName: 'Main Central Warehouse', partId: 4, partNumber: 'KYB-SA-33331', partName: 'KYB Front Strut Assembly', quantity: 2, rackLocation: 'Rack-D3', binLocation: 'Bin-01' },
        { id: 5, warehouseName: 'Chittagong Port Warehouse', partId: 1, partNumber: 'BOS-BP-001', partName: 'Front Ceramic Brake Pad Set', quantity: 20, rackLocation: 'Rack-CTG-1', binLocation: 'Bin-A' },
      ]);
      setWarehouses([
        { id: 1, name: 'Main Central Warehouse', code: 'WH-MAIN', location: 'Tejgaon, Dhaka' },
        { id: 2, name: 'Chittagong Port Warehouse', code: 'WH-CTG', location: 'Agrabad, CTG' },
        { id: 3, name: 'Uttara Showroom & Store', code: 'WH-UTT', location: 'Sector 3, Uttara' }
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleAdjustStock = (e) => {
    e.preventDefault();
    const delta = adjustType === 'Add' ? Number(adjustQty) : -Number(adjustQty);
    setStocks(stocks.map(s => {
      if (s.partId === Number(selectedPartId)) {
        return { ...s, quantity: Math.max(0, s.quantity + delta) };
      }
      return s;
    }));
    setIsAdjustModalOpen(false);
    setAdjustQty('');
  };

  const handleAddWarehouse = (e) => {
    e.preventDefault();
    setWarehouses([...warehouses, { id: Date.now(), name: whName, code: whCode, location: whLocation }]);
    setIsWhModalOpen(false);
    setWhName('');
    setWhCode('');
    setWhLocation('');
  };

  const handleDeleteStockItem = (id) => {
    if (!window.confirm('Are you sure you want to remove this stock record?')) return;
    setStocks(stocks.filter(s => s.id !== id));
  };

  const filtered = stocks.filter(s => {
    const q = search.toLowerCase();
    const matchesSearch = s.partNumber.toLowerCase().includes(q) || s.partName.toLowerCase().includes(q);
    const matchesWh = selectedWh ? s.warehouseName === selectedWh : true;
    return matchesSearch && matchesWh;
  });

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiDatabase /></span>
            Warehouse Inventory &amp; Stock Levels
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Multi-warehouse stock tracking, bin locations, and inventory adjustments
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <button type="button" className="btn btn-primary" onClick={() => setIsAdjustModalOpen(true)}>
            <FiPlus />
            <span>Stock Adjustment</span>
          </button>
          <button type="button" className="btn btn-ghost" onClick={() => setIsWhModalOpen(true)}>
            <FiPlus />
            <span>Add Warehouse</span>
          </button>

          <ExportToolbar
            title="Warehouse Stock Inventory Report"
            columns={['Warehouse', 'Part #', 'Item Name', 'Stock Qty', 'Rack Location', 'Bin']}
            rows={filtered.map(s => [
              s.warehouseName,
              s.partNumber,
              s.partName,
              s.quantity,
              s.rackLocation || '-',
              s.binLocation || '-',
            ])}
          />
        </div>
      </div>

      <div className="card mb-16">
        <div className="flex-between" style={{ flexWrap: 'wrap', gap: '12px' }}>
          <div className="search-wrapper">
            <FiSearch className="search-icon" />
            <input
              type="text"
              className="search-input"
              placeholder="Search Part # or Name in stock..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>

          <select
            className="form-control"
            style={{ width: '220px' }}
            value={selectedWh}
            onChange={(e) => setSelectedWh(e.target.value)}
          >
            <option value="">All Warehouses</option>
            {warehouses.map(w => (
              <option key={w.id} value={w.name}>{w.name}</option>
            ))}
          </select>
        </div>
      </div>

      <div className="card">
        <DataTable
          loading={loading}
          data={filtered}
          columns={[
            { header: 'Warehouse', accessor: 'warehouseName', render: (r) => <span className="badge badge-primary">{r.warehouseName}</span> },
            { header: 'Part #', accessor: 'partNumber', render: (r) => <strong>{r.partNumber}</strong> },
            { header: 'Item Description', accessor: 'partName' },
            {
              header: 'Quantity In Stock',
              accessor: 'quantity',
              render: (r) => (
                <span className={`badge ${r.quantity <= 5 ? 'badge-danger' : 'badge-success'}`}>
                  {r.quantity} Pcs
                </span>
              )
            },
            { header: 'Rack Location', accessor: 'rackLocation', render: (r) => r.rackLocation || '-' },
            { header: 'Bin Location', accessor: 'binLocation', render: (r) => r.binLocation || '-' },
            {
              header: 'Actions',
              align: 'right',
              render: (r) => (
                <div className="td-actions" style={{ justifyContent: 'flex-end' }}>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDeleteStockItem(r.id)}
                    title="Remove Record"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* STOCK ADJUSTMENT MODAL */}
      <Modal
        isOpen={isAdjustModalOpen}
        onClose={() => setIsAdjustModalOpen(false)}
        title="Adjust Inventory Stock"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsAdjustModalOpen(false)}>Cancel</button>
            <button type="button" className="btn btn-primary" onClick={handleAdjustStock}>Save Adjustment</button>
          </>
        }
      >
        <form onSubmit={handleAdjustStock} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Select Part</label>
            <select
              className="form-control"
              value={selectedPartId}
              onChange={(e) => setSelectedPartId(e.target.value)}
            >
              {stocks.map(s => (
                <option key={s.id} value={s.partId}>{s.partNumber} - {s.partName} (Current: {s.quantity})</option>
              ))}
            </select>
          </div>

          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Adjustment Action</label>
              <select
                className="form-control"
                value={adjustType}
                onChange={(e) => setAdjustType(e.target.value)}
              >
                <option value="Add">Add (+) Stock</option>
                <option value="Subtract">Subtract (-) Stock</option>
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Quantity</label>
              <input
                type="number"
                className="form-control"
                required
                placeholder="10"
                value={adjustQty}
                onChange={(e) => setAdjustQty(e.target.value)}
              />
            </div>
          </div>

          <div className="form-group">
            <label className="form-label">Reason</label>
            <input
              type="text"
              className="form-control"
              value={adjustReason}
              onChange={(e) => setAdjustReason(e.target.value)}
            />
          </div>
        </form>
      </Modal>

      {/* ADD WAREHOUSE MODAL */}
      <Modal
        isOpen={isWhModalOpen}
        onClose={() => setIsWhModalOpen(false)}
        title="Add New Warehouse"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsWhModalOpen(false)}>Cancel</button>
            <button type="button" className="btn btn-primary" onClick={handleAddWarehouse}>Save Warehouse</button>
          </>
        }
      >
        <form onSubmit={handleAddWarehouse} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Warehouse Name <span className="required">*</span></label>
            <input
              type="text"
              className="form-control"
              required
              placeholder="e.g. Gazipur Central Hub"
              value={whName}
              onChange={(e) => setWhName(e.target.value)}
            />
          </div>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Warehouse Code <span className="required">*</span></label>
              <input
                type="text"
                className="form-control"
                required
                placeholder="WH-GZP"
                value={whCode}
                onChange={(e) => setWhCode(e.target.value)}
              />
            </div>
            <div className="form-group">
              <label className="form-label">Location / Address</label>
              <input
                type="text"
                className="form-control"
                placeholder="Gazipur Bypass Road"
                value={whLocation}
                onChange={(e) => setWhLocation(e.target.value)}
              />
            </div>
          </div>
        </form>
      </Modal>
    </div>
  );
}
