// 1. "Bỏ" (comment lại) code của react-router-dom
// import {
//   createBrowserRouter,
//   RouterProvider,
// } from "react-router-dom";

// 2. Bỏ import trang Home
// import HomePage from "../pages/Home/HomePage";

// 3. Chỉ import trang bạn muốn xem
import SignInPage from "../pages/SignIn/SignInPage"; // (Đảm bảo đường dẫn này đúng)
import SignUpPage from "../pages/SignUp/SignUpPage"; // (Đảm bảo đường dẫn này đúng)

export default function App() {
  // 4. Hiển thị trực tiếp trang đó
  return <SignUpPage />;
}