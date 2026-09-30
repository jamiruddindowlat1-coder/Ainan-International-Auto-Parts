import React, { createContext, useContext, useState } from 'react';
import api from '../services/api';

const AuthContext = createContext(null);

// Remove any leftover demo session from older builds
if (localStorage.getItem('aiaps_token') === 'demo_token') {
  localStorage.removeItem('aiaps_token');
  localStorage.removeItem('aiaps_user');
}

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('aiaps_user');
    if (saved) {
      try {
        return JSON.parse(saved);
      } catch {
        localStorage.removeItem('aiaps_user');
      }
    }
    return null;
  });

  const [token, setToken] = useState(() => localStorage.getItem('aiaps_token') || null);

  const saveSession = (newToken, userData) => {
    setToken(newToken);
    setUser(userData);
    localStorage.setItem('aiaps_token', newToken);
    localStorage.setItem('aiaps_user', JSON.stringify(userData));
  };

  const login = async (username, password) => {
    try {
      const res = await api.post('/auth/login', { username, password });
      if (res.data?.success) {
        const { token: newToken, ...userData } = res.data.data;
        saveSession(newToken, userData);
        return { success: true };
      }
      return { success: false, message: res.data?.message || 'Login failed' };
    } catch (err) {
      return {
        success: false,
        message: err.response?.data?.message || 'Invalid username or password',
      };
    }
  };

  const loginWithGoogle = async (idToken) => {
    try {
      const res = await api.post('/auth/google', { idToken });
      if (res.data?.success) {
        const { token: newToken, ...userData } = res.data.data;
        saveSession(newToken, userData);
        return { success: true };
      }
      return { success: false, message: res.data?.message || 'Google login failed' };
    } catch (err) {
      console.error('Google login error:', err.response?.data || err);
      return {
        success: false,
        message: err.response?.data?.message || 'Google login failed',
      };
    }
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem('aiaps_token');
    localStorage.removeItem('aiaps_user');
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated: !!user,
        login,
        loginWithGoogle,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
