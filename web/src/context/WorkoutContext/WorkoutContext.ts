import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ExerciseDto } from '@/types/ExerciseDto';
import { createContext } from 'react';

interface WorkoutContextProps {
  fetchWorkouts: () => Promise<{ completedWorkouts: WorkoutResponseDto[]; upcomingWorkouts: WorkoutResponseDto[] }>;
  fetchWorkoutById: (workoutId: string) => Promise<WorkoutResponseDto>;
  addExerciseToWorkout: (workoutId: string, exercise: ExerciseDto) => Promise<ExerciseDto>;
  deleteWorkout: (workoutId: number) => Promise<void>;
  error: string | null;
}

export const WorkoutContext = createContext<WorkoutContextProps | undefined>(undefined);