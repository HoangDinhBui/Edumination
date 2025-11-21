import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import {
  CheckCircle2,
  Star,
  Clock,
  Users,
  Award,
  BookOpen,
  ChevronLeft,
  ArrowRight,
  Target,
  TrendingUp,
} from "lucide-react";

const Booster: React.FC = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);

  React.useEffect(() => {
    document.body.style.overflow = "auto";
    document.documentElement.style.overflow = "auto";
    return () => {
      document.body.style.overflow = "";
      document.documentElement.style.overflow = "";
    };
  }, []);

  const handleBuyCourse = async () => {
    setIsLoading(true);
    try {
      const response = await axios.post(
        `http://localhost:8081/api/v1/checkout/courses/12`,
        { provider: "STRIPE" },
        {
          headers: { Authorization: `Bearer ${localStorage.getItem("Token")}` },
        }
      );
      const { checkoutUrl } = response.data;
      if (checkoutUrl) {
        window.location.href = checkoutUrl; // Redirect to the payment page
      } else {
        alert("Checkout URL not provided. Please try again.");
      }
      console.log(response.data);
    } catch (error) {
      console.error("Error purchasing course:", error);
      alert("Unable to purchase course. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  const courseFeatures = [
    "90 hours of intensive training",
    "Specialized focus on all 4 IELTS skills",
    "Practice with real IELTS exam formats",
    "Personalized feedback from expert instructors",
    "Access to exclusive online learning platform",
    "Weekly mock tests and progress tracking",
  ];

  const skillsBreakdown = [
    { skill: "Listening", hours: "20h", icon: "🎧" },
    { skill: "Reading", hours: "20h", icon: "📖" },
    { skill: "Writing", hours: "25h", icon: "✍️" },
    { skill: "Speaking", hours: "25h", icon: "🗣️" },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-slate-100">
      {/* Hero Section */}
      <div className="relative overflow-hidden bg-gradient-to-br from-[#7BA5D1] to-[#5B9BD5] text-white">
        <div className="absolute inset-0 opacity-10">
          <div
            className="absolute inset-0"
            style={{
              backgroundImage: `radial-gradient(circle, white 1px, transparent 1px)`,
              backgroundSize: "30px 30px",
            }}
          ></div>
        </div>

        <div className="relative max-w-7xl mx-auto px-6 sm:px-10 lg:px-20 py-16">
          <button
            onClick={() => navigate(-1)}
            className="flex items-center gap-2 text-white/90 hover:text-white mb-8 transition-colors group"
          >
            <ChevronLeft className="h-5 w-5 group-hover:-translate-x-1 transition-transform" />
            <span className="text-sm font-medium">Back to Courses</span>
          </button>

          <div className="grid lg:grid-cols-2 gap-12 items-center">
            <div className="space-y-6">
              <div className="inline-flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-full">
                <Target className="h-4 w-4" />
                <span className="text-sm font-semibold">
                  Intermediate Level
                </span>
              </div>

              <h1 className="text-5xl md:text-6xl font-serif leading-tight">
                Booster Course
              </h1>

              <div className="flex items-center gap-2">
                <span className="text-3xl font-bold">5.5 – 6.0</span>
                <TrendingUp className="h-8 w-8 text-yellow-300" />
              </div>

              <p className="text-lg text-white/90 leading-relaxed max-w-xl">
                Accelerate your IELTS journey with our intensive Booster
                program. Designed for intermediate learners ready to achieve
                their target band score.
              </p>

              <div className="flex flex-wrap gap-4 pt-4">
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Clock className="h-5 w-5" />
                  <span className="font-medium">90 Hours</span>
                </div>
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Users className="h-5 w-5" />
                  <span className="font-medium">Small Groups</span>
                </div>
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Award className="h-5 w-5" />
                  <span className="font-medium">Certified</span>
                </div>
              </div>
            </div>

            <div className="relative">
              <div className="absolute -top-4 -right-4 w-72 h-72 bg-yellow-300/30 rounded-full blur-3xl"></div>
              <div className="relative bg-white/10 backdrop-blur-md rounded-3xl p-8 border border-white/20 shadow-2xl">
                <div className="flex items-center justify-between mb-6">
                  <span className="text-sm font-semibold text-white/80">
                    Course Rating
                  </span>
                  <div className="flex items-center gap-1">
                    {[...Array(5)].map((_, i) => (
                      <Star
                        key={i}
                        className="h-5 w-5 fill-yellow-300 text-yellow-300"
                      />
                    ))}
                  </div>
                </div>

                <div className="space-y-4">
                  <div className="text-4xl font-bold">$599</div>
                  <div className="text-white/80 text-sm">
                    One-time payment • Lifetime access
                  </div>

                  <button
                    onClick={handleBuyCourse}
                    disabled={isLoading}
                    className="w-full bg-white text-[#7BA5D1] font-bold py-4 rounded-xl hover:bg-blue-50 transition-all duration-300 hover:scale-105 shadow-lg flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    {isLoading ? "Processing..." : "Enroll Now"}
                    {!isLoading && <ArrowRight className="h-5 w-5" />}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-6 sm:px-10 lg:px-20 py-16">
        {/* Course Overview */}
        <div className="mb-16">
          <div className="flex items-center gap-3 mb-8">
            <BookOpen className="h-8 w-8 text-[#7BA5D1]" />
            <h2 className="text-3xl font-serif text-[#7BA5D1]">
              Course Overview
            </h2>
          </div>

          <div className="bg-white rounded-3xl p-8 shadow-lg ring-1 ring-slate-200">
            <p className="text-lg text-gray-700 leading-relaxed mb-6">
              The Booster course is meticulously crafted for intermediate
              learners who have a solid foundation in English and are determined
              to reach band scores of 5.5 to 6.0. This comprehensive program
              combines intensive skill-building exercises with proven
              test-taking strategies to maximize your IELTS performance.
            </p>

            <div className="grid md:grid-cols-2 gap-6">
              <div className="space-y-4">
                <h3 className="font-bold text-xl text-slate-800">
                  What You'll Learn
                </h3>
                <ul className="space-y-3">
                  {courseFeatures.map((feature, idx) => (
                    <li key={idx} className="flex items-start gap-3">
                      <CheckCircle2 className="h-5 w-5 mt-0.5 text-[#4AB8A1] flex-shrink-0" />
                      <span className="text-gray-700">{feature}</span>
                    </li>
                  ))}
                </ul>
              </div>

              <div className="space-y-4">
                <h3 className="font-bold text-xl text-slate-800">
                  Skills Breakdown
                </h3>
                <div className="grid grid-cols-2 gap-4">
                  {skillsBreakdown.map((item, idx) => (
                    <div
                      key={idx}
                      className="bg-gradient-to-br from-blue-50 to-sky-50 rounded-2xl p-4 border border-blue-100"
                    >
                      <div className="text-3xl mb-2">{item.icon}</div>
                      <div className="font-semibold text-slate-800">
                        {item.skill}
                      </div>
                      <div className="text-sm text-gray-600">{item.hours}</div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Why Choose This Course */}
        <div className="mb-16">
          <h2 className="text-3xl font-serif text-[#7BA5D1] mb-8 text-center">
            Why Choose the Booster Course?
          </h2>

          <div className="grid md:grid-cols-3 gap-6">
            {[
              {
                title: "Expert Instructors",
                description:
                  "Learn from certified IELTS teachers with 8+ years of experience",
                icon: "👨‍🏫",
              },
              {
                title: "Proven Results",
                description:
                  "95% of our students achieve their target band score",
                icon: "🎯",
              },
              {
                title: "Flexible Learning",
                description:
                  "Online and in-person classes available to fit your schedule",
                icon: "⏰",
              },
            ].map((item, idx) => (
              <div
                key={idx}
                className="bg-white rounded-2xl p-6 shadow-lg ring-1 ring-slate-200 hover:shadow-xl transition-shadow"
              >
                <div className="text-5xl mb-4">{item.icon}</div>
                <h3 className="font-bold text-xl text-slate-800 mb-2">
                  {item.title}
                </h3>
                <p className="text-gray-600">{item.description}</p>
              </div>
            ))}
          </div>
        </div>

        {/* CTA Section */}
        <div className="bg-gradient-to-br from-[#7BA5D1] to-[#5B9BD5] rounded-3xl p-12 text-center text-white shadow-2xl">
          <h2 className="text-3xl md:text-4xl font-serif mb-4">
            Ready to Boost Your IELTS Score?
          </h2>
          <p className="text-lg text-white/90 mb-8 max-w-2xl mx-auto">
            Join thousands of successful students who have achieved their dream
            scores with Edumination
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <button
              onClick={handleBuyCourse}
              disabled={isLoading}
              className="px-8 py-4 bg-white text-[#7BA5D1] font-bold rounded-xl hover:bg-blue-50 transition-all duration-300 hover:scale-105 shadow-lg disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoading ? "Processing..." : "Enroll Now"}
            </button>
            <button
              onClick={() => navigate(-1)}
              className="px-8 py-4 bg-white/20 backdrop-blur-sm text-white font-semibold rounded-xl hover:bg-white/30 transition-all duration-300 border border-white/30"
            >
              View All Courses
            </button>
          </div>
        </div>
      </div>

      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=Montserrat:wght@400;500;600;700&display=swap');
        
        .font-serif {
          font-family: 'Playfair Display', serif;
        }
        
        body {
          font-family: 'Montserrat', sans-serif;
        }
      `}</style>
    </div>
  );
};

export default Booster;
