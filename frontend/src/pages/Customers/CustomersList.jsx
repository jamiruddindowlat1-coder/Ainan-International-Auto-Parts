import React, { useState, useEffect } from 'react';
import { FiUsers, FiSearch, FiPlus, FiEdit2, FiTrash2 } from 'react-icons/fi';
import api from '../../services/api';
import DataTable from '../../components/Table/DataTable';
import Modal from '../../components/Modal/Modal';
import ExportToolbar from '../../components/ExportToolbar/ExportToolbar';
import branding from '../../config/branding';

export default function CustomersList() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingCustomer, setEditingCustomer] = useState(null);

  const [formData, setFormData] = useState({
    name: '',
    customerType: 'Retail',
    phone: '',
    email: '',
    address: '',
    creditLimit: 0
  });

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      const res = await api.get('/customers');
      if (res.data?.data) setCustomers(res.data.data);
    } catch {
      setCustomers([
        { id: 1, name: 'Apex Auto Garage', customerType: 'Garage', phone: '+880 1812-345678', email: 'apex@autogarage.com', address: 'Mirpur-10, Dhaka', creditLimit: 150000, currentBalance: 24500 },
        { id: 2, name: 'Rahim Motors Wholesale', customerType: 'Wholesale', phone: '+880 1913-987654', email: 'rahim@motorsbd.com', address: 'Dholaikhal, Old Dhaka', creditLimit: 500000, currentBalance: 85000 },
        { id: 3, name: 'Kabir Express Fleet', customerType: 'Corporate', phone: '+880 1715-112233', email: 'fleet@kabirexpress.com', address: 'Mohakhali, Dhaka', creditLimit: 300000, currentBalance: 0 },
        { id: 4, name: 'Walk-in Customer (Counter)', customerType: 'Retail', phone: '+880 1700-000000', email: 'retail@aiaps.com', address: 'Store Counter', creditLimit: 0, currentBalance: 0 }
      ]);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (customer = null) => {
    if (customer) {
      setEditingCustomer(customer);
      setFormData({
        name: customer.name,
        customerType: customer.customerType || 'Retail',
        phone: customer.phone,
        email: customer.email || '',
        address: customer.address || '',
        creditLimit: customer.creditLimit || 0
      });
    } else {
      setEditingCustomer(null);
      setFormData({
        name: '',
        customerType: 'Retail',
        phone: '',
        email: '',
        address: '',
        creditLimit: 0
      });
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = { ...formData, creditLimit: Number(formData.creditLimit || 0) };
      if (editingCustomer) {
        await api.put(`/customers/${editingCustomer.id}`, payload);
      } else {
        await api.post('/customers', payload);
      }
      setIsModalOpen(false);
      fetchCustomers();
    } catch {
      if (editingCustomer) {
        setCustomers(customers.map(c => c.id === editingCustomer.id ? { ...c, ...formData } : c));
      } else {
        setCustomers([...customers, { id: Date.now(), ...formData, currentBalance: 0 }]);
      }
      setIsModalOpen(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this customer?')) return;
    try {
      await api.delete(`/customers/${id}`);
      fetchCustomers();
    } catch {
      setCustomers(customers.filter(c => c.id !== id));
    }
  };

  const filtered = customers.filter(c => {
    const q = search.toLowerCase();
    return c.name.toLowerCase().includes(q) || c.phone.includes(q);
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div>
      <div className="page-header">
        <div>
          <h1 className="page-title">
            <span className="page-title-icon"><FiUsers /></span>
            Customer Directory &amp; Ledger
          </h1>
          <p className="text-muted" style={{ marginTop: '4px' }}>
            Manage garages, wholesalers, fleets and retail customer balances
          </p>
        </div>

        <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap' }}>
          <button type="button" className="btn btn-primary" onClick={() => handleOpenModal()}>
            <FiPlus />
            <span>Add Customer</span>
          </button>
          <ExportToolbar
            title="Customers Balance Ledger"
            columns={['Customer Name', 'Type', 'Phone', 'Address', 'Credit Limit', 'Current Due Balance']}
            rows={filtered.map(c => [
              c.name,
              c.customerType,
              c.phone,
              c.address || '-',
              c.creditLimit,
              c.currentBalance,
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
            placeholder="Search Customer by Name or Phone..."
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
            { header: 'Customer Name', accessor: 'name', render: (r) => <strong>{r.name}</strong> },
            { header: 'Type', accessor: 'customerType', render: (r) => <span className="badge badge-primary">{r.customerType}</span> },
            { header: 'Phone', accessor: 'phone' },
            { header: 'Address', accessor: 'address' },
            { header: 'Credit Limit', accessor: 'creditLimit', render: (r) => `${currency} ${r.creditLimit?.toLocaleString()}` },
            {
              header: 'Due Balance',
              accessor: 'currentBalance',
              render: (r) => (
                <strong style={{ color: r.currentBalance > 0 ? 'var(--danger)' : 'var(--success)' }}>
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
                    title="Edit Customer"
                  >
                    <FiEdit2 />
                  </button>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm text-danger"
                    onClick={() => handleDelete(r.id)}
                    title="Delete Customer"
                  >
                    <FiTrash2 />
                  </button>
                </div>
              )
            }
          ]}
        />
      </div>

      {/* CREATE / EDIT CUSTOMER MODAL */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={editingCustomer ? `Edit Customer: ${editingCustomer.name}` : "Add New Customer"}
        footer={
          <>
            <button type="button" className="btn btn-ghost" onClick={() => setIsModalOpen(false)}>Cancel</button>
            <button type="button" className="btn btn-primary" onClick={handleSubmit}>
              {editingCustomer ? 'Save Changes' : 'Save Customer'}
            </button>
          </>
        }
      >
        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '14px' }}>
          <div className="form-group">
            <label className="form-label">Customer Name <span className="required">*</span></label>
            <input
              type="text"
              className="form-control"
              required
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              placeholder="e.g. Apex Auto Garage"
            />
          </div>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Customer Type</label>
              <select
                className="form-control"
                value={formData.customerType}
                onChange={(e) => setFormData({ ...formData, customerType: e.target.value })}
              >
                <option value="Retail">Retail</option>
                <option value="Garage">Garage / Workshop</option>
                <option value="Wholesale">Wholesale</option>
                <option value="Corporate">Corporate / Fleet</option>
              </select>
            </div>
            <div className="form-group">
              <label className="form-label">Phone <span className="required">*</span></label>
              <input
                type="text"
                className="form-control"
                required
                value={formData.phone}
                onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
                placeholder="+880 1812-345678"
              />
            </div>
          </div>
          <div className="form-grid-2">
            <div className="form-group">
              <label className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                placeholder="contact@customer.com"
              />
            </div>
            <div className="form-group">
              <label className="form-label">Credit Limit (BDT)</label>
              <input
                type="number"
                className="form-control"
                value={formData.creditLimit}
                onChange={(e) => setFormData({ ...formData, creditLimit: e.target.value })}
                placeholder="100000"
              />
            </div>
          </div>
          <div className="form-group">
            <label className="form-label">Address</label>
            <textarea
              className="form-control"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              placeholder="Full shop or garage address..."
            />
          </div>
        </form>
      </Modal>
    </div>
  );
}
