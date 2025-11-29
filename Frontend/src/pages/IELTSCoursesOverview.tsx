import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  ArrowRight,
  CheckCircle2,
  Star,
  Clock,
  Users,
  TrendingUp,
  Zap,
  Crown,
  Flame,
  BookOpen,
  Target,
  Sparkles,
} from "lucide-react";
import Navbar from "../components/Navbar";

const IELTSCoursesOverview: React.FC = () => {
  const navigate = useNavigate();
  const [hoveredCard, setHoveredCard] = useState<number | null>(null);

  React.useEffect(() => {
    document.body.style.overflow = "auto";
    document.documentElement.style.overflow = "auto";
    return () => {
      document.body.style.overflow = "";
      document.documentElement.style.overflow = "";
    };
  }, []);

  const courses = [
    {
      id: 1,
      title: "Foundation",
      subtitle: "Build Your Base",
      bandScore: "0.0 – 5.0",
      description:
        "Perfect for beginners starting their IELTS journey. Build essential English skills from the ground up.",
      price: "$299",
      originalPrice: "$399",
      duration: "80 Hours",
      students: "Basic Groups",
      level: "Beginner",
      icon: BookOpen,
      gradient: "from-emerald-500 to-teal-600",
      bgGradient: "from-emerald-50 via-teal-50 to-cyan-50",
      accentColor: "emerald",
      features: [
        "Essential grammar foundation",
        "Basic vocabulary building",
        "Simple exam techniques",
        "Weekly practice tests",
      ],
      badge: "Best for Beginners",
      badgeColor: "from-emerald-400 to-teal-500",
      route: "/foundation",
      rating: 4.7,
      successRate: "92%",
    },
    {
      id: 2,
      title: "Booster",
      subtitle: "Accelerate Your Skills",
      bandScore: "5.5 – 6.0",
      description:
        "Boost your existing skills to the next level. Perfect for intermediate learners targeting competitive scores.",
      price: "$499",
      originalPrice: "$649",
      duration: "100 Hours",
      students: "Standard Groups",
      level: "Intermediate",
      icon: Zap,
      gradient: "from-blue-500 to-indigo-600",
      bgGradient: "from-blue-50 via-indigo-50 to-violet-50",
      accentColor: "blue",
      features: [
        "Intermediate skills development",
        "Exam strategy mastery",
        "10+ mock tests",
        "Group study sessions",
      ],
      badge: "Most Balanced",
      badgeColor: "from-blue-400 to-indigo-500",
      route: "/booster",
      rating: 4.8,
      successRate: "95%",
    },
    {
      id: 3,
      title: "Intensive",
      subtitle: "Master Advanced Techniques",
      bandScore: "6.0 – 7.5",
      description:
        "Elite training for ambitious learners targeting top universities. Master advanced techniques and achieve exceptional scores.",
      price: "$799",
      originalPrice: "$999",
      duration: "120 Hours",
      students: "Elite Groups",
      level: "Advanced",
      icon: Flame,
      gradient: "from-purple-500 to-pink-600",
      bgGradient: "from-purple-50 via-pink-50 to-rose-50",
      accentColor: "purple",
      features: [
        "Advanced exam strategies",
        "15+ full mock tests",
        "1-on-1 coaching sessions",
        "University application support",
      ],
      badge: "Most Popular",
      badgeColor: "from-orange-400 to-pink-500",
      route: "/intensive",
      rating: 4.9,
      successRate: "98%",
    },
    {
      id: 4,
      title: "Mastery",
      subtitle: "Achieve Perfection",
      bandScore: "7.5 – 9.0",
      description:
        "The ultimate IELTS experience. Achieve native-level proficiency with our most comprehensive and exclusive program.",
      price: "$1,299",
      originalPrice: "$1,599",
      duration: "150 Hours",
      students: "VIP Access",
      level: "Expert",
      icon: Crown,
      gradient: "from-amber-500 to-orange-600",
      bgGradient: "from-amber-50 via-yellow-50 to-orange-50",
      accentColor: "amber",
      features: [
        "Native-level fluency training",
        "20+ full mock tests",
        "Unlimited coaching sessions",
        "PhD/scholarship support",
      ],
      badge: "Most Exclusive",
      badgeColor: "from-yellow-400 to-amber-500",
      route: "/mastery",
      rating: 5.0,
      successRate: "100%",
    },
  ];

  const handleCourseClick = (route: string) => {
    navigate(route);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-slate-100">
      <Navbar />
      {/* Hero Section */}
      <div className="relative overflow-hidden bg-gradient-to-br from-indigo-600 via-blue-600 to-cyan-600 text-white">
        <div className="absolute inset-0 opacity-10">
          <div
            className="absolute inset-0"
            style={{
              backgroundImage: `radial-gradient(circle, white 1px, transparent 1px)`,
              backgroundSize: "30px 30px",
            }}
          ></div>
        </div>

        {/* Animated orbs */}
        <div className="absolute top-20 right-10 w-96 h-96 bg-purple-400/30 rounded-full blur-3xl animate-pulse"></div>
        <div
          className="absolute bottom-20 left-10 w-80 h-80 bg-cyan-400/20 rounded-full blur-3xl animate-pulse"
          style={{ animationDelay: "1.5s" }}
        ></div>

        <div className="relative max-w-7xl mx-auto px-6 sm:px-10 lg:px-20 py-20 text-center">
          <div className="inline-flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-full mb-6">
            <Sparkles className="h-4 w-4 text-yellow-300" />
            <span className="text-sm font-semibold">
              Choose Your Perfect Path
            </span>
          </div>

          <h1 className="text-5xl md:text-6xl font-serif mb-6 leading-tight">
            IELTS Courses
          </h1>

          <p className="text-xl text-white/90 max-w-3xl mx-auto leading-relaxed mb-8">
            From beginner foundations to mastery-level excellence. Find the
            perfect course tailored to your current level and target score.
          </p>

          <div className="flex flex-wrap justify-center gap-6 text-sm">
            <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
              <CheckCircle2 className="h-4 w-4" />
              <span>Expert Instructors</span>
            </div>
            <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
              <CheckCircle2 className="h-4 w-4" />
              <span>Proven Results</span>
            </div>
            <div className="flex items-center gap-2 bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
              <CheckCircle2 className="h-4 w-4" />
              <span>Lifetime Access</span>
            </div>
          </div>
        </div>
      </div>

      {/* Courses Grid */}
      <div className="max-w-7xl mx-auto px-6 sm:px-10 lg:px-20 py-16">
        <div className="grid md:grid-cols-2 gap-8">
          {courses.map((course, index) => {
            const Icon = course.icon;
            return (
              <div
                key={course.id}
                className={`relative bg-gradient-to-br ${course.bgGradient} rounded-3xl p-8 shadow-lg ring-1 ring-slate-200 hover:shadow-2xl transition-all duration-500 cursor-pointer group`}
                onMouseEnter={() => setHoveredCard(index)}
                onMouseLeave={() => setHoveredCard(null)}
                onClick={() => handleCourseClick(course.route)}
              >
                {/* Badge */}
                <div className="absolute -top-3 -right-3">
                  <div
                    className={`bg-gradient-to-r ${course.badgeColor} text-white px-4 py-2 rounded-full text-xs font-bold shadow-lg flex items-center gap-1`}
                  >
                    <Star className="h-3 w-3 fill-white" />
                    {course.badge}
                  </div>
                </div>

                {/* Icon */}
                <div
                  className={`inline-flex items-center justify-center w-16 h-16 rounded-2xl bg-gradient-to-br ${course.gradient} text-white mb-6 group-hover:scale-110 transition-transform duration-500`}
                >
                  <Icon className="h-8 w-8" />
                </div>

                {/* Content */}
                <div className="space-y-4">
                  <div>
                    <div className="flex items-center gap-2 mb-2">
                      <h3 className="text-3xl font-serif font-bold text-gray-900">
                        {course.title}
                      </h3>
                      <div
                        className={`text-xs font-bold px-2 py-1 rounded-full bg-${course.accentColor}-100 text-${course.accentColor}-700`}
                      >
                        {course.level}
                      </div>
                    </div>
                    <p className="text-sm text-gray-600 mb-3">
                      {course.subtitle}
                    </p>
                  </div>

                  {/* Band Score */}
                  <div className="flex items-center gap-2">
                    <Target className="h-5 w-5 text-gray-700" />
                    <span className="text-2xl font-bold text-gray-900">
                      {course.bandScore}
                    </span>
                  </div>

                  <p className="text-gray-700 leading-relaxed">
                    {course.description}
                  </p>

                  {/* Features */}
                  <ul className="space-y-2 py-2">
                    {course.features.map((feature, idx) => (
                      <li
                        key={idx}
                        className="flex items-start gap-2 text-sm text-gray-700"
                      >
                        <CheckCircle2
                          className={`h-4 w-4 mt-0.5 text-${course.accentColor}-600 flex-shrink-0`}
                        />
                        <span>{feature}</span>
                      </li>
                    ))}
                  </ul>

                  {/* Stats */}
                  <div className="flex items-center gap-4 pt-4 border-t border-gray-200">
                    <div className="flex items-center gap-2">
                      <Clock className="h-4 w-4 text-gray-600" />
                      <span className="text-sm font-medium text-gray-700">
                        {course.duration}
                      </span>
                    </div>
                    <div className="flex items-center gap-2">
                      <Users className="h-4 w-4 text-gray-600" />
                      <span className="text-sm font-medium text-gray-700">
                        {course.students}
                      </span>
                    </div>
                    <div className="flex items-center gap-1">
                      <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                      <span className="text-sm font-medium text-gray-700">
                        {course.rating}
                      </span>
                    </div>
                  </div>

                  {/* Success Rate */}
                  <div className="bg-white/60 backdrop-blur-sm rounded-xl p-3 flex items-center justify-between">
                    <span className="text-sm font-medium text-gray-700">
                      Success Rate
                    </span>
                    <div className="flex items-center gap-2">
                      <TrendingUp className="h-4 w-4 text-green-600" />
                      <span className="text-lg font-bold text-green-600">
                        {course.successRate}
                      </span>
                    </div>
                  </div>

                  {/* Price & CTA */}
                  <div className="flex items-center justify-between pt-4">
                    <div>
                      <div className="flex items-baseline gap-2">
                        <span className="text-3xl font-bold text-gray-900">
                          {course.price}
                        </span>
                        <span className="text-sm text-gray-500 line-through">
                          {course.originalPrice}
                        </span>
                      </div>
                      <span className="text-xs text-gray-600">
                        One-time payment
                      </span>
                    </div>

                    <button
                      className={`flex items-center gap-2 px-6 py-3 bg-gradient-to-r ${course.gradient} text-white font-bold rounded-xl hover:scale-105 transition-all duration-300 shadow-lg group-hover:shadow-xl`}
                    >
                      <span>Explore</span>
                      <ArrowRight className="h-5 w-5 group-hover:translate-x-1 transition-transform" />
                    </button>
                  </div>
                </div>
              </div>
            );
          })}
        </div>

        {/* Comparison Section */}
        <div className="mt-16 bg-white rounded-3xl p-8 shadow-lg ring-1 ring-slate-200">
          <h2 className="text-3xl font-serif text-center mb-8 text-gray-900">
            Compare All Courses
          </h2>

          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b-2 border-gray-200">
                  <th className="text-left py-4 px-4 font-semibold text-gray-900">
                    Feature
                  </th>
                  {courses.map((course) => (
                    <th
                      key={course.id}
                      className="text-center py-4 px-4 font-semibold text-gray-900"
                    >
                      {course.title}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                <tr className="border-b border-gray-100">
                  <td className="py-4 px-4 font-medium text-gray-700">
                    Band Score
                  </td>
                  {courses.map((course) => (
                    <td
                      key={course.id}
                      className="py-4 px-4 text-center text-gray-600"
                    >
                      {course.bandScore}
                    </td>
                  ))}
                </tr>
                <tr className="border-b border-gray-100">
                  <td className="py-4 px-4 font-medium text-gray-700">
                    Duration
                  </td>
                  {courses.map((course) => (
                    <td
                      key={course.id}
                      className="py-4 px-4 text-center text-gray-600"
                    >
                      {course.duration}
                    </td>
                  ))}
                </tr>
                <tr className="border-b border-gray-100">
                  <td className="py-4 px-4 font-medium text-gray-700">
                    Success Rate
                  </td>
                  {courses.map((course) => (
                    <td
                      key={course.id}
                      className="py-4 px-4 text-center text-gray-600"
                    >
                      {course.successRate}
                    </td>
                  ))}
                </tr>
                <tr>
                  <td className="py-4 px-4 font-medium text-gray-700">Price</td>
                  {courses.map((course) => (
                    <td
                      key={course.id}
                      className="py-4 px-4 text-center font-bold text-gray-900"
                    >
                      {course.price}
                    </td>
                  ))}
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        {/* CTA Section */}
        <div className="mt-16 bg-gradient-to-br from-indigo-600 via-blue-600 to-cyan-600 rounded-3xl p-12 text-center text-white shadow-2xl relative overflow-hidden">
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
              Not Sure Which Course is Right for You?
            </h2>
            <p className="text-lg text-white/90 mb-8 max-w-2xl mx-auto">
              Take our free level assessment test to find your perfect course
              match and start your IELTS journey today.
            </p>
            <button className="px-8 py-4 bg-white text-indigo-600 font-bold rounded-xl hover:scale-105 transition-all duration-300 shadow-lg">
              Take Free Assessment
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

export default IELTSCoursesOverview;
