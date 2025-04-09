import React, { useState } from 'react';
import { ExerciseContext } from './ExerciseContext';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

export const ExerciseProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [error, setError] = useState<string | null>(null);

  const fetchExercises = async (workoutId: string): Promise<ExerciseResponseDto[]> => {
    try {
      const res = await fetch(`http://localhost:5168/workouts/${workoutId}/exercises`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) {
        throw new Error('Failed to fetch exercises.');
      }

      return await res.json();
    } catch (err: unknown) {
      if (err instanceof Error) {
        setError(err.message || 'An unexpected error occurred.');
      } else {
        setError('An unexpected error occurred.');
      }
      throw err;
    }
  };

  const addExercise = async (workoutId: string, exercise: ExerciseResponseDto): Promise<ExerciseResponseDto> => {
    try {
      const res = await fetch(`http://localhost:5168/workouts/${workoutId}/exercises`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
        body: JSON.stringify(exercise),
      });

      if (!res.ok) {
        throw new Error('Failed to add exercise.');
      }

      return await res.json();
    } catch (err: unknown) {
      if (err instanceof Error) {
        setError(err.message || 'An unexpected error occurred.');
      } else {
        setError('An unexpected error occurred.');
      }
      throw err;
    }
  };

  const deleteExercise = async (exerciseId: number): Promise<void> => {
    try {
      const res = await fetch(`http://localhost:5168/exercises/${exerciseId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) throw new Error('Failed to delete exercise.');
    } catch (err: unknown) {
      if (err instanceof Error) {
        setError(err.message || 'An unexpected error occurred.');
      } else {
        setError('An unexpected error occurred.');
      }
      throw err;
    }
  };

  return (
    <ExerciseContext.Provider value={{ fetchExercises, addExercise, deleteExercise, error }}>
      {children}
    </ExerciseContext.Provider>
  );
};