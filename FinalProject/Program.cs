using Entities;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        using (ZooDbContext db = new())
        {
            db.Database.EnsureCreated();

        Zoo myZoo = new Zoo();

        //load data from the db
        var enclosuresFromDb = db.Enclosures
            .Include(e => e.Animals.OfType<Penguin>())  //load penguin
            .Include(e => e.Animals.OfType<Dragon>())   //load dragon
            .ToList();

        foreach (var enc in enclosuresFromDb)
        {
            myZoo.AddEnclosure(enc);
        }

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n=== 🐧🔥 DRAGON & PENGUIN ZOO 🐉❄ ===");
                Console.WriteLine("1. Add a new enclosure");
                Console.WriteLine("2. Add a new animal");
                Console.WriteLine("3. Move an animal");
                Console.WriteLine("4. Wait to advance the time (1 step)");
                Console.WriteLine("5. Save the zoo");
                Console.WriteLine("6. Delete the zoo");
                Console.WriteLine("7. Quit game, bye animals...");

                switch (GetKey())
                {
                    case CheckKey._1:
                        Console.Write("Name your new enclosure, choose wisely: ");
                        string encName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(encName))
                        {
                            myZoo.AddEnclosure(new Enclosure(encName));
                            myZoo.AddRange(db);
                        }
                        else
                        {
                            Console.WriteLine("Invalid enclosure name.");
                        }
                        break;

                    case CheckKey._2:
                        AddAnimalMenu(myZoo, db);
                        break;

                    case CheckKey._3:
                        MoveAnimalMenu(myZoo);
                        break;

                    case CheckKey._4:
                        myZoo.AdvanceTime();
                        break;

                    case CheckKey._5:
                        myZoo.AddRange(db);
                        myZoo.SaveZoo();
                        break;

                    case CheckKey._6:
                        db.Database.EnsureDeleted();
                        myZoo.DeleteZoo();
                        break;

                    case CheckKey._7:
                        isRunning = false;
                        Console.WriteLine("Exiting fabulous zoo simulator. Goodbye! 🐧🔥");
                        break;

                    default:
                        Console.WriteLine("Oops, can't do that... Try again! 😅");
                        break;
                }
            }
        }
    }

    static ConsoleKey GetKey() => Console.ReadKey(true).Key;

    static void AddAnimalMenu(Zoo myZoo, ZooDbContext db)
    {
        Console.WriteLine("Add a dragon 🐉 or a penguin 🐧?");
        string choice = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Enter name: ");
        string name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name. Animal not created.");
            return;
        }

        Animal animal = choice switch
        {
            "dragon" => new Dragon { Name = name, CurrentMood = "Calm" },
            "penguin" => new Penguin { Name = name, CurrentMood = "Calm" },
            _ => null
        };

        if (animal == null)
        {
            Console.WriteLine("Invalid animal type. Animal not created.");
            return;
        }

        Console.WriteLine("Select an enclosure:");
        var enclosures = myZoo.GetEnclosures();
        if (enclosures.Count == 0)
        {
            Console.WriteLine("No enclosures exist. You have to build one first or else the animals will roam free!");
            return;
        }

        for (int i = 0; i < enclosures.Count; i++)
            Console.WriteLine($"{i + 1}. {enclosures[i].Name}");

        int selected = int.TryParse(Console.ReadLine(), out int val) && val > 0 && val <= enclosures.Count
            ? val - 1
            : -1;

        if (selected == -1)
        {
            Console.WriteLine("That's not an option!!!");
            return;
        }

        enclosures[selected].AddAnimal(animal);
        myZoo.AddRange(db); // Save to database
    }

    static void MoveAnimalMenu(Zoo myZoo)
    {
        var enclosures = myZoo.GetEnclosures();
        if (enclosures.Count < 2)
        {
            Console.WriteLine("You need at least two enclosures to move an animal. duh");
            return;
        }

        Console.WriteLine("Select the enclosure to move FROM:");
        for (int i = 0; i < enclosures.Count; i++)
            Console.WriteLine($"{i + 1}. {enclosures[i].Name}");

        int fromIndex = int.TryParse(Console.ReadLine(), out int valFrom) && valFrom > 0 && valFrom <= enclosures.Count
            ? valFrom - 1
            : -1;
        if (fromIndex == -1)
        {
            Console.WriteLine("Can't choose that!!!");
            return;
        }

        var fromEnclosure = enclosures[fromIndex];
        var animals = fromEnclosure.GetAnimals();
        if (animals.Count == 0)
        {
            Console.WriteLine("There are no animals in that enclosure.");
            return;
        }

        Console.WriteLine("Select which animal you want to move:");
        for (int i = 0; i < animals.Count; i++)
            Console.WriteLine($"{i + 1}. {animals[i].Name} ({animals[i].GetType().Name})");

        int animalIndex = int.TryParse(Console.ReadLine(), out int valAnimal) && valAnimal > 0 && valAnimal <= animals.Count
            ? valAnimal - 1
            : -1;
        if (animalIndex == -1)
        {
            Console.WriteLine("Can't choose that.");
            return;
        }

        var selectedAnimal = animals[animalIndex];

        Console.WriteLine("Select the enclosure to move TO:");
        var toOptions = new List<Enclosure>();
        for (int i = 0; i < enclosures.Count; i++)
        {
            if (i != fromIndex)
            {
                Console.WriteLine($"{toOptions.Count + 1}. {enclosures[i].Name}");
                toOptions.Add(enclosures[i]);
            }
        }

        int toIndex = int.TryParse(Console.ReadLine(), out int valTo) && valTo > 0 && valTo <= toOptions.Count
            ? toOptions.Count - 1
            : -1;
        if (toIndex == -1)
        {
            Console.WriteLine("Can't choose that.");
            return;
        }

        var toEnclosure = toOptions[toIndex];
        myZoo.MoveAnimal(selectedAnimal, fromEnclosure, toEnclosure);
    }
}
/*
⬜⬜⬜⬜⬜⬜⬜🏽⬜⬜⬜⬜⬜🏽⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜⬜⬜🏿🟨⬜⬜⬜⬜🏿⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜⬜🟨🏽🟨⬜🟨⬜⬜🏽⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜⬜🟨🟨🟨🟨🟨⬜🏽🏿⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜⬜🟨🟨🟨🟪🟪🏽🏿⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜🟪🟪🟨🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜🟪🟪🟪🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜⬜⬛🟪🟪🟪⬛🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜🟪🟪🟪🟪🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
⬜⬜⬜⬜🟪🟪🟪🟪🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜
🏿🏽🏽🏿🟪🟪🟪🟪🟪🟪🟪🟪⬜⬜⬜🏿🏽🏽🏽🏽⬜⬜🟨⬜🟨⬜🟨⬜⬜⬜
⬜🟪🟪🟥🏿⬜🟪🟨🟨🟪🟪⬜⬜⬜🏿🟥🟪🟥🟪⬜⬜⬜🟨🟨🟨🟨🟨⬜⬜⬜
⬜⬜🟪🟥🏽⬜⬜🟧🟧🟪⬜⬜⬜🏿🏽🟥🟪⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬜⬜⬜
⬜⬜⬜🟥🟪🏿⬜🟨🟨🟪⬜🏿🏿🏽🟥🟪⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬛⬜⬜
⬜⬜⬜⬜⬜🏽🏿🟧🟧🟪🟪🟪🏿⬜⬜⬜⬜⬜⬜⬜⬜⬛⬜⬜⬛⬜⬜⬛⬜⬜
⬜⬜⬜⬜⬜⬜🟨🟨🟨🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬜⬜⬛⬛⬜⬜⬜⬛⬛⬜⬜
⬜⬜⬜⬜⬜🟪🟧🟧🟧🟧🟪🟪🟪🟪🟪⬜⬜⬜⬜⬜⬛⬛⬜⬜🟨⬜⬜⬛⬛⬜
⬜⬜⬜⬜⬜🟪🟨🟨🟨🟨🟪🟪🟪🟪🟪🟧🟧⬜⬜⬜⬛⬛⬜⬜⬜⬜⬜⬛⬛⬜
⬜⬜⬜⬜⬜🟪🟪🟧🟧🟧🟧🟪🟪🟪🟪🟪🟪⬜⬜⬜⬛⬜⬜⬜⬜⬜⬜⬜⬛⬜
⬜⬜⬜⬜⬜🟪🟪⬜⬜🟪🟪🟪🟪🟪🟪⬜⬜🟪⬜⬜⬜⬛⬜⬜⬜⬜⬜⬛⬜⬜
⬜⬜⬜⬜⬜🟪⬜⬜⬜⬜🟪🟪⬜⬜🟪⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬛⬜⬜
⬜⬜⬜⬜⬜🟪⬜⬜⬜⬜⬜🟪⬜⬜⬜⬜⬜⬜⬜⬜⬜⬜🟨🟨⬜🟨🟨⬜⬜⬜

*/