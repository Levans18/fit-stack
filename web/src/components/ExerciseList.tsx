import React, { useContext, useEffect, useState } from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseDto';
import { ExerciseContext } from '@/context/ExerciseContext/ExerciseContext';

interface ExerciseListProps {
  exercises: ExerciseResponseDto[];
  onExerciseDeleted?: (exerciseId: number) => void; // Optional callback for parent updates
}

const ExerciseList: React.FC<ExerciseListProps> = ({ exercises: initialExercises, onExerciseDeleted }) => {
  const { deleteExercise, error } = useContext(ExerciseContext)!;
  const [exercises, setExercises] = useState<ExerciseResponseDto[]>(initialExercises);
  const [localError, setLocalError] = useState<string | null>(null);

  // Sync local state with parent prop changes
  useEffect(() => {
    setExercises(initialExercises);
  }, [initialExercises]);

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

  return (
    <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      {localError && <p className="col-span-full text-red-500">{localError}</p>}
      {error && <p className="col-span-full text-red-500">{error}</p>}
      {exercises.length === 0 ? (
        <p className="col-span-full text-center text-gray-700">No exercises added yet.</p>
      ) : (
        exercises.map((exercise) => (
          <div
            key={exercise.id}
            className="bg-white shadow-md rounded-lg p-4 border border-gray-200 relative"
          >
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
          </div>
        ))
      )}
    </div>
  );
};

export default ExerciseList;