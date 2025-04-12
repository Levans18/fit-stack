import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { AddWorkoutModal } from '@/components/modals/AddWorkoutModal';
import { useWorkoutContext } from '@/hooks/useWorkoutContext';
import WorkoutList from '@/components/WorkoutList';
import WorkoutCalendar from '@/components/WorkoutCalendar';
import { ArrowLeft } from 'lucide-react';

type Workout = {
  id: number;
  name: string;
  date: string;
};

export default function MyWorkoutsPage() {
  const { fetchWorkouts, deleteWorkout, error } = useWorkoutContext(); // Use context actions
  const [completedWorkouts, setCompletedWorkouts] = useState<Workout[]>([]);
  const [upcomingWorkouts, setUpcomingWorkouts] = useState<Workout[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);

  // Fetch workouts and update state
  const handleFetchWorkouts = async () => {
    setLoading(true);
    try {
      const data = await fetchWorkouts();
      console.log('Fetched Workouts:', data); // Debugging line
      setCompletedWorkouts(data.completedWorkouts);
      setUpcomingWorkouts(data.upcomingWorkouts);
    } finally {
      setLoading(false);
    }
  };

  // Delete a workout and refetch workouts
  const handleDeleteWorkout = async (e: React.MouseEvent, workoutId: number) => {
    e.stopPropagation(); // Prevent the click from triggering the Link
    await deleteWorkout(workoutId);
    await handleFetchWorkouts();
  };

  const handleDateClick = (date: Date) => {
    console.log('Selected Date:', date);
  };

  useEffect(() => {
    handleFetchWorkouts();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  const noWorkouts = completedWorkouts.length === 0 && upcomingWorkouts.length === 0;

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-100 to-white p-6">
      <header className="flex items-start mb-8">
        <Link to="/dashboard" className="text-blue-600 hover:underline mt-.75">
          <ArrowLeft size={40} className="inline mr-1" />
        </Link>
        <h1 className="text-4xl font-bold text-blue-900">My Workouts</h1>
      </header>
      <section>
        <button
          onClick={() => setIsModalOpen(true)}
          className="mb-4 bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
        >
          Add Workout
        </button>
      </section>
      {noWorkouts ? (
        <div className="text-center mt-16">
          <p className="text-gray-700 text-lg mb-4">
            You don't have any workouts yet. Start tracking your fitness journey today!
          </p>
          <button
            onClick={() => setIsModalOpen(true)}
            className="bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
          >
            Add Your First Workout
          </button>
        </div>
      ) : (
        <section className="mb-8 flex justify-between h-[calc(100vh-200px)]">
          {/* Left: Upcoming Workouts */}
          <div className="w-1/2 pr-4 overflow-y-auto">
            <WorkoutList
              title="Upcoming Workouts"
              workouts={upcomingWorkouts}
              onDelete={handleDeleteWorkout}
            />
            <WorkoutList
              title="Completed Workouts"
              workouts={completedWorkouts}
              onDelete={handleDeleteWorkout}
            />
          </div>
          {/* Right: Calendar */}
          <div className="w-1/2 pl-4 h-full">
            <WorkoutCalendar
              workouts={[...completedWorkouts, ...upcomingWorkouts]}
              onDateClick={handleDateClick}
            />
          </div>
        </section>
      )}
      <AddWorkoutModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onWorkoutAdded={handleFetchWorkouts}
      />
    </div>
  );
}