import React, { useEffect, useState } from 'react';
import { Eye, Edit, CheckCircle } from 'lucide-react';

export default function TestManagement() {
  const [papers, setPapers] = useState<any[]>([]);

  const fetchPapers = async () => {
      // Lấy tất cả paper (cần thêm param ở API để lấy cả Draft nếu là admin)
      const res = await fetch('http://localhost:8081/api/v1/papers?skill=ALL SKILLS&limit=100'); 
      const data = await res.json();
      setPapers(data.Items || []);
  };

  useEffect(() => { fetchPapers(); }, []);

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-slate-800">Test Library</h1>
        <button className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition font-medium">
            + Create New Test
        </button>
      </div>

      <div className="grid grid-cols-1 gap-4">
         {papers.map((paper) => (
             <div key={paper.Id} className="bg-white p-5 rounded-xl border border-slate-200 flex justify-between items-center shadow-sm hover:shadow-md transition">
                 <div>
                     <h3 className="font-bold text-lg text-slate-800">{paper.Name}</h3>
                     <p className="text-sm text-slate-500 mt-1">ID: {paper.Id} • Created: {new Date(paper.CreatedAt).toLocaleDateString()}</p>
                 </div>
                 <div className="flex items-center gap-3">
                     <span className={`px-3 py-1 rounded-full text-xs font-bold ${
                         paper.Status === 'PUBLISHED' ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-700'
                     }`}>
                         {paper.Status || 'PUBLISHED'}
                     </span>
                     <button className="p-2 text-slate-500 hover:bg-slate-100 rounded-lg" title="Edit">
                         <Edit className="w-5 h-5"/>
                     </button>
                     <button className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg" title="Preview">
                         <Eye className="w-5 h-5"/>
                     </button>
                 </div>
             </div>
         ))}
      </div>
    </div>
  );
}