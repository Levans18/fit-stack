import React from 'react';
import { WorkoutProvider } from './WorkoutContext/WorkoutProvider';
import { ExerciseProvider } from './ExerciseContext/ExerciseProvider';

const CombinedProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <WorkoutProvider>
      <ExerciseProvider>
      {children}
      </ExerciseProvider>
    </WorkoutProvider>
  );
};

export default CombinedProvider;