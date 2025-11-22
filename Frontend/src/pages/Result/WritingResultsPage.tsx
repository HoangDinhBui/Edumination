import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { CheckCircle, FileText, Home, BarChart } from "lucide-react";
import edmLogo from "../../assets/img/edm-logo.png";

const WritingResultsPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { results, paperName } = location.state || {};

  if (!results) {
    return (
      <div className="w-screen h-screen flex flex-col items-center justify-center">
        <p>No results found. Please submit a test first.</p>
        <button
          onClick={() => navigate("/library")}
          className="mt-4 px-6 py-2 bg-blue-600 text-white rounded-lg"
        >
          Go to Library
        </button>
      </div>
    );
  }

  const averageScore = (
    results.reduce((sum, r) => sum + r.score, 0) / results.length
  ).toFixed(1);

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100 font-['Be_Vietnam_Pro']">
      {/* Header */}
      <nav className="w-full bg-white shadow-sm border-b border-slate-200">
        <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
          <img src={edmLogo} alt="EDM" className="h-8" />
          <button
            onClick={() => navigate("/library")}
            className="flex items-center gap-2 text-slate-600 hover:text-orange-600 transition"
          >
            <Home className="w-5 h-5" />
            <span>Back to Library</span>
          </button>
        </div>
      </nav>

      {/* Main Content */}
      <div className="max-w-5xl mx-auto px-6 py-12">
        {/* Success Banner */}
        <div className="bg-white rounded-2xl shadow-lg p-8 mb-8 border-2 border-green-500">
          <div className="flex items-center gap-4 mb-4">
            <CheckCircle className="w-16 h-16 text-green-500" />
            <div>
              <h1 className="text-3xl font-bold text-slate-800">
                🎉 Submission Successful!
              </h1>
              <p className="text-slate-600 mt-1">
                {paperName || "Your writing test"} has been graded
              </p>
            </div>
          </div>

          {/* Average Score */}
          <div className="flex items-center justify-center py-8">
            <div className="text-center">
              <p className="text-slate-600 text-lg mb-2">Overall Score</p>
              <div className="text-7xl font-bold text-orange-600">
                {averageScore}
                <span className="text-4xl text-slate-400">/10</span>
              </div>
            </div>
          </div>
        </div>

        {/* Individual Results */}
        <div className="space-y-6">
          {results.map((result, index) => (
            <div
              key={result.submissionId}
              className="bg-white rounded-xl shadow-md p-6 border border-slate-200"
            >
              <div className="flex items-start justify-between mb-4">
                <div className="flex items-center gap-3">
                  <FileText className="w-6 h-6 text-orange-600" />
                  <h2 className="text-xl font-semibold text-slate-800">
                    Task {index + 1}
                  </h2>
                </div>
                <div className="text-right">
                  <div className="text-4xl font-bold text-orange-600">
                    {result.score}/10
                  </div>
                  <p className="text-sm text-slate-500 mt-1">
                    {result.wordsCount} words
                  </p>
                </div>
              </div>

              {/* Feedback */}
              <div className="bg-slate-50 rounded-lg p-6 border border-slate-200">
                <h3 className="font-semibold text-slate-700 mb-3 flex items-center gap-2">
                  <BarChart className="w-5 h-5" />
                  Detailed Feedback
                </h3>
                <p className="text-slate-700 leading-relaxed whitespace-pre-wrap">
                  {result.feedback}
                </p>
              </div>

              {/* Stats */}
              <div className="grid grid-cols-3 gap-4 mt-6">
                <div className="text-center p-4 bg-blue-50 rounded-lg">
                  <p className="text-sm text-slate-600">Submission ID</p>
                  <p className="font-semibold text-slate-800">
                    #{result.submissionId}
                  </p>
                </div>
                <div className="text-center p-4 bg-green-50 rounded-lg">
                  <p className="text-sm text-slate-600">Word Count</p>
                  <p className="font-semibold text-slate-800">
                    {result.wordsCount}
                  </p>
                </div>
                <div className="text-center p-4 bg-purple-50 rounded-lg">
                  <p className="text-sm text-slate-600">Submitted</p>
                  <p className="font-semibold text-slate-800">
                    {new Date(result.createdAt).toLocaleDateString()}
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>

        {/* Actions */}
        <div className="flex gap-4 mt-8">
          <button
            onClick={() => navigate("/library")}
            className="flex-1 py-4 bg-orange-600 text-white rounded-xl font-semibold hover:bg-orange-700 transition shadow-lg"
          >
            Back to Library
          </button>
          <button
            onClick={() => window.print()}
            className="px-8 py-4 bg-slate-200 text-slate-700 rounded-xl font-semibold hover:bg-slate-300 transition"
          >
            Print Results
          </button>
        </div>
      </div>
    </div>
  );
};

export default WritingResultsPage;
