import React from "react";
import {
  ChevronDown,
  CheckCircle2,
  Star,
  ChevronLeft,
  ChevronRight,
  Mail,
  MapPin,
  Phone,
} from "lucide-react";

/**
 * EDM — HomePage (single-file preview)
 * - TailwindCSS layout, light & airy aesthetic
 * - Minimal interactivity (dropdowns on hover, simple carousels)
 * - No external state managers required
 */

const badges = [
  "Pioneering Learning Model University Lecture",
  "Rapidly upgrading your band level in just 90 hours of study",
  "Specialized training in IELTS/Children's English",
  "Commitment to output quality",
];

const partners = [
  { name: "British Council" },
  { name: "University of Cambridge" },
  { name: "idp" },
];

const stats = [
  { value: "8+", label: "Years of operation" },
  { value: "10+", label: "Facilities nationwide" },
  { value: "200+", label: "Excellent students 8.0+" },
  { value: "100%", label: "Specialized teachers" },
  { value: "5.000+", label: "Students reached the finish line" },
  { value: "10+", label: "Top Platinum Partners of IDP & BC" },
];

const programs = [
  {
    title: "IELTS at the center",
    desc: "A comprehensive IELTS training journey, specifically designed for each learner, fast-tracking foundation and language thinking across all ages and levels.",
    badge: "IELTS",
  },
  {
    title: "Kid & Teenager",
    desc: "An comprehensive English curriculum developed logically and scientifically, focusing on the unique realities of the Cambridge criteria.",
    badge: "Cambridge",
  },
  {
    title: "SAT Preparation",
    desc: "A scientific SAT program, competitively-focused, learning actively with essential test skills.",
    badge: "SAT",
  },
];

const campusImages = [
  // You can replace these links with your own static assets
  "https://images.unsplash.com/photo-1523580846011-d3a5bc25702b?q=80&w=1200&auto=format&fit=crop",
  "https://images.unsplash.com/photo-1523050854058-8df90110c9f1?q=80&w=1200&auto=format&fit=crop",
  "https://images.unsplash.com/photo-1460518451285-97b6aa326961?q=80&w=1200&auto=format&fit=crop",
];

function useCarousel<T>(items: T[]) {
  const [idx, setIdx] = React.useState(0);
  const next = () => setIdx((p) => (p + 1) % items.length);
  const prev = () => setIdx((p) => (p - 1 + items.length) % items.length);
  return { idx, next, prev };
}

