import { useState } from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

interface ExerciseFormProps {
  workoutId: string;
  onExerciseAdded: (exercise: ExerciseResponseDto) => void;
}

const ExerciseForm: React.FC<ExerciseFormProps> = ({ workoutId, onExerciseAdded }) => {
  const [name, setName] = useState('');
  const [reps, setReps] = useState('');
  const [sets, setSets] = useState('');
  const [weight, setWeight] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    const exerciseData = {
      name,
      reps: parseInt(reps),
      sets: parseInt(sets),
      weight: parseFloat(weight),
    };

    try {
      const res = await fetch(`http://localhost:5168/workouts/${workoutId}/exercises`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
        body: JSON.stringify(exerciseData),
      });

      if (!res.ok) throw new Error('Failed to add exercise.');

      const createdExercise = await res.json();
      onExerciseAdded(createdExercise); // Notify parent component
      setName('');
      setReps('');
      setSets('');
      setWeight('');
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Something went wrong');
      }
    }
  };

  return (
    <form onSubmit={handleSubmit} className="mb-4">
      {error && <p className="text-red-500">{error}</p>}
      <div>
        <label htmlFor="name" className="block">Exercise Name:</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          className="border p-2 w-full"
        />
      </div>
      <div>
        <label htmlFor="reps" className="block">Reps:</label>
        <input
          type="number"
          id="reps"
          value={reps}
          onChange={(e) => setReps(e.target.value)}
          required
          className="border p-2 w-full"
        />
      </div>
      <div>
        <label htmlFor="sets" className="block">Sets:</label>
        <input
          type="number"
          id="sets"
          value={sets}
          onChange={(e) => setSets(e.target.value)}
          required
          className="border p-2 w-full"
        />
      </div>
      <div>
        <label htmlFor="weight" className="block">Weight (lbs):</label>
        <input
          type="number"
          id="weight"
          value={weight}
          onChange={(e) => setWeight(e.target.value)}
          required
          className="border p-2 w-full"
        />
      </div>
      <button type="submit" className="bg-blue-600 text-white p-2 mt-4">Add Exercise</button>
    </form>
  );
};

export default ExerciseForm;