import React, { useEffect, useState } from 'react';
import { MoreHorizontal, Search, UserCheck, UserX } from 'lucide-react';

export default function UserManagement() {
    // Modal state
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState<{ show: boolean, id: number | null, fullName: string, email: string }>( { show: false, id: null, fullName: '', email: '' });
    const [showRolesModal, setShowRolesModal] = useState<{ show: boolean, id: number | null, roles: string[] }>( { show: false, id: null, roles: [] });
    const [newUser, setNewUser] = useState({ fullName: '', email: '', password: '' });
    const [editUser, setEditUser] = useState({ fullName: '', email: '' });
    const [roleInput, setRoleInput] = useState('');
    // Create user
    const handleCreateUser = async (e?: React.FormEvent) => {
      if (e) e.preventDefault();
      try {
        const token = localStorage.getItem("Token");
        const API_URL = "http://localhost:8081/api/v1/admin/users";
        const body = {
          FullName: newUser.fullName,
          Email: newUser.email,
          Password: newUser.password
        };
        const res = await fetch(API_URL, {
          method: "POST",
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error("Create user failed");
        await fetchUsers();
        setShowCreateModal(false);
        setNewUser({ fullName: '', email: '', password: '' });
        alert("User created successfully!");
      } catch (err) {
        alert("Failed to create user.");
        console.error(err);
      }
    };

    // Edit user
    const openEditModal = (user: any) => {
      setShowEditModal({ show: true, id: user.Id, fullName: user.FullName, email: user.Email });
      setEditUser({ fullName: user.FullName, email: user.Email });
    };
    const handleEditUser = async (e: React.FormEvent) => {
      e.preventDefault();
      try {
        const token = localStorage.getItem("Token");
        const API_URL = `http://localhost:8081/api/v1/admin/users/${showEditModal.id}`;
        const body = {
          FullName: editUser.fullName,
          Email: editUser.email
        };
        const res = await fetch(API_URL, {
          method: "PATCH",
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error("Edit user failed");
        await fetchUsers();
        setShowEditModal({ show: false, id: null, fullName: '', email: '' });
        alert("User updated successfully!");
      } catch (err) {
        alert("Failed to update user.");
        console.error(err);
      }
    };

    // Set roles
    const openRolesModal = (user: any) => {
      setShowRolesModal({ show: true, id: user.Id, roles: user.Roles || [] });
      setRoleInput('');
    };
    const handleSetRoles = async (e: React.FormEvent) => {
      e.preventDefault();
      try {
        const token = localStorage.getItem("Token");
        const API_URL = `http://localhost:8081/api/v1/admin/users/${showRolesModal.id}/roles`;
        const body = { Roles: showRolesModal.roles };
        const res = await fetch(API_URL, {
          method: "POST",
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(body)
        });
        if (!res.ok) throw new Error("Set roles failed");
        await fetchUsers();
        setShowRolesModal({ show: false, id: null, roles: [] });
        alert("Roles updated successfully!");
      } catch (err) {
        alert("Failed to update roles.");
        console.error(err);
      }
    };

    // Remove role
    const handleRemoveRole = async (role: string) => {
      try {
        const token = localStorage.getItem("Token");
        const API_URL = `http://localhost:8081/api/v1/admin/users/${showRolesModal.id}/roles/${role}`;
        const res = await fetch(API_URL, {
          method: "DELETE",
          headers: {
            'Authorization': `Bearer ${token}`,
          }
        });
        if (!res.ok) throw new Error("Remove role failed");
        // Remove role from local state
        setShowRolesModal(m => ({ ...m, roles: m.roles.filter(r => r !== role) }));
        await fetchUsers();
        alert("Role removed successfully!");
      } catch (err) {
        alert("Failed to remove role.");
        console.error(err);
      }
    };
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
        <div className="flex gap-3 items-center">
          <div className="relative">
              <input 
                 type="text" 
                 placeholder="Search by email..." 
                 className="pl-10 pr-4 py-2 border border-slate-300 rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
                 onChange={(e) => setSearch(e.target.value)}
              />
              <Search className="w-4 h-4 text-slate-400 absolute left-3 top-3" />
          </div>
          <button
            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium"
            onClick={() => setShowCreateModal(true)}
          >+ Create User</button>
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
                  <div className="flex gap-2 justify-end">
                    <button className="p-2 hover:bg-slate-100 rounded-full" title="Edit" onClick={() => openEditModal(user)}>
                      <MoreHorizontal className="w-5 h-5 text-slate-500" />
                    </button>
                    <button className="p-2 hover:bg-blue-50 rounded-full" title="Roles" onClick={() => openRolesModal(user)}>
                      <span className="text-xs text-blue-600 font-bold">Roles</span>
                    </button>
                  </div>
                </td>
                    {/* Create User Modal */}
                    {showCreateModal && (
                      <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                        <form
                          className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                          onSubmit={handleCreateUser}
                        >
                          <button
                            type="button"
                            className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                            onClick={() => setShowCreateModal(false)}
                          >×</button>
                          <h2 className="text-xl font-bold mb-6 text-slate-800">Create User</h2>
                          <div className="mb-4">
                            <label className="block text-sm font-medium mb-1">Full Name</label>
                            <input
                              type="text"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              value={newUser.fullName}
                              onChange={e => setNewUser(u => ({ ...u, fullName: e.target.value }))}
                              required
                            />
                          </div>
                          <div className="mb-4">
                            <label className="block text-sm font-medium mb-1">Email</label>
                            <input
                              type="email"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              value={newUser.email}
                              onChange={e => setNewUser(u => ({ ...u, email: e.target.value }))}
                              required
                            />
                          </div>
                          <div className="mb-6">
                            <label className="block text-sm font-medium mb-1">Password</label>
                            <input
                              type="password"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              value={newUser.password}
                              onChange={e => setNewUser(u => ({ ...u, password: e.target.value }))}
                              required
                            />
                          </div>
                          <button
                            type="submit"
                            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                          >Create</button>
                        </form>
                      </div>
                    )}

                    {/* Edit User Modal */}
                    {showEditModal.show && (
                      <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                        <form
                          className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                          onSubmit={handleEditUser}
                        >
                          <button
                            type="button"
                            className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                            onClick={() => setShowEditModal({ show: false, id: null, fullName: '', email: '' })}
                          >×</button>
                          <h2 className="text-xl font-bold mb-6 text-slate-800">Edit User</h2>
                          <div className="mb-4">
                            <label className="block text-sm font-medium mb-1">Full Name</label>
                            <input
                              type="text"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              value={editUser.fullName}
                              onChange={e => setEditUser(u => ({ ...u, fullName: e.target.value }))}
                              required
                            />
                          </div>
                          <div className="mb-6">
                            <label className="block text-sm font-medium mb-1">Email</label>
                            <input
                              type="email"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              value={editUser.email}
                              onChange={e => setEditUser(u => ({ ...u, email: e.target.value }))}
                              required
                            />
                          </div>
                          <button
                            type="submit"
                            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                          >Save</button>
                        </form>
                      </div>
                    )}

                    {/* Roles Modal */}
                    {showRolesModal.show && (
                      <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                        <form
                          className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                          onSubmit={handleSetRoles}
                        >
                          <button
                            type="button"
                            className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                            onClick={() => setShowRolesModal({ show: false, id: null, roles: [] })}
                          >×</button>
                          <h2 className="text-xl font-bold mb-6 text-slate-800">Set Roles</h2>
                          <div className="mb-4">
                            <label className="block text-sm font-medium mb-1">Roles</label>
                            <div className="flex flex-wrap gap-2 mb-2">
                              {showRolesModal.roles.map(r => (
                                <span key={r} className="px-2 py-1 bg-blue-100 text-blue-700 text-xs rounded-md flex items-center gap-1">
                                  {r}
                                  <button type="button" className="ml-1 text-red-500" onClick={() => handleRemoveRole(r)}>×</button>
                                </span>
                              ))}
                            </div>
                            <input
                              type="text"
                              className="w-full border border-slate-300 rounded-lg px-3 py-2"
                              placeholder="Add role (e.g. ADMIN, TEACHER, STUDENT)"
                              value={roleInput}
                              onChange={e => setRoleInput(e.target.value)}
                              onKeyDown={e => {
                                if (e.key === 'Enter' && roleInput.trim()) {
                                  e.preventDefault();
                                  if (!showRolesModal.roles.includes(roleInput.trim())) {
                                    setShowRolesModal(m => ({ ...m, roles: [...m.roles, roleInput.trim()] }));
                                    setRoleInput('');
                                  }
                                }
                              }}
                            />
                          </div>
                          <button
                            type="submit"
                            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                          >Save Roles</button>
                        </form>
                      </div>
                    )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}