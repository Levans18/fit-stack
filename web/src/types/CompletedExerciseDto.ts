import { CompletedSetDto } from "./CompletedSetDto";

export interface CompletedExerciseDto {
    id: number;
    name: string;
    notes?: string;
    completedSets: CompletedSetDto[];
}