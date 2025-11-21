import React, { useState, useEffect } from 'react';
import { 
  BookOpen, 
  Plus, 
  Search, 
  MoreHorizontal, 
  Edit, 
  Trash2, 
  Users, 
  PlayCircle,
  RefreshCcw
} from 'lucide-react';

export default function CourseManagement() {
  // --- State quản lý dữ liệu ---
  const [courses, setCourses] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchTerm, setSearchTerm] = useState("");

  // --- Hàm gọi API ---
  const fetchCourses = async () => {
    setLoading(true);
    setError("");
    try {
      const token = localStorage.getItem("Token");
      const API_URL = "http://localhost:8081/api/v1/courses"; 

      const res = await fetch(API_URL, {
        headers: { 
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
      });

      if (!res.ok) {
        throw new Error(`Lỗi tải dữ liệu: ${res.statusText}`);
      }

      const data = await res.json();
      
      // 1. Log ra xem thực tế Backend trả về cái gì (Quan trọng để debug)
      console.log("Dữ liệu thô từ API Courses:", data);

      // 2. Xử lý các dạng bao bọc dữ liệu (Wrapper)
      let rawList = [];
      if (Array.isArray(data)) {
          rawList = data;
      } else if (data.items && Array.isArray(data.items)) {
          rawList = data.items;
      } else if (data.Items && Array.isArray(data.Items)) { // Trường hợp C# trả về Items hoa
          rawList = data.Items; 
      } else if (data.data && Array.isArray(data.data)) {
          rawList = data.data;
      }

      // 3. MAP DỮ LIỆU AN TOÀN (Chấp nhận cả Chữ Hoa và Chữ Thường)
      const formattedCourses = rawList.map((c: any) => ({
        // Lấy Id hoặc ID hoặc id
        id: c.id || c.Id || c.ID, 
        
        // Lấy Title hoặc Name hoặc title...
        title: c.title || c.Title || c.name || c.Name || "Khóa học không tên",
        
        // Giảng viên
        instructor: c.instructor_name || c.InstructorName || "Admin",
        
        // Giá tiền
        price: (c.price !== undefined) ? c.price : (c.Price !== undefined ? c.Price : 0),
        
        // Số liệu thống kê
        students: c.enrollment_count || c.EnrollmentCount || 0, 
        modules: c.modules_count || c.ModulesCount || 0,
        
        // Trạng thái
        status: (c.is_published || c.IsPublished) ? "Published" : "Draft",
        
        // Ảnh
        thumbnail: c.thumbnail_url || c.ThumbnailUrl || "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?ixlib=rb-1.2.1&auto=format&fit=crop&w=200&q=80"
      }));

      console.log("Dữ liệu sau khi map:", formattedCourses);
      setCourses(formattedCourses);

    } catch (err: any) {
      console.error("Error fetching courses:", err);
      setError("Không thể kết nối đến máy chủ.");
    } finally {
      setLoading(false);
    }
  };

  // Gọi API khi component mount
  useEffect(() => {
    fetchCourses();
  }, []);

  // --- Helper Functions ---
  const formatCurrency = (amount: number) => {
    if (amount === 0) return "Miễn phí";
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
  };

  // Lọc khóa học theo từ khóa tìm kiếm
  const filteredCourses = courses.filter(course => 
    course.title.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div>
      {/* --- HEADER --- */}
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Quản Lý Khóa Học</h1>
          <p className="text-slate-500 text-sm">Tạo và quản lý nội dung học tập.</p>
        </div>
        <button className="bg-blue-600 text-white px-4 py-2.5 rounded-lg hover:bg-blue-700 transition flex items-center gap-2 font-medium shadow-sm">
           <Plus size={20} />
           Tạo Khóa Mới
        </button>
      </div>

      {/* --- TOOLBAR --- */}
      <div className="bg-white p-4 rounded-xl shadow-sm border border-slate-200 mb-6 flex flex-col md:flex-row justify-between items-center gap-4">
        <div className="relative w-full md:w-96">
           <input 
              type="text" 
              placeholder="Tìm kiếm theo tên khóa học..." 
              className="w-full pl-10 pr-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none text-sm"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
           />
           <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3" />
        </div>
        <div className="flex gap-3 w-full md:w-auto">
           <button 
             onClick={fetchCourses} 
             className="p-2 border border-slate-300 rounded-lg hover:bg-slate-50 text-slate-600"
             title="Tải lại dữ liệu"
           >
             <RefreshCcw size={18} />
           </button>
           <select className="border border-slate-300 rounded-lg px-3 py-2 text-sm outline-none focus:border-blue-500">
              <option>Tất cả trạng thái</option>
              <option>Published</option>
              <option>Draft</option>
           </select>
        </div>
      </div>

      {/* --- CONTENT AREA --- */}
      {loading ? (
        // Loading Skeleton
        <div className="space-y-4">
          {[1, 2, 3].map((i) => (
            <div key={i} className="bg-white p-5 rounded-xl border border-slate-100 h-32 animate-pulse"></div>
          ))}
        </div>
      ) : error ? (
        // Error State
        <div className="text-center py-10 bg-red-50 rounded-xl border border-red-100">
          <p className="text-red-600 mb-2">{error}</p>
          <button onClick={fetchCourses} className="text-sm font-medium underline text-red-700 hover:text-red-800">
            Thử lại
          </button>
        </div>
      ) : filteredCourses.length === 0 ? (
        // Empty State
        <div className="text-center py-16 bg-slate-50 rounded-xl border border-dashed border-slate-300">
          <BookOpen size={48} className="mx-auto text-slate-300 mb-3" />
          <p className="text-slate-500 font-medium">Không tìm thấy khóa học nào.</p>
          <p className="text-slate-400 text-sm">Hãy thử tạo khóa học mới hoặc thay đổi bộ lọc.</p>
        </div>
      ) : (
        // Course List
        <div className="grid grid-cols-1 gap-4">
           {filteredCourses.map((course) => (
              <div key={course.id} className="bg-white p-5 rounded-xl border border-slate-200 hover:shadow-md transition flex flex-col md:flex-row items-start md:items-center gap-6">
                 
                 {/* Thumbnail */}
                 <div className="w-full md:w-32 h-48 md:h-24 flex-shrink-0 rounded-lg overflow-hidden bg-slate-100">
                    <img src={course.thumbnail} alt={course.title} className="w-full h-full object-cover" />
                 </div>

                 {/* Info */}
                 <div className="flex-1 min-w-0 w-full">
                    <div className="flex justify-between items-start">
                       <div>
                          <h3 className="font-bold text-lg text-slate-800 truncate pr-4">{course.title}</h3>
                          <p className="text-sm text-slate-500 mt-1">Giảng viên: <span className="text-slate-700 font-medium">{course.instructor}</span></p>
                       </div>
                       <span className={`px-3 py-1 rounded-full text-xs font-bold flex-shrink-0 ${
                          course.status === 'Published' 
                          ? 'bg-green-100 text-green-700' 
                          : 'bg-yellow-100 text-yellow-700'
                       }`}>
                          {course.status}
                       </span>
                    </div>
                    
                    <div className="flex flex-wrap items-center gap-4 md:gap-6 mt-4 text-sm text-slate-500">
                       <div className="flex items-center gap-1.5">
                          <Users size={16} className="text-blue-500"/>
                          <span>{course.students} học viên</span>
                       </div>
                       <div className="flex items-center gap-1.5">
                          <PlayCircle size={16} className="text-purple-500"/>
                          <span>{course.modules} chương</span>
                       </div>
                       <div className="flex items-center gap-1.5">
                          <span className="font-bold text-slate-800 text-base">{formatCurrency(course.price)}</span>
                       </div>
                    </div>
                 </div>

                 {/* Actions */}
                 <div className="flex items-center gap-2 w-full md:w-auto border-t md:border-t-0 md:border-l border-slate-100 pt-4 md:pt-0 md:pl-6 mt-4 md:mt-0 justify-end">
                    <button className="p-2 text-slate-500 hover:bg-blue-50 hover:text-blue-600 rounded-lg transition" title="Chỉnh sửa">
                       <Edit size={18} />
                    </button>
                    <button className="p-2 text-slate-500 hover:bg-red-50 hover:text-red-600 rounded-lg transition" title="Xóa">
                       <Trash2 size={18} />
                    </button>
                    <button className="p-2 text-slate-500 hover:bg-slate-100 rounded-lg transition">
                       <MoreHorizontal size={18} />
                    </button>
                 </div>
              </div>
           ))}
        </div>
      )}

      {/* Pagination (UI tĩnh - chưa có logic API) */}
      {!loading && courses.length > 0 && (
        <div className="flex justify-center mt-8">
           <div className="flex gap-2">
              <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm disabled:opacity-50" disabled>Trước</button>
              <button className="px-3 py-1 bg-blue-600 text-white rounded text-sm">1</button>
              <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm">2</button>
              <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm">Sau</button>
           </div>
        </div>
      )}
    </div>
  );
}