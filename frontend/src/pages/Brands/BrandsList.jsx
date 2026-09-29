import React, { useState, useEffect } from 'react';
import { FiTag, FiSearch, FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import branding from '../../config/branding';

const DEMO_BRANDS = [
  { id: 1, brandName: 'Denso', country: 'Japan', category: 'Electrical & Ignition', description: 'World-leading auto parts manufacturer', status: 'Active', partsCount: 48 },
  { id: 2, brandName: 'Bosch', country: 'Germany', category: 'Brakes & Filters', description: 'Premium German engineering auto parts', status: 'Active', partsCount: 62 },
  { id: 3, brandName: 'KYB', country: 'Japan', category: 'Suspension', description: 'Leading shock absorber & strut manufacturer', status: 'Active', partsCount: 35 },
  { id: 4, brandName: 'Mann-Filter', country: 'Germany', category: 'Filters', description: 'OEM-quality filtration solutions', status: 'Active', partsCount: 27 },
  { id: 5, brandName: 'Mobil 1', country: 'USA', category: 'Lubricants', description: 'Fully synthetic engine oil', status: 'Active', partsCount: 14 },
  { id: 6, brandName: 'NGK', country: 'Japan', category: 'Ignition', description: 'Spark plug & sensor specialist', status: 'Active', partsCount: 31 },
  { id: 7, brandName: 'AISIN', country: 'Japan', category: 'Drivetrain', description: 'OEM clutch kit & drivetrain parts', status: 'Active', partsCount: 19 },
  { id: 8, brandName: 'Castrol', country: 'UK', category: 'Lubricants', description: 'Engine oil & lubricants brand', status: 'Active', partsCount: 12 },
  { id: 9, brandName: 'Philips', country: 'Netherlands', category: 'Electrical', description: 'Auto lighting & bulbs', status: 'Active', partsCount: 9 },
  { id: 10, brandName: 'Gates', country: 'USA', category: 'Belts & Hoses', description: 'Timing belts, drive belts & cooling hoses', status: 'Active', partsCount: 22 },
];

export default function BrandsList() {
  const [brands, setBrands] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingBrand, setEditingBrand] = useState(null);
  const [formData, setFormData] = useState({
    brandName: '',
    country: '',
    category: '',
    description: '',
    status: 'Active',
  });

  useEffect(() => { fetchBrands(); }, []);

  const fetchBrands = async () => {
    try {
      setLoading(true);
      const res = await api.get('/brands');
      if (res.data?.data) setBrands(res.data.data);
      else setBrands(DEMO_BRANDS);
    } catch {
      setBrands(DEMO_BRANDS);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (brand = null) => {
    if (brand) {
      setEditingBrand(brand);
      setFormData({ brandName: brand.brandName, country: brand.country, category: brand.category, description: brand.description, status: brand.status });
    } else {
      setEditingBrand(null);
      setFormData({ brandName: '', country: '', category: '', description: '', status: 'Active' });
    }
    setIsModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      if (editingBrand) {
        await api.put(`/brands/${editingBrand.id}`, formData);
      } else {
        await api.post('/brands', formData);
      }
      setIsModalOpen(false);
      fetchBrands();
    } catch {
      if (editingBrand) {
        setBrands(prev => prev.map(b => b.id === editingBrand.id ? { ...b, ...formData } : b));
      } else {
        setBrands(prev => [...prev, { id: Date.now(), ...formData, partsCount: 0 }]);
      }
      setIsModalOpen(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this brand?')) return;
    try {
      await api.delete(`/brands/${id}`);
      fetchBrands();
    } catch {
      setBrands(prev => prev.filter(b => b.id !== id));
    }
  };

  const filtered = brands.filter(b =>
    b.brandName?.toLowerCase().includes(search.toLowerCase()) ||
    b.country?.toLowerCase().includes(search.toLowerCase()) ||
    b.category?.toLowerCase().includes(search.toLowerCase())
  );

  const exportRows = filtered.map((b, i) => [
    i + 1, b.brandName, b.country, b.category, b.partsCount ?? 0, b.status
  ]);

  return (
    <div>
      {/* PAGE HEADER */}
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiTag /></span>
            Parts Brands Management
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Manage all auto parts brands — {filtered.length} brands registered
          </p>
        </div>
        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <ExportToolbar
            title="Parts Brands List"
            columns={['#', 'Brand Name', 'Country', 'Category', 'Parts Count', 'Status']}
            rows={exportRows}
            extraInfo={{ 'Company': branding.company.name, 'Date': new Date().toLocaleDateString('en-GB') }}
          />
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            <FiPlus /> <span>Add Brand</span>
          </button>
        </div>
      </div>

      {/* SEARCH */}
      <div className="card mb-16">
        <div style={{ position: 'relative', maxWidth: '320px' }}>
          <FiSearch style={{ position: 'absolute', left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
          <input
            type="text"
            className="form-control"
            style={{ paddingLeft: '36px' }}
            placeholder="Search brand, country or category..."
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>
      </div>

      {/* TABLE */}
      <div className="card">
        <DataTable
          loading={loading}
          columns={[
            { header: '#', accessor: 'id', render: (_, i) => i + 1 },
            {
              header: 'Brand Name', accessor: 'brandName', render: (r) => (
                <div>
                  <div style={{ fontWeight: 700, color: '#fff' }}>{r.brandName}</div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{r.description}</div>
                </div>
              )
            },
            { header: 'Country', accessor: 'country', render: r => <span className="badge badge-primary">{r.country}</span> },
            { header: 'Category', accessor: 'category' },
            { header: 'Parts', accessor: 'partsCount', render: r => <strong>{r.partsCount ?? 0}</strong> },
            {
              header: 'Status', accessor: 'status', render: r => (
                <span className={`badge ${r.status === 'Active' ? 'badge-success' : 'badge-warning'}`}>{r.status}</span>
              )
            },
            {
              header: 'Actions', accessor: 'id', render: (r) => (
                <div style={{ display: 'flex', gap: '8px' }}>
                  <button className="btn btn-ghost btn-sm" onClick={() => handleOpenModal(r)} title="Edit">
                    <FiEdit2 style={{ color: 'var(--accent)' }} />
                  </button>
                  <button className="btn btn-ghost btn-sm" onClick={() => handleDelete(r.id)} title="Delete">
                    <FiTrash2 style={{ color: 'var(--danger)' }} />
                  </button>
                </div>
              )
            },
          ]}
          data={filtered}
        />
      </div>

      {/* MODAL */}
      <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} title={editingBrand ? 'Edit Brand' : 'Add New Brand'}>
        <form onSubmit={handleSave} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Brand Name *</label>
            <input type="text" className="form-control" required value={formData.brandName}
              onChange={e => setFormData({ ...formData, brandName: e.target.value })} placeholder="e.g. Denso" />
          </div>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Country of Origin</label>
              <input type="text" className="form-control" value={formData.country}
                onChange={e => setFormData({ ...formData, country: e.target.value })} placeholder="e.g. Japan" />
            </div>
            <div className="form-group">
              <label className="form-label">Category</label>
              <input type="text" className="form-control" value={formData.category}
                onChange={e => setFormData({ ...formData, category: e.target.value })} placeholder="e.g. Filters" />
            </div>
          </div>
          <div className="form-group">
            <label className="form-label">Description</label>
            <textarea className="form-control" value={formData.description}
              onChange={e => setFormData({ ...formData, description: e.target.value })} rows={2} />
          </div>
          <div className="form-group">
            <label className="form-label">Status</label>
            <select className="form-control" value={formData.status}
              onChange={e => setFormData({ ...formData, status: e.target.value })}>
              <option value="Active">Active</option>
              <option value="Inactive">Inactive</option>
            </select>
          </div>
          <div style={{ display: 'flex', gap: '10px', justifyContent: 'flex-end', marginTop: '8px' }}>
            <button type="button" className="btn btn-ghost" onClick={() => setIsModalOpen(false)}>Cancel</button>
            <button type="submit" className="btn btn-primary">{editingBrand ? 'Update Brand' : 'Add Brand'}</button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
