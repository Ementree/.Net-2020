INSERT INTO "Workers" as W
VALUES
       (0, 'Дмитрий', 'Фронтендер', 76547, 3000, 'Хорошо верстает', 'Нет', 'Нет', 'Николаевич', 'Чемкин'),
       (1, 'Дмитрий', 'Бэкендер', 56437, 3000, 'Не верстает', 'Нет', 'Нет', 'Павлович', 'Орлов'),
       (2, 'Игорь', 'Бэкендер', 76747, 3000, 'Тащит на личе', 'Нет', 'Нет', 'Константинович', 'Квасников')
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
    "Surname" = excluded."Surname"
