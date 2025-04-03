import React, { useEffect, useState } from "react";
import { Card, CardContent } from "../components/ui/card";
import { Button } from "../components/ui/button";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";

const Dashboard = () => {
  const [summary, setSummary] = useState({
    workoutsThisWeek: 0,
    totalMinutes: 0,
    totalExercises: 0,
  });

  const [weeklyData, setWeeklyData] = useState([
    { day: "Mon", workouts: 0 },
    { day: "Tue", workouts: 0 },
    { day: "Wed", workouts: 0 },
    { day: "Thu", workouts: 0 },
    { day: "Fri", workouts: 0 },
    { day: "Sat", workouts: 0 },
    { day: "Sun", workouts: 0 },
  ]);

  useEffect(() => {
    // Simulate fetching data
    setSummary({ workoutsThisWeek: 4, totalMinutes: 180, totalExercises: 25 });
    setWeeklyData([
      { day: "Mon", workouts: 1 },
      { day: "Tue", workouts: 0 },
      { day: "Wed", workouts: 1 },
      { day: "Thu", workouts: 1 },
      { day: "Fri", workouts: 0 },
      { day: "Sat", workouts: 1 },
      { day: "Sun", workouts: 0 },
    ]);
  }, []);

  return (
    <div className="min-h-screen bg-white text-[#1a1a1a] p-6">
      <h1 className="text-3xl font-bold mb-4 text-blue-600">Welcome to FitStack 💪</h1>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        <Card className="shadow-md">
          <CardContent className="p-4">
            <p className="text-sm text-[#6c757d]">Workouts This Week</p>
            <p className="text-2xl font-semibold text-blue-600">{summary.workoutsThisWeek}</p>
          </CardContent>
        </Card>

        <Card className="shadow-md">
          <CardContent className="p-4">
            <p className="text-sm text-[#6c757d]">Total Minutes</p>
            <p className="text-2xl font-semibold text-blue-600">{summary.totalMinutes}</p>
          </CardContent>
        </Card>

        <Card className="shadow-md">
          <CardContent className="p-4">
            <p className="text-sm text-[#6c757d]">Exercises Logged</p>
            <p className="text-2xl font-semibold text-blue-600">{summary.totalExercises}</p>
          </CardContent>
        </Card>
      </div>

      <div className="bg-white rounded-lg shadow-md p-4">
        <h2 className="text-xl font-bold mb-4 text-blue-600">Weekly Activity</h2>
        <ResponsiveContainer width="100%" height={250}>
          <BarChart data={weeklyData}>
            <XAxis dataKey="day" />
            <YAxis allowDecimals={false} />
            <Tooltip />
            <Bar dataKey="workouts" fill="#4a90e2" radius={[4, 4, 0, 0]} />
          </BarChart>
        </ResponsiveContainer>
      </div>

      <div className="mt-6">
        <Button className="bg-blue-600 hover:bg-blue-700 text-white">Start New Workout</Button>
      </div>
    </div>
  );
};

export default Dashboard;