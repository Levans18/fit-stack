import { motion } from 'framer-motion'

export default function LandingPage() {
  return (
    <div className="relative h-screen w-full overflow-hidden bg-white text-black font-sans flex flex-col">
      {/* Floating background blob */}
      <div className="absolute inset-0 z-0 overflow-hidden">
        <div className="absolute -top-40 -left-40 w-[600px] h-[600px] bg-blue-300 opacity-30 rounded-full blur-3xl animate-pulse-slow" />
        <div className="absolute bottom-0 right-0 w-[400px] h-[400px] bg-blue-500 opacity-20 rounded-full blur-2xl animate-pulse-fast" />
      </div>

      {/* Header */}
      <header className="z-10 px-6 py-5 border-b border-gray-200">
        <div className="max-w-7xl mx-auto flex items-center justify-between">
          <h1 className="text-2xl font-bold">FitStack</h1>
          <nav className="space-x-4">
            <a href="/login" className="text-sm text-gray-700 hover:underline">Log in</a>
            <a
              href="/register"
              className="px-4 py-2 text-sm bg-black text-white rounded-md hover:bg-gray-900 transition"
            >
              Get Started
            </a>
          </nav>
        </div>
      </header>

      {/* Hero Section */}
      <main className="z-10 flex-1 flex items-center justify-center px-6 text-center">
        <motion.div
          className="max-w-2xl"
          initial={{ opacity: 0, y: 40 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, ease: 'easeOut' }}
        >
          <h2 className="text-5xl sm:text-6xl font-extrabold leading-tight mb-6 text-brand-dark">
            Track your fitness.
            <br />
            Level up your life.
          </h2>
          <p className="text-lg text-gray-600 mb-8">
            FitStack helps you build better workout habits, stay consistent, and see real progress.
          </p>
          <motion.a
            href="/register"
            className="inline-block px-6 py-3 bg-brand text-white text-base font-medium rounded-lg hover:bg-brand-dark transition"
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.98 }}
          >
            Start Tracking
          </motion.a>
        </motion.div>
      </main>

      {/* Footer */}
      <footer className="z-10 text-sm text-gray-400 text-center py-4">
        © 2025 FitStack. Built with 💪
      </footer>
    </div>
  )
}