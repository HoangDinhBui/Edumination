import React, { useState } from "react";
import { Camera } from "lucide-react";
import Navbar from "../../components/Navbar";

export default function CreateProfilePage() {
  const [step] = useState(1);
  const [avatar, setAvatar] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    dateOfBirth: "",
    phoneNumber: "",
  });

  const handleAvatarChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const img = e.target.files?.[0];
    if (!img) return;

    const reader = new FileReader();
    reader.onload = () => setAvatar(reader.result as string);
    reader.readAsDataURL(img);
  };

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <div className="min-h-screen mt-12 flex flex-col">
      <Navbar />

      <div className="flex-1 w-full">
        {/* Header */}
        <div className="text-center pt-10">
          <h1
            className="text-3xl font-extrabold text-[#294563]"
            style={{ fontFamily: "'Paytone One', sans-serif" }}
          >
            CREATE YOUR PROFILE
          </h1>

          {/* Steps */}
          <div className="flex justify-center items-center gap-2 mt-6">
            {[1, 2].map((n) => (
              <React.Fragment key={n}>
                <div
                  className={`w-12 h-12 rounded-full flex items-center justify-center font-bold text-lg transition-all ${
                    step === n
                      ? "bg-[#3B5998] text-white shadow-md"
                      : "bg-gray-300 text-white"
                  }`}
                >
                  {n}
                </div>
                {n < 2 && (
                  <div className="flex items-center gap-6 mx-1">
                    <div className="w-2 h-2 rounded-full bg-gray-400"></div>
                    <div className="w-2 h-2 rounded-full bg-gray-400"></div>
                    <div className="w-2 h-2 rounded-full bg-gray-400"></div>
                  </div>
                )}
              </React.Fragment>
            ))}
          </div>
        </div>

        {/* About you! heading */}
        <h2 className="text-[#3B5998] text-xl font-semibold text-center mt-10 mb-8">
          About you!
        </h2>

        {/* Center container */}
        <div className="flex justify-center w-full">
          {/* Full width grid */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-5 w-full max-w-6xl px-8">
            {/* Left - Avatar */}
            <div className="flex justify-end items-start py-8 pr-10">
              <div className="relative">
                {/* Avatar label */}
                <div className="absolute -top-6 left-1/2 -translate-x-1/2 text-sm text-gray-600 font-medium">
                  Avatar
                </div>

                <div className="w-[220px] h-[220px] border-2 border-[#3B5998] rounded-2xl shadow-sm bg-white flex items-center justify-center overflow-hidden">
                  {avatar ? (
                    <img
                      src={avatar}
                      className="w-full h-full object-cover"
                      alt="Avatar"
                    />
                  ) : (
                    <div className="text-center text-slate-400">
                      <Camera className="w-12 h-12 mx-auto mb-2" />
                      <p className="text-xs">No Avatar</p>
                    </div>
                  )}
                </div>

                {/* Upload Button */}
                <label className="absolute -bottom-5 left-1/2 -translate-x-1/2">
                  <div className="px-6 py-2 bg-[#3B5998] text-white rounded-full shadow-md flex items-center gap-2 hover:bg-[#2d4373] text-sm cursor-pointer">
                    Choose Picture
                  </div>
                  <input
                    type="file"
                    accept="image/*"
                    className="hidden"
                    onChange={handleAvatarChange}
                  />
                </label>
              </div>
            </div>

            {/* Right - Form */}
            <div className="flex justify-start items-start py-2 pl-10">
              <div className="space-y-4 w-full pr-16">
                {[
                  { key: "firstName", label: "First Name", required: true },
                  { key: "lastName", label: "Last Name", required: true },
                  { key: "email", label: "Email", required: true },
                  {
                    key: "dateOfBirth",
                    label: "Date of birth",
                    type: "date",
                    required: true,
                    placeholder: "nn/mm/yyyy",
                  },
                  { key: "phoneNumber", label: "Phone Number" },
                ].map((item) => (
                  <div
                    key={item.key}
                    className="grid grid-cols-[200px_1fr] gap-8 items-center"
                  >
                    <label className="text-[#3B5998] text-sm font-medium text-left">
                      {item.label}{" "}
                      {item.required && (
                        <span className="text-red-500">(*)</span>
                      )}
                    </label>

                    <input
                      type={item.type || "text"}
                      value={formData[item.key as keyof typeof formData]}
                      onChange={(e) => handleChange(item.key, e.target.value)}
                      placeholder={item.placeholder}
                      className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:ring-1 focus:ring-[#3B5998] focus:border-[#3B5998] text-sm"
                    />
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>

        {/* Save Button */}
        <div className="mt-10 pb-10 flex justify-center">
          <button className="px-12 py-2.5 bg-[#3B5998] text-white font-semibold rounded-full shadow-md hover:bg-[#2d4373] text-base">
            Save
          </button>
        </div>
      </div>
    </div>
  );
}
