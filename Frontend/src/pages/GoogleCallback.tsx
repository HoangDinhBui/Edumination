import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

export default function GoogleCallback() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  useEffect(() => {
    const token = searchParams.get("token");
    const userId = searchParams.get("userId");

    if (token) {
      localStorage.setItem("Token", token);
      if (userId) localStorage.setItem("UserId", userId);
      sessionStorage.removeItem("oauth_state");
      navigate("/", { replace: true });
    } else {
      navigate("/signin?error=oauth");
    }
  }, [searchParams, navigate]);

  return (
    <div className="flex items-center justify-center h-screen">
      <div className="text-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
        <p>Đang hoàn tất đăng nhập...</p>
      </div>
    </div>
  );
}
