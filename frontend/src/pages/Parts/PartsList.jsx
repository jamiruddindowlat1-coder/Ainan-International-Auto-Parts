import React, { useState, useEffect } from 'react';
import { FiPlus, FiSearch, FiEdit2, FiTrash2, FiBox } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import branding from '../../config/branding';

export default function PartsList() {
  const [parts, setParts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [brands, setBrands] = useState([]);
  const [units, setUnits] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [selectedCat, setSelectedCat] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingPart, setEditingPart] = useState(null);

  const [formData, setFormData] = useState({
    partNumber: '',
    oemNumber: '',
    barcode: '',
    name: '',
    description: '',
    categoryId: 1,
    brandId: 1,
    unitId: 1,
    costPrice: '',
    sellingPrice: '',
    wholesalePrice: '',
    minStockAlert: 5,
  });

  useEffect(() => {
    fetchParts();
    fetchMetadata();
  }, []);

  const fetchParts = async () => {
    try {
      setLoading(true);
      const res = await api.get('/parts?pageSize=100');
      if (res.data?.data) {
        setParts(res.data.data);
      }
    } catch {
      // Demo fallback
      setParts([
        { id: 1, partNumber: 'BOS-BP-001', oemNumber: '04465-02220', name: 'Front Ceramic Brake Pad Set', categoryName: 'Brake System', brandName: 'Bosch', unitName: 'Pair', costPrice: 2200, sellingPrice: 3200, wholesalePrice: 2700, totalStock: 45, minStockAlert: 10 },
        { id: 2, partNumber: 'DEN-SP-IX01', oemNumber: '90919-01210', name: 'Iridium Power Spark Plug (SK20R11)', categoryName: 'Electrical', brandName: 'Denso', unitName: 'Pcs', costPrice: 650, sellingPrice: 950, wholesalePrice: 800, totalStock: 120, minStockAlert: 30 },
        { id: 3, partNumber: 'MNN-OF-W68', oemNumber: '90915-YZZE1', name: 'Mann Spin-On Engine Oil Filter', categoryName: 'Filtration', brandName: 'Mann-Filter', unitName: 'Pcs', costPrice: 400, sellingPrice: 650, wholesalePrice: 520, totalStock: 95, minStockAlert: 20 },
        { id: 4, partNumber: 'KYB-SA-33331', oemNumber: '48510-80255', name: 'KYB Excel-G Front Strut Assembly', categoryName: 'Suspension', brandName: 'KYB', unitName: 'Pcs', costPrice: 6500, sellingPrice: 9200, wholesalePrice: 7800, totalStock: 12, minStockAlert: 4 },
        { id: 5, partNumber: 'MOB-OIL-5W30', oemNumber: 'MOBIL-SYN-4L', name: 'Mobil 1 Fully Synthetic Engine Oil (4L)', categoryName: 'Lubricants', brandName: 'Mobil 1', unitName: 'Pcs', costPrice: 3800, sellingPrice: 5200, wholesalePrice: 4400, totalStock: 38, minStockAlert: 15 },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const fetchMetadata = async () => {
    try {
      const [cRes, bRes, uRes] = await Promise.all([
        api.get('/parts/categories'),
        api.get('/parts/brands'),
        api.get('/parts/units')
      ]);
      if (cRes.data?.data) setCategories(cRes.data.data);
      if (bRes.data?.data) setBrands(bRes.data.data);
      if (uRes.data?.data) setUnits(uRes.data.data);
    } catch {
      setCategories([{ id: 1, name: 'Brake System' }, { id: 2, name: 'Electrical' }, { id: 3, name: 'Filtration' }, { id: 4, name: 'Suspension' }, { id: 5, name: 'Lubricants' }]);
      setBrands([{ id: 1, name: 'Bosch' }, { id: 2, name: 'Denso' }, { id: 3, name: 'Mann-Filter' }, { id: 4, name: 'KYB' }, { id: 5, name: 'Mobil 1' }]);
      setUnits([{ id: 1, shortCode: 'Pcs' }, { id: 2, shortCode: 'Pair' }, { id: 3, shortCode: 'Set' }, { id: 4, shortCode: 'Box' }]);
    }
  };

  const handleOpenModal = (part = null) => {
    if (part) {
      setEditingPart(part);
      setFormData({
        partNumber: part.partNumber,
        oemNumber: part.oemNumber || '',
        barcode: part.barcode || '',
        name: part.name,
        description: part.description || '',
        categoryId: part.categoryId || 1,
        brandId: part.brandId || 1,
        unitId: part.unitId || 1,
        costPrice: part.costPrice,
        sellingPrice: part.sellingPrice,
        wholesalePrice: part.wholesalePrice,
        minStockAlert: part.minStockAlert || 5,
      });
    } else {
      setEditingPart(null);
      setFormData({
        partNumber: '',
        oemNumber: '',
        barcode: '',
        name: '',
        description: '',
        categoryId: 1,
        brandId: 1,
        unitId: 1,
        costPrice: '',
        sellingPrice: '',
        wholesalePrice: '',
        minStockAlert: 5,
      });
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        ...formData,
        costPrice: Number(formData.costPrice || 0),
        sellingPrice: Number(formData.sellingPrice || 0),
        wholesalePrice: Number(formData.wholesalePrice || 0),
        minStockAlert: Number(formData.minStockAlert || 5),
      };

      if (editingPart) {
        await api.put(`/parts/${editingPart.id}`, payload);
      } else {
        await api.post('/parts', payload);
      }
      setIsModalOpen(false);
      fetchParts();
    } catch {
      // Demo update
      if (editingPart) {
        setParts(parts.map(p => p.id === editingPart.id ? { ...p, ...formData } : p));
      } else {
        setParts([...parts, { id: Date.now(), ...formData, totalStock: 0 }]);
      }
      setIsModalOpen(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this part?')) return;
    try {
      await api.delete(`/parts/${id}`);
      fetchParts();
    } catch {
      setParts(parts.filter(p => p.id !== id));
    }
  };

  const filtered = parts.filter(p => {
    const s = search.toLowerCase();
    const matchesSearch = p.name.toLowerCase().includes(s) ||
                          p.partNumber.toLowerCase().includes(s) ||
                          (p.oemNumber && p.oemNumber.toLowerCase().includes(s));
    const matchesCat = selectedCat ? p.categoryName === selectedCat : true;
    return matchesSearch && matchesCat;
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      {/* PAGE HEADER */}
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiBox /></span>
            Auto Parts Catalog &amp; Inventory
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Comprehensive parts catalog with OEM numbers, brands and stock levels
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <button type="button" className="btn btn-primary" onClick={() => handleOpenModal()}>
            <FiPlus />
            <span>Add New Part</span>
          </button>

          <ExportToolbar
            title="Auto Parts Master Catalog"
            columns={['Part #', 'OEM #', 'Name', 'Category', 'Brand', 'Cost', 'Selling', 'Stock']}
            rows={filtered.map(p => [
              p.partNumber,
              p.oemNumber || '-',
              p.name,
              p.categoryName || '-',
              p.brandName || '-',
              p.costPrice,
              p.sellingPrice,
              p.totalStock || 0,
            ])}
            extraInfo={{
              'Total Items': filtered.length,
              'Exported By': 'AIAPS ERP System'
            }}
          />
        </div>
      </div>

      {/* FILTER BAR */}
      <div className="card mb-16">
        <div className="flex-between" style={{ flexWrap: 'wrap', gap: '12px' }}>
          <div className="search-wrapper">
            <FiSearch className="search-icon" />
            <input
              type="text"
              className="search-input"
              placeholder="Search by Part #, OEM, or Name..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>

          <div style={{ display: 'flex', gap: '10px' }}>
            <select
              className="form-control"
              style={{ width: '180px' }}
              value={selectedCat}
              onChange={(e) => setSelectedCat(e.target.value)}
            >
              <option value="">All Categories</option>
              {categories.map(c => (
                <option key={c.id} value={c.name}>{c.name}</option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {/* DATA TABLE */}
      <div className="card">
        <DataTable
          loading={loading}
          data={filtered}
          columns={[
            {
              header: 'Part Number',
              accessor: 'partNumber',
              render: (r) => (
                <div>
                  <strong>{r.partNumber}</strong>
                  {r.oemNumber && (
                    <div style={{ fontSize: '0.72rem', color: 'var(--text-muted)' }}>
                      OEM: {r.oemNumber}
                    </div>
                  )}
                </div>
              )
            },
            {
              header: 'Item Description',
              accessor: 'name',
              render: (r) => (
                <div>
                  <div style={{ fontWeight: 600, color: '#fff' }}>{r.name}</div>
                  <div style={{ fontSize: '0.72rem', color: 'var(--text-muted)' }}>
                    {r.categoryName} &bull; {r.brandName}
                  </div>
                </div>
              )
            },
            {
              header: 'Cost Price',
              accessor: 'costPrice',
              render: (r) => `${currency} ${r.costPrice?.toLocaleString()}`
            },
            {
              header: 'Selling Price',
              accessor: 'sellingPrice',
              render: (r) => (
                <strong style={{ color: 'var(--accent-light)' }}>
                  {currency} {r.sellingPrice?.toLocaleString()}
                </strong>
              )
            },
            {
              header: 'Stock Status',
              accessor: 'totalStock',
              render: (r) => {
                const isLow = (r.totalStock || 0) <= (r.minStockAlert || 5);
                return (
                  <span className={`badge ${isLow ? 'badge-danger' : 'badge-success'}`}>
                    {r.totalStock || 0} {r.unitName || 'Pcs'}
                  </span>
                );
              }
            },
            {
              header: 'Actions',
              align: 'right',
              render: (r) => (
                <div className="td-actions" style={{ justifyContent: 'flex-end' }}>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    onClick={() => handleOpenModal(r)}
                    title="Edit Part"
                  >
                    <FiEdit2 />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDelete(r.id)}
                    title="Delete Part"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* CREATE / EDIT MODAL */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingPart ? `Edit Part: ${editingPart.partNumber}` : 'Add New Auto Part'}
        size="lg"
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsModalOpen(false)}>
              Cancel
            </button>
            <button type="button" className="btn btn-primary" onClick={handleSubmit}>
              {editingPart ? 'Save Changes' : 'Create Part'}
            </button>
          </>
        }
      >
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          <div className="form-grid-3">
            <div className="form-group">
              <label className="form-label">Part Number <span className="required">*</span></label>
              <input
                type="text"
                className="form-control"
                required
                value={formData.partNumber}
                onChange={(e) => setFormData({ ...formData, partNumber: e.target.value })}
                placeholder="e.g. BOS-BP-001"
              />
            </div>
            <div className="form-group">
              <label className="form-label">OEM Number</label>
              <input
                type="text"
                className="form-control"
                value={formData.oemNumber}
                onChange={(e) => setFormData({ ...formData, oemNumber: e.target.value })}
                placeholder="e.g. 04465-02220"
              />
            </div>
            <div className="form-group">
              <label className="form-label">Barcode</label>
              <input
                type="text"
                className="form-control"
                value={formData.barcode}
                onChange={(e) => setFormData({ ...formData, barcode: e.target.value })}
                placeholder="e.g. 890123450001"
              />
            </div>
          </div>

          <div className="form-group">
            <label className="form-label">Part Name / Title <span className="required">*</span></label>
            <input
              type="text"
              className="form-control"
              required
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="e.g. Front Ceramic Brake Pad Set"
            />
          </div>

          <div className="form-grid-3">
            <div className="form-group">
              <label className="form-label">Category</label>
              <select
                className="form-control"
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: Number(e.target.value) })}
              >
                {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Brand</label>
              <select
                className="form-control"
                value={formData.brandId}
                onChange={(e) => setFormData({ ...formData, brandId: Number(e.target.value) })}
              >
                {brands.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Unit</label>
              <select
                className="form-control"
                value={formData.unitId}
                onChange={(e) => setFormData({ ...formData, unitId: Number(e.target.value) })}
              >
                {units.map(u => <option key={u.id} value={u.id}>{u.shortCode}</option>)}
              </select>
            </div>
          </div>

          <div className="form-grid-3">
            <div className="form-group">
              <label className="form-label">Cost Price (BDT) <span className="required">*</span></label>
              <input
                type="number"
                className="form-control"
                required
                value={formData.costPrice}
                onChange={(e) => setFormData({ ...formData, costPrice: e.target.value })}
                placeholder="2200"
              />
            </div>
            <div className="form-group">
              <label className="form-label">Selling Price (Retail) <span className="required">*</span></label>
              <input
                type="number"
                className="form-control"
                required
                value={formData.sellingPrice}
                onChange={(e) => setFormData({ ...formData, sellingPrice: e.target.value })}
                placeholder="3200"
              />
            </div>
            <div className="form-group">
              <label className="form-label">Wholesale Price</label>
              <input
                type="number"
                className="form-control"
                value={formData.wholesalePrice}
                onChange={(e) => setFormData({ ...formData, wholesalePrice: e.target.value })}
                placeholder="2700"
              />
            </div>
          </div>
        </form>
      </Modal>
    </div>
  );
}
