import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { WorkoutResponseDto } from '@/types/WorkoutResponseDto';
import { ArrowLeft } from 'lucide-react';

export default function WorkoutPage(){
    const [workout, setWorkout] = useState<WorkoutResponseDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const { workoutId } = useParams();
    const token = localStorage.getItem('token');

    useEffect(() => {

        const fetchWorkout = async () => {
            setLoading(true);
            try {
                const res = await fetch(`http://localhost:5168/workouts/${workoutId}`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`,
                    },
                });

                if (!res.ok) throw new Error('Failed to fetch workout data.');
                const json = await res.json();
                console.log(json);
                setWorkout(json);
            } catch (err) {
                if (err instanceof Error) {
                    setError(err.message)
                  } else {
                    setError('Something went wrong')
                  }
            } finally {
                setLoading(false);
            }
        };
        fetchWorkout();

    }, [token, workoutId]);

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-100 to-white p-6">
            <header className="flex items-start mb-8">
                <Link to="/my-workouts" className="text-blue-600 hover:underline mt-.75">
                    <ArrowLeft size={40} className="inline mr-1" />
                </Link>
                {loading && <p>Loading...</p>}
                {error && <p className="text-red-500">{error}</p>}

                {workout && <h1 className="text-4xl font-bold text-blue-900 mb-8">{workout.name}</h1>

                }       
            </header>
      </div>
    );
}