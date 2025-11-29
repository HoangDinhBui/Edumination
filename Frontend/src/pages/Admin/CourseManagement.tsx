import React, { useEffect, useState } from 'react';
import axios from 'axios'; 
import { 
  Users, FileText, BookOpen, Clock, TrendingUp, Activity
} from 'lucide-react';
import { 
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, 
  PieChart, Pie, Cell, Legend 
} from 'recharts';

// --- 1. MOCK DATA (GIỮ NGUYÊN VÌ CHƯA CÓ API REPORT) ---
const monthlyData = [
  { name: 'Jan', users: 400, tests: 240 },
  { name: 'Feb', users: 300, tests: 139 },
  { name: 'Mar', users: 200, tests: 980 },
  { name: 'Apr', users: 278, tests: 390 },
  { name: 'May', users: 189, tests: 480 },
];

const skillsData = [
  { name: 'Reading', value: 35 },
  { name: 'Listening', value: 25 },
  { name: 'Writing', value: 20 },
  { name: 'Speaking', value: 20 },
];

const MOCK_ACTIVITIES = [
  { id: 1, user: "system@edumination.com", action: "System Check", target: "Maintenance", time: "Just now" },
  { id: 2, user: "guest_user", action: "Viewed Course", target: "IELTS Basic", time: "5 mins ago" },
  { id: 3, user: "demo_student", action: "Failed Login", target: "Auth", time: "1 hour ago" },
];

const PIE_COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444'];

// --- CẤU HÌNH API ---
// Đổi port 8081 nếu Backend của bạn chạy port khác
const API_BASE_URL = "http://localhost:8081"; 

// --- 2. COMPONENT THẺ THỐNG KÊ ---
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

