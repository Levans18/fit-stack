import React, { useContext, useEffect, useState } from 'react';
import { ExerciseDto } from '@/types/ExerciseDto';
import { CompletedExerciseDto } from '@/types/CompletedExerciseDto';
import { ExerciseContext } from '@/context/ExerciseContext/ExerciseContext';

interface ExerciseListProps {
  exercises: ExerciseDto[];
  completedExercises: CompletedExerciseDto[]; // List of completed exercises
  onExerciseDeleted?: (exerciseId: number) => void; // Optional callback for parent updates
}

const ExerciseList: React.FC<ExerciseListProps> = ({
  exercises: initialExercises,
  completedExercises: initialCompletedExercises,
  onExerciseDeleted,
}) => {
  const { deleteExercise, completeExercise, error } = useContext(ExerciseContext)!;
  const [exercises, setExercises] = useState<ExerciseDto[]>(initialExercises);
  const [completedExercises, setCompletedExercises] = useState<CompletedExerciseDto[]>(initialCompletedExercises);
  const [localError, setLocalError] = useState<string | null>(null);

  // Sync local state with parent prop changes
  useEffect(() => {
    setExercises(initialExercises);
    setCompletedExercises(initialCompletedExercises);
  }, [initialExercises, initialCompletedExercises]);

  const handleDelete = async (exerciseId: number) => {
    try {
      await deleteExercise(exerciseId); // Call the context's delete function
      setExercises((prev) => prev.filter((exercise) => exercise.id !== exerciseId)); // Update local state
      if (onExerciseDeleted) onExerciseDeleted(exerciseId); // Notify parent if needed
    } catch (err) {
      if (err instanceof Error) {
        setLocalError(err.message);
      } else {
        setLocalError('An unexpected error occurred.');
      }
    }
  };

  const handleComplete = async (exerciseId: number) => {
    try {
      const completedExercise = await completeExercise(exerciseId); // Call the context's complete function
      if ('completedSets' in completedExercise) {
        setCompletedExercises((prev) => [...prev, completedExercise as CompletedExerciseDto]); // Add to completed exercises
      } else {
        setLocalError('Invalid completed exercise data.');
      }
    } catch (err) {
      if (err instanceof Error) {
        setLocalError(err.message);
      } else {
        setLocalError('An unexpected error occurred.');
      }
    }
  };

  const isExerciseCompleted = (exerciseId: number) =>
    completedExercises.some((completed) => completed.id === exerciseId);

  return (
    <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {localError && <p className="col-span-full text-red-500">{localError}</p>}
      {error && <p className="col-span-full text-red-500">{error}</p>}
      {exercises.length === 0 ? (
        <p className="col-span-full text-center text-gray-700">No exercises added yet.</p>
      ) : (
        exercises.map((exercise) => {
          const completed = isExerciseCompleted(exercise.id);
          return (
            <div
              key={exercise.id}
              className={`relative bg-white shadow-md rounded-lg p-4 border ${
                completed ? 'border-green-500' : 'border-gray-200'
              }`}
            >
              {completed && (
                <span className="absolute top-2 left-2 bg-green-500 text-white text-xs px-2 py-1 rounded">
                  Completed
                </span>
              )}
              <button
                onClick={() => handleDelete(exercise.id)}
                className="absolute top-2 right-2 text-red-500 hover:text-red-700"
              >
                ✕
              </button>
              <h3 className="text-lg font-bold text-blue-800">{exercise.name}</h3>
              <p className="text-gray-700">Sets: {exercise.sets}</p>
              <p className="text-gray-700">Reps: {exercise.reps}</p>
              <p className="text-gray-700">Weight: {exercise.weight} lbs</p>
              <button
                onClick={() => handleComplete(exercise.id)}
                className={`mt-2 font-bold py-1 px-2 rounded transition ${
                  completed
                    ? 'bg-gray-400 text-white cursor-not-allowed'
                    : 'bg-green-600 text-white hover:bg-green-700'
                }`}
                disabled={completed}
              >
                {completed ? 'Completed' : 'Complete'}
              </button>
            </div>
          );
        })
      )}
    </div>
  );
};

export default ExerciseList;