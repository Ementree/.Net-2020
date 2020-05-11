CREATE FUNCTION updateResource() RETURNS trigger AS $updateResource$
    BEGIN
        if (tg_op = 'INSERT') then
            INSERT INTO "Resources" ("Id", "ResourceGroupTypeId", "EmployeeId")
            VALUES (new."Id", 3, new."Id")
            ON CONFLICT ("Id") DO UPDATE
            SET "ResourceGroupTypeId" = excluded."ResourceGroupTypeId",
                "EmployeeId" = excluded."EmployeeId";
        end if;
        if (tg_op = 'DELETE') then
            DELETE FROM "Resources"
            WHERE ("Id" = old."Id");
        end if;
        Return new;
    END
$updateResource$ LANGUAGE plpgsql;

CREATE TRIGGER resourse_update
    AFTER UPDATE OR INSERT OR DELETE ON "Employee"
    FOR EACH ROW
    EXECUTE PROCEDURE updateResource();

INSERT INTO "ResourceGroupsTypes" ("Id", "Type", "Group")
VALUES
    (1, 'Сотрудник', 'Специалист'),
    (2, 'Подрядчик', 'Подрядчик'),
    (3, 'Сотрудник', 'Студент'),
    (4, 'Сотрудник', 'Подрядчик'),
    (5, 'Подрядчик', 'Студент')
ON CONFLICT ("Id") DO UPDATE
SET "Type" = excluded."Type",
    "Group" = excluded."Group";
SELECT setval('"ResourceGroupsTypes_Id_seq"', (SELECT Max("Id") FROM "ResourceGroupsTypes"));

INSERT INTO "GradeToGrade" as p
VALUES
       (1,2),
       (1,3),
       (2,4),
       (3,4),
       (4,5)
       ON CONFLICT ("GradeId", "NextGradeId") DO NOTHING;


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
       (3, 'Игорь', 'Квасников', 'Константинович',3, 3, 76747, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (4, 'Мухаммад', 'Мукиев', 'Мухаммадович', 3, 500000, 0, 'начинающий аналитик', 'coderlar', '1 коммерческий проект', 'SpecificWorkerModel'),
       (5, 'Яков', 'Григорьев', 'Александрович',2, 2, 45673, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (6, 'Анна', 'Федорова', 'Эдуардовна', 2, 2, 76747, 'Делает бд', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (7, 'Тагмир', 'Гилязов', 'Радикович', 1, 2, 76747, 'Знает ангуляр', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (8, 'Артур', 'Саттаров', 'Рустамович', 2, 2, 76747, 'Главный специалист по рефакторингу', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (9, 'Егор', 'Малышкин', 'Андреевич', 3, 2, 76747, 'Делает отсутсвия', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (10, 'Айдар', 'Габдрахманов', 'Ренатович', 1, 2, 76747, 'Наверно верстает', 'Нет', 'Нет', 'SpecificWorkerModel'),
       (11, 'Равиль', 'Насыбуллин', 'Эдуардович', 3, 2, 76747, 'Наверно не верстает', 'Нет', 'Нет', 'SpecificWorkerModel')

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

INSERT INTO "ProjectStatuses" ("Id", "Status")
VALUES
       (1, 'Завершенные проекты'),
       (2, 'Текущие обязательства'),
       (3, 'Внутренние проекты')
ON CONFLICT ("Id") DO UPDATE
SET "Status"= excluded."Status";
SELECT setval('"ProjectStatuses_Id_seq"', (SELECT Max("Id") FROM "ProjectStatuses"));

INSERT INTO "EmployeeCalendar" ("Id", "EmployeeId", "TotalDayOfVacation", "IsLastVacationApproved")
VALUES
       (1, 1, 0, FALSE),
       (2, 2, 0, FALSE),
       (3, 3, 0, FALSE),
       (4, 4, 0, FALSE),
       (5, 5, 0, FALSE),
       (6, 6, 0, FALSE),
       (7, 7, 0, FALSE),
       (8, 8, 0, FALSE),
       (9, 9, 0, FALSE),
       (10, 10, 0, FALSE),
       (11, 11, 0, FALSE);
