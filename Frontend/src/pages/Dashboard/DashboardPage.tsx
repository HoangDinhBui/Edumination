import React, { useState } from "react";
import { Target, TrendingUp, FileText, Clock, Award, Volume2, BookOpen, Edit, Mic, Calendar, Mail, Lock, User } from "lucide-react";
import Navbar from "../../components/Navbar";

export default function ProfileDashboard() {
  const [activeTab, setActiveTab] = useState("LISTENING");
  const [formData, setFormData] = useState({
    firstName: "Tran Dung",
    dateOfBirth: "",
    email: "dungn812@gmail.com",
    password: ""
  });

  const stats = [
    { icon: Target, label: "Target Score", value: "7.0", color: "text-blue-600", bgColor: "bg-blue-50" },
    { icon: TrendingUp, label: "Average Score", value: "1", color: "text-green-600", bgColor: "bg-green-50" },
    { icon: FileText, label: "Total Tests Taken", value: "4", color: "text-purple-600", bgColor: "bg-purple-50" },
    { icon: Clock, label: "Average Time", value: "13:46 mins", color: "text-orange-600", bgColor: "bg-orange-50" },
    { icon: Award, label: "Accuracy", value: "1.67%", color: "text-pink-600", bgColor: "bg-pink-50" }
  ];

  const tabs = [
    { id: "LISTENING", icon: Volume2, label: "LISTENING" },
    { id: "READING", icon: BookOpen, label: "READING" },
    { id: "WRITING", icon: Edit, label: "WRITING" },
    { id: "SPEAKING", icon: Mic, label: "SPEAKING" }
  ];

  const testHistory = [
    { date: "11/07/2025", time: "23:25:30", name: "IELTS Mock Test 2025 January_Listening Practice Test 1", type: "Academic", score: 4, timeSpent: "32:00" },
    { date: "11/07/2025", time: "23:25:30", name: "IELTS Mock Test 2025 January_Listening Practice Test 1", type: "Academic", score: 4, timeSpent: "32:00" },
    { date: "11/07/2025", time: "23:25:30", name: "IELTS Mock Test 2025 January_Listening Practice Test 1", type: "Academic", score: 4, timeSpent: "32:00" },
    { date: "11/07/2025", time: "23:25:30", name: "IELTS Mock Test 2025 January_Listening Practice Test 1", type: "Academic", score: 4, timeSpent: "32:00" }
  ];

  const chartData = [
    { date: "09/09/2025", value: 4, overall: 7.0, grade: 20.0, usergrade: 10.0 },
    { date: "10/09/2025", value: 7, overall: 7.0, grade: 20.0, usergrade: 10.0 },
    { date: "11/09/2025", value: 8, overall: 7.0, grade: 20.0, usergrade: 10.0 },
    { date: "12/09/2025", value: 5, overall: 7.0, grade: 20.0, usergrade: 10.0 }
  ];

  return (
    <div className="min-h-screen">
      {/* Navbar */}
      <Navbar/>

      {/* Main Content */}
      <div className="w-full px-8 py-6 mt-12">
        <div className="grid grid-cols-1 lg:grid-cols-[320px_1fr] gap-6">
          
          {/* Left Sidebar - Profile */}
          <div className="bg-white/80 backdrop-blur-sm rounded-2xl shadow-lg p-6 h-fit sticky top-20 border border-gray-100">
            {/* Avatar */}
            <div className="flex flex-col items-center mb-6">
              <div className="relative">
                <div className="w-24 h-24 rounded-full overflow-hidden mb-3 border-4 border-gradient-to-r from-blue-400 to-indigo-500 shadow-xl ring-4 ring-blue-100">
                  <img 
                    src="https://i.pravatar.cc/150?img=12" 
                    alt="Avatar" 
                    className="w-full h-full object-cover"
                  />
                </div>
                <div className="absolute bottom-3 right-0 bg-green-500 w-5 h-5 rounded-full border-4 border-white"></div>
              </div>
              <h3 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent mb-1">{formData.firstName}</h3>
              <p className="text-xs text-gray-500 mb-2">{formData.email}</p>
            </div>

            {/* Form Fields */}
            <div className="space-y-3">
              <div>
                <label className="text-gray-700 text-sm font-semibold mb-2 block flex items-center gap-2">
                  <User className="w-4 h-4 text-blue-600" />
                  First name <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.firstName}
                  onChange={(e) => setFormData({...formData, firstName: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all bg-gray-50"
                />
              </div>

              <div>
                <label className="text-gray-700 text-sm font-semibold mb-2 block flex items-center gap-2">
                  <Calendar className="w-4 h-4 text-blue-600" />
                  Date of birth <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  placeholder="dd/mm/yyyy"
                  value={formData.dateOfBirth}
                  onChange={(e) => setFormData({...formData, dateOfBirth: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all bg-gray-50"
                />
              </div>

              <div>
                <label className="text-gray-700 text-sm font-semibold mb-2 block flex items-center gap-2">
                  <Mail className="w-4 h-4 text-blue-600" />
                  Email <span className="text-red-500">*</span>
                </label>
                <input
                  type="email"
                  value={formData.email}
                  onChange={(e) => setFormData({...formData, email: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all bg-gray-50"
                />
                <p className="text-xs text-red-500 mt-1 flex items-center gap-1">
                  <span className="w-1.5 h-1.5 bg-red-500 rounded-full"></span>
                  This email is not verified, click to verify
                </p>
              </div>

              <div>
                <label className="text-gray-700 text-sm font-semibold mb-2 block flex items-center gap-2">
                  <Lock className="w-4 h-4 text-blue-600" />
                  Password
                </label>
                <input
                  type="password"
                  placeholder="Change Password"
                  value={formData.password}
                  onChange={(e) => setFormData({...formData, password: e.target.value})}
                  className="w-full px-4 py-2.5 border border-gray-300 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all bg-gray-50"
                />
              </div>

              <button className="w-full bg-gradient-to-r from-blue-600 to-indigo-600 text-white py-2.5 rounded-xl font-semibold hover:from-blue-700 hover:to-indigo-700 transition-all shadow-lg shadow-blue-500/30 hover:shadow-xl hover:shadow-blue-500/40 transform hover:-translate-y-0.5">
                Save Changes
              </button>
            </div>
          </div>

          {/* Right Content - Stats & Chart */}
          <div className="space-y-6">
            
            {/* Stats Cards */}
            <div className="grid grid-cols-1 md:grid-cols-5 gap-4">
              {stats.map((stat, idx) => (
                <div key={idx} className="bg-white/80 backdrop-blur-sm rounded-xl shadow-lg p-4 border border-gray-100 hover:shadow-xl transition-all transform hover:-translate-y-1">
                  <div className={`w-10 h-10 ${stat.bgColor} rounded-lg flex items-center justify-center mb-2`}>
                    <stat.icon className={`w-5 h-5 ${stat.color}`} />
                  </div>
                  <div className="text-xs text-gray-600 font-medium mb-1">{stat.label}</div>
                  <div className={`text-2xl font-bold ${stat.color}`}>{stat.value}</div>
                </div>
              ))}
            </div>

            {/* Chart Section */}
            <div className="bg-white/80 backdrop-blur-sm rounded-2xl shadow-lg p-6 border border-gray-100">
              {/* Tabs */}
              <div className="flex gap-2 mb-6 flex-wrap">
                {tabs.map((tab) => (
                  <button
                    key={tab.id}
                    onClick={() => setActiveTab(tab.id)}
                    className={`flex items-center gap-2 px-4 py-2 rounded-lg font-semibold text-sm transition-all ${
                      activeTab === tab.id
                        ? "bg-gradient-to-r from-blue-600 to-indigo-600 text-white shadow-lg shadow-blue-500/30"
                        : "bg-gray-100 text-gray-600 hover:bg-gray-200"
                    }`}
                  >
                    <tab.icon className="w-4 h-4" />
                    {tab.label}
                  </button>
                ))}
              </div>

              {/* Chart */}
              <div className="relative h-80 bg-gradient-to-br from-blue-50 to-indigo-50 rounded-xl p-4">
                {/* Y-axis labels */}
                <div className="absolute left-6 top-6 bottom-14 flex flex-col justify-between text-sm text-gray-600 font-medium">
                  {[9, 8, 7, 6, 5, 4, 3, 2, 1, 0].map(n => (
                    <div key={n}>{n}</div>
                  ))}
                </div>

                {/* Chart area */}
                <div className="ml-12 h-full border-l-2 border-b-2 border-gray-300 relative rounded-bl-lg">
                  {/* Grid lines */}
                  <div className="absolute inset-0 flex flex-col justify-between">
                    {Array(10).fill(0).map((_, i) => (
                      <div key={i} className="border-t border-gray-200"></div>
                    ))}
                  </div>

                  {/* Bars */}
                  <div className="absolute inset-0 flex items-end justify-around px-8 pb-10">
                    {chartData.map((item, idx) => (
                      <div key={idx} className="flex flex-col items-center gap-3 flex-1 max-w-[100px] group">
                        {/* Tooltip */}
                        <div className="bg-white border-2 border-blue-200 rounded-xl p-3 text-xs mb-2 shadow-lg opacity-0 group-hover:opacity-100 transition-opacity">
                          <div className="font-semibold text-blue-700">Overall: {item.overall}</div>
                          <div className="text-gray-600">Grade: {item.grade}</div>
                          <div className="text-gray-600">Your Grade: {item.usergrade}</div>
                        </div>
                        
                        {/* Bar */}
                        <div 
                          className={`w-full rounded-t-xl transition-all duration-300 group-hover:scale-105 ${
                            idx === 2 
                              ? 'bg-gradient-to-t from-blue-600 to-indigo-600 shadow-lg shadow-blue-500/50' 
                              : 'bg-gradient-to-t from-blue-300 to-blue-400'
                          }`}
                          style={{ height: `${(item.value / 9) * 100}%` }}
                        ></div>
                        
                        {/* Date label */}
                        <div className="text-xs text-gray-700 font-medium whitespace-nowrap">{item.date}</div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>

            {/* Test History */}
            <div className="bg-white/80 backdrop-blur-sm rounded-2xl shadow-lg p-6 border border-gray-100">
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent">My Practice Test History</h2>
                <button className="text-blue-600 text-sm font-semibold hover:text-blue-700 flex items-center gap-2 px-3 py-1.5 rounded-lg hover:bg-blue-50 transition-all">
                  View details →
                </button>
              </div>

              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead>
                    <tr className="border-b-2 border-gray-200">
                      <th className="text-left py-3 px-3 text-sm font-bold text-gray-700">Date</th>
                      <th className="text-left py-3 px-3 text-sm font-bold text-gray-700">Test name</th>
                      <th className="text-left py-3 px-3 text-sm font-bold text-gray-700">Type</th>
                      <th className="text-left py-3 px-3 text-sm font-bold text-gray-700">Score</th>
                      <th className="text-left py-3 px-3 text-sm font-bold text-gray-700">Time Spent</th>
                    </tr>
                  </thead>
                  <tbody>
                    {testHistory.map((test, idx) => (
                      <tr key={idx} className="border-b border-gray-100 hover:bg-blue-50/50 transition-colors">
                        <td className="py-3 px-3">
                          <div className="text-sm font-medium text-gray-800">{test.date}</div>
                          <div className="text-xs text-gray-500">{test.time}</div>
                        </td>
                        <td className="py-3 px-3 text-sm text-gray-700 font-medium">{test.name}</td>
                        <td className="py-3 px-3">
                          <span className="px-2.5 py-0.5 bg-purple-100 text-purple-700 text-xs font-semibold rounded-full">
                            {test.type}
                          </span>
                        </td>
                        <td className="py-3 px-3">
                          <span className="text-xl font-bold text-blue-600">{test.score}</span>
                        </td>
                        <td className="py-3 px-3">
                          <div className="flex items-center gap-2">
                            <span className="text-sm text-gray-700 font-medium">{test.timeSpent}</span>
                            <button className="flex items-center gap-1 text-xs font-semibold text-blue-600 border-2 border-blue-600 rounded-lg px-2.5 py-1 hover:bg-blue-600 hover:text-white transition-all">
                              <FileText className="w-3 h-3" />
                              Review
                            </button>
                          </div>
                        </td>
                      </tr>
                    ))}
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