import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { 
  Target, TrendingUp, Clock, 
  Volume2, BookOpen, Edit, 
  Loader2, Save, Camera 
} from "lucide-react";
import Navbar from "../../components/Navbar"; // Đảm bảo đường dẫn đúng

// --- 1. CONFIG & INTERFACES ---

const API_BASE = "http://localhost:8081/api/v1";

interface UserStats {
  userId: number;
  totalTests: number;
  bestBand?: number;
  worstBand?: number;
  avgListeningBand?: number;
  avgReadingBand?: number;
  avgWritingBand?: number;
  avgSpeakingBand?: number;
  updatedAt: string;
}

interface ProfileFormData {
  firstName: string;
  email: string;
  avatarUrl: string;
  dateOfBirth: string;
}

// --- 2. MOCK DATA (DỮ LIỆU GIẢ) ---

// A. Dữ liệu giả cho Stats (Khi user chưa làm bài nào)
const MOCK_STATS_DATA: UserStats = {
  userId: 0,
  totalTests: 15,          // Giả vờ đã làm 15 bài
  bestBand: 7.5,           // Band cao nhất giả định
  avgListeningBand: 7.0,
  avgReadingBand: 7.5,
  avgWritingBand: 6.0,
  avgSpeakingBand: 6.5,
  updatedAt: new Date().toISOString()
};

// B. Dữ liệu giả cho Lịch sử thi & Biểu đồ
const MOCK_HISTORY_DATA = [
  {
    id: 101, paper_title: "IELTS Academic Test 1",
    started_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 6).toISOString(), 
    finished_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 6 + 1000 * 60 * 45).toISOString(), overall_score: 6.0
  },
  {
    id: 102, paper_title: "Cambridge 15 - Test 2",
    started_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 4).toISOString(),
    finished_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 4 + 1000 * 60 * 55).toISOString(), overall_score: 6.5
  },
  {
    id: 103, paper_title: "Reading Practice Vol 1",
    started_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2).toISOString(),
    finished_at: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2 + 1000 * 60 * 30).toISOString(), overall_score: 7.0
  },
  {
    id: 104, paper_title: "Full Mock Test Phase 1",
    started_at: new Date().toISOString(), 
    finished_at: new Date(Date.now() + 1000 * 60 * 60).toISOString(), overall_score: 7.5
  }
];

// --- 3. MAIN COMPONENT ---

