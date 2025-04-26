import { CompletedExerciseDto } from './CompletedExerciseDto';

export interface CompletedWorkoutDto {
  id: number; // ID of the workout completion
  workoutId: number; // ID of the associated workout
  completedAt: string; // Timestamp of when the workout was completed
  notes?: string; // Optional notes for the workout
  completedExercises: CompletedExerciseDto[]; // List of completed exercises
}