import { ExerciseDto } from './ExerciseDto';

export interface WorkoutResponseDto {
    id: number;
    name: string;
    date: string; // Use string because dates are typically serialized as strings in JSON
    exercises: ExerciseDto[];
}