const Dropdown: React.FC<{
  title: string;
  sections: { header?: string; items: string[] }[];
}> = ({ title, sections }) => {
  return (
    <div className="relative group">
      <button className="inline-flex items-center gap-1 text-slate-700 hover:text-slate-900 font-medium">
        {title}
        <ChevronDown className="h-4 w-4" />
      </button>
      <div className="absolute top-full left-1/2 -translate-x-1/2 hidden group-hover:flex gap-4 p-3">
        {sections.map((sec, i) => (
          <div
            key={i}
            className="bg-white/95 backdrop-blur shadow-xl ring-1 ring-slate-100 rounded-2xl p-3 w-56"
          >
            {sec.header && (
              <div className="text-[13px] font-semibold text-sky-700 mb-2">
                {sec.header}
              </div>
            )}
            <ul className="space-y-2">
              {sec.items.map((it) => (
                <li
                  key={it}
                  className="text-sm text-slate-600 hover:text-slate-900 cursor-pointer"
                >
                  {it}
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </div>
  );
};

const Hero: React.FC = () => {
  const { idx, next, prev } = useCarousel([0, 1, 2]);
  return (
    <section className="relative overflow-hidden">
      {/* background decorative blob */}
      <div className="absolute -right-40 -top-28 h-[520px] w-[520px] rounded-full bg-gradient-to-br from-sky-50 via-indigo-50 to-transparent blur-0" />
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pt-10 pb-16">
        <div className="grid lg:grid-cols-2 items-center gap-12">
          {/* Left copy */}
          <div>
            <div className="inline-flex items-center gap-2 mb-4">
              <img
                src="https://dummyimage.com/40x28/17a/fff&text=EDM"
                alt="EDM"
                className="h-7 rounded"
              />
            </div>
            <h1 className="text-4xl md:text-5xl font-serif tracking-tight text-slate-800">
              EDUMINATION <br />
              <span className="text-slate-700">English Center</span>
            </h1>
            <ul className="mt-6 space-y-3">
              {badges.map((b) => (
                <li key={b} className="flex items-start gap-3 text-slate-600">
                  <CheckCircle2 className="h-5 w-5 mt-0.5 text-emerald-500" />{" "}
                  {b}
                </li>
              ))}
            </ul>
            <div className="mt-8 flex flex-wrap items-center gap-4">
              <a
                href="#start"
                className="inline-flex items-center justify-center rounded-full bg-gradient-to-r from-rose-300 via-pink-300 to-emerald-300 px-6 py-3 text-slate-800 font-semibold shadow hover:opacity-95"
              >
                Start Your Free Practice
              </a>
              <div className="flex items-center gap-4 opacity-80">
                {partners.map((p) => (
                  <div
                    key={p.name}
                    className="text-xs text-slate-500 border rounded-md px-2 py-1 bg-white/60"
                  >
                    {p.name}
                  </div>
                ))}
              </div>
            </div>
          </div>

          {/* Right visual */}
          <div className="relative">
            <div className="relative mx-auto w-full max-w-[460px]">
              {/* circular bg */}
              <div className="absolute inset-0 -z-10">
                <div className="absolute left-1/2 top-2 -translate-x-1/2 h-[380px] w-[380px] rounded-full bg-gradient-to-b from-sky-100 to-indigo-50" />
                <div className="absolute left-10 top-20 h-24 w-24 rounded-full bg-sky-50" />
              </div>

              <div className="aspect-[4/5] w-full overflow-hidden rounded-[28px] bg-white shadow-xl ring-1 ring-slate-100 flex items-center justify-center">
                {/* Placeholder avatar */}
                <img
                  src="https://images.unsplash.com/photo-1607748851802-9fd8c6fef7a1?q=80&w=800&auto=format&fit=crop"
                  className="h-full object-cover"
                  alt="Student highlight"
                />
              </div>

              {/* Star badge */}
              <div className="absolute -left-6 top-1/2 -translate-y-1/2">
                <div className="relative">
                  <Star className="h-16 w-16 text-rose-500 fill-rose-500 rotate-6 drop-shadow" />
                  <div className="absolute inset-0 grid place-items-center text-white text-sm font-bold">
                    9.0
                    <span className="block text-[10px] font-semibold -mt-1">
                      IELTS
                    </span>
                  </div>
                </div>
              </div>

              {/* Carousel controls */}
              <button
                onClick={prev}
                aria-label="prev"
                className="absolute -left-8 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-white shadow ring-1 ring-slate-100 hover:bg-slate-50"
              >
                <ChevronLeft className="h-5 w-5 text-slate-700" />
              </button>
              <button
                onClick={next}
                aria-label="next"
                className="absolute -right-8 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-white shadow ring-1 ring-slate-100 hover:bg-slate-50"
              >
                <ChevronRight className="h-5 w-5 text-slate-700" />
              </button>

              {/* Caption */}
              <div className="mt-6 text-center">
                <div className="text-sky-700 font-semibold">Bui Dinh Hoang</div>
                <div className="text-xs text-slate-500 max-w-sm mx-auto">
                  a student of University of Transport and Communications – Ho
                  Chi Minh City Campus
                </div>
              </div>

              {/* Dots */}
              <div className="mt-2 flex items-center justify-center gap-2">
                {[0, 1, 2].map((i) => (
                  <span
                    key={i}
                    className={`h-2 w-2 rounded-full ${
                      idx === i ? "bg-sky-500" : "bg-slate-300"
                    }`}
                  />
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

const Stats: React.FC = () => (
  <section className="relative py-14">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <div className="grid grid-cols-2 md:grid-cols-3 gap-6">
        {stats.map((s) => (
          <div
            key={s.label}
            className="rounded-3xl bg-white shadow-sm ring-1 ring-slate-100 p-6"
          >
            <div className="text-3xl font-bold text-slate-800">{s.value}</div>
            <div className="text-slate-500 mt-1 text-sm">{s.label}</div>
          </div>
        ))}
      </div>
    </div>
  </section>
);

const WhyDifferent: React.FC = () => (
  <section className="relative overflow-hidden">
    <div className="absolute inset-x-0 -top-24 h-24 bg-gradient-to-b from-transparent to-sky-50" />
    <div className="bg-sky-50/60">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-16">
        <h2 className="text-center text-2xl md:text-3xl font-serif text-slate-800">
          What makes <span className="text-sky-700">EDUMINATION</span>{" "}
          different?
        </h2>
        <div className="mt-10 grid lg:grid-cols-2 gap-10 items-center">
          <div className="overflow-hidden rounded-3xl shadow-lg ring-1 ring-slate-200">
            <img
              src="https://images.unsplash.com/photo-1523580846011-d3a5bc25702b?q=80&w=1200&auto=format&fit=crop"
              className="w-full h-full object-cover"
              alt="class"
            />
          </div>
          <div className="space-y-4 text-slate-600">
            <p>
              Edumination is an educational organization specializing in IELTS
              and SAT test preparation, as well as teaching English at all
              levels.
            </p>
            <ul className="space-y-3">
              {[
                "Effective classroom-lecture learning model; leveraging knowledge",
                "Exclusive course 'Lecture' structure alongside blended classes",
                "Practical-focused activities: debate, academic and everyday English",
                "100% lesson-based curriculum for all students",
              ].map((t) => (
                <li key={t} className="flex items-start gap-3">
                  <CheckCircle2 className="h-5 w-5 mt-0.5 text-emerald-500" />
                  <span>{t}</span>
                </li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    </div>
  </section>
);

const ProgramCards: React.FC = () => (
  <section className="py-16">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
      <h3 className="text-center text-2xl md:text-3xl font-serif text-slate-800 mb-10">
        High quality training program
      </h3>
      <div className="grid md:grid-cols-3 gap-6">
        {programs.map((p) => (
          <article
            key={p.title}
            className="relative rounded-3xl bg-gradient-to-b from-white to-slate-50 ring-1 ring-slate-100 shadow-sm p-6 hover:shadow-md transition-shadow"
          >
            <div className="text-[11px] inline-flex items-center gap-1 rounded-full bg-sky-50 text-sky-700 font-semibold px-3 py-1">
              {p.badge}
            </div>
            <h4 className="mt-3 text-lg font-semibold text-slate-800">
              {p.title}
            </h4>
            <p className="mt-2 text-sm text-slate-600 leading-relaxed">
              {p.desc}
            </p>
          </article>
        ))}
      </div>
    </div>
  </section>
);

const CampusCarousel: React.FC = () => {
  const { idx, next, prev } = useCarousel(campusImages);
  return (
    <section className="py-14">
      <div className="mx-auto max-w-6xl px-4 sm:px-6 lg:px-8">
        <div className="relative overflow-hidden rounded-3xl shadow-lg ring-1 ring-slate-200">
          <img
            src={campusImages[idx]}
            className="w-full h-[420px] object-cover"
            alt="campus"
          />
          <button
            onClick={prev}
            className="absolute left-3 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-white/90 shadow hover:bg-white"
          >
            <ChevronLeft className="h-5 w-5" />
          </button>
          <button
            onClick={next}
            className="absolute right-3 top-1/2 -translate-y-1/2 grid place-items-center h-10 w-10 rounded-full bg-white/90 shadow hover:bg-white"
          >
            <ChevronRight className="h-5 w-5" />
          </button>
        </div>
      </div>
    </section>
  );
};

const Footer: React.FC = () => (
  <footer className="border-t border-slate-200 py-12">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 grid md:grid-cols-3 gap-10">
      <div>
        <div className="flex items-center gap-2">
          <img
            src="https://dummyimage.com/40x28/17a/fff&text=EDM"
            alt="EDM"
            className="h-7 rounded"
          />
          <span className="font-semibold text-slate-800">Edumination</span>
        </div>
        <p className="mt-3 text-sm text-slate-600 max-w-sm">
          The leading English center specializing in IELTS and SAT exam
          preparation and teaching English for all levels.
        </p>
        <div className="mt-4 space-y-2 text-sm text-slate-600">
          <div className="flex gap-2 items-start">
            <Mail className="h-4 w-4 mt-0.5" /> eduminationielts@gmail.com
          </div>
          <div className="flex gap-2 items-start">
            <MapPin className="h-4 w-4 mt-0.5" /> 450 Le Van Viet, Tang Nhon
            Phu, Thu Duc, TPHCM
          </div>
          <div className="flex gap-2 items-start">
            <Phone className="h-4 w-4 mt-0.5" /> 0866704845
          </div>
        </div>
        <div className="text-xs text-slate-400 mt-4">© 2025 Edumination</div>
      </div>
      <div>
        <div className="font-semibold text-slate-800 mb-3">
          IELTS Exam Library
        </div>
        <ul className="space-y-2 text-sm text-slate-600">
          <li className="hover:text-slate-900">IELTS Listening Test</li>
          <li className="hover:text-slate-900">IELTS Reading Test</li>
          <li className="hover:text-slate-900">IELTS Writing Test</li>
          <li className="hover:text-slate-900">IELTS Speaking Test</li>
          <li className="hover:text-slate-900">IELTS Test Collection</li>
        </ul>
      </div>
      <div>
        <div className="font-semibold text-slate-800 mb-3">IELTS Courses</div>
        <ul className="space-y-2 text-sm text-slate-600">
          <li className="hover:text-slate-900">IELTS Foundation (0.0–5.0)</li>
          <li className="hover:text-slate-900">IELTS 5.5–6.0 Booster</li>
          <li className="hover:text-slate-900">IELTS 6.0–7.5 Intensive</li>
          <li className="hover:text-slate-900">IELTS 7.5–9.0 Mastery</li>
        </ul>
      </div>
    </div>
  </footer>
);

const Navbar: React.FC = () => {
  return (
    <header className="sticky top-0 z-40 bg-white/80 backdrop-blur border-b border-slate-200">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div className="flex items-center gap-8">
          <a href="#" className="flex items-center gap-2">
            <img
              src="https://dummyimage.com/40x28/17a/fff&text=EDM"
              className="h-7 rounded"
              alt="logo"
            />
          </a>
          <nav className="hidden md:flex items-center gap-6">
            <a className="text-slate-700 hover:text-slate-900" href="#">
              Home
            </a>
            <Dropdown
              title="IELTS Exam Library"
              sections={[
                {
                  header: "",
                  items: [
                    "IELTS Listening Test",
                    "IELTS Reading Test",
                    "IELTS Writing Test",
                    "IELTS Speaking Test",
                    "IELTS Test Collection",
                  ],
                },
              ]}
            />
            <Dropdown
              title="IELTS Course"
              sections={[
                {
                  header: "IELTS Foundation (0.0–5.0)",
                  items: [
                    "IELTS 5.5–6.0 Booster",
                    "IELTS 6.0–7.5 Intensive",
                    "IELTS 7.5–9.0 Mastery",
                  ],
                },
              ]}
            />
            <a className="text-slate-700 hover:text-slate-900" href="#">
              Ranking
            </a>
          </nav>
        </div>
        <div className="flex items-center gap-3">
          <a
            href="#signin"
            className="text-slate-600 hover:text-slate-900 text-sm"
          >
            Sign in
          </a>
          <a
            href="#signup"
            className="text-sm font-semibold text-white bg-gradient-to-r from-emerald-400 to-sky-400 px-4 py-2 rounded-full shadow hover:opacity-95"
          >
            Sign up
          </a>
        </div>
      </div>
    </header>
  );
};

export default function HomePage() {
  return (
    <div className="min-h-screen bg-white text-slate-800">
      <Navbar />
      <Hero />
      <Stats />
      <WhyDifferent />
      <ProgramCards />
      <CampusCarousel />
      <Footer />
    </div>
  );
}
