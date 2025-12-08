using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public BaseEntity() {}
    }

    public abstract class Animal : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual double EnergyLevel { get; set; }
        public virtual string CurrentMood { get; set; }

        public abstract void Update();
    }

    public class Penguin : Animal
    {
        public virtual int HungerLevel { get; set; }
        public virtual int MischiefLevel { get; set; }
        public virtual int BraveryLevel { get; set; }
        private bool _canLeadGang = false;

        public override void Update()
        {
            int p = Random.Shared.Next(1, 4);

            if (p == 1)
            {
                Console.WriteLine($"uh oh, {Name} wandered into a different enclosure. that'a a problem...");
                EnergyLevel -= 1;
                BraveryLevel += 1;
            }
            else if (p == 2)
            {
                Console.WriteLine($"{Name} stole some fish from another penguin! Hunger level decreased!");
                EnergyLevel += 1;
                HungerLevel -= 1;
            }
            else if (p == 3 && !_canLeadGang)
            {
                Console.WriteLine($"{Name} has formed a gang! Watch out!");
                MischiefLevel += 1;
                _canLeadGang = true;
            }
            else
            {
                Console.WriteLine($"{Name} and their gang are plotting to take over the dragon fishing hole.");
                MischiefLevel += 1;
            }
        }

        public void Steal(Dragon dragon)
        {
            if (BraveryLevel > dragon.FrightenLevel)
            {
                Console.WriteLine($"{Name} has stolen treasure from {dragon.Name}!");
                dragon.TreasureHoardAmount -= 1;
            }
            else
            {
                Console.WriteLine("The penguins were too scared to steal from the dragon!");
            }
        }
    }

    public class Dragon : Animal
    {
        public virtual int FireLevel { get; set; }
        public virtual int FrightenLevel { get; set; }
        public virtual double TreasureHoardAmount { get; set; }
        private bool _alert = false;

        public override void Update()
        {
            _alert = false;
            int d = Random.Shared.Next(1, 5);

            if (d == 1)
            {
                Console.WriteLine($"{Name} is alert and watchful for intruders.");
                EnergyLevel += 1;
                _alert = true;
            }
            else if (d == 2)
            {
                Console.WriteLine($"{Name} sneezed a puff of fire and singed the fence causing some damage.");
                FireLevel += 1;
            }
            else if (d == 3)
            {
                Console.WriteLine($"{Name} colected some treasure and put it in their hoard!");
                TreasureHoardAmount += 1;    
            }
            else
            {
                Console.WriteLine($"{Name} was angry and roared, causing nearby penguins to panic and flee the scene!");
                FrightenLevel += 1;
            }
        }
    }

    public static class CheckKey
    {
        public const ConsoleKey _1 = ConsoleKey.D1;
        public const ConsoleKey _2 = ConsoleKey.D2;
        public const ConsoleKey _3 = ConsoleKey.D3;
        public const ConsoleKey _4 = ConsoleKey.D4;
        public const ConsoleKey _5 = ConsoleKey.D5;
        public const ConsoleKey _6 = ConsoleKey.D6;
        public const ConsoleKey _7 = ConsoleKey.D7;
    }

    public class Enclosure : BaseEntity
    {
        public string Name { get; private set; }
        public virtual ICollection<Animal> Animals { get; set; } // EF Core tracking

        private Enclosure() // Parameterless constructor for EF Core
        {
            Animals = new List<Animal>();
        }

        public Enclosure(string name)
        {
            Name = name;
            Animals = new List<Animal>();
        }

        public void AddAnimal(Animal critter)
        {
            Animals.Add(critter);
            Console.WriteLine($"{critter.Name} has joined {Name}! 🎉");
        }

        public void RemoveAnimal(Animal critter)
        {
            Animals.Remove(critter);
            Console.WriteLine($"{critter.Name} has left {Name}... 😢 byebye");
        }

        public void RunEnclosureStep()
        {
            Console.WriteLine($"\nUpdating {Name}...");
            foreach (Animal critter in Animals)
            {
                critter.Update();
            }
            HandleShenanigans();
        }

        private void HandleShenanigans()
        {
            if (Animals.Count > 1)
                Console.WriteLine("Some... interesting animal things are happening 🐧🔥");
        }

        public List<Animal> GetAnimals() => new List<Animal>(Animals);
    }

    public static class RandomEvents
    {
        private static Random rng = new Random();

        public static void MaybeTriggerEvent(Enclosure enclosure)
        {
            int roll = rng.Next(1, 101);
            if (roll <= 10)
            {
                Console.WriteLine($"🐟 A FISH FRENZY has hit {enclosure.Name}! Penguins have gone crazy!");
            }
            else if (roll <= 15)
            {
                Console.WriteLine($"🔥 Dragon Fire Cough in {enclosure.Name}! Watch out, little penguins!");
            }
        }
    }

    public class Zoo
    {
        private List<Enclosure> allEnclosures;

        public Zoo()
        {
            allEnclosures = new List<Enclosure>();
        }

        public void AddEnclosure(Enclosure e)
        {
            allEnclosures.Add(e);
            Console.WriteLine($"Enclosure {e.Name} has been created! 🏰");
        }

        public void MoveAnimal(Animal critter, Enclosure from, Enclosure to)
        {
            from.RemoveAnimal(critter);
            to.AddAnimal(critter);
            Console.WriteLine($"{critter.Name} went from {from.Name} to {to.Name}!");
        }

        public void AdvanceTime()
        {
            foreach (Enclosure e in allEnclosures)
            {
                e.RunEnclosureStep();
                RandomEvents.MaybeTriggerEvent(e);
            }
        }

        public void SaveZoo() => Console.WriteLine("💾 Saving the zoo...");
        public void DeleteZoo() => Console.WriteLine("📂 Deleting the zoo...");

        public List<Enclosure> GetEnclosures() => allEnclosures;

        // ef core checking
        public void AddRange(ZooDbContext db)
        {
            foreach (var enclosure in allEnclosures)
            {
                if (!db.Enclosures.Any(e => e.Id == enclosure.Id))
                    db.Enclosures.Add(enclosure);

                foreach (var animal in enclosure.GetAnimals())
                {
                    switch (animal)
                    {
                        case Penguin penguin:
                            if (!db.Penguins.Any(p => p.Id == penguin.Id))
                                db.Penguins.Add(penguin);
                            break;
                        case Dragon dragon:
                            if (!db.Dragons.Any(d => d.Id == dragon.Id))
                                db.Dragons.Add(dragon);
                            break;
                    }
                }
            }

            db.SaveChanges();
            Console.WriteLine("Your zoo has been saved to database! 💾");
        }
    }
}
