import React from 'react';
import { Outlet, NavLink, useNavigate } from 'react-router-dom';
import { LayoutDashboard, Users, FileText, BookOpen, LogOut, Bell } from 'lucide-react';
import edmLogo from "../assets/img/edm-logo.png"; // Hãy đảm bảo đường dẫn ảnh đúng

export default function AdminLayout() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.clear();
    navigate('/signin');
  };

  const navItems = [
    { path: '/admin', label: 'Dashboard', icon: LayoutDashboard, end: true },
    { path: '/admin/users', label: 'Users', icon: Users, end: false },
    { path: '/admin/tests', label: 'Test Papers', icon: FileText, end: false },
    { path: '/admin/courses', label: 'Courses', icon: BookOpen, end: false },
  ];

  return (
    // 1. h-screen overflow-hidden: Khóa chiều cao cửa sổ trình duyệt
    <div className="flex h-screen overflow-hidden bg-slate-50 font-['Be_Vietnam_Pro']">
      
      {/* --- SIDEBAR (Cố định bên trái) --- */}
      <aside className="w-64 bg-white border-r border-slate-200 flex flex-col flex-shrink-0 z-20">
        <div className="h-16 flex items-center px-6 border-b border-slate-100">
           <img src={edmLogo} alt="Logo" className="h-8" />
           <span className="ml-3 font-bold text-slate-800 text-lg">Admin</span>
        </div>

        <nav className="flex-1 p-4 space-y-1 overflow-y-auto">
          {navItems.map((item) => (
            <NavLink
              key={item.path}
              to={item.path}
              end={item.end}
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-blue-50 text-blue-700 shadow-sm'
                    : 'text-slate-600 hover:bg-slate-50 hover:text-slate-900'
                }`
              }
            >
              <item.icon className="w-5 h-5" />
              {item.label}
            </NavLink>
          ))}
        </nav>

        <div className="p-4 border-t border-slate-100 mt-auto">
          <button onClick={handleLogout} className="flex items-center gap-3 px-4 py-3 w-full text-slate-600 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors">
            <LogOut className="w-5 h-5" />
            <span className="text-sm font-medium">Sign Out</span>
          </button>
        </div>
      </aside>

      {/* --- MAIN CONTENT WRAPPER --- */}
      <div className="flex-1 flex flex-col min-w-0">
        
        {/* Header (Luôn dính trên cùng) */}
        <header className="h-16 bg-white border-b border-slate-200 flex items-center justify-between px-8 flex-shrink-0 z-10">
           <h2 className="text-lg font-semibold text-slate-800">Management Portal</h2>
           <div className="flex items-center gap-4">
              <button className="p-2 text-slate-500 hover:bg-slate-100 rounded-full relative">
                 <Bell className="w-5 h-5" />
                 <span className="absolute top-2 right-2 w-2 h-2 bg-red-500 rounded-full"></span>
              </button>
              <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center text-blue-700 font-bold text-sm">
                AD
              </div>
           </div>
        </header>

        {/* Khu vực cuộn nội dung (Scrollable Area)
            - overflow-y-scroll: BẮT BUỘC hiện thanh cuộn (để tránh nhảy layout khi chuyển trang dài/ngắn)
        */}
        <main className="flex-1 overflow-y-scroll bg-slate-50 p-8">
          
          {/* Container giới hạn kích thước
              - max-w-7xl: Giới hạn chiều rộng tối đa (khoảng 1280px)
              - mx-auto: Căn giữa màn hình
              - min-h-full: Đảm bảo background luôn full chiều cao
           */}
          <div className="max-w-7xl mx-auto min-h-full">
             <Outlet />
          </div>
          
        </main>
      </div>
    </div>
  );
}