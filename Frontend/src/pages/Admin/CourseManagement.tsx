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
    // State for edit modal
    const [editModal, setEditModal] = useState<{ show: boolean, courseId: string | number | null, title: string, isPublished: boolean }>({ show: false, courseId: null, title: "", isPublished: false });

    // Open edit modal with course data
    const openEditModal = (course: any) => {
      setEditModal({
        show: true,
        courseId: course.id,
        title: course.title,
        isPublished: course.status === "Published"
      });
    };

    // Call API to update course
    const handleEditCourse = async (e: React.FormEvent) => {
      e.preventDefault();
      try {
        const token = localStorage.getItem("Token");
        const API_URL = `http://localhost:8081/api/v1/courses/${editModal.courseId}`;
        const body = {
          title: editModal.title,
          isPublished: editModal.isPublished
        };
        const res = await fetch(API_URL, {
          method: "PATCH",
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(body)
        });
        if (!res.ok) {
          throw new Error(`Edit failed: ${res.statusText}`);
        }
        await fetchCourses();
        setEditModal({ show: false, courseId: null, title: "", isPublished: false });
        alert("Course updated successfully!");
      } catch (err) {
        alert("Failed to update course.");
        console.error(err);
      }
    };
  // --- State management ---
  const [courses, setCourses] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState("All");

  // --- API call function ---
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
      
      // 1. Log raw data from API (important for debugging)
      console.log("Raw data from API Courses:", data);

      // 2. Handle data wrappers
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

      // 3. SAFE DATA MAP (Accept both uppercase and lowercase)
      const formattedCourses = rawList.map((c: any) => ({
        // Get Id or ID or id
        id: c.id || c.Id || c.ID, 
        
        // Get Title or Name or title...
        title: c.title || c.Title || c.name || c.Name || "Untitled Course",
        
        // Instructor
        instructor: c.instructor_name || c.InstructorName || "Admin",
        
        // Price
        price: (c.price !== undefined) ? c.price : (c.Price !== undefined ? c.Price : 0),
        
        // Statistics
        students: c.enrollment_count || c.EnrollmentCount || 0, 
        modules: c.modules_count || c.ModulesCount || 0,
        
        // Status
        status: (c.is_published || c.IsPublished) ? "Published" : "Draft",
        
        // Image
        thumbnail: c.thumbnail_url || c.ThumbnailUrl || "https://images.unsplash.com/photo-1497633762265-9d179a990aa6?ixlib=rb-1.2.1&auto=format&fit=crop&w=200&q=80"
      }));

      console.log("Mapped data:", formattedCourses);
      setCourses(formattedCourses);

    } catch (err: any) {
      console.error("Error fetching courses:", err);
      setError("Cannot connect to server.");
    } finally {
      setLoading(false);
    }
  };

  // Call API when component mounts
  useEffect(() => {
    fetchCourses();
  }, []);

  // --- Helper Functions ---
  const formatCurrency = (amount: number) => {
    if (amount === 0) return "Miễn phí";
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
  };

  // Filter courses by search keyword and status
  const filteredCourses = courses.filter(course => {
    const matchTitle = course.title.toLowerCase().includes(searchTerm.toLowerCase());
    const matchStatus =
      statusFilter === "All" ||
      (statusFilter === "Published" && course.status === "Published") ||
      (statusFilter === "Draft" && course.status === "Draft");
    return matchTitle && matchStatus;
  });

    // State for modal and new course
    const [showModal, setShowModal] = useState(false);
    const [newCourse, setNewCourse] = useState({
      title: "",
      description: "",
      level: "INTERMEDIATE",
      isPublished: false
    });

    // Create new course API call
    const handleCreateCourse = async (e?: React.FormEvent) => {
      if (e) e.preventDefault();
      try {
        const token = localStorage.getItem("Token");
        const API_URL = "http://localhost:8081/api/v1/courses";
        const res = await fetch(API_URL, {
          method: "POST",
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(newCourse)
        });
        if (!res.ok) {
          throw new Error(`Create failed: ${res.statusText}`);
        }
        await fetchCourses(); // Refresh list
        setShowModal(false);
        setNewCourse({ title: "", description: "", level: "INTERMEDIATE", isPublished: false });
        alert("Course created successfully!");
      } catch (err) {
        alert("Failed to create course.");
        console.error(err);
      }
    };

    return (
      <div>
        {/* --- HEADER --- */}
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-2xl font-bold text-slate-800">Course Management</h1>
            <p className="text-slate-500 text-sm">Create and manage learning content.</p>
          </div>
          <button
            className="bg-blue-600 text-white px-4 py-2.5 rounded-lg hover:bg-blue-700 transition flex items-center gap-2 font-medium shadow-sm"
            onClick={() => setShowModal(true)}
          >
             <Plus size={20} />
             Create New Course
          </button>
              {/* Modal for new course */}
              {showModal && (
                <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                  <form
                    className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                    onSubmit={handleCreateCourse}
                  >
                    <button
                      type="button"
                      className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                      onClick={() => setShowModal(false)}
                    >×</button>
                    <h2 className="text-xl font-bold mb-6 text-slate-800">Create New Course</h2>
                    <div className="mb-4">
                      <label className="block text-sm font-medium mb-1">Title</label>
                      <input
                        type="text"
                        className="w-full border border-slate-300 rounded-lg px-3 py-2"
                        value={newCourse.title}
                        onChange={e => setNewCourse(c => ({ ...c, title: e.target.value }))}
                        required
                      />
                    </div>
                    <div className="mb-4">
                      <label className="block text-sm font-medium mb-1">Description</label>
                      <textarea
                        className="w-full border border-slate-300 rounded-lg px-3 py-2"
                        value={newCourse.description}
                        onChange={e => setNewCourse(c => ({ ...c, description: e.target.value }))}
                        required
                      />
                    </div>
                    <div className="mb-4">
                      <label className="block text-sm font-medium mb-1">Level</label>
                      <select
                        className="w-full border border-slate-300 rounded-lg px-3 py-2"
                        value={newCourse.level}
                        onChange={e => setNewCourse(c => ({ ...c, level: e.target.value }))}
                      >
                        <option value="BEGINNER">BEGINNER</option>
                        <option value="INTERMEDIATE">INTERMEDIATE</option>
                        <option value="ADVANCED">ADVANCED</option>
                      </select>
                    </div>
                    <div className="mb-6 flex items-center gap-2">
                      <input
                        type="checkbox"
                        id="isPublished"
                        checked={newCourse.isPublished}
                        onChange={e => setNewCourse(c => ({ ...c, isPublished: e.target.checked }))}
                      />
                      <label htmlFor="isPublished" className="text-sm">Published</label>
                    </div>
                    <button
                      type="submit"
                      className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                    >Create</button>
                  </form>
                </div>
              )}
        </div>

      {/* --- TOOLBAR --- */}
      <div className="bg-white p-4 rounded-xl shadow-sm border border-slate-200 mb-6 flex flex-col md:flex-row justify-between items-center gap-4">
        <div className="relative w-full md:w-96">
          <input 
            type="text" 
            placeholder="Search by course name..." 
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
           title="Reload data"
          >
           <RefreshCcw size={18} />
          </button>
            <select
             className="border border-slate-300 rounded-lg px-3 py-2 text-sm outline-none focus:border-blue-500"
             value={statusFilter}
             onChange={e => setStatusFilter(e.target.value)}
            >
              <option value="All">All status</option>
              <option value="Published">Published</option>
              <option value="Draft">Draft</option>
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
          Try again
         </button>
        </div>
      ) : filteredCourses.length === 0 ? (
        // Empty State
        <div className="text-center py-16 bg-slate-50 rounded-xl border border-dashed border-slate-300">
         <BookOpen size={48} className="mx-auto text-slate-300 mb-3" />
         <p className="text-slate-500 font-medium">No courses found.</p>
         <p className="text-slate-400 text-sm">Try creating a new course or changing the filter.</p>
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
                    <p className="text-sm text-slate-500 mt-1">Instructor: <span className="text-slate-700 font-medium">{course.instructor}</span></p>
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
                    <span>{course.students} students</span>
                  </div>
                  <div className="flex items-center gap-1.5">
                    <PlayCircle size={16} className="text-purple-500"/>
                    <span>{course.modules} modules</span>
                  </div>
                  <div className="flex items-center gap-1.5">
                    <span className="font-bold text-slate-800 text-base">{formatCurrency(course.price)}</span>
                  </div>
                </div>
              </div>

              {/* Actions */}
              <div className="flex items-center gap-2 w-full md:w-auto border-t md:border-t-0 md:border-l border-slate-100 pt-4 md:pt-0 md:pl-6 mt-4 md:mt-0 justify-end">
                <button
                  className="p-2 text-slate-500 hover:bg-blue-50 hover:text-blue-600 rounded-lg transition"
                  title="Edit"
                  onClick={() => openEditModal(course)}
                >
                  <Edit size={18} />
                </button>
                      {/* Edit Modal */}
                      {editModal.show && (
                        <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                          <form
                            className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                            onSubmit={handleEditCourse}
                          >
                            <button
                              type="button"
                              className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                              onClick={() => setEditModal({ show: false, courseId: null, title: "", isPublished: false })}
                            >×</button>
                            <h2 className="text-xl font-bold mb-6 text-slate-800">Edit Course</h2>
                            <div className="mb-4">
                              <label className="block text-sm font-medium mb-1">Title</label>
                              <input
                                type="text"
                                className="w-full border border-slate-300 rounded-lg px-3 py-2"
                                value={editModal.title}
                                onChange={e => setEditModal(m => ({ ...m, title: e.target.value }))}
                                required
                              />
                            </div>
                            <div className="mb-6 flex items-center gap-2">
                              <input
                                type="checkbox"
                                id="editIsPublished"
                                checked={editModal.isPublished}
                                onChange={e => setEditModal(m => ({ ...m, isPublished: e.target.checked }))}
                              />
                              <label htmlFor="editIsPublished" className="text-sm">Published</label>
                            </div>
                            <button
                              type="submit"
                              className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                            >Save</button>
                          </form>
                        </div>
                      )}
                <button className="p-2 text-slate-500 hover:bg-red-50 hover:text-red-600 rounded-lg transition" title="Delete">
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

      {/* Pagination (static UI - no API logic yet) */}
      {!loading && courses.length > 0 && (
        <div className="flex justify-center mt-8">
          <div className="flex gap-2">
            <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm disabled:opacity-50" disabled>Previous</button>
            <button className="px-3 py-1 bg-blue-600 text-white rounded text-sm">1</button>
            <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm">2</button>
            <button className="px-3 py-1 border border-slate-200 rounded hover:bg-slate-50 text-sm">Next</button>
          </div>
        </div>
      )}
     </div>
    );
}