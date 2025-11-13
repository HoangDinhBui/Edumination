import React from "react";
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";

// 1. Import tất cả các trang của bạn
import HomePage from "../pages/Home/HomePage";
import SignInPage from "../pages/SignIn/SignInPage"; 
import SignUpPage from "../pages/SignUp/SignUpPage"; 
import Answer from "../pages/Answer/Answer";
import ExamLibrary from "../pages/ExamsLibrary/ExamsLibrary"; // Tên file trang thư viện của bạn
import QuarterDetailPage from "../pages/ExamsLibrary/QuarterDetailPage"; // Trang chi tiết bạn vừa tạo
import ForgotPassword from "../pages/ForgotPassword/ForgotPassword";
import EnterOtpPage from "../pages/ForgotPassword/EnterOtpPage";
import ResetPasswordPage from "../pages/ForgotPassword/ResetPasswordPage";
import ListeningTestPage from "../pages/Tests/ListeningTestPage";
import WrintingTestPage from "../pages/Tests/WritingTestPage";
// 2. Định nghĩa các đường dẫn (routes)
const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage />,
  },
  {
    path: "/signin",
    element: <SignInPage />,
  },
  {
    path: "/signup",
    element: <SignUpPage />,
  },
  {
    path: "/library", // Đường dẫn đến trang thư viện
    element: <ExamLibrary />,
  },
  {
    path: "/answer",
    element: <Answer />,
  },
  {
    path: "/listening-test",
    element: <ListeningTestPage />,
  },
  {
    path: "/writing-test",
    element: <WrintingTestPage />,
  },
  {
    // === ĐƯỜNG DẪN ĐỘNG MỚI ===
    // :quarterName là một tham số động
    // Nó sẽ khớp với /quarter/Quarter-1, /quarter/Quarter-2, v.v.
    path: "/quarter/:quarterName", 
    element: <QuarterDetailPage />,
  },
  {
    path: "/forgot-password",
    element: <ForgotPassword />,
  },
  {
    path: "/enter-otp",
    element: <EnterOtpPage />,
  },

  {
    path: "/reset-password",
    element: <ResetPasswordPage />,
  }
]);

// 3. Render bộ định tuyến
export default function App() {
  return <RouterProvider router={router} />;
}
