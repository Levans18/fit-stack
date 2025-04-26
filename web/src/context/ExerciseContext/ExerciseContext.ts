import { createContext } from 'react';
import { ExerciseDto } from '@/types/ExerciseDto';

interface ExerciseContextProps {
  fetchExercises: (workoutId: string) => Promise<ExerciseDto[]>;
  addExercise: (workoutId: string, exercise: ExerciseDto) => Promise<ExerciseDto>;
  deleteExercise: (exerciseId: number) => Promise<void>;
  completeExercise: (exerciseId: number) => Promise<ExerciseDto>;
  error: string | null;
}

export const ExerciseContext = createContext<ExerciseContextProps | undefined>(undefined);