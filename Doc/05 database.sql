-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'registro_estudiantes')
BEGIN
    CREATE DATABASE registro_estudiantes;
END
GO

-- Usar la base de datos
USE registro_estudiantes;
GO

-- Crear la tabla de Estudiantes si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Estudiantes' AND xtype = 'U')
BEGIN
    CREATE TABLE Estudiantes (
        id INT PRIMARY KEY IDENTITY(1,1),
        nombre VARCHAR(100) NOT NULL,
        carrera VARCHAR(100) NOT NULL
    );
END
GO

-- Crear la tabla de Profesores si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Profesores' AND xtype = 'U')
BEGIN
    CREATE TABLE Profesores (
        id INT PRIMARY KEY IDENTITY(1,1),
        nombre VARCHAR(100) NOT NULL
    );
END
GO

-- Crear la tabla de Materias si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Materias' AND xtype = 'U')
BEGIN
    CREATE TABLE Materias (
        id INT PRIMARY KEY IDENTITY(1,1),
        nombre VARCHAR(100) NOT NULL,
        creditos INT NOT NULL DEFAULT 3,
        profesor_id INT,
        FOREIGN KEY (profesor_id) REFERENCES Profesores(id)
    );
END
GO

-- Crear la tabla de Registros si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Registros' AND xtype = 'U')
BEGIN
    CREATE TABLE Registros (
        id INT PRIMARY KEY IDENTITY(1,1),
        estudiante_id INT,
        materia_id INT,
        FOREIGN KEY (estudiante_id) REFERENCES Estudiantes(id),
        FOREIGN KEY (materia_id) REFERENCES Materias(id)
    );
END
GO

-- Eliminar datos existentes
DELETE FROM Registros;
DELETE FROM Estudiantes;
DELETE FROM Materias;
DELETE FROM Profesores;
GO

-- Insertar profesores con nombres más realistas
INSERT INTO Profesores (nombre) VALUES
('Carlos Pérez'),
('María González'),
('Andrés Rodríguez'),
('Luisa Martínez'),
('Javier Fernández');
GO

-- Insertar materias (cada profesor dicta 2 materias, más una sin profesor)
INSERT INTO Materias (nombre, creditos, profesor_id) VALUES
('Matemáticas Avanzadas', 3, 1),
('Física Cuántica', 3, 1),
('Programación en Python', 3, 2),
('Bases de Datos', 3, 2),
('Inteligencia Artificial', 3, 3),
('Redes de Computadoras', 3, 3),
('Ética Profesional', 3, 4),
('Gestión de Proyectos', 3, 4),
('Marketing Digital', 3, 5),
('Finanzas Empresariales', 3, 5),
('Ciencia de Datos', 3, NULL); -- Materia sin profesor
GO

-- Insertar estudiantes con nombres y programas más realistas, más uno sin registros
INSERT INTO Estudiantes (nombre, carrera) VALUES
('Alejandro Ramírez', 'Ingeniería de Software'),
('Laura Méndez', 'Administración de Empresas'),
('Felipe Torres', 'Ingeniería de Software'),
('Diana López', 'Contaduría Pública'),
('Camila Suárez', 'Administración de Empresas'),
('Santiago Vega', 'Ingeniería de Software'); -- Estudiante sin registros
GO

-- Insertar registros (cada estudiante se registra en 3 materias sin repetir profesor)
INSERT INTO Registros (estudiante_id, materia_id) VALUES
(1, 1), (1, 3), (1, 5),  -- Alejandro en Matemáticas, Python, IA
(2, 2), (2, 4), (2, 6),  -- Laura en Física, BD, Redes
(3, 7), (3, 8), (3, 9),  -- Felipe en Ética, Proyectos, Marketing
(4, 1), (4, 5), (4, 10), -- Diana en Matemáticas, IA, Finanzas
(5, 3), (5, 6), (5, 7);  -- Camila en Python, Redes, Ética
GO
