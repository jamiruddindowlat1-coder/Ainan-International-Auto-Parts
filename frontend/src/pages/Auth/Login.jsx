import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FiLock, FiUser, FiArrowRight } from 'react-icons/fi';
import { useAuth } from '../../context/AuthContext';
import branding from '../../config/branding';

export default function Login() {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('Admin@123');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    const res = await login(username, password);
    setLoading(false);
    if (res.success) {
      navigate('/dashboard');
    } else {
      setError(res.message || 'Invalid username or password');
    }
  };

  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'radial-gradient(circle at top right, #1B3A6B 0%, #0F1729 60%)',
        padding: '20px',
      }}
    >
      <div
        className="card"
        style={{
          width: '100%',
          maxWidth: '440px',
          padding: '36px',
          borderRadius: 'var(--radius-xl)',
          boxShadow: 'var(--shadow-lg)',
          border: '1px solid var(--border)',
        }}
      >
        {/* LOGO & TITLE */}
        <div style={{ textAlign: 'center', marginBottom: '28px' }}>
          <img
            src={branding.logo.icon}
            alt="AIAPS Logo"
            style={{
              width: '64px',
              height: '64px',
              margin: '0 auto 16px',
              borderRadius: '16px',
              objectFit: 'contain',
              boxShadow: '0 0 24px rgba(232,119,34,0.5)',
            }}
          />
          <h2 style={{ fontFamily: 'var(--font-heading)', color: '#fff', fontSize: '1.4rem' }}>
            {branding.company.name}
          </h2>
          <p className="text-muted" style={{ fontSize: '0.8rem', marginTop: '4px' }}>
            {branding.company.tagline}
          </p>
        </div>

        {error && (
          <div
            style={{
              background: 'rgba(239,68,68,0.15)',
              border: '1px solid var(--danger)',
              color: '#F87171',
              padding: '10px 14px',
              borderRadius: 'var(--radius-md)',
              fontSize: '0.85rem',
              marginBottom: '18px',
              textAlign: 'center',
            }}
          >
            {error}
          </div>
        )}

        <form onSubmit={handleLogin} style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          <div className="form-group">
            <label className="form-label">Username / Email</label>
            <div style={{ position: 'relative' }}>
              <FiUser
                style={{
                  position: 'absolute',
                  left: '12px',
                  top: '50%',
                  transform: 'translateY(-50%)',
                  color: 'var(--text-muted)',
                }}
              />
              <input
                type="text"
                className="form-control"
                style={{ paddingLeft: '38px' }}
                required
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="admin"
              />
            </div>
          </div>

          <div className="form-group">
            <label className="form-label">Password</label>
            <div style={{ position: 'relative' }}>
              <FiLock
                style={{
                  position: 'absolute',
                  left: '12px',
                  top: '50%',
                  transform: 'translateY(-50%)',
                  color: 'var(--text-muted)',
                }}
              />
              <input
                type="password"
                className="form-control"
                style={{ paddingLeft: '38px' }}
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
              />
            </div>
          </div>

          <button
            type="submit"
            className="btn btn-primary btn-lg"
            style={{ width: '100%', justifyContent: 'center', marginTop: '10px' }}
            disabled={loading}
          >
            <span>{loading ? 'Authenticating...' : 'Sign In to Dashboard'}</span>
            <FiArrowRight />
          </button>
        </form>

        <div style={{ textAlign: 'center', marginTop: '24px', fontSize: '0.78rem', color: 'var(--text-muted)' }}>
          Enterprise Edition &bull; AIAPS ERP v{branding.app.version}
        </div>
      </div>
    </div>
  );
}
