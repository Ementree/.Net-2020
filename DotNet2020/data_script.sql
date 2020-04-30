INSERT INTO "Position" as p
VALUES
       (1,'Фронтенд'),
       (2,'Бэкенд'),
       (3,'Миддленд')
       ON CONFLICT ("Id") DO UPDATE
SET
    "Name"=excluded."Name";
SELECT setval('"Position_Id_seq"',(SELECT Max("Position"."Id") FROM "Position"));


INSERT INTO "Employee" ("Id", "FirstName", "LastName", "MiddleName", "PositionId", "Salary", "Bonus", "Commentary", "PreviousWorkPlaces", "Experience", "Discriminator")
VALUES
       (1, 'Дмитрий', 'Чемкин', 'Николаевич',1, 1, 76547, 'Хорошо верстает', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (2, 'Дмитрий', 'Орлов', 'Павлович',2,2, 56437, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (3, 'Игорь', 'Квасников', 'Константинович',3, 3, 76747, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel')
ON CONFLICT ("Id") DO UPDATE
SET
    "FirstName" = excluded."FirstName",
    "PositionId" = excluded."PositionId",
    "Salary" = excluded."Salary",
    "Bonus" = excluded."Bonus",
    "Commentary" = excluded."Commentary",
    "PreviousWorkPlaces" = excluded."PreviousWorkPlaces",
    "Experience" = excluded."Experience",
    "MiddleName" = excluded."MiddleName",
    "LastName" = excluded."LastName";
SELECT setval('"Employee_Id_seq"', (SELECT Max("Employee"."Id") FROM "Employee"));


INSERT INTO "Competences" as W
VALUES
       (1, 'Базовое программирование', ARRAY['Алгоритмы и структуры данных', 'Знание C# или JavaScript'], ARRAY['Что такое АИСД', 'Что такое var'] ),
       (2, 'Базовый backend', ARRAY['ASPNET MVC', ' ASPNET WEBAPI', 'ASPNET MVC CORE', 'LINQ', 'EF'], ARRAY['Что такое EF', 'Что такое WEBAPI', 'Что такое MVC'] ),
       (3, 'Базовый frontend', ARRAY['Базовые знания HTML, CSS, JS, Jquery', 'Node.js, npm'], ARRAY['Что такое npm', 'Что такое JS'] ),
       (4, 'Продвинутый backend', ARRAY['SOLID', 'GOF', 'REST', 'HTTP', 'CQRS', 'TPL'], ARRAY['Что такое TPL', 'Что такое REST', 'Что такое HTTP'] ),
       (5, 'Продвинутый frontend', ARRAY['ES6 / TypeScript', 'Webpack', 'React + Redux / Angular + rxjs'], ARRAY['Что такое rxjs', 'Что такое webpack', 'Что такое es6'] ),
       (6, 'Управление командной разработки', ARRAY['Опыт управление командой разработки (не менее 3 разработчиков) от 1 года',
           'Знание гибких методологий разработки: Scrum / Kanban'], ARRAY['Что такое scrum', 'Что такое kanban', 'Что такое youtrack'] )
ON CONFLICT ("Id") DO UPDATE
SET "Competence" = excluded."Competence",
    "Content" = excluded."Content",
    "Questions" = excluded."Questions";
SELECT setval('"Competences_Id_seq"', (SELECT Max("Competences"."Id") FROM "Competences"));


INSERT INTO "Grades" as W
VALUES
       (1, 'D1'),
       (2, 'D2(BACKEND)'),
       (3, 'D2(FRONTEND)'),
       (4, 'D3(FULL-STACK)'),
       (5, 'TL(TEAM-LEAD)')
ON CONFLICT ("Id") DO UPDATE
SET "Grade"= excluded."Grade";
SELECT setval('"Grades_Id_seq"', (SELECT Max("Grades"."Id") FROM "Grades"));


INSERT INTO "GradeCompetences" as W
VALUES
       (1, 1),
       (1, 2),
       (1, 3),

       (2, 1),
       (2, 2),
       (2, 3),
       (2, 4),

       (3, 1),
       (3, 2),
       (3, 3),
       (3, 5),

       (4, 1),
       (4, 2),
       (4, 3),
       (4, 4),
       (4, 5),

       (5, 1),
       (5, 2),
       (5, 3),
       (5, 4),
       (5, 5),
       (5, 6)
ON CONFLICT ("GradeId", "CompetenceId") DO NOTHING;

INSERT INTO "SpecificWorkerCompetences" as W
VALUES
       (1, 1),
       (1, 3),

       (2, 1),
       (2, 2),

       (3, 1),
       (3, 2)
ON CONFLICT ("WorkerId", "CompetenceId") DO NOTHING;


