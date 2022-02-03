using System;
using System.IO;
using System.Net;

namespace Task_1
{
    class Program
    {
        // Программа скачивает html страницу по введенному адресу,
        // находит первые 5 изображений по тегам img и скачивает их в папку с проектом, 
        // записывает в лог адреса скачанных файлов.
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите адрес страницы или выберите мой: ");
                Console.WriteLine("1. https://lizardrock2033.github.io/firstWeb/");
                string url = Console.ReadLine();


                // Тут точно работает
                if (url == "1")
                    url = "https://lizardrock2033.github.io/firstWeb/";


                // Создание директории
                string path = @"images\";
                DirectoryInfo dirInfo = new(path);
                dirInfo.Create();


                // Скачивание страницы
                WebClient wc = new();
                wc.DownloadFile(url, "index.html");


                // Преобразование html
                string page = wc.DownloadString(url);
                string[] splittedPage = page.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string[] imgUrl = new string[5];
                string[] imgName = new string[5];
                int j = 0;


                // Поиск и составление ссылок и имен изображений
                for (int i = 0; i < splittedPage.Length; i++)
                {
                    if (splittedPage[i].Contains("<img") && j < 5)
                    {
                        for (int k = 1; k < 3; k++)
                        {
                            if (splittedPage[i + k].StartsWith("src="))
                            {
                                imgUrl[j] = url + splittedPage[i + k].Substring(5, splittedPage[i + k].Length - 6).TrimStart('/').TrimEnd('"', '>', '/');
                                imgName[j] = Path.GetFileName(imgUrl[j]).Trim('"');
                                j++;
                            }
                        }
                    }
                }

                // Вывод адресов изображений
                Console.WriteLine("\nПолученные адреса изображений: \n");
                foreach (string urls in imgUrl)
                    Console.WriteLine(urls);


                // Скачивание и запись в лог
                using (StreamWriter sw = new("log.txt", false, System.Text.Encoding.Default))
                {
                    for (int i = 0; i < 5;)
                    {
                        foreach (string urls in imgUrl)
                        {
                            try
                            {
                                wc.DownloadFile(urls, path + imgName[i]);
                                sw.WriteLine(urls);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("\nОшибка при скачивании изображения!");
                            }
                            i++;
                        }
                    }
                }

                Console.WriteLine("\nЗавершено!");
                Console.WriteLine("\nФайлы сохранены в корневой папке.");
                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("\nНеизвестная ошибка!");
                Console.ReadLine();
            }
        }
    }
}
