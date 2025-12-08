using Entities;

class Program
{
    static void Main()
    {
        //core db
        using (ZooDbContext db = new())
        {
           db.Database.EnsureCreated();

            //memory simulation
            Zoo myZoo = new Zoo();
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n=== 🐧🔥 DRAGON & PENGUIN ZOO 🐉❄ ===");
                Console.WriteLine("1. Add a new enclosure");
                Console.WriteLine("2. Add a new animal");
                Console.WriteLine("3. Move an animal");
                Console.WriteLine("4. Advance time (1 step)");
                Console.WriteLine("5. Save the zoo");
                Console.WriteLine("6. Delete the zoo");
                Console.WriteLine("7. Quit (sad face)");

                switch (GetKey())
                {
                    case CheckKey._1:
                        Console.Write("Name your new enclosure, choose wisely: ");
                        string encName = Console.ReadLine();
                        myZoo.AddEnclosure(new Enclosure(encName));
                        myZoo.AddRange(db);
                        break;

                    case CheckKey._2:
                        AddAnimalMenu(myZoo);
                        myZoo.AddRange(db);
                        break;

                    case CheckKey._3:
                        Console.WriteLine("🐾 m animal logic placeholder - select animals later.");
                        break;

                    case CheckKey._4:
                        myZoo.AdvanceTime();
                        break;

                    case CheckKey._5:
                        db.SaveChanges();
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

        static ConsoleKey GetKey() => Console.ReadKey(true).Key;

        static void AddAnimalMenu(Zoo myZoo)
        {
            Console.WriteLine("Add a dragon 🐉 or a penguin 🐧?");
            string choice = Console.ReadLine()?.ToLower();

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Animal animal = choice switch
            {
                "dragon" => new Dragon { Name = name, CurrentMood = "Calm" },
                "penguin" => new Penguin { Name = name, CurrentMood = "Calm" },
                _ => null
            };

            if (animal == null)
            {
                Console.WriteLine("no. you can't do that.");
                return;
            }

            Console.WriteLine("Select an enclosure:");
            var enclosures = myZoo.GetEnclosures();
            if (enclosures.Count == 0)
            {
                Console.WriteLine("no enclosures exist. you have to build one first.");
                return;
            }

            for (int i = 0; i < enclosures.Count; i++)
                Console.WriteLine($"{i + 1}. {enclosures[i].Name}");

            int selected = int.TryParse(Console.ReadLine(), out int val) && val > 0 && val <= enclosures.Count
                ? val - 1
                : -1;

            if (selected == -1)
            {
                Console.WriteLine("you can't do that.");
                return;
            }

            enclosures[selected].AddAnimal(animal);
        }
    }
}