import { ExerciseResponseDto } from './ExerciseResponseDto';

export interface WorkoutResponseDto {
    id: number;
    name: string;
    date: string; // Use string because dates are typically serialized as strings in JSON
    exercises: ExerciseResponseDto[];
}