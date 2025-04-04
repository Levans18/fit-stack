import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { AddWorkoutModal } from '@/components/modals/AddWorkoutModal';

export default function MyWorkoutsPage() {
  const [workouts, setWorkouts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const fetchWorkouts = async () => {
    setLoading(true);
    try {
      const res = await fetch('http://localhost:5168/workouts', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) throw new Error('Failed to fetch workouts.');
      const data = await res.json();
      setWorkouts(data);
    } catch (err: any) {
      setError(err.message || 'An unexpected error occurred.');
    } finally {
      setLoading(false);
    }
  };

  const handleWorkoutAdded = async () => {
    await fetchWorkouts(); // Refetch workouts from the backend
  };
  
  useEffect(() => {
    fetchWorkouts();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-100 to-white p-6">
      <h1 className="text-3xl font-bold text-blue-900 mb-8">My Workouts</h1>

      {workouts.length === 0 ? (
        <div className="text-center">
          <p className="text-gray-700 text-lg mb-4">You don't have any workouts yet.</p>
          <button
            onClick={() => setIsModalOpen(true)}
            className="inline-block bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
          >
            Add Your First Workout
          </button>
        </div>
      ) : (
        <ul className="space-y-4">
            <button
            onClick={() => setIsModalOpen(true)}
            className="mb-4 bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
          > Add a Workout</button>
          {workouts.map((workout: any) => (
            <li key={workout.id} className="bg-white shadow-md rounded-lg p-4">
              <Link to={`/workouts/${workout.id}`} className="text-blue-600 font-bold">
                {workout.name}
              </Link>
              <p className="text-gray-500">Date: {new Date(workout.date).toLocaleDateString()}</p>
            </li>
          ))}
        </ul>
      )}

    <AddWorkoutModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onWorkoutAdded={handleWorkoutAdded}
      />
    </div>
  );
}