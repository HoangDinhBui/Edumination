import React, { useEffect, useState } from 'react';
import { 
  Users, 
  FileText, 
  BookOpen, 
  Clock, 
  TrendingUp,
  Activity
} from 'lucide-react';
import { 
  BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, 
  PieChart, Pie, Cell, Legend 
} from 'recharts';

// --- 1. MOCK DATA (DỮ LIỆU GIẢ CHO BIỂU ĐỒ) ---
const monthlyData = [
  { name: 'Jan', users: 400, tests: 240 },
  { name: 'Feb', users: 300, tests: 139 },
  { name: 'Mar', users: 200, tests: 980 },
  { name: 'Apr', users: 278, tests: 390 },
  { name: 'May', users: 189, tests: 480 },
  { name: 'Jun', users: 239, tests: 380 },
  { name: 'Jul', users: 349, tests: 430 },
  { name: 'Aug', users: 450, tests: 560 },
];

const skillsData = [
  { name: 'Reading', value: 35 },
  { name: 'Listening', value: 25 },
  { name: 'Writing', value: 20 },
  { name: 'Speaking', value: 20 },
];

// Mock activity list (Audit Logs Mock)
const MOCK_ACTIVITIES = [
  { id: 1, user: "nguyenvan.a@gmail.com", action: "Completed Test", target: "IELTS Reading Cam 18", time: "5 minutes ago" },
  { id: 2, user: "tran.thib@yahoo.com", action: "New Registration", target: "System", time: "30 minutes ago" },
  { id: 3, user: "admin_main", action: "Created Course", target: "Speaking Masterclass", time: "2 hours ago" },
  { id: 4, user: "user123456", action: "Failed Login", target: "Auth", time: "5 hours ago" },
  { id: 5, user: "le.vanc@gmail.com", action: "Enrolled", target: "IELTS Writing Task 2", time: "1 day ago" },
];

const PIE_COLORS = ['#3b82f6', '#10b981', '#f59e0b', '#ef4444'];

// --- 2. COMPONENT THẺ THỐNG KÊ ---
const StatCard = ({ title, value, icon: Icon, color, loading }: any) => (
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
    // Modal state for full email list
    const [showEmailModal, setShowEmailModal] = useState(false);
  // State lưu dữ liệu
  const [stats, setStats] = useState({
    users: 0,
    papers: 0,
    courses: 0
  });
  const [recentActivities, setRecentActivities] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

    useEffect(() => {
    // Simulate API call (Mocking API Call)
    const loadFakeData = () => {
      setLoading(true);
      setTimeout(() => {
        setStats({
          users: 12,  // Fake number
          papers: 18,   // Fake number
          courses: 12   // Fake number
        });
        setRecentActivities(MOCK_ACTIVITIES);
        setLoading(false);
      }, 800);
    };
    loadFakeData();
    }, []);

  return (
    <div className="space-y-6 pb-10">
      {/* --- HEADER --- */}
      <div>
        <h1 className="text-2xl font-bold text-slate-800">Dashboard Overview</h1>
        <p className="text-slate-500">System activity overview.</p>
      </div>

      {/* --- STATS CARDS (FAKE DATA) --- */}
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

      {/* --- CHARTS SECTION (DATA FAKE) --- */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        
        {/* Chart 1: User Growth */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 lg:col-span-2">
          <h3 className="font-bold text-slate-800 mb-4 flex items-center gap-2">
            <TrendingUp size={18} className="text-slate-400"/>
            User Growth & Test Attempts
          </h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={monthlyData}>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <YAxis axisLine={false} tickLine={false} tick={{fill: '#94a3b8'}} />
                <Tooltip 
                  contentStyle={{borderRadius: '8px', border: 'none', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)'}}
                />
                <Legend />
                <Bar dataKey="users" fill="#3b82f6" name="New Users" radius={[4, 4, 0, 0]} barSize={30} />
                <Bar dataKey="tests" fill="#10b981" name="Tests Taken" radius={[4, 4, 0, 0]} barSize={30} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Chart 2: Skill Distribution */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100">
          <h3 className="font-bold text-slate-800 mb-4">Skill Distribution</h3>
          <div className="h-64 flex justify-center items-center">
             <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={skillsData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={80}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {skillsData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={PIE_COLORS[index % PIE_COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend layout="horizontal" verticalAlign="bottom" align="center" />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="text-center mt-4 text-xs text-slate-400">
            *Demo data
          </div>
        </div>
      </div>

      {/* --- BOTTOM SECTION: LOGS & LISTS --- */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        
        {/* Recent Activity (FAKE DATA) */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100">
          <div className="flex justify-between items-center mb-4">
            <h3 className="font-bold text-slate-800 flex items-center gap-2">
              <Activity size={18} className="text-slate-400"/>
              Recent System Activity
            </h3>
            <button className="text-xs text-blue-600 font-medium hover:underline" onClick={() => setShowEmailModal(true)}>View All</button>
                {/* Email List Modal */}
                {showEmailModal && (
                  <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                    <div className="bg-white rounded-xl shadow-lg p-8 w-full max-w-lg relative">
                      <button
                        type="button"
                        className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                        onClick={() => setShowEmailModal(false)}
                      >×</button>
                      <h2 className="text-xl font-bold mb-6 text-slate-800">All User Emails</h2>
                      <div className="max-h-96 overflow-y-auto">
                        <ul className="divide-y divide-slate-100">
                          {recentActivities.map((act, idx) => (
                            <li key={idx} className="py-3 flex items-center gap-3">
                              <span className="font-mono text-slate-700">{act.user}</span>
                              <span className="text-xs text-slate-400 ml-auto">{act.time}</span>
                            </li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  </div>
                )}
          </div>
          
          <div className="space-y-4">
            {loading ? (
              <p className="text-slate-400 text-sm">Loading activities...</p>
            ) : recentActivities.length > 0 ? (
              recentActivities.map((act) => (
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
              ))
            ) : (
              <div className="text-center py-8 text-slate-400 text-sm">
               No data available.
              </div>
            )}
          </div>
        </div>
        
        {/* Popular Tests Placeholder */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-slate-100 flex flex-col">
           <h3 className="font-bold text-slate-800 mb-4">Most Attempted Tests</h3>
           <div className="flex-1 flex flex-col items-center justify-center text-slate-400 bg-slate-50 rounded-lg border border-dashed border-slate-200 min-h-[200px]">
              <FileText size={32} className="mb-2 opacity-50"/>
              <p className="text-sm font-medium">IELTS Reading Cam 16 - Test 1</p>
              <p className="text-xs mt-1 text-slate-500">560 attempts • Avg: 6.5</p>
              
              <div className="w-full border-t border-slate-100 my-4"></div>

              <FileText size={32} className="mb-2 opacity-50"/>
              <p className="text-sm font-medium">Listening Practice Vol 2</p>
              <p className="text-xs mt-1 text-slate-500">320 attempts • Avg: 7.0</p>
           </div>
        </div>

      </div>
    </div>
  );
}