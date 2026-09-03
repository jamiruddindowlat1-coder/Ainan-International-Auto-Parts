import React, { useState } from 'react';
import { Outlet } from 'react-router-dom';
import Sidebar from '../components/Sidebar/Sidebar';
import Navbar from '../components/Navbar/Navbar';

export default function MainLayout() {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <div className="app-shell">
      <Sidebar collapsed={collapsed} setCollapsed={setCollapsed} />
      <div className={`main-content ${collapsed ? 'collapsed' : ''}`}>
        <Navbar collapsed={collapsed} />
        <main className="page-wrapper">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
