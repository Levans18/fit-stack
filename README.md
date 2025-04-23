# 💪 FitStack

**FitStack** is a full-stack fitness tracking app that helps users log, manage, and visualize their workout progress across both web and mobile platforms. Built with a modern, cloud-ready architecture, FitStack delivers a seamless experience for tracking strength training, cardio sessions, and personal records.

---

## 🔧 Tech Stack

### Frontend (Web)
- [React](https://reactjs.org/) + [Vite](https://vitejs.dev/) + TypeScript
- Axios for API communication
- React Router for navigation

### Backend
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- Microsoft SQL Server (hosted via [Amazon RDS](https://aws.amazon.com/rds/sqlserver/))
- JSON Web Tokens (JWT) for authentication

### Mobile (Coming Soon)
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)

### DevOps / Deployment
- Frontend: [Vercel](https://vercel.com/)
- Backend:  [AWS Elastic Beanstalk](https://aws.amazon.com/elasticbeanstalk/) or [App Runner](https://aws.amazon.com/apprunner/)
- CI/CD: GitHub Actions (optional)
- Database: Amazon RDS (Free Tier-compatible)

---

## 🚀 Features

- 🔐 User registration and login with secure JWT auth
- 🏋️ Add, view, and manage workouts
  - Exercises, sets, reps, weight, and notes
- 📅 View workout history (calendar/timeline)
- 📊 Dashboard with personal records (coming soon)
- 💬 Push/email reminders (planned)

---

## 📁 Project Structure

```plaintext
FitStack/
├── api/          # ASP.NET Core Web API
├── web/          # React frontend (Vite)
├── mobile/       # .NET MAUI app (in progress)
├── README.md
└── FitStack.sln
```

## 📌 Project Status

> **Current Phase:** Active Development

### ✅ Completed
- Project scaffold: React + Vite frontend, ASP.NET Core backend
- GitHub repository and project structure
- Initial README with goals and tech stack
- Implementing JWT-based user authentication
- Creating workout tracking models and API endpoints
- Connecting React frontend to the backend API
- Basic frontend pages (Login, Register, Dashboard)
- Workout history timeline/calendar
- Adding Workout Creation and Tracking

### 🛠️ In Progress
- Exercise Creation, Deletion and Editing
- Setting up AWS (free tier) for Prod
- Workout/Exercise Notes
- Customizable Reps for each set

### 🔜 Coming Soon
- Personal record tracking and charts
- Responsive mobile-friendly design
- Native mobile app using .NET MAUI
- Push/email notifications and reminder system


