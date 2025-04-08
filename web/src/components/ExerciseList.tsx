import React from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

interface ExerciseListProps {
  exercises: ExerciseResponseDto[];
}

const ExerciseList: React.FC<ExerciseListProps> = ({ exercises }) => {
  return (
    <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      {exercises.length === 0 ? (
        <p className="col-span-full text-center text-gray-700">No exercises added yet.</p>
      ) : (
        exercises.map((exercise) => (
          <div
            key={exercise.id}
            className="bg-white shadow-md rounded-lg p-4 border border-gray-200"
          >
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