import React, { useState, useEffect } from 'react';
import {
  FiSearch,
  FiTrash2,
  FiPlus,
  FiMinus,
  FiPrinter,
  FiCheck,
  FiUser,
  FiShoppingCart
} from 'react-icons/fi';
import api from '../../services/api';
import { printDocument } from '../../utils/printUtils';
import branding from '../../config/branding';

export default function POS() {
  const [parts, setParts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCat, setSelectedCat] = useState(null);
  const [search, setSearch] = useState('');
  const [cart, setCart] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [selectedCustomer, setSelectedCustomer] = useState(1);
  const [paymentMethod, setPaymentMethod] = useState('Cash');
  const [discountPercent, setDiscountPercent] = useState(0);
  const [paidAmount, setPaidAmount] = useState('');
  const [isProcessing, setIsProcessing] = useState(false);

  useEffect(() => {
    fetchInitialData();
  }, []);

  const fetchInitialData = async () => {
    try {
      const [partsRes, catRes, custRes] = await Promise.all([
        api.get('/parts?pageSize=100'),
        api.get('/parts/categories'),
        api.get('/customers')
      ]);

      if (partsRes.data?.data) setParts(partsRes.data.data);
      if (catRes.data?.data) setCategories(catRes.data.data);
      if (custRes.data?.data) setCustomers(custRes.data.data);
    } catch {
      // Fallback demo data
      setParts([
        { id: 1, partNumber: 'BOS-BP-001', name: 'Front Ceramic Brake Pad Set', brandName: 'Bosch', sellingPrice: 3200, totalStock: 45, barcode: '890123450001' },
        { id: 2, partNumber: 'DEN-SP-IX01', name: 'Iridium Spark Plug (SK20R11)', brandName: 'Denso', sellingPrice: 950, totalStock: 120, barcode: '890123450002' },
        { id: 3, partNumber: 'MNN-OF-W68', name: 'Mann Engine Oil Filter', brandName: 'Mann-Filter', sellingPrice: 650, totalStock: 95, barcode: '890123450004' },
        { id: 4, partNumber: 'KYB-SA-33331', name: 'KYB Front Strut Assembly', brandName: 'KYB', sellingPrice: 9200, totalStock: 12, barcode: '890123450005' },
        { id: 5, partNumber: 'MOB-OIL-5W30', name: 'Mobil 1 Full Synthetic 5W-30 (4L)', brandName: 'Mobil 1', sellingPrice: 5200, totalStock: 38, barcode: '890123450006' },
      ]);
      setCategories([
        { id: 1, name: 'Brake System' },
        { id: 2, name: 'Electrical' },
        { id: 3, name: 'Filtration' },
        { id: 4, name: 'Suspension' },
        { id: 5, name: 'Lubricants' },
      ]);
      setCustomers([
        { id: 1, name: 'Walk-in Customer (Counter)', phone: '+880 1700-000000' },
        { id: 2, name: 'Apex Auto Garage', phone: '+880 1812-345678' },
        { id: 3, name: 'Rahim Motors Wholesale', phone: '+880 1913-987654' },
      ]);
    }
  };

  const addToCart = (part) => {
    const existing = cart.find(item => item.id === part.id);
    if (existing) {
      setCart(cart.map(item =>
        item.id === part.id ? { ...item, quantity: item.quantity + 1 } : item
      ));
    } else {
      setCart([...cart, { ...part, quantity: 1, unitPrice: part.sellingPrice }]);
    }
  };

  const updateQuantity = (id, delta) => {
    setCart(cart.map(item => {
      if (item.id === id) {
        const newQty = item.quantity + delta;
        return newQty > 0 ? { ...item, quantity: newQty } : null;
      }
      return item;
    }).filter(Boolean));
  };

  const removeFromCart = (id) => {
    setCart(cart.filter(item => item.id !== id));
  };

  const subTotal = cart.reduce((sum, item) => sum + (item.quantity * item.unitPrice), 0);
  const discountAmount = (subTotal * Number(discountPercent || 0)) / 100;
  const grandTotal = subTotal - discountAmount;
  const finalPaid = paidAmount === '' ? grandTotal : Number(paidAmount);
  const changeDue = finalPaid - grandTotal;

  const handleCheckout = async () => {
    if (cart.length === 0) return;

    setIsProcessing(true);
    const invoiceNo = 'INV-' + new Date().toISOString().slice(0,10).replace(/-/g,'') + '-' + Math.floor(1000 + Math.random()*9000);
    const customerObj = customers.find(c => c.id === Number(selectedCustomer)) || { name: 'Walk-in Customer', phone: '-' };

    try {
      await api.post('/sales', {
        customerId: Number(selectedCustomer),
        warehouseId: 1,
        saleType: 'POS',
        discountAmount,
        paidAmount: finalPaid,
        paymentMethod,
        items: cart.map(item => ({
          partId: item.id,
          quantity: item.quantity,
          unitPrice: item.unitPrice,
          discountPercent: 0
        }))
      });
    } catch {
      // Demo continue
    }

    // Auto print branded receipt
    printDocument({
      title: 'POS SALES INVOICE',
      columns: ['#', 'Part Description', 'Qty', 'Unit Price (BDT)', 'Total (BDT)'],
      rows: cart.map((item, idx) => [
        idx + 1,
        `${item.partNumber} - ${item.name}`,
        item.quantity,
        item.unitPrice.toLocaleString(),
        (item.quantity * item.unitPrice).toLocaleString()
      ]),
      extraInfo: {
        'Invoice No': invoiceNo,
        'Customer': `${customerObj.name} (${customerObj.phone || ''})`,
        'Payment Method': paymentMethod,
        'Subtotal': `${branding.defaultCurrency.symbol} ${subTotal.toLocaleString()}`,
        'Discount': `${discountPercent}% (${branding.defaultCurrency.symbol} ${discountAmount.toLocaleString()})`,
        'Grand Total': `${branding.defaultCurrency.symbol} ${grandTotal.toLocaleString()}`,
        'Amount Paid': `${branding.defaultCurrency.symbol} ${finalPaid.toLocaleString()}`,
        'Change / Balance': `${branding.defaultCurrency.symbol} ${changeDue >= 0 ? changeDue.toLocaleString() : 'Due: ' + Math.abs(changeDue).toLocaleString()}`
      }
    });

    setCart([]);
    setPaidAmount('');
    setDiscountPercent(0);
    setIsProcessing(false);
  };

  const filteredParts = parts.filter(p => {
    const matchesSearch = p.name.toLowerCase().includes(search.toLowerCase()) ||
                          p.partNumber.toLowerCase().includes(search.toLowerCase()) ||
                          (p.barcode && p.barcode.includes(search));
    const matchesCat = selectedCat ? p.categoryId === selectedCat : true;
    return matchesSearch && matchesCat;
  });

  const currency = branding.defaultCurrency.symbol;

  return (
    <div style={{ display: 'grid', gridTemplateColumns: '1.8fr 1.2fr', gap: '20px' }}>
      {/* LEFT: PRODUCTS CATALOG & SEARCH */}
      <div>
        <div className="card mb-16">
          <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
            <div className="search-wrapper" style={{ flex: 1 }}>
              <FiSearch className="search-icon" />
              <input
                type="text"
                className="search-input"
                placeholder="Scan barcode or type Part # / Name..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                autoFocus
              />
            </div>

            <button
              type="button"
              className={`btn btn-sm ${selectedCat === null ? 'btn-primary' : 'btn-ghost'}`}
              onClick={() => setSelectedCat(null)}
            >
              All Items
            </button>
          </div>

          {/* CATEGORIES CHIPS */}
          <div style={{ display: 'flex', gap: '8px', overflowX: 'auto', marginTop: '12px', paddingBottom: '4px' }}>
            {categories.map((cat) => (
              <button
                key={cat.id}
                type="button"
                className={`btn btn-sm ${selectedCat === cat.id ? 'btn-primary' : 'btn-ghost'}`}
                onClick={() => setSelectedCat(cat.id)}
                style={{ fontSize: '0.78rem' }}
              >
                {cat.name}
              </button>
            ))}
          </div>
        </div>

        {/* PRODUCTS GRID */}
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '14px' }}>
          {filteredParts.map((part) => (
            <div
              key={part.id}
              onClick={() => addToCart(part)}
              className="card card-sm"
              style={{
                cursor: 'pointer',
                transition: 'all 0.2s ease',
                background: 'var(--bg-input)',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'space-between',
              }}
              onMouseEnter={(e) => e.currentTarget.style.borderColor = 'var(--primary-light)'}
              onMouseLeave={(e) => e.currentTarget.style.borderColor = 'var(--border)'}
            >
              <div>
                <span className="badge badge-accent" style={{ fontSize: '0.68rem', marginBottom: '6px' }}>
                  {part.brandName || 'Brand'}
                </span>
                <div style={{ fontWeight: 700, fontSize: '0.9rem', color: '#fff' }}>{part.partNumber}</div>
                <div style={{ fontSize: '0.78rem', color: 'var(--text-muted)', marginTop: '2px', lineClamp: 2 }}>
                  {part.name}
                </div>
              </div>

              <div className="flex-between" style={{ marginTop: '14px' }}>
                <span style={{ fontSize: '1rem', fontWeight: 800, color: 'var(--accent-light)' }}>
                  {currency} {part.sellingPrice?.toLocaleString()}
                </span>
                <span className="badge badge-primary" style={{ fontSize: '0.7rem' }}>
                  Stock: {part.totalStock || 0}
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* RIGHT: CART & POS CHECKOUT */}
      <div className="card" style={{ display: 'flex', flexDirection: 'column', height: 'fit-content' }}>
        <div className="flex-between mb-16" style={{ borderBottom: '1px solid var(--border)', paddingBottom: '12px' }}>
          <h3 className="flex" style={{ gap: '8px', alignItems: 'center' }}>
            <FiShoppingCart style={{ color: 'var(--accent)' }} />
            Active POS Cart ({cart.reduce((s, i) => s + i.quantity, 0)} items)
          </h3>
          {cart.length > 0 && (
            <button type="button" className="btn btn-ghost btn-sm text-danger" onClick={() => setCart([])}>
              Clear Cart
            </button>
          )}
        </div>

        {/* CUSTOMER SELECT */}
        <div className="form-group mb-16">
          <label className="form-label flex" style={{ gap: '6px', alignItems: 'center' }}>
            <FiUser /> Customer
          </label>
          <select
            className="form-control"
            value={selectedCustomer}
            onChange={(e) => setSelectedCustomer(e.target.value)}
          >
            {customers.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name} {c.phone ? `(${c.phone})` : ''}
              </option>
            ))}
          </select>
        </div>

        {/* CART ITEMS LIST */}
        <div style={{ maxHeight: '280px', overflowY: 'auto', display: 'flex', flexDirection: 'column', gap: '8px', marginBottom: '16px' }}>
          {cart.length === 0 ? (
            <div style={{ textAlign: 'center', padding: '40px 0', color: 'var(--text-muted)' }}>
              <div style={{ fontSize: '2rem', marginBottom: '8px' }}>🛒</div>
              <p>Cart is empty. Click items on the left to add.</p>
            </div>
          ) : (
            cart.map((item) => (
              <div
                key={item.id}
                style={{
                  background: 'var(--bg-input)',
                  padding: '10px 12px',
                  borderRadius: 'var(--radius-md)',
                  border: '1px solid var(--border)',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'space-between',
                  gap: '10px',
                }}
              >
                <div style={{ flex: 1, minWidth: 0 }}>
                  <div style={{ fontWeight: 600, fontSize: '0.85rem', color: '#fff' }} className="truncate">
                    {item.partNumber}
                  </div>
                  <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                    {currency} {item.unitPrice?.toLocaleString()} each
                  </div>
                </div>

                {/* QTY CONTROLS */}
                <div style={{ display: 'flex', alignItems: 'center', gap: '6px' }}>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    style={{ padding: '4px 8px' }}
                    onClick={() => updateQuantity(item.id, -1)}
                  >
                    <FiMinus />
                  </button>
                  <span style={{ fontWeight: 700, minWidth: '24px', textAlign: 'center' }}>{item.quantity}</span>
                  <button
                    type="button"
                    className="btn btn-ghost btn-sm"
                    style={{ padding: '4px 8px' }}
                    onClick={() => updateQuantity(item.id, 1)}
                  >
                    <FiPlus />
                  </button>
                </div>

                <div style={{ fontWeight: 700, fontSize: '0.9rem', color: 'var(--accent-light)', minWidth: '70px', textAlign: 'right' }}>
                  {currency} {(item.quantity * item.unitPrice).toLocaleString()}
                </div>

                <button
                  type="button"
                  onClick={() => removeFromCart(item.id)}
                  style={{ color: 'var(--danger)', padding: '4px' }}
                >
                  <FiTrash2 />
                </button>
              </div>
            ))
          )}
        </div>

        {/* BILL SUMMARY */}
        <div style={{ background: 'var(--bg-dark)', padding: '16px', borderRadius: 'var(--radius-md)', border: '1px solid var(--border)' }}>
          <div className="flex-between mb-8" style={{ fontSize: '0.85rem' }}>
            <span className="text-muted">Subtotal</span>
            <span>{currency} {subTotal.toLocaleString()}</span>
          </div>

          <div className="flex-between mb-8" style={{ fontSize: '0.85rem' }}>
            <span className="text-muted">Discount (%)</span>
            <input
              type="number"
              className="form-control"
              style={{ width: '80px', padding: '4px 8px', textAlign: 'right' }}
              value={discountPercent}
              onChange={(e) => setDiscountPercent(e.target.value)}
              min="0"
              max="100"
            />
          </div>

          <div className="flex-between mb-16" style={{ fontSize: '1.2rem', fontWeight: 800, color: 'var(--accent)', borderTop: '1px solid var(--border)', paddingTop: '10px' }}>
            <span>Grand Total</span>
            <span>{currency} {grandTotal.toLocaleString()}</span>
          </div>

          {/* PAYMENT METHOD & PAID AMOUNT */}
          <div className="form-grid-2 mb-16">
            <div className="form-group">
              <label className="form-label">Payment Method</label>
              <select
                className="form-control"
                value={paymentMethod}
                onChange={(e) => setPaymentMethod(e.target.value)}
              >
                <option value="Cash">Cash</option>
                <option value="Card">Credit/Debit Card</option>
                <option value="bKash / Nagad">Mobile Banking</option>
                <option value="Bank">Bank Transfer</option>
                <option value="Credit">Credit (Due)</option>
              </select>
            </div>

            <div className="form-group">
              <label className="form-label">Tendered (Paid)</label>
              <input
                type="number"
                className="form-control"
                placeholder={grandTotal.toString()}
                value={paidAmount}
                onChange={(e) => setPaidAmount(e.target.value)}
              />
            </div>
          </div>

          {/* CHECKOUT & PRINT BUTTON */}
          <button
            type="button"
            className="btn btn-accent btn-lg"
            style={{ width: '100%', justifyContent: 'center', gap: '10px' }}
            disabled={cart.length === 0 || isProcessing}
            onClick={handleCheckout}
          >
            <FiPrinter />
            <span>Complete &amp; Print Receipt ({currency} {grandTotal.toLocaleString()})</span>
          </button>
        </div>
      </div>
    </div>
  );
}
