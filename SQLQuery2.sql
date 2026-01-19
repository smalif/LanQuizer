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

Drop Table Students;

CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentID VARCHAR(11),
    StudentName VARCHAR(100),
    Section VARCHAR(50),
    Course VARCHAR(50),
    TeacherID NVARCHAR(10) NOT NULL,
    TeacherEmail NVARCHAR(400) NOT NULL
);
GO


Select * from Students



/*Demo Table*/
CREATE TABLE QuizTable
(
    QuizID INT IDENTITY(1,1) PRIMARY KEY,
    ExamName NVARCHAR(200) NOT NULL,
    Course NVARCHAR(50) NOT NULL,
    Section NVARCHAR(50) NOT NULL,
    DurationMinutes INT NOT NULL,
    QuizPassword NVARCHAR(50) NOT NULL,
    TeacherID VARCHAR(12) NOT NULL,
    TeacherEmail NVARCHAR(100) NOT NULL,

    Questions NVARCHAR(MAX) NOT NULL,
    -- Example JSON:
    -- [
    --   { "Question":"What is 2+2?", "Options":["1","2","3","4"], "CorrectIndex":3, "Marks":1 },
    --   { "Question":"Capital of France?", "Options":["Paris","London","Berlin"], "CorrectIndex":0, "Marks":2 }
    -- ]

    CreatedAt DATETIME DEFAULT GETDATE(),
    StartTime DATETIME NULL
);
GO
Select * from QuizTable;


CREATE TABLE StudentAttempts
(
    AttemptID INT IDENTITY(1,1) PRIMARY KEY,
    QuizID INT NOT NULL,
    StudentID VARCHAR(11) NOT NULL,
    Section NVARCHAR(50) NOT NULL,
    Course NVARCHAR(50) NOT NULL,

    RandomizedQuestions NVARCHAR(MAX) NOT NULL,
    -- Example after shuffling:
    -- [
    --   { "Question":"Capital of France?", "Options":["Berlin","Paris","London"], "CorrectIndex":1 },
    --   { "Question":"What is 2+2?", "Options":["3","4","1","2"], "CorrectIndex":1 }
    -- ]

    Answers NVARCHAR(MAX) NULL,
    -- Example when student selects options:
    -- [
    --   { "QuestionIndex":0, "SelectedOption":1 },
    --   { "QuestionIndex":1, "SelectedOption":3 }
    -- ]

    Score INT NULL,
    LoginTime DATETIME NOT NULL DEFAULT GETDATE(),
    SubmitTime DATETIME NULL
);
GO
Select * from StudentAttempts;


/*Dunny Data Insertion for QuizTable*/
INSERT INTO QuizTable (ExamName, Course, Section, DurationMinutes, QuizPassword, TeacherID, TeacherEmail, Questions)
VALUES 
(
    'Math Quiz Demo',
    'Math101',
    'A1',
    30,
    'quiz123',
    '12-34567-3',
    'teacher@aiub.com',
    '[
        {"Question":"2 + 2 = ?","Options":["1","2","3","4"],"CorrectIndex":3,"Marks":1},
        {"Question":"5 - 3 = ?","Options":["1","2","3","4"],"CorrectIndex":1,"Marks":1},
        {"Question":"3 * 3 = ?","Options":["6","9","12"],"CorrectIndex":1,"Marks":1},
        {"Question":"10 / 2 = ?","Options":["2","4","5","10"],"CorrectIndex":2,"Marks":1},
        {"Question":"7 + 6 = ?","Options":["12","13","14"],"CorrectIndex":1,"Marks":1},
        {"Question":"9 - 4 = ?","Options":["5","6","4"],"CorrectIndex":0,"Marks":1},
        {"Question":"6 * 2 = ?","Options":["10","12","14"],"CorrectIndex":1,"Marks":1},
        {"Question":"8 / 4 = ?","Options":["2","4"],"CorrectIndex":0,"Marks":1},
        {"Question":"1 + 1 = ?","Options":["1","2"],"CorrectIndex":1,"Marks":1},
        {"Question":"5 * 5 = ?","Options":["20","25","30"],"CorrectIndex":1,"Marks":1},
        {"Question":"12 / 3 = ?","Options":["3","4","6"],"CorrectIndex":1,"Marks":1},
        {"Question":"14 + 5 = ?","Options":["18","19","20"],"CorrectIndex":1,"Marks":1},
        {"Question":"7 - 2 = ?","Options":["4","5","6"],"CorrectIndex":1,"Marks":1},
        {"Question":"3 * 4 = ?","Options":["12","7","8"],"CorrectIndex":0,"Marks":1},
        {"Question":"10 / 5 = ?","Options":["2","5"],"CorrectIndex":0,"Marks":1},
        {"Question":"6 + 7 = ?","Options":["12","13","14"],"CorrectIndex":1,"Marks":1},
        {"Question":"9 - 6 = ?","Options":["2","3","4"],"CorrectIndex":1,"Marks":1},
        {"Question":"8 * 1 = ?","Options":["7","8","9"],"CorrectIndex":1,"Marks":1},
        {"Question":"4 / 2 = ?","Options":["1","2","3"],"CorrectIndex":1,"Marks":1},
        {"Question":"5 + 5 = ?","Options":["9","10","11"],"CorrectIndex":1,"Marks":1}
    ]'
);
GO


