import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import MainLayout from './layouts/MainLayout';
import Login from './pages/Auth/Login';
import Dashboard from './pages/Dashboard/Dashboard';
import POS from './pages/Sales/POS';
import PartsList from './pages/Parts/PartsList';
import SalesList from './pages/Sales/SalesList';
import PurchaseList from './pages/Purchase/PurchaseList';
import CustomersList from './pages/Customers/CustomersList';
import SuppliersList from './pages/Suppliers/SuppliersList';
import InventoryList from './pages/Inventory/InventoryList';
import AccountingView from './pages/Accounting/AccountingView';
import ReportsPage from './pages/Reports/ReportsPage';
import SettingsPage from './pages/Settings/SettingsPage';

function ProtectedRoute({ children }) {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  return children;
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />

          <Route
            path="/"
            element={
              <ProtectedRoute>
                <MainLayout />
              </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="dashboard" element={<Dashboard />} />
            <Route path="pos" element={<POS />} />
            <Route path="parts" element={<PartsList />} />
            <Route path="sales" element={<SalesList />} />
            <Route path="purchase" element={<PurchaseList />} />
            <Route path="customers" element={<CustomersList />} />
            <Route path="suppliers" element={<SuppliersList />} />
            <Route path="inventory" element={<InventoryList />} />
            <Route path="accounting" element={<AccountingView />} />
            <Route path="reports" element={<ReportsPage />} />
            <Route path="settings" element={<SettingsPage />} />
          </Route>

          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
