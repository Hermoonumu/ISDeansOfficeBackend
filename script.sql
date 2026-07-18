CREATE TABLE "Departments" (
    "Id" uuid NOT NULL,
    "DepartmentName" character varying(255) NOT NULL,
    CONSTRAINT "PK_Departments" PRIMARY KEY ("Id")
);


CREATE TABLE "Subjects" (
    "Id" uuid NOT NULL,
    "SubjectName" character varying(255) NOT NULL,
    CONSTRAINT "PK_Subjects" PRIMARY KEY ("Id")
);


CREATE TABLE "Programs" (
    "Id" uuid NOT NULL,
    "ProgramName" character varying(255) NOT NULL,
    "ProgramCode" character varying(4) NOT NULL,
    "ProgramStatus" integer NOT NULL,
    "DepartmentId" uuid,
    CONSTRAINT "PK_Programs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Programs_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES "Departments" ("Id") ON DELETE SET NULL
);


CREATE TABLE "Curricula" (
    "Id" uuid NOT NULL,
    "EdProgramId" uuid,
    "SubjectId" uuid,
    "Semester" integer,
    "LectureHours" integer NOT NULL,
    "PracticeHours" integer NOT NULL,
    "LabHours" integer NOT NULL,
    "CourseWorkHours" integer NOT NULL,
    "AssessmentType" integer,
    "IsActive" boolean NOT NULL,
    CONSTRAINT "PK_Curricula" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Curricula_Programs_EdProgramId" FOREIGN KEY ("EdProgramId") REFERENCES "Programs" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Curricula_Subjects_SubjectId" FOREIGN KEY ("SubjectId") REFERENCES "Subjects" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "FirstName" character varying(255) NOT NULL,
    "LastName" character varying(255) NOT NULL,
    "Username" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "BirthDate" timestamp with time zone NOT NULL,
    "Position" integer NOT NULL,
    "ProgramId" uuid,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Users_Programs_ProgramId" FOREIGN KEY ("ProgramId") REFERENCES "Programs" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "EducCurr" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CurriculumId" uuid NOT NULL,
    CONSTRAINT "PK_EducCurr" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EducCurr_Curricula_CurriculumId" FOREIGN KEY ("CurriculumId") REFERENCES "Curricula" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EducCurr_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Grades" (
    "Id" uuid NOT NULL,
    "StudentId" uuid NOT NULL,
    "CurriculumId" uuid NOT NULL,
    "Status" integer NOT NULL,
    "ConfirmFailure" boolean NOT NULL,
    "Grade" integer,
    "GradingDate" timestamp with time zone,
    "GradedById" uuid,
    CONSTRAINT "PK_Grades" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Grades_Curricula_CurriculumId" FOREIGN KEY ("CurriculumId") REFERENCES "Curricula" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Grades_Users_GradedById" FOREIGN KEY ("GradedById") REFERENCES "Users" ("Id"),
    CONSTRAINT "FK_Grades_Users_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_Curricula_EdProgramId" ON "Curricula" ("EdProgramId");


CREATE INDEX "IX_Curricula_SubjectId" ON "Curricula" ("SubjectId");


CREATE UNIQUE INDEX "IX_Departments_DepartmentName" ON "Departments" ("DepartmentName");


CREATE INDEX "IX_EducCurr_CurriculumId" ON "EducCurr" ("CurriculumId");


CREATE UNIQUE INDEX "IX_EducCurr_UserId_CurriculumId" ON "EducCurr" ("UserId", "CurriculumId");


CREATE INDEX "IX_Grades_CurriculumId" ON "Grades" ("CurriculumId");


CREATE INDEX "IX_Grades_GradedById" ON "Grades" ("GradedById");


CREATE INDEX "IX_Grades_StudentId" ON "Grades" ("StudentId");


CREATE INDEX "IX_Programs_DepartmentId" ON "Programs" ("DepartmentId");


CREATE UNIQUE INDEX "IX_Subjects_SubjectName" ON "Subjects" ("SubjectName");


CREATE INDEX "IX_Users_ProgramId" ON "Users" ("ProgramId");


CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");



