import { motion } from 'framer-motion'
import { Link, useNavigate } from 'react-router-dom'
import { ArrowLeft } from 'lucide-react'
import { useState } from 'react'

export default function LoginPage() {
  const [usernameOrEmail, setUsernameOrEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  const handleLogin = async (usernameOrEmail: string, password: string) => {
    setLoading(true)
    setError(null)

    try {
      const res = await fetch('http://localhost:5168/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ usernameOrEmail, password }),
      })

      if (!res.ok) {
        const err = await res.text()
        throw new Error(err || 'Login failed')
      }

      const data = await res.json()
      localStorage.setItem('token', data.token)

      navigate('/dashboard')
    } catch (err) {
        if (err instanceof Error) {
          setError(err.message)
        } else {
          setError('Something went wrong')
        }
    } finally {
      setLoading(false)
    }
  }

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    handleLogin(usernameOrEmail, password)
  }

  return (
    <div className="h-screen flex items-center justify-center bg-gradient-to-br from-white via-gray-100 to-white px-4 relative">

      {/* Back arrow */}
      <Link
        to="/"
        className="absolute top-6 left-6 flex items-center text-blue-600 hover:underline text-sm z-20"
      >
        <ArrowLeft className="w-4 h-4 mr-1" />
        Back to home
      </Link>

      <motion.div
        className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8 border border-border z-10"
        initial={{ opacity: 0, y: 24 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5, ease: 'easeOut' }}
      >
        <h2 className="text-3xl font-bold text-blue-900 mb-6 text-center">
          Welcome back 👋
        </h2>

        <form className="space-y-5" onSubmit={onSubmit}>
          <div>
            <label htmlFor="usernameOrEmail" className="block text-sm text-gray-700 mb-1">
              Username or Email
            </label>
            <input
              type="usernameOrEmail"
              id="usernameOrEmail"
              value={usernameOrEmail}
              onChange={(e) => setUsernameOrEmail(e.target.value)}
              className="w-full px-4 py-2 border border-border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="you@example.com"
              required
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm text-gray-700 mb-1">
              Password
            </label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full px-4 py-2 border border-border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="••••••••"
              required
            />
          </div>

          {error && (
            <p className="text-sm text-red-600 text-center">{error}</p>
          )}

          <div className="pt-4">
            <button
              type="submit"
              className="w-full py-2 bg-blue-600 text-white font-medium rounded-md hover:bg-blue-700 transition disabled:opacity-50"
              disabled={loading}
            >
              {loading ? 'Logging in...' : 'Log In'}
            </button>
          </div>
        </form>

        <p className="text-sm text-gray-500 text-center mt-6">
          Don&apos;t have an account?{' '}
          <Link to="/register" className="text-blue-600 hover:underline">
            Sign up
          </Link>
        </p>
      </motion.div>
    </div>
  )
}