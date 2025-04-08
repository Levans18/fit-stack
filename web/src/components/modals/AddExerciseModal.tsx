import { useState } from 'react';
import { ExerciseResponseDto } from '@/types/ExerciseResponseDto';

interface AddExerciseModalProps {
  isOpen: boolean;
  onClose: () => void;
  onExerciseAdded: (exercise: ExerciseResponseDto) => void;
  workoutId: string;
}

const AddExerciseModal: React.FC<AddExerciseModalProps> = ({ isOpen, onClose, onExerciseAdded, workoutId }) => {
  const [name, setName] = useState('');
  const [reps, setReps] = useState('');
  const [sets, setSets] = useState('');
  const [weight, setWeight] = useState('');
  const [error, setError] = useState<string | null>(null);

  if (!isOpen) return null;

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
      onClose(); // Close the modal
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Something went wrong');
      }
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white p-6 rounded shadow-lg w-96">
        <h2 className="text-2xl font-bold mb-4">Add Exercise</h2>
        {error && <p className="text-red-500">{error}</p>}
        <form onSubmit={handleSubmit}>
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
          <div className="flex justify-end mt-4">
            <button
              type="button"
              onClick={onClose}
              className="bg-gray-300 text-gray-700 py-2 px-4 rounded mr-2 hover:bg-gray-400 transition"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition"
            >
              Add Exercise
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddExerciseModal;