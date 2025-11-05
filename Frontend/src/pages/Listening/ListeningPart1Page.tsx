import React, { useState, useEffect } from 'react';
import { 
    Play, Pause, Volume2, Maximize, Clock, CheckCircle, 
    ChevronDown, Filter, Search, User, Briefcase, BookOpen, BarChart, UserCheck 
} from 'lucide-react';

// === MOCK DATA: Dữ liệu giả lập cho bài thi ===
const mockQuestions = [
    { id: 'part1', type: 'FORM_COMPLETION', title: 'Questions 1-5', instruction: 'Complete the following form with NO MORE THAN THREE WORDS AND/OR A NUMBER for each answer.' },
    { id: 'q6', type: 'MULTIPLE_CHOICE_SINGLE', title: 'Question 6', instruction: 'Mark ONE letter that represents the correct answer.' },
    { id: 'q7-10', type: 'BLANK_FILL', title: 'Questions 7-10', instruction: 'Fill in the blanks with NO MORE THAN THREE WORDS for each answer.' },
    // Thêm một dạng câu hỏi nữa để minh họa
    { id: 'q11-14', type: 'TABLE_COMPLETION', title: 'Questions 11-14', instruction: 'Complete the table below with NO MORE THAN TWO WORDS...' },
];


// === COMPONENTS CỐ ĐỊNH ===

// 1. Navbar (Giữ nguyên)
const Navbar = () => (
    <header className="sticky top-0 z-40 bg-white/95 backdrop-blur-sm border-b border-slate-200">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
            <a href="#" className="flex items-center text-2xl font-black text-sky-700 tracking-wider">
                 EDM 
            </a>
            <div className="flex items-center space-x-4">
                <div className="flex items-center text-slate-600 font-semibold text-sm">
                    <Clock className="w-4 h-4 mr-1 text-sky-600" />
                    32 minutes remaining
                </div>
                <button className="px-5 py-2 bg-sky-500 text-white font-semibold rounded-lg shadow-md hover:bg-sky-600 transition duration-150 flex items-center">
                    Submit
                    <CheckCircle className="w-5 h-5 ml-2" />
                </button>
            </div>
        </div>
    </header>
);


// === COMPONENTS CÂU HỎI CỤ THỂ (Dynamic UI) ===

// Component 1: Dạng Điền Form (Questions 1-5) - Tối ưu cho màn hình rộng
const FormCompletion = ({ data }) => (
    <div className="space-y-4">
        <h4 className="font-semibold text-lg text-slate-800 mb-4">PERSONAL DETAILS FOR HOMESTAY APPLICATION</h4>
        <div className="grid grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-x-12 gap-y-4 text-sm">
            {['First name', 'Family name', 'Gender', 'Age', 'Passport number', 'Nationality', 'Course enrolled', 'Length of the course', 'Homestay time'].map((label, index) => (
                <div key={label} className="flex items-center border-b border-slate-200 py-2">
                    <label className="w-2/5 text-slate-600 font-medium">{label}</label>
                    <div className="w-3/5 relative">
                        <input
                            type="text"
                            placeholder="Your answer"
                            className="w-full px-3 py-1 border border-slate-300 rounded-md focus:border-sky-500 focus:ring-sky-500 text-slate-800"
                        />
                        <span className="absolute -left-7 top-1 text-sky-600 font-bold">{index + 1}</span>
                    </div>
                </div>
            ))}
        </div>
    </div>
);

// Component 2: Dạng Trắc nghiệm (Question 6) (Giữ nguyên)
const MultipleChoice = ({ data, type = 'single' }) => {
    const options = [
        { label: 'A big family with many young children', key: 'A' },
        { label: 'A family without smoker or drinkers', key: 'B' },
        { label: 'A family without any pets', key: 'C' },
        { label: 'A family with many animals or pets', key: 'D' },
    ];
    
    return (
        <div>
            <h4 className="text-base font-semibold text-slate-700 mb-3">Which kind of family does the girls prefer?</h4>
            <div className="space-y-2">
                {options.map(option => (
                    <div key={option.key} className="flex items-center p-2 rounded-lg hover:bg-slate-50 cursor-pointer">
                        <input
                            id={`q6-${option.key}`}
                            name="q6-answer"
                            type="radio"
                            className={`w-4 h-4 text-sky-600 border-slate-300 focus:ring-sky-500`}
                        />
                        <label htmlFor={`q6-${option.key}`} className="ml-3 text-slate-700 font-normal">
                            <span className="font-medium mr-2 text-sky-600">{option.key}</span> {option.label}
                        </label>
                    </div>
                ))}
            </div>
        </div>
    );
};