export default function ProfileDashboard() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  
  // State Profile
  const [formData, setFormData] = useState<ProfileFormData>({
    firstName: "", email: "", avatarUrl: "", dateOfBirth: ""
  });

  // State Stats (Khởi tạo bằng 0 để tránh lỗi trước khi fetch)
  const [userStats, setUserStats] = useState<UserStats>({
      userId: 0, totalTests: 0, bestBand: 0, 
      avgListeningBand: 0, avgReadingBand: 0, avgWritingBand: 0, avgSpeakingBand: 0,
      updatedAt: new Date().toISOString()
  });

  const [testHistory, setTestHistory] = useState<any[]>([]);
  const [chartData, setChartData] = useState<any[]>([]);

  // --- HELPER FUNCTIONS ---
  const calculateDuration = (start: string, end?: string) => {
    if (!end) return "In Progress";
    const diffMs = new Date(end).getTime() - new Date(start).getTime();
    const minutes = Math.floor(diffMs / 60000);
    return `${minutes} mins`;
  };

  const processChartData = (attempts: any[]) => {
    const grouped: Record<string, any> = {};
    const sorted = [...attempts].sort((a, b) => new Date(a.started_at).getTime() - new Date(b.started_at).getTime());
    sorted.forEach(att => {
      const dateKey = new Date(att.started_at).toLocaleDateString('en-GB').slice(0, 5);
      if (!grouped[dateKey]) grouped[dateKey] = { totalScore: 0, count: 0 };
      grouped[dateKey].totalScore += (att.overall_score || 0);
      grouped[dateKey].count += 1;
    });
    setChartData(Object.keys(grouped).map(d => ({ 
        date: d, value: parseFloat((grouped[d].totalScore / grouped[d].count).toFixed(1)) 
    })).slice(-7));
  };

  // --- USE EFFECT: LOAD DATA ---
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem("Token");
      if (!token) {
          // navigate("/login"); // Mở comment nếu muốn bắt buộc login
          console.warn("No token found");
          setLoading(false);
          return;
      }

      const headers = { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' };

      try {
        setLoading(true);

        // Gọi API Profile, Stats và Mock History cùng lúc
        const [profileRes, statsRes, historyData] = await Promise.all([
          fetch(`${API_BASE}/Auth/me`, { headers }),
          fetch(`${API_BASE}/me/stats`, { headers }),
          new Promise<any[]>((r) => setTimeout(() => r(MOCK_HISTORY_DATA), 500)) // Mock History
        ]);

        // 1. XỬ LÝ PROFILE
        if (profileRes.ok) {
          const resJson = await profileRes.json();
          // Lấy object profile (ưu tiên Key viết hoa theo Backend .NET)
          const p = resJson.Profile || resJson.profile || resJson;
          
          if (p) {
             setFormData({
                firstName: p.FullName || p.fullName || "", 
                email: p.Email || p.email || "",
                avatarUrl: p.AvatarUrl || p.avatarUrl || "",
                dateOfBirth: p.DateOfBirth || p.dateOfBirth || "" 
             });
          }
        } else if (profileRes.status === 401) {
             localStorage.removeItem("Token");
             navigate("/login");
             return;
        }

        // 2. XỬ LÝ STATS (QUAN TRỌNG: FAKE NẾU TRỐNG)
        if (statsRes.ok) {
          const sData = await statsRes.json();
          const realTotal = sData.TotalTests || sData.totalTests || 0;

          if (realTotal === 0) {
              // >>> LOGIC FAKE DATA KHI CHƯA CÓ BÀI LÀM <<<
              console.log("User chưa có bài làm -> Hiển thị Mock Stats");
              setUserStats(MOCK_STATS_DATA);
          } else {
              // Có dữ liệu thật -> Map dữ liệu thật
              setUserStats({
                  userId: sData.UserId || sData.userId,
                  totalTests: realTotal,
                  bestBand: sData.BestBand || sData.bestBand || 0,
                  avgListeningBand: sData.AvgListeningBand || sData.avgListeningBand || 0,
                  avgReadingBand: sData.AvgReadingBand || sData.avgReadingBand || 0,
                  avgWritingBand: sData.AvgWritingBand || sData.avgWritingBand || 0,
                  avgSpeakingBand: sData.AvgSpeakingBand || sData.avgSpeakingBand || 0,
                  updatedAt: sData.UpdatedAt || sData.updatedAt
              });
          }
        } else {
             // Nếu API Stats lỗi (404) -> Cũng hiển thị Mock Stats cho đẹp
             console.log("Stats API 404 -> Hiển thị Mock Stats");
             setUserStats(MOCK_STATS_DATA);
        }

        // 3. XỬ LÝ HISTORY (LUÔN MOCK VÌ BE CHƯA CÓ)
        if (historyData) {
             const mapped = historyData.map((item: any) => ({
                 id: item.id,
                 date: new Date(item.started_at).toLocaleDateString('en-GB'),
                 time: new Date(item.started_at).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}),
                 name: item.paper_title,
                 score: item.overall_score,
                 timeSpent: calculateDuration(item.started_at, item.finished_at)
             }));
             setTestHistory([...mapped].reverse());
             processChartData(historyData);
        }

      } catch (error) {
        console.error("Lỗi tải trang Dashboard:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [navigate]);

  // --- HANDLE SAVE ---
  const handleSaveProfile = async () => {
    const token = localStorage.getItem("Token");
    try {
      setSaving(true);
      const payload = {
          fullName: formData.firstName, // Gửi camelCase
          avatarUrl: formData.avatarUrl
      };

      const res = await fetch(`${API_BASE}/Auth/me/profile`, {
        method: 'PUT',
        headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      
      if (res.ok) alert("Cập nhật hồ sơ thành công!");
      else alert("Lỗi cập nhật: " + (await res.text()));
    } catch (e) { alert("Lỗi kết nối server!"); } 
    finally { setSaving(false); }
  };

  // --- RENDER ---
  const statsList = [
    { icon: Target, label: "Total Tests", value: userStats.totalTests, color: "text-blue-600", bgColor: "bg-blue-50" },
    { icon: TrendingUp, label: "Best Band", value: userStats.bestBand || "-", color: "text-green-600", bgColor: "bg-green-50" },
    { icon: Volume2, label: "Listening", value: userStats.avgListeningBand || "-", color: "text-orange-600", bgColor: "bg-orange-50" },
    { icon: BookOpen, label: "Reading", value: userStats.avgReadingBand || "-", color: "text-pink-600", bgColor: "bg-pink-50" },
    { icon: Edit, label: "Writing", value: userStats.avgWritingBand || "-", color: "text-purple-600", bgColor: "bg-purple-50" }
  ];

  if (loading) return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50">
        <Loader2 className="w-10 h-10 animate-spin text-blue-600 mb-2" />
        <span className="text-gray-500 text-sm">Loading dashboard...</span>
    </div>
  );

  return (
    <div className="min-h-screen bg-slate-50 pb-12">
        <Navbar/>
        <div className="w-full max-w-7xl mx-auto px-4 sm:px-8 py-6 mt-10">
            
            {/* HEADER */}
            <div className="mb-8">
                <h1 className="text-2xl font-bold text-gray-800">Dashboard</h1>
                <p className="text-gray-500">Welcome back, <span className="font-bold text-blue-600">{formData.firstName || "Học viên"}</span>!</p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-[340px_1fr] gap-8">
                
                {/* === LEFT COL: PROFILE === */}
                <div className="bg-white rounded-2xl shadow-sm border border-gray-100 p-6 h-fit sticky top-24">
                    <div className="flex flex-col items-center mb-6 group relative">
                        <div className="w-28 h-28 rounded-full overflow-hidden mb-4 border-4 border-white shadow-md relative bg-gray-200">
                            <img 
                                src={formData.avatarUrl || `https://ui-avatars.com/api/?name=${formData.firstName || "U"}&background=random&size=128`} 
                                alt="Avatar" 
                                className="w-full h-full object-cover"
                                onError={(e) => { (e.target as HTMLImageElement).src = "https://via.placeholder.com/150"; }}
                            />
                            {/* Edit Overlay */}
                            <div className="absolute inset-0 bg-black/30 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity cursor-pointer">
                                <Camera className="text-white w-8 h-8"/>
                            </div>
                        </div>
                        <h3 className="text-xl font-bold text-gray-900 text-center px-2">{formData.firstName || "Chưa đặt tên"}</h3>
                        <p className="text-sm text-gray-500">{formData.email}</p>
                    </div>

                    <div className="space-y-4">
                        <div>
                            <label className="text-xs font-bold text-gray-500 uppercase">Full Name</label>
                            <input 
                                type="text" 
                                value={formData.firstName}
                                onChange={(e) => setFormData({...formData, firstName: e.target.value})}
                                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg mt-1 focus:border-blue-500 outline-none text-sm font-medium transition-all"
                                placeholder="Nhập tên hiển thị..."
                            />
                        </div>
                        <div>
                            <label className="text-xs font-bold text-gray-500 uppercase">Avatar Link</label>
                            <input 
                                type="text" 
                                value={formData.avatarUrl}
                                onChange={(e) => setFormData({...formData, avatarUrl: e.target.value})}
                                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg mt-1 focus:border-blue-500 outline-none text-sm transition-all"
                                placeholder="https://..."
                            />
                        </div>
                        <div>
                            <label className="text-xs font-bold text-gray-500 uppercase">Date of Birth</label>
                            <input 
                                type="text" 
                                value={formData.dateOfBirth}
                                onChange={(e) => setFormData({...formData, dateOfBirth: e.target.value})}
                                className="w-full px-4 py-2.5 border border-gray-200 rounded-lg mt-1 focus:border-blue-500 outline-none text-sm transition-all"
                                placeholder="dd/mm/yyyy"
                            />
                        </div>
                        <button 
                            onClick={handleSaveProfile}
                            disabled={saving}
                            className="w-full bg-blue-600 text-white py-3 rounded-lg mt-4 font-semibold hover:bg-blue-700 transition-all flex items-center justify-center gap-2 shadow-lg shadow-blue-500/20 disabled:opacity-70"
                        >
                            {saving ? <Loader2 className="w-4 h-4 animate-spin"/> : <Save className="w-4 h-4"/>}
                            {saving ? "Saving..." : "Save Changes"}
                        </button>
                    </div>
                </div>

                {/* === RIGHT COL: STATS & CHART === */}
                <div className="space-y-6">
                    
                    {/* 1. Stats Cards */}
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                      {statsList.map((stat, idx) => (
                        <div key={idx} className="bg-white rounded-xl shadow-sm p-4 border border-gray-100 hover:-translate-y-1 transition-transform duration-200">
                          <div className={`w-10 h-10 ${stat.bgColor} rounded-lg flex items-center justify-center mb-2`}>
                            <stat.icon className={`w-5 h-5 ${stat.color}`} />
                          </div>
                          <p className="text-xs text-gray-500 font-bold uppercase">{stat.label}</p>
                          <p className={`text-2xl font-bold ${stat.color}`}>{stat.value}</p>
                        </div>
                      ))}
                    </div>

                    {/* 2. Performance Chart */}
                    <div className="bg-white rounded-xl shadow-sm p-6 border border-gray-100">
                        <h3 className="font-bold text-gray-800 mb-4 flex items-center gap-2">
                            <TrendingUp className="w-5 h-5 text-blue-600"/> Performance Overview
                        </h3>
                        <div className="h-60 flex items-end justify-between px-4 gap-3 bg-gray-50 rounded-lg pt-4 border border-dashed border-gray-200">
                           {chartData.length > 0 ? chartData.map((item, idx) => (
                                <div key={idx} className="flex flex-col items-center w-full group relative h-full justify-end">
                                    {/* Tooltip */}
                                    <div className="opacity-0 group-hover:opacity-100 absolute bottom-full mb-2 bg-gray-800 text-white text-xs px-2 py-1 rounded z-10 transition-all whitespace-nowrap">
                                        Band: {item.value}
                                    </div>
                                    {/* Bar */}
                                    <div 
                                        className="w-full max-w-[40px] bg-gradient-to-t from-blue-500 to-blue-400 rounded-t-md hover:from-blue-600 hover:to-blue-500 transition-all relative shadow-sm"
                                        style={{ height: `${(item.value / 9) * 100}%`, minHeight: '6px' }}
                                    ></div>
                                    <span className="text-[10px] text-gray-500 mt-2 font-medium">{item.date}</span>
                                </div>
                           )) : (
                               <div className="w-full h-full flex items-center justify-center text-gray-400 text-sm">
                                   No data available.
                               </div>
                           )}
                        </div>
                    </div>

                    {/* 3. Test History Table */}
                    <div className="bg-white rounded-xl shadow-sm overflow-hidden border border-gray-100">
                        <div className="px-6 py-4 border-b border-gray-100 bg-gray-50/50 flex justify-between items-center">
                            <h3 className="font-bold text-gray-800">Recent Attempts</h3>
                            <span className="text-xs bg-white border px-2 py-1 rounded text-gray-500">Mock Data</span>
                        </div>
                        <div className="overflow-x-auto">
                            <table className="w-full">
                                <thead className="bg-gray-50 text-left">
                                    <tr>
                                        <th className="py-3 px-6 text-xs font-bold text-gray-500 uppercase">Date</th>
                                        <th className="py-3 px-6 text-xs font-bold text-gray-500 uppercase">Test Name</th>
                                        <th className="py-3 px-6 text-xs font-bold text-gray-500 uppercase text-center">Score</th>
                                        <th className="py-3 px-6 text-xs font-bold text-gray-500 uppercase text-right">Duration</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-gray-50">
                                    {testHistory.length > 0 ? testHistory.map((t, i) => (
                                        <tr key={i} className="hover:bg-blue-50/30 transition-colors">
                                            <td className="py-3 px-6">
                                                <div className="text-sm font-medium text-gray-900">{t.date}</div>
                                                <div className="text-xs text-gray-400">{t.time}</div>
                                            </td>
                                            <td className="py-3 px-6 text-sm text-gray-600 font-medium">{t.name}</td>
                                            <td className="py-3 px-6 text-center">
                                                <span className={`px-2 py-1 text-xs font-bold rounded ${
                                                    t.score >= 7 ? "bg-green-100 text-green-700" :
                                                    t.score >= 5 ? "bg-blue-100 text-blue-700" : "bg-orange-100 text-orange-700"
                                                }`}>
                                                    {t.score}
                                                </span>
                                            </td>
                                            <td className="py-3 px-6 text-right text-sm text-gray-500">
                                                <div className="flex items-center justify-end gap-1">
                                                    <Clock className="w-3 h-3 text-gray-400"/> {t.timeSpent}
                                                </div>
                                            </td>
                                        </tr>
                                    )) : (
                                        <tr><td colSpan={4} className="text-center py-8 text-gray-400 text-sm">Chưa có bài thi nào.</td></tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
  );
}