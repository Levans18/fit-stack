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
- Microsoft SQL Server (Azure-hosted)
- JSON Web Tokens (JWT) for authentication

### Mobile (Coming Soon)
- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)

### DevOps / Deployment
- Frontend: [Vercel](https://vercel.com/)
- Backend: [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/)
- CI/CD: GitHub Actions (optional)
- Database: Azure SQL (Free Tier)

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
├── shared/       # Shared models (optional)
├── README.md
└── FitStack.sln
```

## 📌 Project Status

> **Current Phase:** Active Development

### ✅ Completed
- Project scaffold: React + Vite frontend, ASP.NET Core backend
- GitHub repository and project structure
- Initial README with goals and tech stack

### 🛠️ In Progress
- Implementing JWT-based user authentication
- Creating workout tracking models and API endpoints
- Connecting React frontend to the backend API
- Basic frontend pages (Login, Register, Dashboard)

### 🔜 Coming Soon
- Workout history timeline/calendar
- Personal record tracking and charts
- Responsive mobile-friendly design
- Native mobile app using .NET MAUI
- Push/email notifications and reminder system


