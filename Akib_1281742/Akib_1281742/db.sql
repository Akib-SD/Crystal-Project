CREATE TABLE courses
(
	courseid INT IDENTITY PRIMARY KEY,
	coursename NVARCHAR(40)  NOT NULL,
	fee MONEY NOT NULL,
	startdate DATE NOT NULL
)
go
CREATE TABLE tutors
(
	tutorid INT IDENTITY PRIMARY KEY,
	tutorname NVARCHAR(40)  NOT NULL,
	phone NVARCHAR(20) NOT NULL,
	picture NVARCHAR(40) NOT NULL,
	available BIT,
	courseid INT NOT NULL REFERENCES courses ( courseid)
)