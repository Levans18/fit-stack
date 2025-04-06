import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';

interface Workout {
  id: number;
  name: string;
  date: string;
}

interface WorkoutCalendarProps {
  workouts: Workout[];
  onDateClick: (date: Date) => void;
}

export default function WorkoutCalendar({ workouts, onDateClick }: WorkoutCalendarProps) {
  const getWorkoutDates = () => {
    return workouts.map((workout) => new Date(workout.date));
  };

  const renderTileContent = ({ date }: { date: Date }) => {
    const workoutDates = getWorkoutDates();
    const isWorkoutDate = workoutDates.some(
      (workoutDate) => workoutDate.toDateString() === date.toDateString()
    );

    if (isWorkoutDate) {
      return <div className="bg-blue-500 w-2 h-2 rounded-full mx-auto mt-1"></div>;
    }
    return null;
  };

  return (
    <div className="w-full">
      <h2 className="text-2xl font-semibold text-blue-800 mb-4">Workout Calendar</h2>
      <Calendar
        onClickDay={onDateClick}
        tileContent={renderTileContent}
        className="shadow-md rounded-lg"
      />
    </div>
  );
}