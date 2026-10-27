# SkillHubeduardo .NET Performance Evaluation (Prueba de Desempeño)

SkillHubeduardo is a modern management platform built for a training center that offers short, on-site workshops in photography, cooking, public speaking, design, music, and digital marketing. It solves capacity over-enrollment, duplicate registrations, and schedule conflicts through comprehensive business domain rules and Clean Architecture.

---

## 🛠 Technologies & Architecture

- **C# / .NET 10**
- **Clean Architecture** (Domain, Application, Infrastructure, Presentation Layers: Web & API)
- **Entity Framework Core** with Code First & Automated Seeding
- **ASP.NET Core Identity** (Roles: `Employee`, `Participant`)
- **JWT (JSON Web Token)** Authentication & Cookie Authentication for the Web client
- **QuestPDF** for generating official PDF certificates
- **ClosedXML** for parsing Excel workshop enrollment lists (`.xlsx`)
- **Swagger / OpenAPI** for API exploration and testing

---

## 🏗 Architecture Structure

```text
SkillHubeduardo/
├── SkillHubeduardo.Domain/            # Entities, Enums, Interfaces (Domain Model)
├── SkillHubeduardo.Application/       # DTOs, Business Logic Services, Abstractions
├── SkillHubeduardo.Infrastructure/    # EF Core DbContext, Identity, Repositories, PDF & Excel
├── SkillHubeduardo.Api/               # REST API Endpoints protected with JWT
├── SkillHubeduardo.Web/               # Participant Web App (Razor Pages / MVC)
├── SkillHub_Data.xlsx                 # Official data source (Instructors, Participants, Workshops, Enrollments)
└── SkillHubeduardo_Database_Script.sql # Exported SQL script with schema and initial data
```

---

## 👥 Seeded Credentials (From SkillHub_Data.xlsx)

When starting the application, the database is automatically created and populated with all records from `SkillHub_Data.xlsx`:

### Instructors (Employees)
| Full Name | Email | Password | Specialty |
|---|---|---|---|
| Andrés Felipe Mejía Arango | `andres.mejia@correo.com` | `Instructor2026*` | Fotografía |
| Rosa Elena Barrios Polo | `rosa.barrios@correo.com` | `Instructor2026*` | Cocina |
| Mauricio Jiménez Cárdenas | `mauricio.jimenez@correo.com` | `Instructor2026*` | Oratoria |
| Natalia Herrera Castaño | `natalia.herrera@correo.com` | `Instructor2026*` | Diseño gráfico |
| Hernán Darío Quintero Gil | `hernan.quintero@correo.com` | `Instructor2026*` | Música |
| Paola Andrea Vargas Mora | `paola.vargas@correo.com` | `Instructor2026*` | Marketing digital |

### Sample Participants
| Full Name | Email | Password |
|---|---|---|
| Jorge Andrés Pérez Rojas | `jorge.perez@correo.com` | `Skillhub2026*` |
| Luz Marina Ospina Ríos | `luz.ospina@correo.com` | `Skillhub2026*` |
| Carlos Eduardo Rincón Díaz | `carlos.rincon@correo.com` | `Skillhub2026*` |
| Valentina Gómez Ruiz | `valentina.gomez@correo.com` | `Skillhub2026*` |
| Juan Camilo Restrepo Vélez | `juan.restrepo@correo.com` | `Skillhub2026*` |

---

## 🚀 How to Run and Deploy

### 1. Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### 2. Run the API (Backend & Swagger)
Open terminal in the project directory:
```bash
cd SkillHubeduardo.Api
dotnet run
```
Access Swagger UI at:
👉 **http://localhost:5000/swagger** (or https://localhost:5001/swagger)

### 3. Run the Web Application (Participant Client)
In another terminal:
```bash
cd SkillHubeduardo.Web
dotnet run
```
Access the web portal at:
👉 **http://localhost:7000** (or https://localhost:7001)

---

## 📋 API Endpoints Summary

- `POST /api/v1/auth/register` - Public: Register new participant
- `POST /api/v1/auth/login` - Public: Login and retrieve JWT token
- `GET /api/v1/workshops` - Public: List future workshops with calculated available spots
- `GET /api/v1/workshops/{id}` - Public: Workshop details
- `POST /api/v1/workshops` - Employee: Create new workshop
- `GET /api/v1/workshops/{id}/enrollments` - Employee: View attendees for a workshop
- `POST /api/v1/enrollments` - Participant: Enroll authenticated participant
- `GET /api/v1/enrollments/mine` - Participant: View current user enrollments
- `PATCH /api/v1/enrollments/{id}/cancel` - Owner or Employee: Cancel enrollment
- `POST /api/v1/enrollments/import` - Employee: Bulk enroll from Excel (`SkillHub_Data.xlsx` - sheet `Enrollments`)
- `GET /api/v1/enrollments/{id}/certificate` - Owner or Employee: Download PDF certificate
- `GET /api/v1/reports/top-workshops` - Employee: Report of workshops ordered by active enrollments

---

## 👨‍💻 Author

- **Full Name:** Eduardo Beleño
- **Clan:** Clan C#
- **Email:** luisforero662@gmail.com
- **ID Document:** 1043439529