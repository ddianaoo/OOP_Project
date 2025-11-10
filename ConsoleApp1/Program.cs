using ConsoleApp1;
using static System.Reflection.Metadata.BlobBuilder;
Console.OutputEncoding = System.Text.Encoding.UTF8;


List<Product> products = new List<Product>();
bool exit = false;

while (!exit)
    {
        Console.Clear();
        Console.WriteLine("=== МЕНЮ УПРАВЛЕНИЯ ТОВАРАМИ ===");
        Console.WriteLine("1. Додати товар");
        Console.WriteLine("2. Показати всі товари");
        Console.WriteLine("3. Редагувати товар");
        Console.WriteLine("4. Видалити товар");
        Console.WriteLine("5. Пошук товару за назвою");
        Console.WriteLine("6. Зчитати колекцію об'єктів з файлу");
        Console.WriteLine("0. Вихід");
        Console.Write("Оберіть дію: ");

        switch (Console.ReadLine())
        {
            case "1":
                AddProduct();
                break;
            case "2":
                ShowProducts();
                break;
            case "3":
                EditProduct();
                break;
            case "4":
                DeleteProduct();
                break;
            case "5":
                SearchProduct();
                break;
            case "6":
                Console.Write("Введіть назву файлу (без розширення): ");
                string loadFile = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(loadFile)) loadFile = "products";

                List<Product> loaded;
                loaded = LoadProductsFromCsv($"{loadFile}.csv");

                foreach (var p in loaded)
                {
                    products.Add(p);
                }
                break;

        case "0":
                exit = true;
                break;
            default:
                Console.WriteLine("Невірний вибір! Натисніть Enter...");
                Console.ReadLine();
                break;
        }
}


void AddProduct()
{
    Console.Clear();
    Console.WriteLine("=== Додавання нового товару ===");

    string name;
    while (true)
    {
        Console.Write("Назва: ");
        name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Назва не може бути порожньою. Спробуйте ще раз.");
            continue;
        }

        if (products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Товар з такою назвою вже існує. Введіть іншу назву.");
            continue;
        }

        break;
    }

    int quantity;
    while (true)
    {
        Console.Write("Кількість: ");
        if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
            break;
        Console.WriteLine("Введіть коректне число.");
    }

    decimal price;
    while (true)
    {
        Console.Write("Ціна: ");
        if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
            break;
        Console.WriteLine("Введіть коректну ціну.");
    }

    ProductCategory category;
    while (true)
    {
        try
        {
            Console.WriteLine("Категорія (введіть число):");
            foreach (var cat in Enum.GetValues(typeof(ProductCategory)))
                Console.WriteLine($"{(int)cat} - {cat}");

            string input = Console.ReadLine();
            if (!int.TryParse(input, out int categoryNumber) || !Enum.IsDefined(typeof(ProductCategory), categoryNumber))
            {
                Console.WriteLine("Невірний вибір категорії. Спробуйте ще раз.");
                continue;
            }

            category = (ProductCategory)categoryNumber;
            break;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    Console.Write("Опис: ");
    string desc = Console.ReadLine();

    var product = new Product(name, desc, quantity, price, category);
    products.Add(product);

    Console.WriteLine("\nТовар успішно додано!");
    Console.ReadLine();
}

void ShowProducts()
{
    Console.Clear();
    Console.WriteLine("=== Список товарів ===");

    ShowShortList();

    Console.WriteLine("Натисніть Enter, щоб повернутись до меню...");
    Console.ReadLine();
}

void EditProduct()
{
    Console.Clear();
    Console.WriteLine("=== Редагування товару ===");
    ShowShortList();

    Console.Write("Введіть ID товару для редагування: ");
    string idInput = Console.ReadLine();
    if (!Guid.TryParse(idInput, out Guid id))
    {
        Console.WriteLine("Невірний формат ID!");
        Console.ReadLine();
        return;
    }

    var product = products.Find(p => p.Id == id);
    if (product == null)
    {
        Console.WriteLine("Товар не знайдено!");
        Console.ReadLine();
        return;
    }

    Console.Write("Нова назва (або Enter, щоб залишити): ");
    string newName = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newName))
        product.Name = newName;

    Console.Write("Новий опис (або Enter, щоб залишити): ");
    string newDesc = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newDesc))
        product.Description = newDesc;

    Console.Write("Нова кількість (або Enter): ");
    string newQty = Console.ReadLine();
    if (int.TryParse(newQty, out int qty))
        product.Quantity = qty;

    Console.Write("Нова ціна (або Enter): ");
    string newPrice = Console.ReadLine();
    if (decimal.TryParse(newPrice, out decimal price))
        product.Price = price;

    Console.WriteLine("\nДані оновлено!");
    Console.ReadLine();
}

void DeleteProduct()
{
    Console.Clear();
    Console.WriteLine("=== Видалення товару ===");
    ShowShortList();

    Console.Write("Введіть ID товару для видалення: ");
    string idInput = Console.ReadLine();
    if (!Guid.TryParse(idInput, out Guid id))
    {
        Console.WriteLine("Невірний формат ID!");
        Console.ReadLine();
        return;
    }

    var product = products.Find(p => p.Id == id);
    if (product == null)
    {
        Console.WriteLine("Товар не знайдено!");
    }
    else
    {
        products.Remove(product);
        Console.WriteLine("Товар видалено!");
    }

    Console.ReadLine();
}

void SearchProduct()
{
    Console.Clear();
    Console.Write("Введіть частину назви для пошуку: ");
    string term = Console.ReadLine()?.ToLower();

    var results = products.FindAll(p => p.Name.ToLower().Contains(term));

    Console.WriteLine("\n=== Результати пошуку ===");
    if (results.Count == 0)
        Console.WriteLine("Нічого не знайдено.");
    else
        results.ForEach(p => Console.WriteLine(p + "\n" + new string('-', 40)));

    Console.ReadLine();
}

void ShowShortList()
{
    if (products.Count == 0)
    {
        Console.WriteLine("Немає товарів.");
        return;
    }

    Console.WriteLine("ID                                   | Назва              | Ціна (грн)   | К-сть | Категорія        | Опис            | Створено            | Оновлено");
    Console.WriteLine(new string('-', 160));

    foreach (var p in products)
    {
        Console.WriteLine($"{p.ForTable()}");
    }

    Console.WriteLine(new string('-', 160));
}


List<Product> LoadProductsFromCsv(string filePath)
{
    var loadedProducts = new List<Product>();
    if (!File.Exists(filePath))
    {
        Console.WriteLine("Файл не знайдено.");
        return loadedProducts;
    }

    var lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
    if (lines.Length <= 1)
    {
        Console.WriteLine("Файл порожній або не містить даних.");
        return loadedProducts;
    }
    for (int i = 1; i < lines.Length; i++)
    {
        string line = lines[i].Trim();

        if (Product.TryParse(line, out Product? pr))
        {
            loadedProducts.Add(pr);
        }
        else
        {
            Console.WriteLine($"Рядок {i + 1} пропущено — некоректні дані.");
        }
    }

    Console.WriteLine($"Успішно десеріалізовано {loadedProducts.Count} книг(и) з файлу.");
    return loadedProducts;
}
