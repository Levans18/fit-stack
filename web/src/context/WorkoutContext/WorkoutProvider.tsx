import React, { useState } from 'react';
import { WorkoutContext } from './WorkoutContext';
import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ExerciseDto } from '@/types/ExerciseDto';

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
  
      const data: WorkoutResponseDto[] = await res.json(); // The response is an array of workouts
      console.log('API Response:', data); // Debugging line
  
      // Separate workouts into completed and upcoming based on the current date
      const now = new Date();
      const completedWorkouts = data.filter(workout => new Date(workout.date) < now);
      const upcomingWorkouts = data.filter(workout => new Date(workout.date) >= now);
  
      return {
        completedWorkouts,
        upcomingWorkouts,
      };
    } catch (err) {
      console.error('Error fetching workouts:', err);
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

  const addExerciseToWorkout = async (workoutId: string, exercise: ExerciseDto): Promise<ExerciseDto> => {
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