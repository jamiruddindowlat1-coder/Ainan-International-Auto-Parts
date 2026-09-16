import React, { useState, useEffect } from 'react';
import { FiGrid, FiSearch, FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import branding from '../../config/branding';

const DEMO_CATEGORIES = [
  { id: 1, categoryName: 'Engine Parts', description: 'Pistons, valves, gaskets, engine mounts', partsCount: 124, status: 'Active' },
  { id: 2, categoryName: 'Brake System', description: 'Brake pads, discs, drums, calipers, cylinders', partsCount: 87, status: 'Active' },
  { id: 3, categoryName: 'Suspension & Steering', description: 'Shock absorbers, struts, tie rods, ball joints', partsCount: 65, status: 'Active' },
  { id: 4, categoryName: 'Filters', description: 'Oil filters, air filters, fuel filters, cabin filters', partsCount: 98, status: 'Active' },
  { id: 5, categoryName: 'Ignition System', description: 'Spark plugs, ignition coils, distributors', partsCount: 55, status: 'Active' },
  { id: 6, categoryName: 'Electrical & Lighting', description: 'Bulbs, alternators, starters, sensors', partsCount: 73, status: 'Active' },
  { id: 7, categoryName: 'Transmission', description: 'Gearbox parts, clutch kits, CV joints', partsCount: 44, status: 'Active' },
  { id: 8, categoryName: 'Lubricants & Fluids', description: 'Engine oil, gear oil, brake fluid, coolant', partsCount: 36, status: 'Active' },
  { id: 9, categoryName: 'Cooling System', description: 'Radiators, thermostats, water pumps, hoses', partsCount: 51, status: 'Active' },
  { id: 10, categoryName: 'Body Parts', description: 'Mirrors, handles, hinges, bumpers', partsCount: 29, status: 'Active' },
  { id: 11, categoryName: 'Belts & Chains', description: 'Timing belts, V-belts, chains, tensioners', partsCount: 40, status: 'Active' },
  { id: 12, categoryName: 'Exhaust System', description: 'Mufflers, catalytic converters, exhaust pipes', partsCount: 22, status: 'Active' },
];

export default function CategoriesList() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState(null);
  const [formData, setFormData] = useState({
    categoryName: '',
    description: '',
    status: 'Active',
  });

  useEffect(() => { fetchCategories(); }, []);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      const res = await api.get('/categories');
      if (res.data?.data) setCategories(res.data.data);
      else setCategories(DEMO_CATEGORIES);
    } catch {
      setCategories(DEMO_CATEGORIES);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (cat = null) => {
    if (cat) {
      setEditingCategory(cat);
      setFormData({ categoryName: cat.categoryName, description: cat.description, status: cat.status });
    } else {
      setEditingCategory(null);
      setFormData({ categoryName: '', description: '', status: 'Active' });
    }
    setIsModalOpen(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      if (editingCategory) {
        await api.put(`/categories/${editingCategory.id}`, formData);
      } else {
        await api.post('/categories', formData);
      }
      setIsModalOpen(false);
      fetchCategories();
    } catch {
      if (editingCategory) {
        setCategories(prev => prev.map(c => c.id === editingCategory.id ? { ...c, ...formData } : c));
      } else {
        setCategories(prev => [...prev, { id: Date.now(), ...formData, partsCount: 0 }]);
      }
      setIsModalOpen(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this category?')) return;
    try {
      await api.delete(`/categories/${id}`);
      fetchCategories();
    } catch {
      setCategories(prev => prev.filter(c => c.id !== id));
    }
  };

  const filtered = categories.filter(c =>
    c.categoryName?.toLowerCase().includes(search.toLowerCase()) ||
    c.description?.toLowerCase().includes(search.toLowerCase())
  );

  const exportRows = filtered.map((c, i) => [
    i + 1, c.categoryName, c.description, c.partsCount ?? 0, c.status
  ]);

  return (
    <div>
      {/* PAGE HEADER */}
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiGrid /></span>
            Parts Categories Management
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Manage all parts categories — {filtered.length} categories found
          </p>
        </div>
        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <ExportToolbar
            title="Parts Categories List"
            columns={['#', 'Category Name', 'Description', 'Parts Count', 'Status']}
            rows={exportRows}
            extraInfo={{ 'Company': branding.company.name, 'Date': new Date().toLocaleDateString('en-GB') }}
          />
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            <FiPlus /> <span>Add Category</span>
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
            placeholder="Search category or description..."
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>
      </div>

      {/* STATS SUMMARY */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '16px', marginBottom: '20px' }}>
        {[
          { label: 'Total Categories', value: categories.length, color: 'var(--primary-light)' },
          { label: 'Active Categories', value: categories.filter(c => c.status === 'Active').length, color: 'var(--success)' },
          { label: 'Total Parts (All)', value: categories.reduce((s, c) => s + (c.partsCount || 0), 0), color: 'var(--accent)' },
        ].map((stat, i) => (
          <div key={i} className="card" style={{ padding: '16px', display: 'flex', gap: '12px', alignItems: 'center' }}>
            <div style={{ width: '44px', height: '44px', borderRadius: '10px', background: `${stat.color}22`, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
              <FiGrid style={{ color: stat.color, fontSize: '1.2rem' }} />
            </div>
            <div>
              <div style={{ fontSize: '1.4rem', fontWeight: 800, color: '#fff' }}>{stat.value}</div>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{stat.label}</div>
            </div>
          </div>
        ))}
      </div>

      {/* TABLE */}
      <div className="card">
        <DataTable
          loading={loading}
          columns={[
            { header: '#', accessor: 'id', render: (_, i) => i + 1 },
            {
              header: 'Category Name', accessor: 'categoryName', render: (r) => (
                <div>
                  <div style={{ fontWeight: 700, color: '#fff' }}>{r.categoryName}</div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{r.description}</div>
                </div>
              )
            },
            { header: 'Parts Count', accessor: 'partsCount', render: r => (
              <span style={{ fontWeight: 700, color: 'var(--accent)' }}>{r.partsCount ?? 0}</span>
            )},
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
      <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} title={editingCategory ? 'Edit Category' : 'Add New Category'}>
        <form onSubmit={handleSave} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Category Name *</label>
            <input type="text" className="form-control" required value={formData.categoryName}
              onChange={e => setFormData({ ...formData, categoryName: e.target.value })} placeholder="e.g. Brake System" />
          </div>
          <div className="form-group">
            <label className="form-label">Description</label>
            <textarea className="form-control" value={formData.description}
              onChange={e => setFormData({ ...formData, description: e.target.value })}
              rows={3} placeholder="Short description of what parts this category covers..." />
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
            <button type="submit" className="btn btn-primary">{editingCategory ? 'Update Category' : 'Add Category'}</button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
