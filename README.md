# TelemedicinaOdonto_Fase1_MVP

Proyecto MVP de telemedicina odontológica con backend en ASP.NET Core 8 (Clean Architecture) y frontend React (Vite + Material UI).

## Estructura de carpetas

```
/backend
  /TelemedicinaOdonto.sln
  /Domain
  /Application
  /Infrastructure
  /WebApi
/tests
/frontend
  /pages
  /components
  /services
  /routes
  /auth
/sql
  01_create_database.sql
  02_create_tables.sql
  03_seed_minimal.sql
```

## Backend

### Requisitos
- .NET SDK 8
- SQL Server LocalDB o SQL Express

### Configuración
1. Editar `backend/WebApi/appsettings.json` si se necesita ajustar la cadena de conexión.
2. Ejecutar migraciones con EF Core:

```bash
dotnet ef migrations add InitialCreate --project backend/Infrastructure --startup-project backend/WebApi
dotnet ef database update --project backend/Infrastructure --startup-project backend/WebApi
```

3. Ejecutar API:

```bash
dotnet run --project backend/WebApi
```

### Credenciales seed
- Admin: `admin@telemed.com` / `Admin123$`
- Dentist: `dentist@telemed.com` / `Dentist123$`

## Frontend

### Requisitos
- Node.js 18+

### Ejecución

```bash
cd frontend
npm install
npm run dev
```

## SQL Scripts
- `sql/01_create_database.sql`: crea base de datos.
- `sql/02_create_tables.sql`: crea tablas requeridas.
- `sql/03_seed_minimal.sql`: datos mínimos (KB + clinic + service).

## Matriz de cumplimiento (CA → endpoints → tablas → pantallas)

| Criterio | Endpoints | Tablas | Pantallas |
| --- | --- | --- | --- |
| CA1-01 Chat con sesión + historial | `POST /chat/sessions`, `POST /chat/sessions/{id}/messages`, `GET /chat/sessions/{id}/messages` | `ChatSessions`, `ChatMessages` | ChatPage |
| CA1-02 Dolor fuerte + hinchazón → prioridad Alta/Emergencia | `POST /triage` | `TriageResults` | TriagePage |
| CA1-03 Si no se resuelve por chat, ofrecer 3 horarios y crear cita | `GET /appointments/available`, `POST /appointments` | `DentistSchedules`, `Appointments` | AppointmentsPage |
| CA1-04 Odontólogo ve cita con resumen + triage | `GET /dentist/appointments`, `GET /dentist/appointments/{id}` | `Appointments`, `TriageResults` | DentistDashboardPage |
| CA1-05 Ficha médica se guarda y asocia al paciente | `POST /patients/{id}/medical-profile` | `PatientMedicalProfiles`, `Patients` | MedicalProfilePage |
| CA1-06 Auditoría al visualizar ficha médica | `GET /patients/{id}/medical-profile`, `GET /admin/audit` | `AuditLogs` | AdminKbPage (audit) |

## Notas de negocio
- La IA básica consulta `KnowledgeBaseItems` y si no encuentra coincidencia, sugiere agendar.
- La agenda retorna exactamente 3 horarios disponibles.
- La auditoría se registra únicamente en el backend al visualizar ficha médica.
