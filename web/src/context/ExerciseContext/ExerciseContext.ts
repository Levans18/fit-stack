import { createContext } from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

interface ExerciseContextProps {
  fetchExercises: (workoutId: string) => Promise<ExerciseResponseDto[]>;
  addExercise: (workoutId: string, exercise: ExerciseResponseDto) => Promise<ExerciseResponseDto>;
  deleteExercise: (exerciseId: number) => Promise<void>;
  error: string | null;
}

export const ExerciseContext = createContext<ExerciseContextProps | undefined>(undefined);