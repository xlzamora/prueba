USE TelemedicinaOdontoDb;
GO

CREATE TABLE Patients (
  PatientId uniqueidentifier PRIMARY KEY,
  UserId nvarchar(450) NOT NULL,
  FullName nvarchar(150) NOT NULL,
  DocumentId nvarchar(50) NOT NULL,
  BirthDate date NOT NULL,
  CreatedAt datetime2 NOT NULL
);

CREATE TABLE PatientMedicalProfiles (
  PatientId uniqueidentifier PRIMARY KEY,
  BloodType nvarchar(10) NOT NULL,
  AllergiesText nvarchar(max) NOT NULL,
  ConditionsText nvarchar(max) NOT NULL,
  UpdatedAt datetime2 NOT NULL,
  CONSTRAINT FK_PatientMedicalProfiles_Patients FOREIGN KEY (PatientId) REFERENCES Patients(PatientId)
);

CREATE TABLE ChatSessions (
  SessionId uniqueidentifier PRIMARY KEY,
  PatientId uniqueidentifier NULL,
  StartedAt datetime2 NOT NULL,
  EndedAt datetime2 NULL,
  Outcome nvarchar(40) NOT NULL,
  CONSTRAINT FK_ChatSessions_Patients FOREIGN KEY (PatientId) REFERENCES Patients(PatientId)
);

CREATE TABLE ChatMessages (
  MessageId uniqueidentifier PRIMARY KEY,
  SessionId uniqueidentifier NOT NULL,
  Sender nvarchar(20) NOT NULL,
  Text nvarchar(max) NOT NULL,
  CreatedAt datetime2 NOT NULL,
  Intent nvarchar(50) NULL,
  Confidence decimal(5,2) NULL,
  CONSTRAINT FK_ChatMessages_Sessions FOREIGN KEY (SessionId) REFERENCES ChatSessions(SessionId)
);

CREATE TABLE TriageResults (
  TriageId uniqueidentifier PRIMARY KEY,
  SessionId uniqueidentifier NOT NULL,
  PatientId uniqueidentifier NOT NULL,
  MainComplaint nvarchar(20) NOT NULL,
  Priority nvarchar(20) NOT NULL,
  RedFlagsJson nvarchar(max) NOT NULL,
  AnswersJson nvarchar(max) NOT NULL,
  SummaryText nvarchar(max) NOT NULL,
  RecommendedNextStep nvarchar(max) NOT NULL,
  CreatedAt datetime2 NOT NULL,
  CONSTRAINT FK_TriageResults_Sessions FOREIGN KEY (SessionId) REFERENCES ChatSessions(SessionId),
  CONSTRAINT FK_TriageResults_Patients FOREIGN KEY (PatientId) REFERENCES Patients(PatientId)
);

CREATE TABLE KnowledgeBaseItems (
  KbId uniqueidentifier PRIMARY KEY,
  Category nvarchar(20) NOT NULL,
  Title nvarchar(150) NOT NULL,
  Content nvarchar(max) NOT NULL,
  TagsJson nvarchar(max) NOT NULL,
  IsActive bit NOT NULL,
  UpdatedAt datetime2 NOT NULL
);

CREATE TABLE Clinics (
  ClinicId uniqueidentifier PRIMARY KEY,
  Name nvarchar(120) NOT NULL,
  Address nvarchar(200) NOT NULL,
  City nvarchar(100) NOT NULL,
  IsActive bit NOT NULL
);

CREATE TABLE Dentists (
  DentistId uniqueidentifier PRIMARY KEY,
  UserId nvarchar(450) NOT NULL,
  FullName nvarchar(150) NOT NULL,
  Specialty nvarchar(100) NOT NULL,
  ClinicId uniqueidentifier NOT NULL,
  IsActive bit NOT NULL,
  CONSTRAINT FK_Dentists_Clinics FOREIGN KEY (ClinicId) REFERENCES Clinics(ClinicId)
);

CREATE TABLE Services (
  ServiceId uniqueidentifier PRIMARY KEY,
  Name nvarchar(120) NOT NULL,
  DurationMin int NOT NULL,
  Description nvarchar(max) NOT NULL,
  IsActive bit NOT NULL
);

CREATE TABLE DentistSchedules (
  ScheduleId uniqueidentifier PRIMARY KEY,
  DentistId uniqueidentifier NOT NULL,
  DayOfWeek int NOT NULL,
  StartTime time NOT NULL,
  EndTime time NOT NULL,
  SlotMinutes int NOT NULL,
  IsActive bit NOT NULL,
  CONSTRAINT FK_DentistSchedules_Dentists FOREIGN KEY (DentistId) REFERENCES Dentists(DentistId)
);

CREATE TABLE Appointments (
  AppointmentId uniqueidentifier PRIMARY KEY,
  PatientId uniqueidentifier NOT NULL,
  DentistId uniqueidentifier NOT NULL,
  ClinicId uniqueidentifier NOT NULL,
  ServiceId uniqueidentifier NOT NULL,
  StartAt datetime2 NOT NULL,
  EndAt datetime2 NOT NULL,
  Status nvarchar(20) NOT NULL,
  Priority nvarchar(20) NOT NULL,
  TriageId uniqueidentifier NULL,
  SummaryText nvarchar(max) NOT NULL,
  CreatedAt datetime2 NOT NULL,
  UpdatedAt datetime2 NOT NULL,
  CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),
  CONSTRAINT FK_Appointments_Dentists FOREIGN KEY (DentistId) REFERENCES Dentists(DentistId),
  CONSTRAINT FK_Appointments_Clinics FOREIGN KEY (ClinicId) REFERENCES Clinics(ClinicId),
  CONSTRAINT FK_Appointments_Services FOREIGN KEY (ServiceId) REFERENCES Services(ServiceId),
  CONSTRAINT FK_Appointments_Triage FOREIGN KEY (TriageId) REFERENCES TriageResults(TriageId)
);

CREATE TABLE AuditLogs (
  AuditId uniqueidentifier PRIMARY KEY,
  ActorUserId nvarchar(450) NOT NULL,
  ActorRole nvarchar(20) NOT NULL,
  Action nvarchar(20) NOT NULL,
  Entity nvarchar(40) NOT NULL,
  EntityId nvarchar(80) NOT NULL,
  Timestamp datetime2 NOT NULL,
  MetadataJson nvarchar(max) NOT NULL
);
GO
