import React, { useState } from 'react';
import { WorkoutContext } from './WorkoutContext';
import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

export const WorkoutProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [error, setError] = useState<string | null>(null);

  const fetchWorkouts = async (): Promise<{ completedWorkouts: WorkoutResponseDto[]; upcomingWorkouts: WorkoutResponseDto[] }> => {
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
      return {
        completedWorkouts: data.completedWorkouts || [],
        upcomingWorkouts: data.upcomingWorkouts || [],
      };
    } catch (err: unknown) {
      if (err instanceof Error) {
        console.error(err.message || 'An unexpected error occurred.');
      } else {
        console.error('An unexpected error occurred.');
      }
      throw err;
    }
  };

  const fetchWorkoutById = async (workoutId: string): Promise<WorkoutResponseDto> => {
    try {
      const res = await fetch(`http://localhost:5168/workouts/${workoutId}`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });

      if (!res.ok) {
        throw new Error('Failed to fetch workout.');
      }

      return await res.json();
    } catch (err: unknown) {
      if (err instanceof Error) {
        console.error(err.message || 'An unexpected error occurred.');
      } else {
        console.error('An unexpected error occurred.');
      }
      throw err;
    }
  };

  const addExerciseToWorkout = async (workoutId: string, exercise: ExerciseResponseDto): Promise<ExerciseResponseDto> => {
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
        console.error(err.message || 'An unexpected error occurred.');
      } else {
        console.error('An unexpected error occurred.');
      }
      throw err;
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
    <WorkoutContext.Provider value={{ fetchWorkouts, fetchWorkoutById, addExerciseToWorkout, deleteWorkout, error }}>
      {children}
    </WorkoutContext.Provider>
  );
};