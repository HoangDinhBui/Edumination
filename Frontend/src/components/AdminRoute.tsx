import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { jwtDecode } from "jwt-decode"; // Import thư viện

const AdminRoute = () => {
  const token = localStorage.getItem("Token");
  let isAdmin = false;

  if (token) {
    try {
      // Giải mã token để lấy dữ liệu thực (như trong ảnh bạn gửi)
      const decodedToken = jwtDecode(token);
      
      // Log ra xem nó có gì (Xem trong F12 -> Console)
      console.log("Decoded Token:", decodedToken);

      // Kiểm tra role (Lưu ý: trong ảnh key là "role" viết thường)
      if (decodedToken.role === "ADMIN") {
        isAdmin = true;
      }
    } catch (error) {
      console.error("Token không hợp lệ:", error);
      // Nếu token lỗi thì coi như không phải admin
      isAdmin = false;
    }
  }

  // Điều hướng
  if (!isAdmin) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
};

export default AdminRoute;