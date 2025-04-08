import React from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

interface ExerciseListProps {
    exercises: ExerciseResponseDto[];
}

const ExerciseList: React.FC<ExerciseListProps> = ({ exercises }) => {
    return (
        <div className="mt-6">
            <h2 className="text-2xl font-semibold text-blue-800 mb-4">Exercises</h2>
            {exercises.length === 0 ? (
                <p>No exercises added yet.</p>
            ) : (
                <ul className="list-disc pl-5">
                    {exercises.map(exercise => (
                        <li key={exercise.id} className="mb-2">
                            <span className="font-bold">{exercise.name}</span>: {exercise.reps} reps, {exercise.sets} sets
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ExerciseList;