import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';
import { createContext } from 'react';

interface WorkoutContextProps {
  fetchWorkouts: () => Promise<{ completedWorkouts: WorkoutResponseDto[]; upcomingWorkouts: WorkoutResponseDto[] }>;
  fetchWorkoutById: (workoutId: string) => Promise<WorkoutResponseDto>;
  addExerciseToWorkout: (workoutId: string, exercise: ExerciseResponseDto) => Promise<ExerciseResponseDto>;
  deleteWorkout: (workoutId: number) => Promise<void>;
  error: string | null;
}

export const WorkoutContext = createContext<WorkoutContextProps | undefined>(undefined);