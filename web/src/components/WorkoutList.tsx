import { Link } from 'react-router-dom';
import { LucideX } from 'lucide-react';

interface Workout {
  id: number;
  name: string;
  date: string;
}

interface WorkoutListProps {
  title: string;
  workouts: Workout[];
  onDelete: (e: React.MouseEvent, workoutId: number) => void;
}

export default function WorkoutList({ title, workouts, onDelete }: WorkoutListProps) {
  return (
    <section className="mt-8">
      <h2 className="text-2xl font-semibold text-blue-800 mb-4">{title}</h2>
      {workouts.length === 0 ? (
        <p className="text-gray-700">No {title.toLowerCase()}.</p>
      ) : (
        <ul className="space-y-4">
          {workouts.map((workout) => (
            <li
              key={workout.id}
              className="flex align-middle relative bg-white shadow-md rounded-lg p-4 hover:shadow-lg transition-shadow"
            >
              <Link to={`/workouts/${workout.id}`} className="block">
                <h3 className="text-lg font-bold text-blue-600">{workout.name}</h3>
                <p className="text-gray-500">Date: {new Date(workout.date).toLocaleDateString()}</p>
              </Link>
              <button
                onClick={(e) => onDelete(e, workout.id)}
                className="absolute top-2 right-2 text-red-500 hover:text-red-700"
              >
                <LucideX size={40} className="mt-3.5"/>
              </button>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}