// --- 3. COMPONENT CHÍNH ---
export default function DashboardOverview() {
    // --- STATE ---
    const [showEmailModal, setShowEmailModal] = useState(false);
    const [stats, setStats] = useState({
        users: 0,
        papers: 0,
        courses: 0
    });
    
    const [userList, setUserList] = useState([]); 
    const [popularTests, setPopularTests] = useState([]); 
    const [loading, setLoading] = useState(true);

    // --- FETCH DATA TỪ API ---
    useEffect(() => {
        const fetchDashboardData = async () => {
            setLoading(true);
            try {
                // Lấy token (Lưu ý key là "Token" hay "accessToken" tuỳ code login của bạn)
                const token = localStorage.getItem('Token') || localStorage.getItem('accessToken');
                
                const config = {
                    headers: { Authorization: `Bearer ${token}` }
                };

                console.log("Starting fetch with URL:", API_BASE_URL);

                // Gọi song song 3 API
                const [usersRes, papersRes, coursesRes] = await Promise.allSettled([
                    axios.get(`${API_BASE_URL}/api/v1/admin/users?page=1&pageSize=100`, config),
                    axios.get(`${API_BASE_URL}/api/v1/papers?status=PUBLISHED`, config),
                    axios.get(`${API_BASE_URL}/api/v1/courses`, config)
                ]);

                // --- HÀM HELPER ĐỂ ĐỌC DỮ LIỆU AN TOÀN ---
                const safeGetCount = (res) => {
                    if (res.status !== 'fulfilled') return 0;
                    const d = res.value.data;
                    // Kiểm tra đủ các kiểu chữ hoa/thường/mảng
                    if (typeof d.totalCount === 'number') return d.totalCount;
                    if (typeof d.TotalCount === 'number') return d.TotalCount;
                    if (typeof d.Total === 'number') return d.Total; // PagedResult của bạn dùng chữ "Total"
                    if (typeof d.total === 'number') return d.total;
                    if (Array.isArray(d)) return d.length;
                    if (d.items && Array.isArray(d.items)) return d.items.length;
                    if (d.Items && Array.isArray(d.Items)) return d.Items.length;
                    return 0;
                };

                const safeGetItems = (res) => {
                    if (res.status !== 'fulfilled') return [];
                    const d = res.value.data;
                    if (Array.isArray(d)) return d;
                    if (d.items && Array.isArray(d.items)) return d.items;
                    if (d.Items && Array.isArray(d.Items)) return d.Items;
                    return [];
                };

                // --- CẬP NHẬT STATE ---
                setStats({
                    users: safeGetCount(usersRes),
                    papers: safeGetCount(papersRes),
                    courses: safeGetCount(coursesRes)
                });

                setUserList(safeGetItems(usersRes));

                // Map tên bài thi thật + Fake số liệu attempts
                const paperList = safeGetItems(papersRes);
                const mappedPapers = paperList.slice(0, 3).map(p => ({
                    id: p.id || p.Id,
                    title: p.title || p.Title || "Untitled Test",
                    attempts: Math.floor(Math.random() * 500) + 50, 
                    avgScore: (Math.random() * 4 + 5).toFixed(1)
                }));
                setPopularTests(mappedPapers);

            } catch (error) {
                console.error("Critical Error fetching dashboard:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchDashboardData();
    }, []);

  return (
    <div className="space-y-6 pb-10">
      {/* --- HEADER --- */}
      <div>
        <h1 className="text-2xl font-bold text-slate-800">Dashboard Overview</h1>
        <p className="text-slate-500">System metrics from Edumination API.</p>
      </div>

      {/* --- STATS CARDS --- */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <StatCard 
          title="Total Students" 
          value={stats.users.toLocaleString()} 
          icon={Users} 
          color="bg-blue-500 text-blue-600" 
          loading={loading}
        />
        <StatCard 
          title="Published Tests" 
          value={stats.papers} 
          icon={FileText} 
          color="bg-green-500 text-green-600" 
          loading={loading}
        />
        <StatCard 
          title="Active Courses" 
          value={stats.courses} 
          icon={BookOpen} 
          color="bg-purple-500 text-purple-600" 
          loading={loading}
        />
      </div>

      {/* --- CHARTS SECTION (MOCK) --- */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 lg:col-span-2">
          <h3 className="font-bold text-slate-800 mb-4 flex items-center gap-2">
            <TrendingUp size={18} className="text-slate-400"/>
            Growth & Attempts (Simulated)
          </h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={monthlyData}>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <YAxis axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <Tooltip contentStyle={{borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)'}}/>
                <Legend />
                <Bar dataKey="users" fill="#3b82f6" name="New Users" radius={[4, 4, 0, 0]} barSize={30} />
                <Bar dataKey="tests" fill="#10b981" name="Tests Taken" radius={[4, 4, 0, 0]} barSize={30} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100">
          <h3 className="font-bold text-slate-800 mb-4">Skill Dist. (Simulated)</h3>
          <div className="h-64 flex justify-center items-center">
             <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie data={skillsData} cx="50%" cy="50%" innerRadius={60} outerRadius={80} paddingAngle={5} dataKey="value">
                  {skillsData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={PIE_COLORS[index % PIE_COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend layout="horizontal" verticalAlign="bottom" align="center" />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      {/* --- BOTTOM SECTION --- */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        
        {/* Recent Activity (MOCK) */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-slate-800 flex items-center gap-2">
              <Activity size={18} className="text-slate-400"/>
              Recent Activity (Mock)
            </h3>
            <button className="text-xs text-blue-600 font-medium hover:underline" onClick={() => setShowEmailModal(true)}>
                View All Users
            </button>

            {/* Email List Modal (REAL DATA) */}
            {showEmailModal && (
                <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                <div className="bg-white rounded-xl shadow-lg p-8 w-full max-w-lg relative">
                    <button type="button" className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl" onClick={() => setShowEmailModal(false)}>×</button>
                    <h2 className="text-xl font-bold mb-6 text-slate-800">System Users</h2>
                    <div className="max-h-96 overflow-y-auto">
                    <ul className="divide-y divide-slate-100">
                        {loading ? <p>Loading users...</p> : userList.length > 0 ? userList.map((u, idx) => (
                        <li key={idx} className="py-3 flex items-center gap-3">
                            <div>
                                {/* Check cả Email và email đề phòng BE trả về khác nhau */}
                                <span className="font-mono text-slate-700 block">{u.email || u.Email}</span>
                                <span className="text-xs text-slate-400">{u.fullName || u.FullName}</span>
                            </div>
                        </li>
                        )) : <p className="text-slate-500 text-center py-4">No users found.</p>}
                    </ul>
                    </div>
                </div>
                </div>
            )}
          </div>
          
          <div className="space-y-4">
            {MOCK_ACTIVITIES.map((act) => (
               <div key={act.id} className="flex items-start justify-between border-b border-slate-50 pb-4 last:border-0 last:pb-0">
                <div className="flex gap-3">
                  <div className="bg-slate-100 p-2 rounded-full h-10 w-10 flex items-center justify-center text-slate-500 flex-shrink-0">
                    <Clock size={18} />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-slate-800">{act.user}</p>
                    <p className="text-xs text-slate-500 mt-0.5">
                      {act.action} <span className="text-slate-300 mx-1">•</span> <span className="text-blue-600 bg-blue-50 px-1.5 py-0.5 rounded">{act.target}</span>
                    </p>
                  </div>
                </div>
                <div className="text-right flex-shrink-0">
                  <p className="text-xs text-slate-400">{act.time}</p>
                </div>
               </div>
            ))}
          </div>
        </div>
        
        {/* Most Attempted Tests (REAL TITLES) */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 flex flex-col">
           <h3 className="font-bold text-slate-800 mb-4">Latest Published Tests</h3>
           {loading ? (
               <div className="text-center py-10 text-slate-400">Loading papers...</div>
           ) : popularTests.length > 0 ? (
               <div className="space-y-4">
                   {popularTests.map((paper) => (
                       <div key={paper.id} className="flex items-center gap-4 bg-slate-50 p-4 rounded-lg border border-slate-100">
                           <FileText size={24} className="text-blue-400 flex-shrink-0"/>
                           <div className="flex-1 min-w-0">
                               <p className="text-sm font-medium text-slate-800 truncate">{paper.title}</p>
                               <p className="text-xs text-slate-500 mt-1">
                                   ID: {paper.id} • {paper.attempts} attempts
                               </p>
                           </div>
                           <div className="text-right">
                               <span className="text-sm font-bold text-green-600">{paper.avgScore}</span>
                               <p className="text-[10px] text-slate-400">Avg Band</p>
                           </div>
                       </div>
                   ))}
               </div>
           ) : (
               <div className="flex-1 flex flex-col items-center justify-center text-slate-400 bg-slate-50 rounded-lg border border-dashed border-slate-200 min-h-[200px]">
                 <FileText size={32} className="mb-2 opacity-50"/>
                 <p className="text-sm font-medium">No published tests found</p>
               </div>
           )}
        </div>

      </div>
    </div>
  );
}