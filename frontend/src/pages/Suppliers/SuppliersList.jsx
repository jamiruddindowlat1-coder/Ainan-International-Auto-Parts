import React, { useState, useEffect } from 'react';
import { FiTruck, FiSearch, FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import branding from '../../config/branding';

export default function SuppliersList() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingSupplier, setEditingSupplier] = useState(null);

  const [formData, setFormData] = useState({
    name: '',
    company: '',
    phone: '',
    email: '',
    address: '',
    taxNumber: ''
  });

  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      setLoading(true);
      const res = await api.get('/suppliers');
      if (res.data?.data) setSuppliers(res.data.data);
    } catch {
      setSuppliers([
        { id: 1, name: 'Global Auto Parts Trading Dubai', company: 'Global Parts FZE', phone: '+971 4 1234567', email: 'sales@globalpartsfze.ae', address: 'Al Quoz Industrial, Dubai, UAE', currentBalance: 120000 },
        { id: 2, name: 'Nippon Auto Exports Tokyo', company: 'Nippon Exports Co.', phone: '+81 3 55556666', email: 'orders@nipponexports.jp', address: 'Yokohama, Japan', currentBalance: 350000 },
        { id: 3, name: 'Bangla Motor Spares Importers', company: 'BMS Importers Ltd.', phone: '+880 2 9887766', email: 'info@bmsimporters.com', address: 'Motijheel, Dhaka', currentBalance: 45000 },
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (supplier = null) => {
    if (supplier) {
      setEditingSupplier(supplier);
      setFormData({
        name: supplier.name,
        company: supplier.company || '',
        phone: supplier.phone,
        email: supplier.email || '',
        address: supplier.address || '',
        taxNumber: supplier.taxNumber || ''
      });
    } else {
      setEditingSupplier(null);
      setFormData({
        name: '',
        company: '',
        phone: '',
        email: '',
        address: '',
        taxNumber: ''
      });
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingSupplier) {
        await api.put(`/suppliers/${editingSupplier.id}`, formData);
      } else {
        await api.post('/suppliers', formData);
      }
      setIsModalOpen(false);
      fetchSuppliers();
    } catch {
      if (editingSupplier) {
        setSuppliers(suppliers.map(s => s.id === editingSupplier.id ? { ...s, ...formData } : s));
      } else {
        setSuppliers([...suppliers, { id: Date.now(), ...formData, currentBalance: 0 }]);
      }
      setIsModalOpen(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this supplier?')) return;
    try {
      await api.delete(`/suppliers/${id}`);
      fetchSuppliers();
    } catch {
      setSuppliers(suppliers.filter(s => s.id !== id));
    }
  };

  const filtered = suppliers.filter(s => {
    const q = search.toLowerCase();
    return s.name.toLowerCase().includes(q) || (s.company && s.company.toLowerCase().includes(q));
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiTruck /></span>
            Supplier Directory &amp; Accounts Payable
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            International and local spare parts suppliers &amp; importers
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <button type="button" className="btn btn-primary" onClick={() => handleOpenModal()}>
            <FiPlus />
            <span>Add Supplier</span>
          </button>
          <ExportToolbar
            title="Suppliers Payable Directory"
            columns={['Supplier Name', 'Company', 'Phone', 'Email', 'Payable Balance']}
            rows={filtered.map(s => [
              s.name,
              s.company || '-',
              s.phone,
              s.email || '-',
              s.currentBalance,
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
            placeholder="Search Supplier by Name or Company..."
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
            { header: 'Supplier Name', accessor: 'name', render: (r) => <strong>{r.name}</strong> },
            { header: 'Company / Business', accessor: 'company' },
            { header: 'Phone', accessor: 'phone' },
            { header: 'Email', accessor: 'email' },
            {
              header: 'Payable Due',
              accessor: 'currentBalance',
              render: (r) => (
                <strong style={{ color: 'var(--danger)' }}>
                  {currency} {r.currentBalance?.toLocaleString()}
                </strong>
              )
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
                    title="Edit Supplier"
                  >
                    <FiEdit2 />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDelete(r.id)}
                    title="Delete Supplier"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* CREATE / EDIT SUPPLIER MODAL */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingSupplier ? `Edit Supplier: ${editingSupplier.name}` : "Add New Supplier"}
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsModalOpen(false)}>Cancel</button>
            <button type="button" className="btn btn-primary" onClick={handleSubmit}>
              {editingSupplier ? 'Save Changes' : 'Save Supplier'}
            </button>
          </>
        }
      >
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Supplier Name <span className="required">*</span></label>
            <input
              type="text"
              className="form-control"
              required
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="e.g. Global Auto Parts Trading Dubai"
            />
          </div>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Company Name</label>
              <input
                type="text"
                className="form-control"
                value={formData.company}
                onChange={(e) => setFormData({ ...formData, company: e.target.value })}
                placeholder="Global Parts FZE"
              />
            </div>
            <div className="form-group">
              <label className="form-label">Phone <span className="required">*</span></label>
              <input
                type="text"
                className="form-control"
                required
                value={formData.phone}
                onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                placeholder="+971 4 1234567"
              />
            </div>
          </div>
          <div className="form-group">
            <label className="form-label">Email</label>
            <input
              type="email"
              className="form-control"
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              placeholder="orders@supplier.com"
            />
          </div>
          <div className="form-group">
            <label className="form-label">Address</label>
            <textarea
              className="form-control"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              placeholder="Full address..."
            />
          </div>
        </form>
      </Modal>
    </div>
  );
}
