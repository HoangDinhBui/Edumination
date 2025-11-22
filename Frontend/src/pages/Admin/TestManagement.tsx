import React, { useEffect, useState } from 'react';
import { Eye, Edit, CheckCircle } from 'lucide-react';

export default function TestManagement() {
        const [editModal, setEditModal] = useState<{ show: boolean, id: number | null, title: string }>({ show: false, id: null, title: "" });

        const openEditModal = (paper: any) => {
            setEditModal({ show: true, id: paper.Id, title: paper.Name });
        };

        const handleEditTest = async (e: React.FormEvent) => {
            e.preventDefault();
            try {
                const token = localStorage.getItem("Token");
                const API_URL = `http://localhost:8081/api/v1/papers/${editModal.id}`;
                const body = { Title: editModal.title };
                const res = await fetch(API_URL, {
                    method: "PATCH",
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(body)
                });
                if (!res.ok) throw new Error("Edit test failed");
                await fetchPapers();
                setEditModal({ show: false, id: null, title: "" });
                alert("Test updated successfully!");
            } catch (err) {
                alert("Failed to update test.");
                console.error(err);
            }
        };
    const [papers, setPapers] = useState<any[]>([]);
    const [showModal, setShowModal] = useState(false);
    const [newTest, setNewTest] = useState({ title: "", uploadMethod: "MANUAL", pdfAssetId: "" });

    const handleCreateTest = async (e?: React.FormEvent) => {
        if (e) e.preventDefault();
        try {
            const token = localStorage.getItem("Token");
            const API_URL = "http://localhost:8081/api/v1/papers";
            const body = {
                Title: newTest.title,
                UploadMethod: newTest.uploadMethod,
                PdfAssetId: newTest.pdfAssetId || null
            };
            const res = await fetch(API_URL, {
                method: "POST",
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body)
            });
            if (!res.ok) throw new Error("Create test failed");
            await fetchPapers();
            setShowModal(false);
            setNewTest({ title: "", uploadMethod: "MANUAL", pdfAssetId: "" });
            alert("Test created successfully!");
        } catch (err) {
            alert("Failed to create test.");
            console.error(err);
        }
    };

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
                <button
                    className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition font-medium"
                    onClick={() => setShowModal(true)}
                >
                        + Create New Test
                </button>
                {showModal && (
                    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                        <form
                            className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                            onSubmit={handleCreateTest}
                        >
                            <button
                                type="button"
                                className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                                onClick={() => setShowModal(false)}
                            >×</button>
                            <h2 className="text-xl font-bold mb-6 text-slate-800">Create New Test</h2>
                            <div className="mb-4">
                                <label className="block text-sm font-medium mb-1">Title</label>
                                <input
                                    type="text"
                                    className="w-full border border-slate-300 rounded-lg px-3 py-2"
                                    value={newTest.title}
                                    onChange={e => setNewTest(t => ({ ...t, title: e.target.value }))}
                                    required
                                />
                            </div>
                            <div className="mb-4">
                                <label className="block text-sm font-medium mb-1">Upload Method</label>
                                <select
                                    className="w-full border border-slate-300 rounded-lg px-3 py-2"
                                    value={newTest.uploadMethod}
                                    onChange={e => setNewTest(t => ({ ...t, uploadMethod: e.target.value }))}
                                >
                                    <option value="MANUAL">MANUAL</option>
                                    <option value="PDF_PARSER">PDF_PARSER</option>
                                </select>
                            </div>
                            <div className="mb-6">
                                <label className="block text-sm font-medium mb-1">PDF Asset Id (optional)</label>
                                <input
                                    type="text"
                                    className="w-full border border-slate-300 rounded-lg px-3 py-2"
                                    value={newTest.pdfAssetId}
                                    onChange={e => setNewTest(t => ({ ...t, pdfAssetId: e.target.value }))}
                                />
                            </div>
                            <button
                                type="submit"
                                className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                            >Create</button>
                        </form>
                    </div>
                )}
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
                     <button
                         className="p-2 text-slate-500 hover:bg-slate-100 rounded-lg"
                         title="Edit"
                         onClick={() => openEditModal(paper)}
                     >
                         <Edit className="w-5 h-5"/>
                     </button>
                                {editModal.show && (
                                    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
                                        <form
                                            className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md relative"
                                            onSubmit={handleEditTest}
                                        >
                                            <button
                                                type="button"
                                                className="absolute top-3 right-3 text-slate-400 hover:text-red-500 text-xl"
                                                onClick={() => setEditModal({ show: false, id: null, title: "" })}
                                            >×</button>
                                            <h2 className="text-xl font-bold mb-6 text-slate-800">Edit Test</h2>
                                            <div className="mb-4">
                                                <label className="block text-sm font-medium mb-1">Title</label>
                                                <input
                                                    type="text"
                                                    className="w-full border border-slate-300 rounded-lg px-3 py-2"
                                                    value={editModal.title}
                                                    onChange={e => setEditModal(m => ({ ...m, title: e.target.value }))}
                                                    required
                                                />
                                            </div>
                                            <button
                                                type="submit"
                                                className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 font-medium w-full"
                                            >Save</button>
                                        </form>
                                    </div>
                                )}
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