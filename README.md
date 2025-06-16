# 🔍 SECrawler – Search Engine Crawler (Technical Test)

SECrawler is a full-stack web application designed to perform automated search engine queries and track the ranking of a specified domain (e.g., `www.infotrack.co.uk`). This project was built as part of a technical assessment, showcasing clean architecture, modern .NET practices, and a React-based frontend.

---

## Architecture & Design

* **Backend**: [.NET 8](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8) with **Clean Architecture** principles
* **Frontend**: [Vite](https://vitejs.dev/) + [React](https://react.dev/)
* **CQRS** pattern via [MediatR](https://github.com/jbogard/MediatR)
* **Validation** via [FluentValidation](https://docs.fluentvalidation.net/)
* **API Testing**: [NUnit](https://nunit.org/), [Moq](https://github.com/moq/moq)
* **Documentation**: Integrated Swagger (OpenAPI)
* **Database**: Entity Framework Core + SQLite or SQL Server (configurable)

---

## ⚙️ API Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/alikeb1998/InfoTrackTest.git
   cd SECrawler
   ```

2. **Set up the database**
   Edit the `appsettings.json` in `SECrawler.Api` to add a valid connection string:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "your_connection_string_here"
   }
   ```

3. **Run the API**
   The API is self-initializing:

   * It applies EF Core migrations automatically on startup.
   * Swagger UI will be available at `/swagger`.

   Run from the terminal or Rider/Visual Studio:

   ```bash
   dotnet run --project SECrawler.Api
   ```

---

## 💻 Frontend Setup

1. Navigate to the frontend project directory:

   ```bash
   cd SECrawler.Client
   ```

2. Install dependencies and run the development server:

   ```bash
   npm install
   npm run dev
   ```

3. Open the app in your browser:

   ```
   http://localhost:5173
   ```

---

## 🦪 Testing

### Backend

* Tests are written with **NUnit** and **Moq**
* Test projects follow Clean Architecture boundaries
* To run tests:

  ```bash
  dotnet test
  ```

### Frontend

* Unit tests use **Vitest** and **React Testing Library**
* To run frontend tests:

  ```bash
  npx vitest
  ```

---

## 📚 Features

* 🧠 Uses real search engine endpoints (e.g., Bing, Google)
* 🔎 Dynamically parses and extracts link rankings for a specific domain
* 📊 Stores search result history with keywords, engine type, and timestamp
* 📌 Displays result history in a styled table on the frontend
* 🌐 Swagger support for all API endpoints

---

## 🧱 Clean Architecture Layers

```
SECrawler.sln
│
├── SECrawler.Domain            → Entities, Enums, Core Interfaces
├── SECrawler.Application       → CQRS Handlers, Validators
├── SECrawler.Infrastructure    → EF Repositories
├── SECrawler.Api               → Controllers, Swagger, Middleware
├── SECrawler.Client            → Vite + React frontend
└── SECrawler.Tests             → Unit test projects for all layers
```

---

## 🚀 Possible Improvements (Given More Time)

* 🔐 Implement user **authentication & authorization**
* 🔄 Add **AutoMapper** to streamline DTO/entity mapping
* 🎨 Improve **UI design** and accessibility
* 🧵 Introduce background jobs for scheduled crawling
* 🧠 Add AI-based ranking analysis (e.g., semantic relevance)

---

## 📄 License

This project is open-source and was created as part of a technical coding test.

---
![image](https://github.com/user-attachments/assets/a3d2fc7b-e556-4156-875c-d03b4143b645)

## 🙇‍♂️ Author

**Ali Keb** – Full Stack Developer
