USE TelemedicinaOdontoDb;
GO

INSERT INTO Clinics (ClinicId, Name, Address, City, IsActive)
VALUES (NEWID(), 'Clínica Central', 'Av. Principal 123', 'Lima', 1);

INSERT INTO Services (ServiceId, Name, DurationMin, Description, IsActive)
VALUES (NEWID(), 'Consulta general', 30, 'Consulta odontológica inicial.', 1);

INSERT INTO KnowledgeBaseItems (KbId, Category, Title, Content, TagsJson, IsActive, UpdatedAt)
VALUES
(NEWID(), 'FAQ', 'Dolor de muelas', 'Puedes enjuagar con agua tibia y evitar alimentos duros. Si el dolor es fuerte, agenda una cita.', '["dolor","muelas"]', 1, SYSDATETIME()),
(NEWID(), 'Servicio', 'Limpieza dental', 'Ofrecemos limpieza dental con evaluación básica.', '["limpieza","profilaxis"]', 1, SYSDATETIME());
GO
