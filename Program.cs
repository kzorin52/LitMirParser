using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

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

            var catId = input("ID категории:"); //sg276
            var pages = input("Количество страниц:").intParse();
            List<int> been = new List<int>();
            Random rnd = new Random();

            #endregion
            #region Создаём драйвер Chrome

            var options = new ChromeOptions
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome Beta\Application\chrome.exe"
            };

            #region Отключаем логгирование

            options.AddArgument("--log-level=3");
            options.AddArgument("--disable-logging");

            #endregion
            var driver = new ChromeDriver(options);

            #endregion

            for (int page = pages; page > 0; page--) // По сути, качаем с конца, но так даже удобнее
            {
                int rand = 0;
            generate:
                rand = rnd.Next(1, 63);
                if (been.Contains(rand))
                    goto generate;
                been.Add(rand);

                driver.Url = $"https://www.litmir.me/bs/?g={catId}&o=100&p={rand}"; // Устанавливаем Url

                var books = driver.FindElements(By.XPath("//img[@class='lazy']/following-sibling::a")); // Находим кнопки "Скачать"

                var rand2 = rnd.Next(0, 100);
            again:
                try
                {
                    try { driver.FindElement(By.XPath($"(//label[text()='fb2'])[{rand2}]")).Click(); } catch { } // Выбираем FB2
                    driver.ExecuteScript(books[rand2].GetAttribute("onclick")).ToString(); // Скачиваем
                    Console.WriteLine($"\tКнига {rand2} на странице {rand} скачана!"); // Лог в консоль
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\tУпс...");
                    Console.ForegroundColor = ConsoleColor.White;
                    rand2 = rnd.Next(0, 100);
                    goto again;
                }
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