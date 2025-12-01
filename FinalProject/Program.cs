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

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Name your new enclosure: ");
                string encName = Console.ReadLine();
                myZoo.AddEnclosure(new Enclosure(encName));
                break;

            case "2":
                Console.WriteLine("🚨 Add animal here - partner handles this part!");
                break;

            case "3":
                Console.WriteLine("🐾 Move animal logic placeholder - select animals later.");
                break;

            case "4":
                myZoo.AdvanceTime();
                break;

            case "5":
                myZoo.SaveZoo();
                z.SaveChanges();
                break;

            case "6":
                myZoo.LoadZoo();
                z.Database.EnsureDeleted();
                break;

            case "7":
                isRunning = false;
                Console.WriteLine("Exiting zoo simulator. Goodbye! 🐧🔥");
                break;

            default:
                Console.WriteLine("Oops, invalid choice. Try again! 😅");
                break;
        }
    }
}