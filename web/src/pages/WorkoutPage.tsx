import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

export default function WorkoutPage(){
    const [workout, setWorkout] = useState(null);
    const [loading, setLoading] = useState(true);

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
                console.error(err);
            } finally {
                setLoading(false);
            }
        };
        fetchWorkout();

    }, [token, workoutId]);

    return (
        <div className="h-screen flex items-center justify-center bg-gradient-to-br from-white via-gray-100 to-white px-4 relative">
            <h1 className="text-2xl font-bold">Workout Page</h1>
            <p>Workout ID: {workoutId}</p>
        </div>
    );
}