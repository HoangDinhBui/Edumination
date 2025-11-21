import React, { useEffect, useState } from 'react';
import { Users, FileText, BookOpen, TrendingUp } from 'lucide-react';

const StatCard = ({ title, value, icon: Icon, color }: any) => (
  <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 flex items-center">
    <div className={`p-4 rounded-full ${color} bg-opacity-10 mr-4`}>
      <Icon className={`w-8 h-8 ${color.replace('bg-', 'text-')}`} />
    </div>
    <div>
      <p className="text-slate-500 text-sm font-medium">{title}</p>
      <h3 className="text-2xl font-bold text-slate-800">{value}</h3>
    </div>
  </div>
);

export default function AdminDashboard() {
  const [stats, setStats] = useState({ users: 0, tests: 0, courses: 0 });

  useEffect(() => {
    const fetchData = async () => {
       const token = localStorage.getItem("Token");
       const headers = { Authorization: `Bearer ${token}` };
       
       try {
         // Gọi song song 3 API để lấy số liệu
         const [usersRes, papersRes, coursesRes] = await Promise.all([
            fetch('http://localhost:8081/api/v1/admin/users', { headers }), // API này bạn cần check lại xem có phân trang không
            fetch('http://localhost:8081/api/v1/papers'),
            fetch('http://localhost:8081/api/v1/courses')
         ]);

         const usersData = await usersRes.json(); // Nếu trả về { items: [], total: ... } thì lấy total
         const papersData = await papersRes.json();
         const coursesData = await coursesRes.json();

         setStats({
            users: Array.isArray(usersData) ? usersData.length : (usersData.TotalCount || 0),
            tests: papersData.Items ? papersData.Items.length : 0, // Dựa vào DTO PaperLibraryResponseDto
            courses: Array.isArray(coursesData) ? coursesData.length : 0
         });
       } catch (e) { console.error(e); }
    };
    fetchData();
  }, []);

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-800 mb-6">Dashboard Overview</h1>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <StatCard title="Total Users" value={stats.users} icon={Users} color="bg-blue-500 text-blue-600" />
        <StatCard title="Total Tests" value={stats.tests} icon={FileText} color="bg-green-500 text-green-600" />
        <StatCard title="Total Courses" value={stats.courses} icon={BookOpen} color="bg-purple-500 text-purple-600" />
      </div>
      
      {/* Chart Placeholder (Nếu muốn làm đẹp thêm thì dùng Recharts) */}
      <div className="mt-8 bg-white p-6 rounded-xl shadow-sm border border-slate-100 h-96 flex items-center justify-center text-slate-400">
         <p>Biểu đồ thống kê (Chart Integration Area)</p>
      </div>
    </div>
  );
}