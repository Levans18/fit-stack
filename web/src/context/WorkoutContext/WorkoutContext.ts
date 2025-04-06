import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { createContext } from 'react';



interface WorkoutContextProps {
  fetchWorkouts: () => Promise<{ pastWorkouts: WorkoutResponseDto[]; upcomingWorkouts: WorkoutResponseDto[] }>;
  deleteWorkout: (workoutId: number) => Promise<void>;
  error: string | null;
}

// Create and export the context
export const WorkoutContext = createContext<WorkoutContextProps | undefined>(undefined);