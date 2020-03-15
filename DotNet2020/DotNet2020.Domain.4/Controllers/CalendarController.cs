using DotNet2020.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IRecomendation recomendation;

        public CalendarController(IRecomendation recomendation)
        {
            this.recomendation = recomendation;
        }

        public IActionResult Index()
        {
            // TODO: Если User.Posision привелигированный, то перебрасывать его на метод Admin, иначе return View
            // TODO: Получать рекомендации через сервис recomendation
            // TODO: Получать остаток оплачиваемого отпуска // Как быть создавать расширенного пользователя или отдельную сущность или хранить в vacation?
            // TODO: Получать сообщение об успешном согласованиии или отказе, если есть ?
            return View();
        }

        [HttpGet]
        public IActionResult AddEventGet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEventPost()
        {
            // TODO: Создать новый объект или получить в виде аргумента метода
            // TODO: Нужно будет кастовать и в зависимости от типа доавлять в бд
            // TODO: Провести валидацию
            // TODO: Сохранить в бд
            return Redirect("Index");
        }

        public IActionResult Admin()
        {
            // TODO: Проверить авторизацию
            // TODO: Проверить права на админа
            // TODO: Получить все не апрувнутые отпуски
            // TODO: Получать рекомендации через сервис recomendation
            return View();
        }

        [HttpGet]
        public IActionResult AddHolidayGet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddHolidayPost()
        {
            // TODO: Создать новый объект или получить в виде аргумента метода
            // TODO: Провести валидацию
            // TODO: Сохранить в бд
            return View();
        }

        [HttpGet]
        public IActionResult AddRecommendationGet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRecommendationPost()
        {
            // TODO: Создать новый объект или получить в виде аргумента метода
            // TODO: Провести валидацию
            // TODO: Сохранить в бд
            return View();
        }

        [HttpPost]
        public IActionResult AproveEvent()
        {
            // TODO: Создать новый объект или получить в виде аргумента метода
            // TODO: Провести валидацию, создать сервис эвристики, пока что самой простой
            // TODO: Сохранить в бд
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult DeclineEvent()
        {
            // TODO: Создать новый объект или получить в виде аргумента метода
            // TODO: Провести валидацию, создать сервис эвристики, пока что самой простой
            // TODO: Сохранить в бд
            return Redirect("Index");
        }
    }
}