// Component 3: Dạng Điền từ vào chỗ trống (Questions 7-10) (Giữ nguyên)
const BlankFill = ({ data }) => {
    const sentences = [
        { id: 7, text: "Although the girl is not a vegetarian, she doesn't eat a lot of meat. Her favourite food is", placeholder: "Your answer", suffix: "." },
        { id: 8, text: "The girls has given up playing handball. Now, she just play", placeholder: "Your answer", suffix: " with her friends at weekends." },
        { id: 9, text: "The girl does not like the bus because they are always late. She would rather", placeholder: "Your answer", suffix: "." },
        { id: 10, text: "The girl can get the information about the homestay family that she wants", placeholder: "Your answer", suffix: "." },
    ];
    
    return (
        <div className="space-y-6">
            <h4 className="text-base font-semibold text-slate-700 mb-3">Fill in the blanks with <span className="text-red-500">NO MORE THAN THREE WORDS</span> for each answer.</h4>
            {sentences.map(item => (
                <div key={item.id} className="flex flex-wrap items-center text-base text-slate-700">
                    <span className="font-bold mr-3 text-sky-600">{item.id}</span>
                    <p className="mr-2">{item.text}</p>
                    <input
                        type="text"
                        placeholder={item.placeholder}
                        className="flex-grow max-w-sm px-3 py-1 border-b-2 border-slate-300 focus:border-sky-500 focus:outline-none text-slate-800"
                    />
                    <p className="ml-2">{item.suffix}</p>
                </div>
            ))}
        </div>
    );
};

