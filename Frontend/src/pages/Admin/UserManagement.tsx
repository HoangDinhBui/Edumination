import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { 
  Users, FileText, BookOpen, Clock, TrendingUp, Activity
} from 'lucide-react';
import { 
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, 
  PieChart, Pie, Cell, Legend 
} from 'recharts';

// --- MOCK DATA (Giữ nguyên cho biểu đồ) ---
const monthlyData = [
  { name: 'Jan', users: 400, tests: 240 },
  { name: 'Feb', users: 300, tests: 139 },
  { name: 'Mar', users: 200, tests: 980 },
  { name: 'Apr', users: 278, tests: 390 },
  { name: 'May', users: 189, tests: 480 },
];
const skillsData = [
  { name: 'Reading', value: 35 }, { name: 'Listening', value: 25 },
  { name: 'Writing', value: 20 }, { name: 'Speaking', value: 20 },
];
const MOCK_ACTIVITIES = [
  { id: 1, user: "system@edumination.com", action: "System Check", target: "Maintenance", time: "Just now" },
  { id: 2, user: "guest_user", action: "Viewed Course", target: "IELTS Basic", time: "5 mins ago" },
];
const PIE_COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444'];

// --- STAT CARD COMPONENT ---
const StatCard = ({ title, value, icon: Icon, color, loading }) => (
  <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 flex items-center justify-between">
    <div>
      <p className="text-slate-500 text-sm font-medium mb-1">{title}</p>
      {loading ? (
        <div className="h-8 w-16 bg-slate-100 animate-pulse rounded"></div>
      ) : (
        <h3 className="text-2xl font-bold text-slate-800">{value}</h3>
      )}
    </div>
    <div className={`p-4 rounded-full ${color} bg-opacity-10`}>
      <Icon className={`w-6 h-6 ${color.replace('bg-', 'text-')}`} />
    </div>
  </div>
);

// --- COMPONENT CHÍNH ---
export default function DashboardOverview() {
    const [stats, setStats] = useState({ users: 0, papers: 0, courses: 0 });
    const [popularTests, setPopularTests] = useState([]);
    const [loading, setLoading] = useState(true);

    // Hàm helper: Bắt dữ liệu thông minh (Copy logic từ UserManagement)
    const extractDataList = (data) => {
        if (!data) return [];
        if (Array.isArray(data)) return data;
        if (data.Items && Array.isArray(data.Items)) return data.Items; // .NET thường trả về cái này
        if (data.items && Array.isArray(data.items)) return data.items;
        if (data.data && Array.isArray(data.data)) return data.data;
        return [];
    };

    const extractTotalCount = (data) => {
        if (!data) return 0;
        // Ưu tiên lấy số Total từ phân trang
        if (typeof data.Total === 'number') return data.Total;
        if (typeof data.total === 'number') return data.total;
        if (typeof data.totalCount === 'number') return data.totalCount;
        
        // Nếu không có field Total, đếm thủ công mảng
        const list = extractDataList(data);
        return list.length;
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                // 1. Dùng đúng Key "Token" như trang UserManagement
                const token = localStorage.getItem("Token");
                const config = { headers: { Authorization: `Bearer ${token}` } };
                
                // 2. Dùng URL tuyệt đối (http://localhost:8081) để tránh lỗi HTML
                const API_BASE = "http://localhost:8081/api/v1";

                const [usersRes, papersRes, coursesRes] = await Promise.allSettled([
                    axios.get(`${API_BASE}/admin/users?page=1&pageSize=1`, config), // Chỉ cần lấy 1 trang để xem Total
                    axios.get(`${API_BASE}/papers?status=PUBLISHED`, config),
                    axios.get(`${API_BASE}/courses`, config)
                ]);

                // 3. Xử lý Users
                let userCount = 0;
                if (usersRes.status === 'fulfilled') {
                    userCount = extractTotalCount(usersRes.value.data);
                }

                // 4. Xử lý Papers
                let paperCount = 0;
                let paperItems = [];
                if (papersRes.status === 'fulfilled') {
                    // API Papers của bạn trả về { Items: [...] } hoặc mảng
                    paperCount = extractTotalCount(papersRes.value.data);
                    paperItems = extractDataList(papersRes.value.data);
                }

                // 5. Xử lý Courses
                let courseCount = 0;
                if (coursesRes.status === 'fulfilled') {
                    courseCount = extractTotalCount(coursesRes.value.data);
                }

                setStats({
                    users: userCount,
                    papers: paperCount,
                    courses: courseCount
                });

                // Map dữ liệu cho bảng "Latest Tests"
                const mappedPapers = paperItems.slice(0, 3).map(p => ({
                    id: p.id || p.Id,
                    title: p.title || p.Title || "No Title",
                    attempts: Math.floor(Math.random() * 500) + 50, // Fake số lượt thi
                    avgScore: (Math.random() * 4 + 5).toFixed(1)
                }));
                setPopularTests(mappedPapers);

            } catch (error) {
                console.error("Dashboard error:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

  return (
    <div className="space-y-6 pb-10">
      <div className="flex justify-between items-center">
        <div>
            <h1 className="text-2xl font-bold text-slate-800">Dashboard Overview</h1>
            <p className="text-slate-500">System metrics.</p>
        </div>
      </div>

      {/* STATS CARDS */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <StatCard title="Total Students" value={stats.users.toLocaleString()} icon={Users} color="bg-blue-500 text-blue-600" loading={loading} />
        <StatCard title="Published Tests" value={stats.papers} icon={FileText} color="bg-green-500 text-green-600" loading={loading} />
        <StatCard title="Active Courses" value={stats.courses} icon={BookOpen} color="bg-purple-500 text-purple-600" loading={loading} />
      </div>

      {/* CHARTS (MOCK) */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 lg:col-span-2">
          <h3 className="font-bold text-slate-800 mb-4 flex items-center gap-2">
            <TrendingUp size={18} className="text-slate-400"/> Growth (Simulated)
          </h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={monthlyData}>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <YAxis axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <Tooltip />
                <Bar dataKey="users" fill="#3b82f6" radius={[4, 4, 0, 0]} barSize={30} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 flex flex-col">
           <h3 className="font-bold text-slate-800 mb-4">Latest Tests</h3>
           {loading ? <p className="text-center text-slate-400">Loading...</p> : (
               <div className="space-y-4">
                   {popularTests.map((paper) => (
                       <div key={paper.id} className="flex items-center gap-4 bg-slate-50 p-4 rounded-lg border border-slate-100">
                           <FileText size={24} className="text-blue-400 flex-shrink-0"/>
                           <div className="flex-1 min-w-0">
                               <p className="text-sm font-medium text-slate-800 truncate">{paper.title}</p>
                               <p className="text-xs text-slate-500">ID: {paper.id}</p>
                           </div>
                       </div>
                   ))}
               </div>
           )}
        </div>
      </div>
    </div>
  );
}