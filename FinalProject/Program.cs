using Microsoft.EntityFrameworkCore;
using Entities;

using (Zoo z = new())
{
    bool created = z.Database.EnsureCreated();

    // Kate's part
    Zoo myZoo = new Zoo();
    bool isRunning = true;

    while (isRunning)
    {
        Console.WriteLine("\n=== 🐧🔥 DRAGON & PENGUIN ZOO 🐉❄ ===");
        Console.WriteLine("1. Add a new enclosure");
        Console.WriteLine("2. Add a new animal");
        Console.WriteLine("3. Move an animal");
        Console.WriteLine("4. Advance time (simulation step)");
        Console.WriteLine("5. Save the zoo");
        Console.WriteLine("6. Delete the zoo");
        Console.WriteLine("7. Quit (sad face)");

        switch (GetKey())
        {
            case CheckKey._1:
                Console.Write("Name your new enclosure: ");
                string encName = Console.ReadLine();
                myZoo.AddEnclosure(new Enclosure(encName));
                break;

            case CheckKey._2:
                Console.WriteLine("Would you like to add a dragon 🐉 or a penguin 🐧?");
                string choice2 = Console.ReadLine();

                break;

            case CheckKey._3:
                Console.WriteLine("🐾 Move animal logic placeholder - select animals later.");
                break;

            case CheckKey._4:
                myZoo.AdvanceTime();
                break;

            case CheckKey._5:
                myZoo.SaveZoo();
                z.SaveChanges();
                break;

            case CheckKey._6:
                myZoo.LoadZoo();
                z.Database.EnsureDeleted();
                break;

            case CheckKey._7:
                isRunning = false;
                Console.WriteLine("Exiting zoo simulator. Goodbye! 🐧🔥");
                break;

            default:
                Console.WriteLine("Oops, invalid choice. Try again! 😅");
                break;
        }
    }
}

static ConsoleKey GetKey()
{
    ConsoleKey getKey = Console.ReadKey(true).Key;
    return getKey;
}