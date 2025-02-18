CREATE TABLE subjects
(
	id INT PRIMARY KEY IDENTITY,
	title NVARCHAR (50) NOT NULL,
	[level] VARCHAR(20) NOT NULL
)

CREATE TABLE teachers
(
	id INT PRIMARY KEY IDENTITY,
	teacher_code VARCHAR(50) NOT NULL,
	full_name NVARCHAR(80) NOT NULL,
	gender CHAR(1) CHECK (gender = 'm' OR gender = 'f'),
	date_of_birth DATE NOT NULL,
	email NVARCHAR(50) NOT NULL,
	phone NVARCHAR(50) NOT NULL,
	working_days INT NOT NULL
)

CREATE TABLE classrooms
(
	id INT PRIMARY KEY IDENTITY,
	[floor] INT NOT NULL,
	capacity INT NOT NULL,
	[description] NVARCHAR(50) NOT NULL 
)

CREATE TABLE parents
(
	id INT PRIMARY KEY IDENTITY,
	parent_code VARCHAR(50) NOT NULL,
	full_name NVARCHAR(80) NOT NULL,
	email NVARCHAR(30) NOT NULL,
	phone NVARCHAR(12) NOT NULL
)

CREATE TABLE classes
(
	id INT PRIMARY KEY IDENTITY,
	class_number INT NOT NULL,
	class_letter NCHAR(1) NOT NULL,
	class_teacher_id INT REFERENCES teachers(id),
	classroom_id INT REFERENCES classrooms(id)
)

CREATE TABLE students
(
	id INT PRIMARY KEY IDENTITY,
	students_code VARCHAR(20) NOT NULL,
	full_name NVARCHAR(80) NOT NULL,
	gender CHAR(1) CHECK(gender = 'm' OR gender = 'f'),
	date_of_birth DATE NOT NULL,
	email NVARCHAR(30) NOT NULL,
	phone VARCHAR(12) NOT NULL,
	class_id INT REFERENCES classes(id),
	is_active BIT NOT NULL
)

CREATE TABLE teachers_subjects
(
	id INT PRIMARY KEY IDENTITY,
	teacher_id INT REFERENCES teachers(id),
	subject_id INT REFERENCES subjects(id)
)

CREATE TABLE classes_subjects
(
	id INT PRIMARY KEY IDENTITY,
	class_id INT REFERENCES classes(id),
	subject_id INT REFERENCES subjects(id)
)

CREATE TABLE students_parents
(
	id INT PRIMARY KEY IDENTITY,
	student_id INT REFERENCES students(id),
	parent_id INT REFERENCES parents(id)
)

INSERT INTO subjects (title, [level]) VALUES
('Mathematics', 'High School'),
('Science', 'High School'),
('History', 'Middle School'),
('Literature', 'Middle School'),
('Biology', 'High School'),
('Physics', 'College'),
('Chemistry', 'College'),
('Geography', 'High School'),
('Music', 'Middle School'),
('Art', 'College');

INSERT INTO teachers (teacher_code, full_name, gender, date_of_birth, email, phone, working_days) VALUES
('T001', 'John Doe', 'm', '1985-02-15', 'johndoe@email.com', '555-1234', 5),
('T002', 'Mary Smith', 'f', '1979-08-21', 'marysmith@email.com', '555-5678', 4),
('T003', 'David Williams', 'm', '1982-11-10', 'davidw@email.com', '555-9876', 5),
('T004', 'Emily Johnson', 'f', '1990-05-30', 'emilyj@email.com', '555-3456', 3),
('T005', 'James Brown', 'm', '1987-09-14', 'jamesb@email.com', '555-7890', 5);

INSERT INTO classrooms ([floor], capacity, [description]) VALUES
(1, 30, 'Science Lab'),
(2, 40, 'Mathematics Classroom'),
(3, 25, 'History Classroom'),
(1, 35, 'Art Studio'),
(2, 50, 'Large Auditorium');

INSERT INTO parents (parent_code, full_name, email, phone) VALUES
('P001', 'Alice Green', 'alicegreen@email.com', '555-1111'),
('P002', 'Robert White', 'robertwhite@email.com', '555-2222'),
('P003', 'Linda Black', 'lindablack@email.com', '555-3333'),
('P004', 'Thomas Gray', 'thomasgray@email.com', '555-4444'),
('P005', 'Jessica Blue', 'jessicablue@email.com', '555-5555');

INSERT INTO classes (class_number, class_letter, class_teacher_id, classroom_id) VALUES
(10, 'A', 1, 1),
(12, 'B', 2, 2),
(8, 'C', 3, 3),
(9, 'D', 4, 4),
(11, 'E', 5, 5);

INSERT INTO students (students_code, full_name, gender, date_of_birth, email, phone, class_id, is_active) VALUES
('S001', 'Jason Lee', 'm', '2005-04-12', 'jasonlee@email.com', '555-2223', 1, 1),
('S002', 'Sophia Taylor', 'f', '2006-03-18', 'sophiat@email.com', '555-2224', 2, 1),
('S003', 'William Clark', 'm', '2007-01-09', 'williamc@email.com', '555-2225', 3, 1),
('S004', 'Olivia Harris', 'f', '2006-07-19', 'oliviah@email.com', '555-2226', 4, 1),
('S005', 'Liam Martin', 'm', '2005-06-23', 'liamm@email.com', '555-2227', 5, 0);

INSERT INTO teachers_subjects (teacher_id, subject_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

INSERT INTO classes_subjects (class_id, subject_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

INSERT INTO students_parents (student_id, parent_id) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

SELECT * FROM classes
WHERE class_number = 11 AND class_letter = 'B'