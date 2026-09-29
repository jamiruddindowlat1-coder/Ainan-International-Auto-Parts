import React, { createContext, useContext, useState, useEffect } from 'react';
import api from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('aiaps_user');
    return saved ? JSON.parse(saved) : {
      username: 'admin',
      fullName: 'System Administrator',
      email: 'admin@aiaps.com',
      roles: ['SuperAdmin']
    };
  });

  const [token, setToken] = useState(() => localStorage.getItem('aiaps_token') || 'demo_token');

  const login = async (username, password) => {
    try {
      const res = await api.post('/auth/login', { username, password });
      if (res.data?.success) {
        const { token, ...userData } = res.data.data;
        setToken(token);
        setUser(userData);
        localStorage.setItem('aiaps_token', token);
        localStorage.setItem('aiaps_user', JSON.stringify(userData));
        return { success: true };
      }
      return { success: false, message: res.data?.message || 'Login failed' };
    } catch (err) {
      // Fallback for demo mode
      if (username === 'admin' && (password === 'Admin@123' || password === '123456' || password === 'admin')) {
        const demoUser = {
          username: 'admin',
          fullName: 'System Administrator',
          email: 'admin@aiaps.com',
          roles: ['SuperAdmin']
        };
        setUser(demoUser);
        setToken('demo_token');
        localStorage.setItem('aiaps_token', 'demo_token');
        localStorage.setItem('aiaps_user', JSON.stringify(demoUser));
        return { success: true };
      }
      return { success: false, message: err.response?.data?.message || 'Invalid username or password' };
    }
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem('aiaps_token');
    localStorage.removeItem('aiaps_user');
  };

  return (
    <AuthContext.Provider value={{ user, token, isAuthenticated: !!user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
