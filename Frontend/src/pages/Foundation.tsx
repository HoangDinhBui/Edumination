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
  Sparkles,
  GraduationCap,
  Lightbulb,
} from "lucide-react";
import Navbar from "../components/Navbar";

const Foundation: React.FC = () => {
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
        `http://localhost:8081/api/v1/checkout/courses/11`,
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
    "Complete beginner-friendly curriculum",
    "Build strong foundations in all 4 skills",
    "Step-by-step guided learning path",
    "Interactive practice exercises daily",
    "Personal tutor support and feedback",
    "Progress tracking and regular assessments",
  ];

  const skillsBreakdown = [
    {
      skill: "Listening",
      hours: "15h",
      icon: "🎧",
      color: "from-blue-50 to-blue-100",
    },
    {
      skill: "Reading",
      hours: "15h",
      icon: "📖",
      color: "from-green-50 to-green-100",
    },
    {
      skill: "Writing",
      hours: "20h",
      icon: "✍️",
      color: "from-purple-50 to-purple-100",
    },
    {
      skill: "Speaking",
      hours: "20h",
      icon: "🗣️",
      color: "from-orange-50 to-orange-100",
    },
  ];

  const learningPath = [
    {
      phase: "Phase 1: Basics",
      duration: "Weeks 1-3",
      topics: [
        "English fundamentals",
        "Basic grammar structures",
        "Essential vocabulary",
      ],
      icon: "🌱",
    },
    {
      phase: "Phase 2: Skills",
      duration: "Weeks 4-7",
      topics: [
        "IELTS format introduction",
        "Skill-building exercises",
        "Practice tests",
      ],
      icon: "📚",
    },
    {
      phase: "Phase 3: Practice",
      duration: "Weeks 8-10",
      topics: ["Mock tests", "Error analysis", "Exam strategies"],
      icon: "🎯",
    },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-slate-100">
      <Navbar />
      {/* Hero Section */}
      <div className="relative overflow-hidden bg-gradient-to-br from-[#4AB8A1] to-[#2986B7] text-white">
        <div className="absolute inset-0 opacity-10">
          <div
            className="absolute inset-0"
            style={{
              backgroundImage: `radial-gradient(circle, white 1px, transparent 1px)`,
              backgroundSize: "30px 30px",
            }}
          ></div>
        </div>

        {/* Decorative floating elements */}
        <div className="absolute top-20 right-10 w-64 h-64 bg-yellow-300/20 rounded-full blur-3xl"></div>
        <div className="absolute bottom-20 left-10 w-80 h-80 bg-blue-300/20 rounded-full blur-3xl"></div>

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
                <Sparkles className="h-4 w-4" />
                <span className="text-sm font-semibold">Beginner Level</span>
              </div>

              <h1 className="text-5xl md:text-6xl font-serif leading-tight">
                Foundation Course
              </h1>

              <div className="flex items-center gap-2">
                <span className="text-3xl font-bold">0.0 – 5.0</span>
                <GraduationCap className="h-8 w-8 text-yellow-300" />
              </div>

              <p className="text-lg text-white/90 leading-relaxed max-w-xl">
                Start your IELTS journey from scratch! Perfect for beginners who
                want to build a solid foundation in English and achieve their
                first milestone band score.
              </p>

              <div className="flex flex-wrap gap-4 pt-4">
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Clock className="h-5 w-5" />
                  <span className="font-medium">70 Hours</span>
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
                  <div className="text-4xl font-bold">$449</div>
                  <div className="text-white/80 text-sm">
                    One-time payment • Lifetime access
                  </div>

                  <button
                    onClick={handleBuyCourse}
                    disabled={isLoading}
                    className="w-full bg-white text-[#4AB8A1] font-bold py-4 rounded-xl hover:bg-blue-50 transition-all duration-300 hover:scale-105 shadow-lg flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    {isLoading ? "Processing..." : "Enroll Now"}
                    {!isLoading && <ArrowRight className="h-5 w-5" />}
                  </button>

                  <div className="pt-4 border-t border-white/20">
                    <div className="flex items-center gap-2 text-sm text-white/80">
                      <Lightbulb className="h-4 w-4" />
                      <span>Perfect for complete beginners</span>
                    </div>
                  </div>
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
            <BookOpen className="h-8 w-8 text-[#4AB8A1]" />
            <h2 className="text-3xl font-serif text-[#4AB8A1]">
              Course Overview
            </h2>
          </div>

          <div className="bg-white rounded-3xl p-8 shadow-lg ring-1 ring-slate-200">
            <p className="text-lg text-gray-700 leading-relaxed mb-6">
              The Foundation course is specially designed for absolute beginners
              and those with limited English proficiency. We understand that
              starting from zero can be challenging, so our expert instructors
              will guide you step-by-step through essential grammar, vocabulary,
              and all four IELTS skills in a supportive, encouraging
              environment.
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
                      className={`bg-gradient-to-br ${item.color} rounded-2xl p-4 border border-slate-200`}
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

        {/* Learning Path */}
        <div className="mb-16">
          <h2 className="text-3xl font-serif text-[#4AB8A1] mb-8 text-center">
            Your 10-Week Learning Journey
          </h2>

          <div className="grid md:grid-cols-3 gap-6">
            {learningPath.map((phase, idx) => (
              <div
                key={idx}
                className="relative bg-white rounded-2xl p-6 shadow-lg ring-1 ring-slate-200"
              >
                {/* Connector line */}
                {idx < learningPath.length - 1 && (
                  <div className="hidden md:block absolute top-1/2 -right-3 w-6 h-0.5 bg-gradient-to-r from-[#4AB8A1] to-[#2986B7]"></div>
                )}

                <div className="text-5xl mb-4">{phase.icon}</div>
                <h3 className="font-bold text-xl text-slate-800 mb-2">
                  {phase.phase}
                </h3>
                <div className="text-sm text-[#4AB8A1] font-semibold mb-4">
                  {phase.duration}
                </div>
                <ul className="space-y-2">
                  {phase.topics.map((topic, topicIdx) => (
                    <li
                      key={topicIdx}
                      className="flex items-start gap-2 text-sm text-gray-600"
                    >
                      <span className="text-[#4AB8A1] mt-1">•</span>
                      <span>{topic}</span>
                    </li>
                  ))}
                </ul>
              </div>
            ))}
          </div>
        </div>

        {/* Why Choose This Course */}
        <div className="mb-16">
          <h2 className="text-3xl font-serif text-[#4AB8A1] mb-8 text-center">
            Why Start with Foundation?
          </h2>

          <div className="grid md:grid-cols-3 gap-6">
            {[
              {
                title: "Zero to Hero",
                description:
                  "No prior English knowledge required. We start from absolute basics",
                icon: "🚀",
              },
              {
                title: "Patient Teachers",
                description:
                  "Experienced instructors specialized in teaching beginners",
                icon: "❤️",
              },
              {
                title: "Confidence Building",
                description:
                  "Small classes and supportive environment help you learn without fear",
                icon: "💪",
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

        {/* Success Stories Preview */}
        <div className="mb-16">
          <div className="bg-gradient-to-r from-blue-50 to-sky-50 rounded-3xl p-8 border border-blue-100">
            <div className="text-center mb-6">
              <h3 className="text-2xl font-serif text-slate-800 mb-2">
                From Zero to 5.0 in Just 10 Weeks
              </h3>
              <p className="text-gray-600">
                Join hundreds of successful students who started from scratch
              </p>
            </div>

            <div className="grid md:grid-cols-3 gap-6 text-center">
              <div className="bg-white rounded-xl p-6">
                <div className="text-4xl font-bold text-[#4AB8A1] mb-2">
                  87%
                </div>
                <div className="text-sm text-gray-600">Achieve 5.0+ band</div>
              </div>
              <div className="bg-white rounded-xl p-6">
                <div className="text-4xl font-bold text-[#2986B7] mb-2">
                  4.8/5
                </div>
                <div className="text-sm text-gray-600">
                  Student satisfaction
                </div>
              </div>
              <div className="bg-white rounded-xl p-6">
                <div className="text-4xl font-bold text-purple-600 mb-2">
                  500+
                </div>
                <div className="text-sm text-gray-600">Graduates in 2024</div>
              </div>
            </div>
          </div>
        </div>

        {/* CTA Section */}
        <div className="bg-gradient-to-br from-[#4AB8A1] to-[#2986B7] rounded-3xl p-12 text-center text-white shadow-2xl">
          <h2 className="text-3xl md:text-4xl font-serif mb-4">
            Begin Your IELTS Journey Today
          </h2>
          <p className="text-lg text-white/90 mb-8 max-w-2xl mx-auto">
            Every expert was once a beginner. Take the first step towards your
            dream score with Edumination's Foundation course
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <button
              onClick={handleBuyCourse}
              disabled={isLoading}
              className="px-8 py-4 bg-white text-[#4AB8A1] font-bold rounded-xl hover:bg-blue-50 transition-all duration-300 hover:scale-105 shadow-lg disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoading ? "Processing..." : "Start Learning Now"}
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

export default Foundation;
