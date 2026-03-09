<h1 align="center">🎓 EduPlay — AI-Powered Educational Gaming Platform</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET_Core-Web_API-blueviolet?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/Google_Gemini-AI-4285F4?style=for-the-badge&logo=google&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL_Server-EF_Core_8-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens" />
  <img src="https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" />
</p>

<p align="center">
  A full-featured <strong>RESTful API backend</strong> for an educational quiz and gaming platform,
  built with <strong>ASP.NET Core 8</strong> and powered by <strong>Google Gemini AI</strong>
  for intelligent answer evaluation. Designed for students, teachers, and admins to collaborate
  in a structured, gamified learning environment.
</p>

---

## 📖 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Game Modes](#-game-modes)
- [API Endpoints](#-api-endpoints)
- [Data Models](#-data-models)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Database Setup](#-database-setup)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)

---

## 🌟 Overview

**EduPlay** is a graduation project that reimagines studying as gameplay. The platform provides two main quiz domains — **Education** (curriculum-based) and **Entertainment** (general knowledge) — with three distinct game mechanics per domain. Teachers submit questions, admins curate them, and students earn points while learning.

An integrated **Google Gemini AI** engine evaluates open-ended student answers in real time, providing instant, intelligent feedback without requiring exact string matches.

---

## ✨ Features

| Feature | Details |
|---|---|
| 🔐 **JWT Authentication** | Secure stateless auth for Students, Teachers, and Admins |
| 🤖 **AI Answer Checking** | Google Gemini evaluates free-text answers intelligently |
| 📚 **Dual Domains** | Education (curriculum) & Entertainment (general knowledge) |
| 🎮 **Three Game Modes** | Difficulty, Hint, and Offside game modes per domain |
| 🏆 **Points System** | Students accumulate total points across all games |
| 💡 **Hint System** | Each question supports multiple progressive hints |
| 📊 **Difficulty Levels** | Easy → Medium → Difficult → Very Difficult → Extreme |
| 📝 **Question Review Workflow** | Under Review → Approved / Rejected by Admin |
| 👨‍🏫 **Teacher Management** | Teachers submit & manage subject-specific questions |
| 🎓 **Grade-based Curriculum** | Students assigned to grades; questions scoped to subjects |
| 📧 **Email Service** | Integrated email sending (password reset, notifications) |
| 📖 **Swagger UI** | Fully documented interactive API explorer |
| 🌐 **CORS Support** | Configured for cross-origin frontend integration |

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| **Framework** | ASP.NET Core 8 Web API |
| **ORM** | Entity Framework Core 8 |
| **Database** | Microsoft SQL Server |
| **AI Engine** | Google Gemini (via `Google_GenerativeAI` & `Mscc.GenerativeAI`) |
| **Authentication** | ASP.NET Core Identity + JWT Bearer Tokens |
| **Email** | SMTP Email Service |
| **API Docs** | Swashbuckle / Swagger (OpenAPI) |
| **CORS** | `Microsoft.AspNetCore.Cors` |
| **Language** | C# 12 / .NET 8 |

---

## 🏗 Architecture

```
┌─────────────────────────────────────────────────────┐
│                    Client (Frontend)                 │
└────────────────────────┬────────────────────────────┘
                         │ HTTP / REST
┌────────────────────────▼────────────────────────────┐
│              ASP.NET Core 8 Web API                 │
│  ┌──────────────┐  ┌──────────────┐  ┌───────────┐  │
│  │  Accounts    │  │  Education   │  │  Entertain│  │
│  │  Controller  │  │  Controllers │  │  ment Ctrl│  │
│  └──────────────┘  └──────────────┘  └───────────┘  │
│  ┌──────────────────────────────────────────────┐    │
│  │           Admin Controller                   │    │
│  └──────────────────────────────────────────────┘    │
│  ┌──────────────────────────────────────────────┐    │
│  │     Services: AI Answer Checker, Email       │    │
│  └──────────────────────────────────────────────┘    │
│  ┌──────────────────────────────────────────────┐    │
│  │        Entity Framework Core (EF8)           │    │
│  └──────────────────────────────────────────────┘    │
└────────────────────────┬────────────────────────────┘
                         │
          ┌──────────────▼──────────────┐
          │      SQL Server Database    │
          └─────────────────────────────┘
                         │
          ┌──────────────▼──────────────┐
          │     Google Gemini AI API    │
          └─────────────────────────────┘
```

---

## 🎮 Game Modes

Both **Education** and **Entertainment** domains share three game modes:

### 1. 🏅 Difficulty Game
Questions are served incrementally by difficulty level. Students start at **Easy** and advance through **Medium → Difficult → Very Difficult → Extreme** as they answer correctly. Points are awarded per level.

### 2. 💡 Hint Game
Questions include optional progressive hints. Students choose whether to use hints before answering — using fewer hints yields higher points. The AI evaluates answers to allow paraphrased or partial responses.

### 3. ⚽ Offside Game
A rapid-fire mode where students answer a set of questions in a single session. Designed for timed, competitive play.

---

## 📡 API Endpoints

### 🔑 Authentication (`/api/Accounts`)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/Register` | Register as student or teacher |
| `POST` | `/Login` | Login and receive JWT token |
| `POST` | `/ForgetPassword` | Request password reset email |
| `POST` | `/ResetPassword` | Reset password with token |
| `PUT`  | `/ChangePassword` | Change authenticated user's password |

### 🛡 Admin (`/api/Admin`)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/GetUnderReviewQuestions` | List all pending questions |
| `PUT`  | `/ApproveQuestion/{id}` | Approve a teacher-submitted question |
| `PUT`  | `/RejectQuestion/{id}` | Reject a teacher-submitted question |
| `POST` | `/AddGrade` | Add a new school grade |
| `POST` | `/AddSubject` | Add a new subject |
| `POST` | `/AddGradeSubject` | Assign a subject to a grade |

### 📚 Education — Difficulty Game (`/api/Education/DifficultyGame`)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/GetQuestion` | Get next question by difficulty |
| `POST` | `/SubmitAnswer` | Submit and evaluate an answer (AI-graded) |
| `POST` | `/MakeQuestion` | Teacher submits a new question |
| `POST` | `/AddChapter` | Add a chapter for a subject |

### 💡 Education — Hint Game (`/api/Education/HintGame`)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/GetQuestion` | Get question with available hints |
| `GET`  | `/GetHint` | Retrieve the next hint for a question |
| `POST` | `/SubmitAnswer` | Submit answer with hint usage tracked |

### ⚡ Education — Offside Game (`/api/Education/OffsideGame`)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/GetSixQuestions` | Get a batch of 6 questions |
| `POST` | `/SubmitAnswers` | Submit all answers at once |

> 📝 **Entertainment** domain mirrors the same three game controllers under `/api/Entertainment/`.

---

## 🗃 Data Models

```
Students ──── has a ──── Grade
    │
    └──── plays ──── QuestionsHistory

Teachers ──── belongs to ──── Subject
    │
    └──── submits ──── EducationQuestions / EntertainmentQuestions

Questions (abstract)
    ├── EducationQuestions  (inherits → links to Chapter → Subject → Grade)
    └── EntertainmentQuestions
         │
         └──── has many ──── Hints

Grades ──── has many ──── GradeSubject ──── has ──── Subjects
                                              │
                                              └── has many ──── Chapters
```

### Key Enumerations
| Enum | Values |
|---|---|
| `Difficulty` | `easy`, `medium`, `difficult`, `very_difficult`, `extreme` |
| `Status` | `underreview`, `approved`, `rejected` |
| `QuestionType` | `Education`, `Entertainment` |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local or remote)
- A [Google Gemini API Key](https://aistudio.google.com/app/apikey)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/your-username/EduPlay.git
cd EduPlay

# 2. Restore NuGet packages
dotnet restore

# 3. Apply database migrations
dotnet ef database update

# 4. Run the application
dotnet run
```

The API will be available at `https://localhost:7070` and the Swagger UI at `https://localhost:7070/swagger`.

---

## ⚙ Configuration

Copy `appsettings.json` and fill in your own values. **Never commit secrets to version control.**

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  },
  "ConnectionStrings": {
    "con": "Server=YOUR_SERVER; Database=EduPlay; User Id=YOUR_USER; Password=YOUR_PASSWORD; Encrypt=True; TrustServerCertificate=True;"
  },
  "jwt": {
    "secretKey": "YOUR_BASE64_SECRET_KEY",
    "AudienceIP": "http://localhost:4200/",
    "IssuerIP": "https://localhost:7070/"
  }
}
```

> ⚠️ **Security Note:** Add `appsettings.json` to your `.gitignore` or use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) / environment variables for production deployments.

---

## 🗄 Database Setup

The project uses **Entity Framework Core Code-First** migrations.

```bash
# Create a new migration (after model changes)
dotnet ef migrations add <MigrationName>

