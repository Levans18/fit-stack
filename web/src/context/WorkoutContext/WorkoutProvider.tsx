import React, { useState } from 'react';

import { WorkoutContext } from './WorkoutContext';
import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';

export const WorkoutProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [error, setError] = useState<string | null>(null);

  const fetchWorkouts = async (): Promise<{ pastWorkouts: WorkoutResponseDto[]; upcomingWorkouts: WorkoutResponseDto[] }> => {
    try {
      const res = await fetch('http://localhost:5168/workouts', {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) {
        throw new Error('Failed to fetch workouts.');
      }

      const data = await res.json();

      // Ensure the response contains valid data, even if it's empty
      return {
        pastWorkouts: data.pastWorkouts || [],
        upcomingWorkouts: data.upcomingWorkouts || [],
      };
    } catch (err: unknown) {
      if (err instanceof Error) {
        console.error(err.message || 'An unexpected error occurred.');
      } else {
        console.error('An unexpected error occurred.');
      }
      throw err; // Re-throw the error for the calling component to handle
    }
  };

  const deleteWorkout = async (workoutId: number): Promise<void> => {
    try {
      const res = await fetch(`http://localhost:5168/workouts/${workoutId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) throw new Error('Failed to delete workout.');
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
    <WorkoutContext.Provider value={{ fetchWorkouts, deleteWorkout, error }}>
      {children}
    </WorkoutContext.Provider>
  );
};