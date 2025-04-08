import { useCallback, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { useWorkoutContext } from '@/hooks/useWorkoutContext';
import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ArrowLeft } from 'lucide-react';
import ExerciseList from '@/components/ExerciseList';
import AddExerciseModal from '@/components/modals/AddExerciseModal';

export default function WorkoutPage() {
  const { fetchWorkoutById, error } = useWorkoutContext();
  const [workout, setWorkout] = useState<WorkoutResponseDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { workoutId } = useParams();

  const fetchWorkout = useCallback(async () => {
    setLoading(true);
    try {
      const data = await fetchWorkoutById(workoutId!);
      setWorkout(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [fetchWorkoutById, workoutId]);

  useEffect(() => {
    fetchWorkout();
  }, [fetchWorkout, workoutId]);

  const handleAddExercise = async () => {
    try {
      await fetchWorkout(); // Re-fetch the workout after adding an exercise
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-100 to-white p-6">
      <header className="flex items-start mb-8">
        <Link to="/my-workouts" className="text-blue-600 hover:underline mt-.75">
          <ArrowLeft size={40} className="inline mr-1" />
        </Link>
        {loading && <p>Loading...</p>}
        {error && <p className="text-red-500">{error}</p>}
        {workout && <h1 className="text-4xl font-bold text-blue-900 mb-8">{workout.name}</h1>}
      </header>
      {workout && (
        <>
          <ExerciseList exercises={workout.exercises} />
          <button
            onClick={() => setIsModalOpen(true)}
            className="mt-4 bg-blue-600 text-white font-bold py-2 px-4 rounded hover:bg-blue-700 transition"
          >
            Add Exercise
          </button>
          <AddExerciseModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            onExerciseAdded={handleAddExercise}
            workoutId={workoutId!}
          />
        </>
      )}
    </div>
  );
}