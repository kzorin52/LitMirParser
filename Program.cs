using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

// Все права на данную программу принадлежат некоему 
// читаке с ником Temnij, тобишь мне. Связь со мной возможна
// через Telegram - https://t.me/temnij52, или через VK -
// https://vk.com/temnijvk.

/// <summary>
/// Парсер LitMir
/// </summary>
namespace LitMirParser
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Всякие переменные

            var catId = input("ID категории:");
            var pages = input("Количество страниц:").intParse();

            #endregion
            #region Создаём драйвер Chrome

            var options = new ChromeOptions();
            #region Отключаем логгирование
            options.AddArgument("--log-level=3");
            options.AddArgument("--disable-logging");
            #endregion
            var driver = new ChromeDriver(options);

            #endregion

            for (int page = pages; page > 0; page--) // По сути, качаем с конца, но так даже удобнее
            {
                driver.Url = $"https://www.litmir.me/bs/?g={catId}&o=100&p={page}"; // Устанавливаем Url

                var books = driver.FindElements(By.XPath("//img[@class='lazy']/following-sibling::a")); // Находим кнопки "Скачать"

                for (int book = 0; book < books.Count; book++)
                {
                    try { driver.FindElement(By.XPath($"(//label[text()='fb2'])[{book}]")).Click(); } catch { } // Выбираем FB2
                    driver.ExecuteScript(books[book].GetAttribute("onclick")).ToString(); // Скачиваем
                    Console.WriteLine($"\tКнига {book} скачана!"); // Лог в консоль
                }

                Console.WriteLine($"Страница {page} скачана!"); // Лог в консоль
            }
        }

        #region Так удобнее

        /// <summary>
        /// Взял из питона, но так рил удобнее. Метод для ввода текста, 
        /// но с выводом другого текста
        /// </summary>
        /// <param name="text">Текст для вывода</param>
        /// <returns>Введённую строку</returns>
        static string input(string text = "")
        {
            if (text != "")
                Console.WriteLine(text);
            return Console.ReadLine();
        }

        #endregion
    }

    #region Расширения

    /// <summary>
    /// Класс расширений
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Преобразует строку в число (по-моему, так удобнее)
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>Число</returns>
        public static int intParse(this string str) => int.Parse(str);
    }

    #endregion
}