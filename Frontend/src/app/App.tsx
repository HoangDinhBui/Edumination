import React from "react";
import { createBrowserRouter, RouterProvider } from "react-router-dom";

// --- 1. CÁC TRANG CÔNG KHAI (User) ---
import HomePage from "../pages/Home/HomePage";
import SignInPage from "../pages/SignIn/SignInPage";
import SignUpPage from "../pages/SignUp/SignUpPage";
import AnswerPage from "../pages/Answer/Answer"; // Đã đổi tên cho khớp logic mới
import RankingPage from "../pages/Ranking/RankingPage"; // Trang Ranking mới làm
import ExamLibrary from "../pages/ExamsLibrary/ExamsLibrary";
import QuarterDetailPage from "../pages/ExamsLibrary/QuarterDetailPage";
import ForgotPassword from "../pages/ForgotPassword/ForgotPassword";
import EnterOtpPage from "../pages/ForgotPassword/EnterOtpPage";
import ResetPasswordPage from "../pages/ForgotPassword/ResetPasswordPage";
import ListeningTestPage from "../pages/Tests/ListeningTestPage";
import WritingTestPage from "../pages/Tests/WritingTestPage"; // Sửa lại tên file nếu cần (Wrinting -> Writing)
import ReadingTestPage from "../pages/Tests/ReadingTestPage";
import SpeakingTestPage from "../pages/Tests/SpeakingTestPage";
import GoogleCallback from "../pages/GoogleCallback";
import DashboardPage from "../pages/Dashboard/DashboardPage";

// --- 2. CÁC COMPONENT ADMIN (Mới thêm) ---
import AdminRoute from "../components/AdminRoute"; // Component bảo vệ
import AdminLayout from "../layouts/AdminLayout";   // Khung giao diện Admin
import AdminDashboard from "../pages/Admin/DashboardOverview";
import UserManagement from "../pages/Admin/UserManagement";
import TestManagement from "../pages/Admin/TestManagement";
import CourseManagement from "../pages/Admin/CourseManagement";
// import CourseManagement from "../pages/Admin/CourseManagement"; // Bỏ comment khi tạo xong file này

const router = createBrowserRouter([
  // ==============================
  // KHU VỰC PUBLIC (Ai cũng vào được)
  // ==============================
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
  },
  {
    path: "/google-callback",
    element: <GoogleCallback />,
  },

  // --- Khu vực Học tập & Thi ---
  {
    path: "/library",
    element: <ExamLibrary />,
  },
  {
    path: "/quarter/:quarterName",
    element: <QuarterDetailPage />,
  },
  {
    path: "/listening-test",
    element: <ListeningTestPage />,
  },
  {
    path: "/writing-test",
    element: <WritingTestPage />,
  },
  {
    path: "/reading-test",
    element: <ReadingTestPage />,
  },
  {
    path: "/speaking-test",
    element: <SpeakingTestPage />,
  },
  {
    path: "/answer", // Hoặc /answer-detail tùy bạn đặt
    element: <AnswerPage />,
  },
  {
    path: "/ranking", // Trang xếp hạng
    element: <RankingPage />,
  },
  {
    path: "/ranking", // Trang xếp hạng
    element: <RankingPage />,
  },
  {
    path: "/dashboard", // Trang xếp hạng
    element: <DashboardPage />,
  },

  // ==============================
  // KHU VỰC ADMIN (Bảo mật)
  // ==============================
  {
    // Bọc ngoài cùng là AdminRoute để kiểm tra quyền
    element: <AdminRoute />, 
    children: [
      {
        path: "/admin",
        element: <AdminLayout />, // Layout có Sidebar
        children: [
          {
            index: true, // Mặc định vào /admin sẽ hiện Dashboard
            element: <AdminDashboard />,
          },
          {
            path: "users", // /admin/users
            element: <UserManagement />,
          },
          {
            path: "tests", // /admin/tests
            element: <TestManagement />,
          },
          {
            path: "courses", // /admin/courses
            // element: <CourseManagement />, // Bỏ comment khi làm xong
            element: <CourseManagement />,
          },
        ],
      },
    ],
  },
]);

export default function App() {
  return <RouterProvider router={router} />;
}