# Apply all pending migrations to DB
dotnet ef database update

# Rollback to a specific migration
dotnet ef database update <MigrationName>
```

---

## 📁 Project Structure

```
Graduation Project/
│
├── Controllers/
│   ├── AccountsController.cs       # Registration, Login, Password management
│   ├── AdminController.cs          # Question review & curriculum management
│   ├── Education/
│   │   ├── DifficultyGameController.cs
│   │   ├── HintGameController.cs
│   │   └── OffsideGameController.cs
│   └── Entertainment/
│       ├── DifficultyGameController.cs
│       ├── HintGameController.cs
│       └── OffsideGameController.cs
│
├── Models/
│   ├── Questions.cs                # Abstract base for all questions
│   ├── EducationQuestions.cs       # Curriculum-based questions
│   ├── EntertainmentQuestions.cs   # General knowledge questions
│   ├── Hints.cs                    # Progressive hints per question
│   ├── students.cs                 # Student entity with grade & points
│   ├── Teachers.cs                 # Teacher entity with subject link
│   ├── Grades.cs                   # School grades
│   ├── Subjects.cs                 # Academic subjects
│   ├── Chapters.cs                 # Chapters within subjects
│   ├── GradeSubject.cs             # Grade–Subject mapping
│   ├── QuestionsHistory.cs         # Student answer history
│   ├── Admin.cs                    # Admin entity
│   └── context.cs                  # EF Core DbContext
│
├── services/
│   ├── AiAnswerChecker.cs          # Google Gemini AI integration
│   ├── EmailService.cs             # SMTP email sender
│   └── IEmailService.cs            # Email service interface
│
├── Migrations/                     # EF Core migration history
├── Program.cs                      # App startup, DI, middleware config
└── appsettings.json                # App configuration (DO NOT commit secrets)
```

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. **Fork** the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "feat: add your feature"`
4. Push to your branch: `git push origin feature/your-feature-name`
5. Open a **Pull Request**

Please make sure your code follows existing conventions and all migrations are included.

---

## 👥 Team

> This project was developed as a graduation project. Feel free to update this section with your team members.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

<p align="center">Made with ❤️ as a Graduation Project — Powered by ASP.NET Core 8 & Google Gemini AI</p>
