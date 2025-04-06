import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { Link } from 'react-router-dom';

// Define the expected structure of the dashboard data
interface DashboardData {
  totalWorkouts: number;
  totalExercises: number;
  totalVolume: number;
  weeklyStreak: number;
}

export default function Dashboard() {
  const [data, setData] = useState<DashboardData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        console.log("Token:", localStorage.getItem("token"));
        const res = await fetch('http://localhost:5168/dashboard', {
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
          },
        });

        if (!res.ok) throw new Error('Failed to fetch dashboard data.');
        const json = await res.json();
        setData(json);
      } catch (err: unknown) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError('An unexpected error occurred.');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-100 to-white p-6">
      <h1 className="text-4xl font-bold text-blue-900 mb-8">Your Dashboard</h1>

      {loading && <p>Loading...</p>}
      {error && <p className="text-red-500">{error}</p>}

      {data && (
        <motion.div
          className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
        >
          <Card title="Total Workouts" value={data.totalWorkouts} />
          <Card title="Total Exercises" value={data.totalExercises} />
          <Card title="Weekly Streak" value={`${data.weeklyStreak} days`} />
        </motion.div>
      )}

      {/* Add the button here */}
      <div className="mt-8">
        <Link
          to="/my-workouts"
          className="inline-block bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
        >
          View My Workouts
        </Link>
      </div>
    </div>
  );
}

function Card({ title, value }: { title: string; value: string | number }) {
  return (
    <motion.div
      whileHover={{ scale: 1.03 }}
      className="bg-white rounded-xl shadow-md p-6 text-center border border-border"
    >
      <h2 className="text-xl font-semibold text-gray-700 mb-2">{title}</h2>
      <p className="text-3xl font-bold text-blue-600">{value}</p>
    </motion.div>
  );
}
