-- =====================================================
-- PROYECTO: SkillHub - Base de Datos Relacional
-- AUTOR: Eduardo Beleño | CLAN: C# | CC: 1043439529
-- =====================================================

-- 1. TABLA DE ROLES
CREATE TABLE IF NOT EXISTS AspNetRoles (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    NormalizedName TEXT NOT NULL,
    ConcurrencyStamp TEXT
);

-- 2. TABLA DE USUARIOS
CREATE TABLE IF NOT EXISTS AspNetUsers (
    Id TEXT PRIMARY KEY,
    UserName TEXT NOT NULL,
    NormalizedUserName TEXT NOT NULL,
    Email TEXT NOT NULL,
    NormalizedEmail TEXT NOT NULL,
    EmailConfirmed INTEGER NOT NULL,
    PasswordHash TEXT,
    SecurityStamp TEXT,
    ConcurrencyStamp TEXT,
    PhoneNumber TEXT,
    PhoneNumberConfirmed INTEGER NOT NULL,
    TwoFactorEnabled INTEGER NOT NULL,
    LockoutEnd TEXT,
    LockoutEnabled INTEGER NOT NULL,
    AccessFailedCount INTEGER NOT NULL,
    FullName TEXT NOT NULL,
    Specialty TEXT,
    Role TEXT NOT NULL,
    CreatedAt TEXT NOT NULL
);

-- 3. RELACIÓN USUARIO - ROLES
CREATE TABLE IF NOT EXISTS AspNetUserRoles (
    UserId TEXT NOT NULL,
    RoleId TEXT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- 4. TABLA DE TALLERES
CREATE TABLE IF NOT EXISTS Workshops (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    InstructorId TEXT NOT NULL,
    StartDate TEXT NOT NULL,
    EndDate TEXT NOT NULL,
    MaxCapacity INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (InstructorId) REFERENCES AspNetUsers(Id)
);

-- 5. TABLA DE INSCRIPCIONES
CREATE TABLE IF NOT EXISTS Enrollments (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ParticipantId TEXT NOT NULL,
    WorkshopId INTEGER NOT NULL,
    EnrolledAt TEXT NOT NULL,
    Status INTEGER NOT NULL,
    CancelledAt TEXT,
    CancelledByUserId TEXT,
    FOREIGN KEY (ParticipantId) REFERENCES AspNetUsers(Id),
    FOREIGN KEY (WorkshopId) REFERENCES Workshops(Id),
    FOREIGN KEY (CancelledByUserId) REFERENCES AspNetUsers(Id)
);

-- 6. ROLES INICIALES
INSERT OR IGNORE INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('role-employee', 'Employee', 'EMPLOYEE');
INSERT OR IGNORE INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('role-participant', 'Participant', 'PARTICIPANT');

-- 7. INSTRUCTORES INICIALES
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-001', 'andres.mejia@correo.com', 'ANDRES.MEJIA@CORREO.COM', 'andres.mejia@correo.com', 'ANDRES.MEJIA@CORREO.COM', 1, 0, 0, 0, 0, 'Andrés Felipe Mejía Arango', 'Fotografía', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-001', 'role-employee');
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-002', 'rosa.barrios@correo.com', 'ROSA.BARRIOS@CORREO.COM', 'rosa.barrios@correo.com', 'ROSA.BARRIOS@CORREO.COM', 1, 0, 0, 0, 0, 'Rosa Elena Barrios Polo', 'Cocina', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-002', 'role-employee');
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-003', 'mauricio.jimenez@correo.com', 'MAURICIO.JIMENEZ@CORREO.COM', 'mauricio.jimenez@correo.com', 'MAURICIO.JIMENEZ@CORREO.COM', 1, 0, 0, 0, 0, 'Mauricio Jiménez Cárdenas', 'Oratoria', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-003', 'role-employee');
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-004', 'natalia.herrera@correo.com', 'NATALIA.HERRERA@CORREO.COM', 'natalia.herrera@correo.com', 'NATALIA.HERRERA@CORREO.COM', 1, 0, 0, 0, 0, 'Natalia Herrera Castaño', 'Diseño gráfico', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-004', 'role-employee');
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-005', 'hernan.quintero@correo.com', 'HERNAN.QUINTERO@CORREO.COM', 'hernan.quintero@correo.com', 'HERNAN.QUINTERO@CORREO.COM', 1, 0, 0, 0, 0, 'Hernán Darío Quintero Gil', 'Música', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-005', 'role-employee');
INSERT OR IGNORE INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FullName, Specialty, Role, CreatedAt) VALUES ('inst-006', 'paola.vargas@correo.com', 'PAOLA.VARGAS@CORREO.COM', 'paola.vargas@correo.com', 'PAOLA.VARGAS@CORREO.COM', 1, 0, 0, 0, 0, 'Paola Andrea Vargas Mora', 'Marketing digital', 'Employee', datetime('now'));
INSERT OR IGNORE INTO AspNetUserRoles (UserId, RoleId) VALUES ('inst-006', 'role-employee');

-- 8. TALLERES INICIALES
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('FOT-001', 'Fotografía con celular', 'Composición, luz y edición básica desde el celular', 'inst-001', '2026-12-05 08:00:00', '2026-12-05 12:00:00', 15, datetime('now'));
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('COC-001', 'Cocina costeña', 'Arroz con coco, patacones y mojarra frita', 'inst-002', '2026-12-05 14:00:00', '2026-12-05 18:00:00', 12, datetime('now'));
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('ORA-001', 'Oratoria para principiantes', 'Técnicas para hablar en público con seguridad', 'inst-003', '2026-12-12 09:00:00', '2026-12-12 12:00:00', 20, datetime('now'));
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('DIS-001', 'Diseño en Canva', 'Piezas gráficas para redes sociales', 'inst-004', '2026-12-19 14:00:00', '2026-12-19 17:00:00', 18, datetime('now'));
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('MUS-001', 'Guitarra desde cero', 'Acordes básicos y primeras canciones', 'inst-005', '2026-12-12 14:00:00', '2026-12-12 17:00:00', 10, datetime('now'));
INSERT OR IGNORE INTO Workshops (Code, Title, Description, InstructorId, StartDate, EndDate, MaxCapacity, CreatedAt) VALUES ('MKT-001', 'Marketing en redes sociales', 'Estrategia de contenido para pequeños negocios', 'inst-006', '2026-12-19 08:00:00', '2026-12-19 12:00:00', 20, datetime('now'));
