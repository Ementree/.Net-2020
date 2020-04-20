INSERT INTO "Workers" as W
VALUES
       (0, 'Дмитрий', 'Фронтендер', 76547, 3000, 'Хорошо верстает', 'Нет', 'Нет', 'Николаевич', 'Чемкин'),
       (1, 'Дмитрий', 'Бэкендер', 56437, 3000, 'Не верстает', 'Нет', 'Нет', 'Павлович', 'Орлов'),
       (2, 'Игорь', 'Бэкендер', 76747, 3000, 'Не верстает', 'Нет', 'Нет', 'Константинович', 'Квасников')
ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = excluded."Name",
    "Position" = excluded."Position",
    "Salary" = excluded."Salary",
    "Bonus" = excluded."Bonus",
    "Commentary" = excluded."Commentary",
    "PreviousWorkPlaces" = excluded."PreviousWorkPlaces",
    "Experience" = excluded."Experience",
    "Patronymic" = excluded."Patronymic",
    "Surname" = excluded."Surname";

INSERT INTO "Competences" as W
VALUES
       (0, 'Базовое программирование', ARRAY['Алгоритмы и структуры данных', 'Знание C# или JavaScript'], ARRAY['Что такое АИСД', 'Что такое var'] ),
       (1, 'Базовый backend', ARRAY['ASPNET MVC', ' ASPNET WEBAPI', 'ASPNET MVC CORE', 'LINQ', 'EF'], ARRAY['Что такое EF', 'Что такое WEBAPI', 'Что такое MVC'] ),
       (2, 'Базовый frontend', ARRAY['Базовые знания HTML, CSS, JS, Jquery', 'Node.js, npm'], ARRAY['Что такое npm', 'Что такое JS'] ),
       (3, 'Продвинутый backend', ARRAY['SOLID', 'GOF', 'REST', 'HTTP', 'CQRS', 'TPL'], ARRAY['Что такое TPL', 'Что такое REST', 'Что такое HTTP'] ),
       (4, 'Продвинутый frontend', ARRAY['ES6 / TypeScript', 'Webpack', 'React + Redux / Angular + rxjs'], ARRAY['Что такое rxjs', 'Что такое webpack', 'Что такое es6'] ),
       (5, 'Управление командной разработки', ARRAY['Опыт управление командой разработки (не менее 3 разработчиков) от 1 года',
           'Знание гибких методологий разработки: Scrum / Kanban'], ARRAY['Что такое scrum', 'Что такое kanban', 'Что такое youtrack'] )
ON CONFLICT ("Id") DO UPDATE
SET "Competence" = excluded."Competence",
    "Content" = excluded."Content",
    "Questions" = excluded."Questions";

INSERT INTO "Grades" as W
VALUES
       (0, 'D1'),
       (1, 'D2(BACKEND)'),
       (2, 'D2(FRONTEND)'),
       (3, 'D3(FULL-STACK)'),
       (4, 'TL(TEAM-LEAD')
ON CONFLICT ("Id") DO UPDATE
SET "Grade"=excluded."Grade";

INSERT INTO "GradeCompetences" as W
VALUES
       (0, 0),
       (0, 1),
       (0, 2),

       (1, 0),
       (1, 1),
       (1, 2),
       (1, 3),

       (2, 0),
       (2, 1),
       (2, 2),
       (2, 4),

       (3, 0),
       (3, 1),
       (3, 2),
       (3, 3),
       (3, 4),

       (4, 0),
       (4, 1),
       (4, 2),
       (4, 3),
       (4, 4),
       (4, 5)
ON CONFLICT ("GradeId", "CompetenceId") DO NOTHING;

INSERT INTO "SpecificWorkerCompetences" as W
VALUES
       (0, 0),
       (0, 2),

       (1, 0),
       (1, 1),

       (2, 0),
       (2, 1)
ON CONFLICT ("WorkerId", "CompetenceId") DO NOTHING;
