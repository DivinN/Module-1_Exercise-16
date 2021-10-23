using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Module_1_Exercise_16_2
{
    class Program_2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Продолжаем играть в кассу. Теперь будем получать информацию из базы данных о товарах.");

            
            string path = "Example/Products.json";

            string line = "";

            if (!File.Exists(path))
            {
                Console.WriteLine("Файл не найден!");
            }
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    line = sr.ReadToEnd();
                }
            }
            // Убираем лишние скобки
            line = line.Remove(0, 3);
            line = line.Remove(line.Length - 5, 5);


            // Режем строку на массив строк по символу перехода к следующему продукту
            string[] separatingStrings = { "},"};
            string[] stringArray = line.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            // Возвращаем удаленную скобуку у всех продуктов, кроме последнего
            for (int i = 0; i < stringArray.Length - 1; i++)
            {
                stringArray[i] = stringArray[i] + "}";
            }


            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            Product[] prodBase = new Product[stringArray.Length];
            int num = 0;
            // Десериализуем данные
            foreach (string item in stringArray)
            {
                Product prod = JsonSerializer.Deserialize<Product>(item, options);
                prodBase[num] = prod;
                num++;
            }

            double productPriceMax = 0;
            string productPriceMaxName = "";
            // Организуем вывод в консоль
            foreach (Product item in prodBase)
            {
                Console.WriteLine("В базе имеется товар с кодом: {0}, называнием: {1} и стоимостью {2}.", item.ProductCode, item.ProductName, item.ProductPrice);
                if (item.ProductPrice > productPriceMax)
                {
                    productPriceMax = item.ProductPrice;
                    productPriceMaxName = item.ProductName;
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Самый дорогой товар в базе - это {0}, он стоит {1}.", productPriceMaxName, productPriceMax);
            Console.WriteLine("Спасибо за внимание!");
            Console.ReadKey();
        }


        class Product
        {
            public int ProductCode { get; set; }
            public string ProductName { get; set; }
            public double ProductPrice { get; set; }
        }

    }
}
