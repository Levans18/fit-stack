import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import Dashboard from "./pages/DashboardPage";
import LandingPage from "./pages/LandingPage";
import WorkoutPage from "./pages/WorkoutPage";
import MyWorkoutsPage from "./pages/MyWorkoutsPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LandingPage/>} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage/>} />
        <Route path="/dashboard" element={<Dashboard/>} />
        <Route path="/my-workouts" element={<MyWorkoutsPage />} />
        <Route path="/workout/:workoutId" element={<WorkoutPage/>} />
      </Routes>
    </Router>
  );
}

export default App
