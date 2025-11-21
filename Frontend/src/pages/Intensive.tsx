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
  Zap,
  TrendingUp,
  Flame,
  Target,
  Trophy,
  Brain,
} from "lucide-react";

const Intensive: React.FC = () => {
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
        `http://localhost:8081/api/v1/checkout/courses/13`,
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
    "120 hours of intensive advanced training",
    "Master complex grammar and vocabulary",
    "Advanced exam strategies and techniques",
    "Weekly full-length mock tests",
    "One-on-one personalized coaching sessions",
    "Access to premium study materials",
    "Band 8.0+ teacher guidance",
    "University application support",
  ];

  const skillsBreakdown = [
    {
      skill: "Listening",
      hours: "25h",
      icon: "🎧",
      level: "Advanced",
      color: "from-violet-50 to-violet-100",
    },
    {
      skill: "Reading",
      hours: "25h",
      icon: "📖",
      level: "Advanced",
      color: "from-indigo-50 to-indigo-100",
    },
    {
      skill: "Writing",
      hours: "35h",
      icon: "✍️",
      level: "Expert",
      color: "from-purple-50 to-purple-100",
    },
    {
      skill: "Speaking",
      hours: "35h",
      icon: "🗣️",
      level: "Expert",
      color: "from-pink-50 to-pink-100",
    },
  ];

  const masterySections = [
    {
      title: "Advanced Techniques",
      description: "Master sophisticated strategies for high band scores",
      points: [
        "Complex sentence structures",
        "Academic vocabulary mastery",
        "Critical thinking skills",
        "Time management optimization",
      ],
      icon: Brain,
      gradient: "from-purple-500 to-indigo-600",
    },
    {
      title: "Intensive Practice",
      description: "Rigorous training with real exam conditions",
      points: [
        "15+ full mock tests",
        "Timed practice sessions",
        "Detailed performance analysis",
        "Weekly progress reviews",
      ],
      icon: Target,
      gradient: "from-pink-500 to-rose-600",
    },
    {
      title: "Expert Mentorship",
      description: "Learn from IELTS 8.5+ certified instructors",
      points: [
        "Personal study plans",
        "1-on-1 coaching sessions",
        "University admission guidance",
        "Career pathway advice",
      ],
      icon: Trophy,
      gradient: "from-amber-500 to-orange-600",
    },
  ];

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-purple-50 to-slate-100">
      {/* Hero Section */}
      <div className="relative overflow-hidden bg-gradient-to-br from-purple-600 via-violet-600 to-indigo-700 text-white">
        <div className="absolute inset-0 opacity-10">
          <div
            className="absolute inset-0"
            style={{
              backgroundImage: `radial-gradient(circle, white 1px, transparent 1px)`,
              backgroundSize: "25px 25px",
            }}
          ></div>
        </div>

        {/* Animated gradient orbs */}
        <div className="absolute top-20 right-10 w-96 h-96 bg-pink-400/30 rounded-full blur-3xl animate-pulse"></div>
        <div
          className="absolute bottom-20 left-10 w-80 h-80 bg-yellow-400/20 rounded-full blur-3xl animate-pulse"
          style={{ animationDelay: "1s" }}
        ></div>

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
                <Flame className="h-4 w-4 text-orange-300" />
                <span className="text-sm font-semibold">Advanced Level</span>
              </div>

              <h1 className="text-5xl md:text-6xl font-serif leading-tight">
                Intensive Course
              </h1>

              <div className="flex items-center gap-3">
                <span className="text-3xl font-bold">6.0 – 7.5</span>
                <Zap className="h-10 w-10 text-yellow-300 animate-pulse" />
              </div>

              <p className="text-lg text-white/90 leading-relaxed max-w-xl">
                Elite training for ambitious learners targeting top
                universities. Master advanced techniques and achieve exceptional
                band scores with our most rigorous program.
              </p>

              <div className="flex flex-wrap gap-4 pt-4">
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Clock className="h-5 w-5" />
                  <span className="font-medium">120 Hours</span>
                </div>
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Users className="h-5 w-5" />
                  <span className="font-medium">Elite Groups</span>
                </div>
                <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <Award className="h-5 w-5" />
                  <span className="font-medium">Premium</span>
                </div>
              </div>

              {/* Achievement badges */}
              <div className="flex gap-3 pt-2">
                <div className="bg-yellow-400/20 backdrop-blur-sm px-3 py-1.5 rounded-full text-xs font-semibold flex items-center gap-1.5">
                  <Star className="h-3.5 w-3.5 fill-yellow-300 text-yellow-300" />
                  Top Rated
                </div>
                <div className="bg-green-400/20 backdrop-blur-sm px-3 py-1.5 rounded-full text-xs font-semibold flex items-center gap-1.5">
                  <TrendingUp className="h-3.5 w-3.5 text-green-300" />
                  98% Success Rate
                </div>
              </div>
            </div>

            <div className="relative">
              <div className="absolute -top-4 -right-4 w-72 h-72 bg-yellow-300/30 rounded-full blur-3xl"></div>
              <div className="relative bg-white/10 backdrop-blur-md rounded-3xl p-8 border border-white/20 shadow-2xl">
                <div className="absolute -top-3 -right-3">
                  <div className="bg-gradient-to-r from-yellow-400 to-orange-500 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg flex items-center gap-1">
                    <Flame className="h-4 w-4" />
                    Most Popular
                  </div>
                </div>

                <div className="flex items-center justify-between mb-6 pt-4">
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
                  <div className="flex items-baseline gap-2">
                    <div className="text-4xl font-bold">$799</div>
                    <div className="text-white/60 text-sm line-through">
                      $999
                    </div>
                  </div>
                  <div className="text-white/80 text-sm">
                    One-time payment • Lifetime access
                  </div>

                  <button
                    onClick={handleBuyCourse}
                    disabled={isLoading}
                    className="w-full bg-gradient-to-r from-yellow-400 to-orange-500 text-gray-900 font-bold py-4 rounded-xl hover:from-yellow-300 hover:to-orange-400 transition-all duration-300 hover:scale-105 shadow-lg flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    {isLoading ? "Processing..." : "Enroll in Premium"}
                    {!isLoading && <ArrowRight className="h-5 w-5" />}
                  </button>

                  <div className="pt-4 border-t border-white/20 space-y-2">
                    <div className="flex items-center gap-2 text-sm text-white/80">
                      <CheckCircle2 className="h-4 w-4 text-green-300" />
                      <span>15+ full mock tests included</span>
                    </div>
                    <div className="flex items-center gap-2 text-sm text-white/80">
                      <CheckCircle2 className="h-4 w-4 text-green-300" />
                      <span>1-on-1 coaching sessions</span>
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
            <BookOpen className="h-8 w-8 text-purple-600" />
            <h2 className="text-3xl font-serif text-purple-600">
              Course Overview
            </h2>
          </div>

          <div className="bg-white rounded-3xl p-8 shadow-lg ring-1 ring-slate-200">
            <p className="text-lg text-gray-700 leading-relaxed mb-6">
              The Intensive course represents the pinnacle of IELTS preparation.
              Designed for advanced learners with solid English foundations,
              this program pushes you to master complex language skills,
              sophisticated exam strategies, and the critical thinking required
              for band scores of 6.0 to 7.5. Perfect for those targeting top
              universities worldwide.
            </p>

            <div className="grid md:grid-cols-2 gap-6">
              <div className="space-y-4">
                <h3 className="font-bold text-xl text-slate-800 flex items-center gap-2">
                  <Trophy className="h-5 w-5 text-amber-500" />
                  Premium Features
                </h3>
                <ul className="space-y-3">
                  {courseFeatures.map((feature, idx) => (
                    <li key={idx} className="flex items-start gap-3">
                      <CheckCircle2 className="h-5 w-5 mt-0.5 text-purple-600 flex-shrink-0" />
                      <span className="text-gray-700">{feature}</span>
                    </li>
                  ))}
                </ul>
              </div>

              <div className="space-y-4">
                <h3 className="font-bold text-xl text-slate-800">
                  Advanced Skills Breakdown
                </h3>
                <div className="grid grid-cols-2 gap-4">
                  {skillsBreakdown.map((item, idx) => (
                    <div
                      key={idx}
                      className={`bg-gradient-to-br ${item.color} rounded-2xl p-4 border border-slate-200 relative overflow-hidden`}
                    >
                      <div className="absolute top-1 right-1">
                        <div className="bg-purple-600 text-white text-[10px] px-2 py-0.5 rounded-full font-bold">
                          {item.level}
                        </div>
                      </div>
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

        {/* Mastery Sections */}
        <div className="mb-16">
          <h2 className="text-3xl font-serif text-purple-600 mb-8 text-center">
            What Makes This Course Elite
          </h2>

          <div className="grid md:grid-cols-3 gap-6">
            {masterySections.map((section, idx) => {
              const Icon = section.icon;
              return (
                <div
                  key={idx}
                  className="bg-white rounded-3xl p-8 shadow-lg ring-1 ring-slate-200 hover:shadow-xl transition-shadow group"
                >
                  <div
                    className={`inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-br ${section.gradient} text-white mb-6 group-hover:scale-110 transition-transform`}
                  >
                    <Icon className="h-8 w-8" />
                  </div>

                  <h3 className="font-bold text-xl text-slate-800 mb-2">
                    {section.title}
                  </h3>
                  <p className="text-gray-600 text-sm mb-4">
                    {section.description}
                  </p>

                  <ul className="space-y-2">
                    {section.points.map((point, pointIdx) => (
                      <li
                        key={pointIdx}
                        className="flex items-start gap-2 text-sm text-gray-700"
                      >
                        <CheckCircle2 className="h-4 w-4 mt-0.5 text-purple-500 flex-shrink-0" />
                        <span>{point}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              );
            })}
          </div>
        </div>

        {/* Success Metrics */}
        <div className="mb-16">
          <div className="bg-gradient-to-br from-purple-600 to-indigo-600 rounded-3xl p-12 text-white shadow-2xl">
            <h3 className="text-3xl font-serif mb-8 text-center">
              Proven Excellence
            </h3>

            <div className="grid md:grid-cols-4 gap-6">
              <div className="text-center">
                <div className="text-5xl font-bold mb-2">98%</div>
                <div className="text-white/80 text-sm">Achieve 6.5+</div>
              </div>
              <div className="text-center">
                <div className="text-5xl font-bold mb-2">7.2</div>
                <div className="text-white/80 text-sm">Average Score</div>
              </div>
              <div className="text-center">
                <div className="text-5xl font-bold mb-2">850+</div>
                <div className="text-white/80 text-sm">Students in 2024</div>
              </div>
              <div className="text-center">
                <div className="text-5xl font-bold mb-2">4.9/5</div>
                <div className="text-white/80 text-sm">Student Rating</div>
              </div>
            </div>
          </div>
        </div>

        {/* Target Universities */}
        <div className="mb-16">
          <h2 className="text-3xl font-serif text-purple-600 mb-6 text-center">
            Where Our Students Go
          </h2>
          <p className="text-center text-gray-600 mb-8 max-w-2xl mx-auto">
            Our Intensive course graduates have been accepted to top
            universities worldwide
          </p>

          <div className="bg-white rounded-2xl p-8 shadow-lg ring-1 ring-slate-200">
            <div className="flex flex-wrap justify-center gap-8 items-center opacity-60">
              {[
                "Oxford",
                "Cambridge",
                "MIT",
                "Stanford",
                "Harvard",
                "Yale",
              ].map((uni) => (
                <div key={uni} className="text-xl font-serif text-gray-700">
                  {uni}
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* CTA Section */}
        <div className="bg-gradient-to-br from-purple-600 via-violet-600 to-indigo-700 rounded-3xl p-12 text-center text-white shadow-2xl relative overflow-hidden">
          <div className="absolute inset-0 opacity-10">
            <div
              className="absolute inset-0"
              style={{
                backgroundImage: `radial-gradient(circle, white 2px, transparent 2px)`,
                backgroundSize: "40px 40px",
              }}
            ></div>
          </div>

          <div className="relative z-10">
            <h2 className="text-3xl md:text-4xl font-serif mb-4">
              Achieve Your Dream Score
            </h2>
            <p className="text-lg text-white/90 mb-8 max-w-2xl mx-auto">
              Join the elite program trusted by thousands of successful
              students. Your journey to 7.5 starts here.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <button
                onClick={handleBuyCourse}
                disabled={isLoading}
                className="px-8 py-4 bg-gradient-to-r from-yellow-400 to-orange-500 text-gray-900 font-bold rounded-xl hover:from-yellow-300 hover:to-orange-400 transition-all duration-300 hover:scale-105 shadow-lg disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {isLoading ? "Processing..." : "Start Elite Training"}
              </button>
              <button
                onClick={() => navigate(-1)}
                className="px-8 py-4 bg-white/20 backdrop-blur-sm text-white font-semibold rounded-xl hover:bg-white/30 transition-all duration-300 border border-white/30"
              >
                Compare All Courses
              </button>
            </div>
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
        
        @keyframes pulse {
          0%, 100% { opacity: 0.3; }
          50% { opacity: 0.2; }
        }
        
        .animate-pulse {
          animation: pulse 3s ease-in-out infinite;
        }
      `}</style>
    </div>
  );
};

export default Intensive;
