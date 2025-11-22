// falseimport React, { useState } from 'react';
import React, { useState, useEffect } from 'react';
import { ChevronDown, Filter, Search, User, Briefcase, Clock, BookOpen, BarChart, UserCheck } from 'lucide-react';
import logoImage from "../../assets/img/Rectangle 78.png";
const Dropdown: React.FC<{
  title: string;
  sections: { header?: string; items: string[] }[];
}> = ({ title, sections }) => {
  return (
    <div className="relative group">
      <button className="inline-flex items-center gap-1 text-slate-700 hover:text-slate-900 font-medium">
        {title}
        <ChevronDown className="h-4 w-4" />
      </button>
      <div className="absolute top-full left-1/2 -translate-x-1/2 hidden group-hover:flex gap-4 p-3">
        {sections.map((sec, i) => (
          <div
            key={i}
            className="bg-white/95 backdrop-blur shadow-xl ring-1 ring-slate-100 rounded-2xl p-3 w-56"
          >
            {sec.header && (
              <div className="text-[13px] font-semibold text-sky-700 mb-2">
                {sec.header}
              </div>
            )}
            <ul className="space-y-2">
              {sec.items.map((it) => (
                <li
                  key={it}
                  className="text-sm text-slate-600 hover:text-slate-900 cursor-pointer"
                >
                  {it}
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </div>
  );
};


// === COMPONENT NAVBAR CỦA BẠN (ĐÃ CHỈNH SỬA MÀU SẮC) ===
const Navbar: React.FC = () => {
    return (
        <header className="sticky top-0 z-40 bg-white/95 backdrop-blur-sm border-b border-slate-200">
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
                <div className="flex items-center gap-8">
                    {/* Logo (EDM) */}
                    <a href="#" className="flex items-center gap-2">
                      <img
                        src={logoImage}
                        className="h-7 rounded"
                        alt="logo"
                      />
                    </a>
                    <nav className="hidden md:flex items-center gap-6">
                        <a className="text-slate-700 hover:text-sky-600 font-medium transition duration-150" href="#">
                            Home
                        </a>
                        <Dropdown
                            title="IELTS Exam Library"
                            sections={[
                                {
                                    header: "",
                                    items: [
                                        "IELTS Listening Test",
                                        "IELTS Reading Test",
                                        "IELTS Writing Test",
                                        "IELTS Speaking Test",
                                        "IELTS Test Collection",
                                    ],
                                },
                            ]}
                        />
                        <Dropdown
                            title="IELTS Course"
                            sections={[
                                {
                                    header: "IELTS Foundation (0.0–5.0)",
                                    items: [
                                        "IELTS 5.5–6.0 Booster",
                                        "IELTS 6.0–7.5 Intensive",
                                        "IELTS 7.5–9.0 Mastery",
                                    ],
                                },
                            ]}
                        />
                        {/* Mục Ranking được làm nổi bật (Active) */}
                        <a 
                           className="font-bold text-sky-600 border-b-2 border-sky-600 pb-1.5 transition duration-150" 
                           href="#"
                        >
                            Ranking
                        </a>
                    </nav>
                </div>
                <div className="flex items-center gap-3">
                    <a
                        href="#signin"
                        className="text-slate-600 hover:text-sky-600 text-sm font-semibold transition duration-150"
                    >
                        Sign in
                    </a>
                    <a
                        href="#signup"
                        // Chỉnh màu nút Sign up thành xanh lá cây (giống ảnh Ranking gốc)
                        className="text-sm font-semibold text-white bg-green-500 px-4 py-2 rounded-full shadow-md hover:bg-green-600 transition duration-150"
                    >
                        Sign up
                    </a>
                </div>
            </div>
        </header>
    );
};


// === MOCK DATA & CÁC COMPONENTS KHÁC (GIỮ NGUYÊN) ===
const mockUserDetails = {
    email: '6451071025@st.utc2.edu.vn',
    bestBand: '7.5',
    worstBand: '6.0',
    bestSkill: 'Reading (8.0)',
    worstSkill: 'Writing (6.0)',
    avgBand: '6.8',
};

// MOCK LEADERBOARD DATA
const mockLeaderboard = [
  { id: 3, fullName: "Tran Thi My Lam", email: "student1@utc2.edu.vn", bestOverallBand: 7.5, bestAt: "2025-08-19 03:07:57" },
  { id: 4, fullName: "Nguyen Van B", email: "student100@utc2.edu.vn", bestOverallBand: 7.0, bestAt: "2025-08-19 03:16:47" },
  { id: 5, fullName: "Nguyen Van B", email: "student2@utc2.edu.vn", bestOverallBand: 6.5, bestAt: "2025-08-19 03:21:41" },
  { id: 6, fullName: "Nguyen Van C", email: "student3@utc2.edu.vn", bestOverallBand: 8.0, bestAt: "2025-08-19 07:57:27" },
  { id: 7, fullName: "Nguyen Van A", email: "a@abc.edu.vn", bestOverallBand: 8.5, bestAt: "2025-08-22 09:34:11" },
  { id: 8, fullName: "Tran Thi B", email: "b@def.edu.vn", bestOverallBand: 7.8, bestAt: "2025-08-22 09:34:11" },
  { id: 9, fullName: "Le Van C", email: "c@xyz.edu.vn", bestOverallBand: 7.2, bestAt: "2025-08-22 09:34:11" },
  { id: 10, fullName: "Bui Dinh Hoang (updated)", email: "teacher1@utc2.edu.vn", bestOverallBand: 9.0, bestAt: "2025-08-25 04:05:04" },
  { id: 11, fullName: "Truong Dinh Hoang", email: "student19@utc2.edu.vn", bestOverallBand: 8.1, bestAt: "2025-08-27 03:53:47" },
  { id: 12, fullName: "Bùi Đình Hoàng", email: "buidinhhoang1910@gmail.com", bestOverallBand: 8.9, bestAt: "2025-09-01 09:27:00" }
];

// ... (FilterDropdown component giữ nguyên)
interface DropdownProps {
    title: string;
    icon: React.ElementType;
    options: string[];
    value: string;
    onChange: (value: string) => void;
}

const FilterDropdown: React.FC<DropdownProps> = ({ title, icon: Icon, options, value, onChange }) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className="relative">
            <button
                type="button"
                className="flex items-center justify-between px-4 py-2 bg-white border border-slate-300 rounded-lg shadow-sm w-44 text-slate-700 hover:bg-slate-50 transition duration-150"
                onClick={() => setIsOpen(!isOpen)}
            >
                <Icon className="w-5 h-5 text-slate-500 mr-2" />
                <span className="font-medium truncate">{value || title}</span>
                <ChevronDown className={`w-4 h-4 ml-1 transition-transform ${isOpen ? 'rotate-180' : ''}`} />
            </button>
            {isOpen && (
                <div className="absolute z-10 mt-2 w-48 bg-white rounded-lg shadow-xl border border-slate-200">
                    <div className="py-1">
                        {options.map((option) => (
                            <button
                                key={option}
                                className="block w-full text-left px-4 py-2 text-sm text-slate-700 hover:bg-sky-50 hover:text-sky-600"
                                onClick={() => {
                                    onChange(option);
                                    setIsOpen(false);
                                }}
                            >
                                {option}
                            </button>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
};


// === COMPONENT CHÍNH RANKING PAGE ===
export default function RankingPage() {
    const [domain, setDomain] = useState('@st.utc2.edu.vn');
    const [quarter, setQuarter] = useState('Quarter 1');
    const [time, setTime] = useState('Today');
    const [searchQuery, setSearchQuery] = useState('');
    const [leaderboard, setLeaderboard] = useState<Array<Record<string, unknown>>>(mockLeaderboard);
    const [filteredLeaderboard, setFilteredLeaderboard] = useState<Array<Record<string, unknown>>>(mockLeaderboard);

    // Filter logic
    const handleFilter = () => {
      let filtered = leaderboard;
      // Filter by domain (email)
      if (domain && domain !== "All") {
        filtered = filtered.filter(item => String(item.email ?? '').endsWith(domain));
      }
      // Filter by paper (simulate by bestBand for demo)
      if (quarter && quarter !== "All" && quarter !== 'Quarter 1') {
        filtered = filtered.filter(item => Number(item.bestOverallBand ?? 0) >= 7);
      }
      // Filter by time (simulate by bestAt date)
      if (time && time !== "All" && time !== 'Today') {
        filtered = filtered.filter(item => String(item.bestAt ?? '').startsWith('2025-08'));
      }
      // Filter by search query
      if (searchQuery) {
        filtered = filtered.filter(item => String(item.fullName ?? '').toLowerCase().includes(searchQuery.toLowerCase()));
      }
      setFilteredLeaderboard(filtered);
    };

    useEffect(() => {
      const fetchLeaderboard = async () => {
        try {
          const res = await fetch('http://localhost:8081/api/v1/leaderboard?paperId=1&limit=100');
          if (!res.ok) throw new Error('Failed to fetch leaderboard');
          const data = await res.json();
          setLeaderboard(Array.isArray(data) ? data : (data.items || data.Items || data.data || []));
          setFilteredLeaderboard(Array.isArray(data) ? data : (data.items || data.Items || data.data || []));
        } catch {
          setLeaderboard([]);
          setFilteredLeaderboard([]);
        }
      };
      // fetchLeaderboard();
    }, []);

    return (
        <div className="min-h-screen bg-slate-50" style={{ overflowY: 'auto', maxHeight: '100vh' }}>
            {/* === Navigation & Header (SỬ DỤNG NAVBAR MỚI) === */}
            <Navbar /> 

            {/* === Main Content Grid === */}
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
                <h1 className="text-4xl font-bold text-slate-800 mb-10 text-center">Ranking</h1>

                <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
                    {/* === CỘT TRUNG TÂM (FILTERS & TABLE) - 9/12 === */}
                    <div className="lg:col-span-8 space-y-6">
                        {/* --- Filters Row --- */}
                        <div className="flex flex-col gap-4 p-4 bg-white rounded-xl shadow-lg border border-slate-100">
                            <div className="flex flex-row gap-4 w-full">
                                <FilterDropdown
                                    title="Domain EDU"
                                    icon={Briefcase}
                                    options={["All", '@st.utc2.edu.vn', '@gmail.com', '@hcmut.edu.vn']}
                                    value={domain}
                                    onChange={setDomain}
                                />
                                <FilterDropdown
                                    title="Paper"
                                    icon={BookOpen}
                                    options={["All", 'Quarter 1', 'Quarter 2', 'Quarter 3', 'Quarter 4']}
                                    value={quarter}
                                    onChange={setQuarter}
                                />
                                <FilterDropdown
                                    title="Time"
                                    icon={Clock}
                                    options={["All", 'Today', 'Last 7 days', 'Last 30 days', 'This month', 'Last month']}
                                    value={time}
                                    onChange={setTime}
                                />
                                <button className="flex items-center px-4 py-2 bg-sky-600 text-white font-semibold rounded-lg shadow-md hover:bg-sky-700 transition duration-150 h-[42px]" onClick={handleFilter}>
                                    <Filter className="w-5 h-5 mr-2" />
                                    Filter
                                </button>
                            </div>
                            <div className="flex items-center border border-slate-300 rounded-lg bg-white w-full">
                                <input
                                    type="text"
                                    placeholder="Search student..."
                                    value={searchQuery}
                                    onChange={(e) => setSearchQuery(e.target.value)}
                                    className="px-4 py-2 w-full focus:outline-none rounded-l-lg"
                                />
                                <button className="p-2 text-slate-500 hover:text-sky-600">
                                    <Search className="w-5 h-5" />
                                </button>
                            </div>
                        </div>

                        {/* --- Ranking Table --- */}
                        <div className="bg-white rounded-xl shadow-lg overflow-auto border border-slate-100" style={{ maxHeight: '520px' }}>
                            <table className="min-w-full divide-y divide-slate-200" style={{ minWidth: '700px' }}>
                                <thead className="bg-slate-50">
                                    <tr>
                                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">#</th>
                                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Full Name</th>
                                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Email</th>
                                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Best Band</th>
                                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Best At</th>
                                    </tr>
                                </thead>
                                <tbody className="bg-white divide-y divide-slate-200">
                                    {filteredLeaderboard.map((item, idx) => {
                                        const fullName = String(item.fullName ?? item.FullName ?? '');
                                        const email = String(item.email ?? item.Email ?? '');
                                        const bestBand = String(item.bestOverallBand ?? item.BestOverallBand ?? '');
                                        const bestAt = String(item.bestAt ?? item.BestAt ?? '');
                                        return (
                                            <tr key={String(item.id ?? item.Id ?? idx)} className={idx < 3 ? 'bg-sky-50/50' : 'hover:bg-slate-50'}>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-slate-900">{idx + 1}</td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700 flex items-center">
                                                    <User className="w-4 h-4 mr-2 text-sky-600" />
                                                    {fullName}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{email}</td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm font-bold text-sky-600">{bestBand}</td>
                                                <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-500">{bestAt}</td>
                                            </tr>
                                        );
                                    })}
                                </tbody>
                            </table>
                            <style>{`
                              /* Custom Scrollbar for Ranking Table (vertical & horizontal) */
                              div::-webkit-scrollbar {
                                width: 8px;
                                height: 8px;
                                background: #F3F4F6;
                              }
                              div::-webkit-scrollbar-thumb {
                                background: #7BA5D1;
                                border-radius: 8px;
                              }
                              div::-webkit-scrollbar-track {
                                background: #F3F4F6;
                              }
                            `}</style>
                        </div>
                    </div>

                    {/* === CỘT PHẢI (USER DETAILS) - 3/12 === */}
                    <div className="lg:col-span-4 space-y-6">
                        <div className="bg-white rounded-xl shadow-lg p-6 border border-slate-100 sticky top-4">
                            <h2 className="text-xl font-bold text-slate-800 mb-6">User details</h2>
                            
                            {/* User Info */}
                            <div className="flex items-center mb-6 border-b pb-4">
                                <div className="p-3 bg-slate-100 rounded-full mr-4">
                                    <User className="w-6 h-6 text-slate-500" />
                                </div>
                                <div>
                                    <p className="text-sm font-semibold text-slate-800">Student ID / Email</p>
                                    <p className="text-sm text-slate-500 break-all">{mockUserDetails.email}</p>
                                </div>
                            </div>

                            {/* Overview Section */}
                            <h3 className="text-lg font-semibold text-slate-700 mb-4 flex items-center">
                                <BarChart className="w-5 h-5 mr-2 text-sky-600" />
                                Overview
                            </h3>
                            <ul className="space-y-3 text-sm text-slate-700">
                                <li className="flex justify-between">
                                    <span>Best band:</span>
                                    <span className="font-semibold text-sky-600">{mockUserDetails.bestBand}</span>
                                </li>
                                <li className="flex justify-between">
                                    <span>Worst band:</span>
                                    <span className="font-semibold text-red-500">{mockUserDetails.worstBand}</span>
                                </li>
                                <li className="flex justify-between">
                                    <span>Best skill:</span>
                                    <span className="font-semibold">{mockUserDetails.bestSkill}</span>
                                </li>
                                <li className="flex justify-between">
                                    <span>Worst skill:</span>
                                    <span className="font-semibold">{mockUserDetails.worstSkill}</span>
                                </li>
                                <li className="flex justify-between pt-2 border-t mt-2">
                                    <span className="font-bold">AVG band:</span>
                                    <span className="font-bold">{mockUserDetails.avgBand}</span>
                                </li>
                            </ul>

                            {/* My Rank Button */}
                            <button className="w-full mt-8 py-3 flex items-center justify-center bg-slate-100 text-slate-700 font-semibold rounded-lg hover:bg-slate-200 transition duration-150">
                                <UserCheck className="w-5 h-5 mr-2" />
                                My rank
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}