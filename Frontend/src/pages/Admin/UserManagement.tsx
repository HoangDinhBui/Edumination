import React, { useEffect, useState } from 'react';
import { MoreHorizontal, Search, UserCheck, UserX } from 'lucide-react';

export default function UserManagement() {
  const [users, setUsers] = useState<any[]>([]);
  const [search, setSearch] = useState('');

  const fetchUsers = async () => {
    try {
      const token = localStorage.getItem("Token");
      
      // Gọi API
      const res = await fetch(`http://localhost:8081/api/v1/admin/users?email=${search}`, {
        headers: { Authorization: `Bearer ${token}` }
      });

      if (!res.ok) {
        throw new Error(`Lỗi HTTP: ${res.status}`);
      }

      const rawData = await res.json();
      let userList: any[] = [];

      // === BƯỚC 1: TRÍCH XUẤT MẢNG DỮ LIỆU (Tránh lỗi map is not a function) ===
      if (Array.isArray(rawData)) {
        // Trường hợp 1: API trả thẳng mảng [user1, user2]
        userList = rawData;
      } 
      else if (rawData.Items && Array.isArray(rawData.Items)) {
        // Trường hợp 2: API trả về { Items: [...] } (Thường gặp ở .NET/Go)
        userList = rawData.Items;
      } 
      else if (rawData.items && Array.isArray(rawData.items)) {
        // Trường hợp 3: API trả về { items: [...] } (Thường gặp ở JS/Node)
        userList = rawData.items;
      } 
      else if (rawData.data && Array.isArray(rawData.data)) {
        // Trường hợp 4: API trả về { data: [...] }
        userList = rawData.data;
      }

      // === BƯỚC 2: SẮP XẾP ID TĂNG DẦN (1 -> 999) ===
      // Lưu ý: Id trong data của bạn viết hoa là 'Id'
      userList.sort((a: any, b: any) => {
         // Nếu Id là số
         return a.Id - b.Id; 
         
         // *Nếu Id là chuỗi (VD: "User01", "User02") thì dùng dòng dưới:
         // return String(a.Id).localeCompare(String(b.Id));
      });

      // === BƯỚC 3: LƯU VÀO STATE ===
      console.log("Danh sách User sau khi sort:", userList);
      setUsers(userList);

    } catch (error) {
      console.error("Lỗi khi tải danh sách user:", error);
      setUsers([]); // Set rỗng để không crash trang
    }
  };

  useEffect(() => { fetchUsers(); }, [search]);

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-slate-800">Users Management</h1>
        <div className="relative">
            <input 
               type="text" 
               placeholder="Search by email..." 
               className="pl-10 pr-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
               onChange={(e) => setSearch(e.target.value)}
            />
            <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3" />
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead className="bg-slate-50 text-slate-600 text-xs uppercase font-semibold">
            <tr>
              <th className="p-4">ID</th>
              <th className="p-4">Full Name</th>
              <th className="p-4">Email</th>
              <th className="p-4">Roles</th>
              <th className="p-4">Status</th>
              <th className="p-4 text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="text-sm text-slate-700 divide-y divide-slate-100">
            {users.map((user) => (
              <tr key={user.Id} className="hover:bg-slate-50 transition">
                <td className="p-4 font-mono text-slate-500">#{user.Id}</td>
                <td className="p-4 font-medium">{user.FullName}</td>
                <td className="p-4">{user.Email}</td>
                <td className="p-4">
                    {user.Roles?.map((r:string) => (
                        <span key={r} className="px-2 py-1 bg-blue-100 text-blue-700 text-xs rounded-md mr-1">{r}</span>
                    ))}
                </td>
                <td className="p-4">
                    {user.IsActive ? 
                        <span className="flex items-center text-green-600 gap-1"><UserCheck className="w-4 h-4"/> Active</span> : 
                        <span className="flex items-center text-red-500 gap-1"><UserX className="w-4 h-4"/> Banned</span>
                    }
                </td>
                <td className="p-4 text-right">
                  <button className="p-2 hover:bg-slate-100 rounded-full">
                    <MoreHorizontal className="w-5 h-5 text-slate-500" />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}