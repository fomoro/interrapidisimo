SELECT 
    E.id AS estudiante_id, 
    E.nombre AS estudiante, 
    M.id AS materia_id, 
    M.nombre AS materia, 
    P.nombre AS profesor
FROM Registros R
JOIN Estudiantes E ON R.estudiante_id = E.id
JOIN Materias M ON R.materia_id = M.id
LEFT JOIN Profesores P ON M.profesor_id = P.id
WHERE E.id <> 1  -- Excluye al estudiante con id = 1
ORDER BY E.id, M.id;


----------------------------------------------------------------------------------------------------
-- 
SELECT 
    --E.id AS estudiante_id, 
    CASE 
        WHEN E.id IN (
            SELECT DISTINCT R2.estudiante_id 
            FROM Registros R2 
            WHERE R2.materia_id IN (SELECT materia_id FROM Registros WHERE estudiante_id = 1)
        ) 
        THEN E.nombre 
        ELSE '' 
    END AS estudiante,
    --M.id AS materia_id, 
    M.nombre AS materia, 
    P.nombre AS profesor
FROM Registros R
JOIN Estudiantes E ON R.estudiante_id = E.id
JOIN Materias M ON R.materia_id = M.id
LEFT JOIN Profesores P ON M.profesor_id = P.id
WHERE E.id <> 1  -- Excluye al estudiante con id = 1
ORDER BY E.id, M.id;


----------------------------------------------------------------------------------------------------
-- 
DECLARE @EstudianteId INT = 1;

SELECT
    e.nombre AS NombreEstudiante,
    m.nombre AS NombreMateria
FROM Registros r
JOIN Estudiantes e ON r.estudiante_id = e.id
JOIN Materias m ON r.materia_id = m.id
WHERE r.materia_id IN (
    SELECT materia_id
    FROM Registros
    WHERE estudiante_id = @EstudianteId
)
AND r.estudiante_id != @EstudianteId; -- Excluir al estudiante quemado