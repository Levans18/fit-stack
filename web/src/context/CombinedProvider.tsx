import React from 'react';
import { WorkoutProvider } from './WorkoutContext/WorkoutProvider';

const CombinedProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <WorkoutProvider>
      {children}
    </WorkoutProvider>
  );
};

export default CombinedProvider;