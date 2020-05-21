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
    (5, 'Подрядчик', 'Студент'),
    (6, 'Подрядчик', 'Специалист')
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


INSERT INTO "Employee" ("Id", "FirstName", "LastName", "MiddleName", "PositionId", "Salary", "Bonus", "Commentary", "PreviousWorkPlaces", "Experience", "Discriminator", "Email")
VALUES
       (1, 'Дмитрий', 'Чемкин', 'Николаевич',1, 1, 76547, 'Хорошо верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'DCh@gmail.com'),
       (2, 'Дмитрий', 'Орлов', 'Павлович',2,2, 56437, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'DO@gmail.com'),
       (3, 'Игорь', 'Квасников', 'Константинович',3, 3, 76747, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'IK@gmail.com'),
       (4, 'Мухаммад', 'Мукиев', 'Мухаммадович', 3, 500000, 0, 'начинающий аналитик', 'coderlar', '1 коммерческий проект', 'SpecificWorkerModel', 'MM@gmail.com'),
       (5, 'Яков', 'Григорьев', 'Александрович',2, 2, 45673, 'Не верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'YG@gmail.com'),
       (6, 'Анна', 'Федорова', 'Эдуардовна', 2, 2, 76747, 'Делает бд', 'Нет', 'Нет', 'SpecificWorkerModel', 'AF@gmail.com'),
       (7, 'Тагмир', 'Гилязов', 'Радикович', 1, 2, 76747, 'Знает ангуляр', 'Нет', 'Нет', 'SpecificWorkerModel', 'TG@gmail.com'),
       (8, 'Артур', 'Саттаров', 'Рустамович', 2, 2, 76747, 'Главный специалист по рефакторингу', 'Нет', 'Нет', 'SpecificWorkerModel', 'AS@gmail.com'),
       (9, 'Егор', 'Малышкин', 'Андреевич', 3, 2, 76747, 'Делает отсутсвия', 'Нет', 'Нет', 'SpecificWorkerModel', 'EM@gmail.com'),
       (10, 'Айдар', 'Габдрахманов', 'Ренатович', 1, 2, 76747, 'Наверно верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'AG@gmail.com'),
       (11, 'Равиль', 'Насыбуллин', 'Эдуардович', 3, 2, 76747, 'Наверно не верстает', 'Нет', 'Нет', 'SpecificWorkerModel', 'RN@gmail.com')

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
       (1, 'Базовое программирование', ARRAY['Алгоритмы и структуры данных', 'Знание C# или JavaScript']),
       (2, 'Базовый backend', ARRAY['ASPNET MVC', ' ASPNET WEBAPI', 'ASPNET MVC CORE', 'LINQ', 'EF'] ),
       (3, 'Базовый frontend', ARRAY['Базовые знания HTML, CSS, JS, Jquery', 'Node.js, npm'] ),
       (4, 'Продвинутый backend', ARRAY['SOLID', 'GOF', 'REST', 'HTTP', 'CQRS', 'TPL' ] ),
       (5, 'Продвинутый frontend', ARRAY['ES6 / TypeScript', 'Webpack', 'React + Redux / Angular + rxjs'] ),
       (6, 'Управление командной разработки', ARRAY['Опыт управление командой разработки (не менее 3 разработчиков) от 1 года',
           'Знание гибких методологий разработки: Scrum / Kanban']),
        (7, '1 семестр', ARRAY['1 семестр'] )
ON CONFLICT ("Id") DO UPDATE
SET "Competence" = excluded."Competence",
    "Content" = excluded."Content";
SELECT setval('"Competences_Id_seq"', (SELECT Max("Competences"."Id") FROM "Competences"));


INSERT INTO "Grades" as W
VALUES
       (1, 'D1'),
       (2, 'D2 (BACKEND)'),
       (3, 'D2 (FRONTEND)'),
       (4, 'D3 (FULL-STACK)'),
       (5, 'TL (TEAM-LEAD)'),
       (6, '1 семестр')
ON CONFLICT ("Id") DO UPDATE
SET "Grade"= excluded."Grade";
SELECT setval('"Grades_Id_seq"', (SELECT Max("Grades"."Id") FROM "Grades"));

INSERT INTO "QuestionComplexity" as q
VALUES
       (1, 'легкий'),
       (2, 'средний'),
       (3, 'тяжелый')
       ON CONFLICT ("Id") DO UPDATE
SET "Value"=excluded."Value";

INSERT INTO "Questions" ("Id", "Question", "ComplexityId")
VALUES
       (1, 'Что такое базовое программирование', 1),
       (2, 'что такое var', 2),
       (3, 'как объявить переменную типа string', 3),
       (4, 'что такое алгоритм?', 1),
       (5, 'что такое asp net', 1),
       (6, 'Что такое web api', 2),
       (7, 'Что такое rest api', 3),
       (8, 'что такое сервер', 2),
       (9, 'Что такое html', 1),
       (10, 'Что такое css', 2),
       (11, 'Что такое typescript', 3),
       (12, 'Что такое SOLID', 1),
       (13, 'Что такое async await', 2),
       (14, 'Что такое expressionTrees?', 3),
       (15, 'Как управлять проектом?', 1),
       (16, 'Что такое youtrack', 2),
       (17, 'Что такое покер планирование?', 3),
       (18, 'Что такое middleware', 3),
       (19, 'Что такое покер планирование?', 1),
       (20, 'Разница между IEnumerable и IQueryable', 2),
       (21, 'Что такое монада', 3),
       (22, 'что выдаст where при вызове от IEnumerable и IQueryable', 3),
       (23, 'stand up', 1),
       (24, 'Как работает async/await? Во что компилируется тело метода после вызова await? Если await только один? Если в теле метода более одного await?', 3),
       (25, 'String - это Value Type или Reference Type? Что необычного в этом типе?', 1),
       (26, 'Какого размера может достигать ошибка оценки разных этапах готовности требований?', 1),
       (27, 'Какой этап оценки продукта подходит для взятия обязательств?', 1),
       (28, 'На проекте работает 10 человек. Проект опаздывает на 4 недели. В течение какого времени удастся сдать продукт, если добавить в команду разработки еще 10 человек?', 1),
       (29, 'В чем отличие Value Type от Reference Type? В каком случае предпочтительнее использовать Value Type, а в каком - Reference Type?', 2),
       (30, 'Какие методы улучшения качества оценки вы знаете?', 2),
       (31, 'Какие данные необходимы для того, чтобы узнать календарную данную окончания проекта? Какими инструментами можно воспользоваться?', 2),
       (32, 'Перечислите 4 ценности и 2-3 принципа Agile Manifesto. Как вы их понимаете?', 1),
       (33, 'Опишите Scrum и Kanban. В чем основное отличие?', 2),
       (34, 'Перечислите основные отличия функциональных языков от императивных', 3),
       (35, 'Опишите паттерн MVC. За что отвечает каждая из букв в паттерне?', 1),
       (36, 'Перечислите коды ответов протокола HTTP. Что каждый из них означает?', 2),
       (37, 'К каким типам можно применять ключевое слово await?', 1),
       (38, 'Опишите тип Expression. Как можно объявить переменную типа Expression?', 2)
ON CONFLICT ("Id") DO UPDATE
SET "Question"=excluded."Question",
    "ComplexityId"=excluded."ComplexityId";
SELECT setval('"Questions_Id_seq"', (SELECT Max("Questions"."Id") FROM "Questions"));

INSERT INTO "CompetenceQuestions" ("CompetenceId", "QuestionId")
VALUES
       (1,1),
       (1,2),
       (1,3),
       (1,4),
       (2,5),
       (2,6),
       (2,7),
       (2,8),
       (3,9),
       (3,10),
       (3,11),
       (4,12),
       (4,13),
       (4,14),
       (6,15),
       (6,16),
       (6,17),
       (7,18),
       (7,19),
       (7,20),
       (7,21),
       (7,22),
       (7,23),
       (7,24),
       (7,25),
       (7,26),
       (7,27),
       (7,28),
       (7,29),
       (7,30),
       (7,31),
       (7,32),
       (7,33),
       (7,34),
       (7,35),
       (7,36),
       (7,37),
       (7,38)
ON CONFLICT DO NOTHING;


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
