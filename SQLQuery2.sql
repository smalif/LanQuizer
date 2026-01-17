CREATE DATABASE LanQuizerDB;
GO

--Run this everythime to ensure you are using the correct database(when need to change/modify table)
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


UPDATE Teachers
SET TeacherID = '12-34567-3',
    teacherName = 'Kazi Asif Ahmed',
    TeacherpassBox = 'admin'
WHERE TeacherID = '23-53700-3';
GO


CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID VARCHAR(20),
    StudentName VARCHAR(100),
    Section VARCHAR(50),
    Course VARCHAR(50)
);
GO

Select * from Students;