// Component 4: Dạng Điền Bảng (Table Completion) - Mới
const TableCompletion = ({ data }) => {
    return (
        <div className="overflow-x-auto">
            <h4 className="font-semibold text-lg text-slate-800 mb-4">APPOINTMENT SUMMARY</h4>
            <table className="min-w-full divide-y divide-slate-200 border border-slate-200 rounded-lg">
                <thead className="bg-slate-50">
                    <tr>
                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Date</th>
                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Time</th>
                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Topic</th>
                        <th className="px-6 py-3 text-left text-xs font-semibold text-slate-500 uppercase tracking-wider">Notes</th>
                    </tr>
                </thead>
                <tbody className="bg-white divide-y divide-slate-200">
                    {/* Hàng 1 */}
                    <tr className="hover:bg-slate-50">
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">Monday</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">11:00 AM</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">Meeting with Manager</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm relative">
                            <input type="text" placeholder="Your answer (11)" className="w-full px-2 py-1 border-b border-slate-300 focus:border-sky-500 focus:outline-none" />
                            <span className="absolute left-1 top-1 text-xs font-bold text-sky-600">11</span>
                        </td>
                    </tr>
                    {/* Hàng 2 */}
                    <tr className="hover:bg-slate-50">
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">Tuesday</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">1:30 PM</td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm relative">
                            <input type="text" placeholder="Your answer (12)" className="w-full px-2 py-1 border-b border-slate-300 focus:border-sky-500 focus:outline-none" />
                            <span className="absolute left-1 top-1 text-xs font-bold text-sky-600">12</span>
                        </td>
                        <td className="px-6 py-4 whitespace-nowrap text-sm text-slate-700">Discuss marketing plan</td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};


// 5. Component Linh hoạt (Dynamic Rendering)
const QuestionContainer = ({ data }) => {
    
    const renderQuestionUI = () => {
        switch (data.type) {
            case 'FORM_COMPLETION':
                return <FormCompletion data={data} />;
            case 'MULTIPLE_CHOICE_SINGLE':
                return <MultipleChoice data={data} type="single" />; 
            case 'BLANK_FILL':
                return <BlankFill data={data} />;
            case 'TABLE_COMPLETION':
                return <TableCompletion data={data} />;
            default:
                return <div className="p-6 text-red-500">Error: Unknown question type: {data.type}</div>;
        }
    };

    return (
        <div className="p-6 bg-white rounded-xl shadow-lg border border-slate-100 mb-8">
            <h3 className="text-xl font-bold text-slate-800 mb-2">{data.title}</h3>
            <p className="text-sm text-slate-600 font-medium mb-6">{data.instruction}</p>
            {renderQuestionUI()}
        </div>
    );
};


// === COMPONENT CHÍNH TRANG NGHE ===
export default function ListeningPage() {
    const [isPlaying, setIsPlaying] = useState(false);
    const [activeQuestion, setActiveQuestion] = useState(1); 

    const totalQuestions = 40;
    const parts = [1, 2, 3, 4]; // 4 Part IELTS

    return (
        <div className="min-h-screen bg-slate-50">
            <Navbar />

            {/* === THANH CHỌN PART (Part Selector Bar) - Cố định (Fixed) === */}
            {/* Đã mở rộng max-w-5xl thành max-w-7xl để đồng bộ */}
            <div className="fixed bottom-20 left-0 right-0 z-20 bg-white border-t border-b border-slate-200 shadow-lg">
                <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex justify-between items-center">
                    
                    {/* Thanh tiến trình Part */}
                    <div className="flex items-center space-x-2 text-sm font-semibold text-slate-700">
                        {parts.map(partNum => (
                            <div key={partNum} className="flex items-center">
                                <span className={`mr-2 py-1 px-3 rounded-full ${
                                    partNum === 1 ? 'bg-sky-500 text-white shadow-md' : 'bg-slate-100 text-slate-500 hover:bg-slate-200'
                                } cursor-pointer transition duration-150`}>
                                    Part {partNum}
                                </span>
                                {partNum < 4 && <div className="w-px h-6 bg-slate-300 mx-2"></div>}
                            </div>
                        ))}
                    </div>

                    {/* Vùng chọn số câu hỏi (Questions 1 - 40) */}
                    <div className="flex space-x-1 p-1 bg-slate-100 rounded-full">
                        {/* Chỉ hiển thị các nút câu hỏi liên quan đến Part hiện tại (1-10) để tránh quá tải trên max-w-7xl */}
                        {[...Array(10)].map((_, index) => { // Chỉ hiển thị 10 câu đầu
                            const qNum = index + 1;
                            const isAnswered = qNum === 3; 

                            return (
                                <button 
                                    key={qNum} 
                                    onClick={() => setActiveQuestion(qNum)}
                                    className={`w-8 h-8 rounded-full text-xs font-medium transition duration-150 relative
                                        ${qNum === activeQuestion ? 'bg-sky-600 text-white shadow-inner' : ''}
                                        ${qNum !== activeQuestion ? 'bg-white text-sky-600 border border-sky-300 hover:bg-sky-50' : ''}
                                        ${isAnswered && qNum !== activeQuestion ? 'bg-green-100 text-green-700 border border-green-300 hover:bg-green-200' : ''}
                                    `}
                                >
                                    {qNum}
                                </button>
                            );
                        })}
                        {/* Thêm nút Next Part */}
                        <div className="w-px h-8 bg-slate-300 mx-2"></div>
                        <button className="px-3 py-1 text-sm font-semibold text-slate-700 hover:text-sky-600">
                            Next Part &gt;
                        </button>
                    </div>
                </div>
            </div>

            {/* === Nội dung chính (ĐÃ MỞ RỘNG: max-w-7xl) === */}
            <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-10 pb-56"> 
                <h1 className="text-3xl font-bold text-slate-800 mb-2">Part 1</h1>
                <p className="text-lg text-slate-600 mb-8">The housing officer takes some details from the girl.</p>
                
                {mockQuestions.map(q => (
                    <QuestionContainer key={q.id} data={q} />
                ))}
            </div>
            
            {/* Audio Player Bar (Cố định ở dưới cùng) */}
            <div className="fixed bottom-0 left-0 right-0 bg-slate-800 text-white shadow-2xl z-30 p-4 h-16">
                <div className="mx-auto max-w-7xl flex items-center justify-between">
                    <div className="flex items-center space-x-6">
                        <button onClick={() => setIsPlaying(!isPlaying)} className="p-2 bg-sky-600 rounded-full hover:bg-sky-700 transition">
                            {isPlaying ? <Pause className="w-5 h-5" /> : <Play className="w-5 h-5" />}
                        </button>
                        <div className="w-96 bg-slate-700 h-1 rounded-full relative">
                            <div className="absolute top-0 left-0 h-1 bg-sky-500 w-[40%] rounded-full"></div>
                            <span className="absolute -top-1.5 left-[40%] w-4 h-4 bg-white border-2 border-sky-500 rounded-full cursor-pointer"></span>
                        </div>
                        <div className="text-sm text-slate-300">01:32 / 04:05</div>
                    </div>
                    <div className="flex items-center space-x-4">
                        <Volume2 className="w-5 h-5 text-slate-400" />
                        <Maximize className="w-5 h-5 text-slate-400 cursor-pointer hover:text-white" />
                    </div>
                </div>
            </div>
        </div>
    );
}