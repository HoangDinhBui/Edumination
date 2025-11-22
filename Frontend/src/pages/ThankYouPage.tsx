import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const ThankYouPage = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      navigate("/");
    }, 3000); // Redirect after 3 seconds
    return () => clearTimeout(timer);
  }, [navigate]);

  return <h1>Thank you for your purchase! Redirecting to home...</h1>;
};

export default ThankYouPage;
