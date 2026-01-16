CREATE DATABASE LanQuizerDB;
GO

USE LanQuizerDB;
GO

CREATE TABLE Teachers (
    TeacherID VARCHAR(12) PRIMARY KEY UNIQUE,      -- Example: 12-53700-3
    teacherEmail VARCHAR(100) NOT NULL UNIQUE,
    TeacherpassBox VARCHAR(100) NOT NULL,
    teacherName VARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO Teachers (TeacherID, teacherEmail, TeacherpassBox, teacherName)
VALUES
('23-53700-3', 'admin@aiub.edu', 'admin', 'ADMIN'),
('12-34567-8', 'teacher@aiub.com', 'admin', 'Kazi Asif Ahmed');
GO

Select * from Teachers;


