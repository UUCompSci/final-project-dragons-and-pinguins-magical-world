using System;
using System.Collections.Generic;

namespace Entities
{
    public abstract class Animal
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public abstract string Name { get; set; }
        public double EnergyLevel { get; set; }
        public string CurrentMood { get; set; }

        public abstract void Update();
    }

    public class Penguin : Animal
    {
        public override string Name { get; set; }
        public int HungerLevel { get; set; }
        public int MischiefLevel { get; set; }
        public int BraveryLevel { get; set; }
        private bool _canLeadGang = false;

        public override void Update()
        {
            int p = Random.Shared.Next(1, 4);

            if (p == 1)
            {
                Console.WriteLine($"uh oh, {Name} wandered into a different enclosure. that'a a problem...");
            }
            else if (p == 2)
            {
                Console.WriteLine($"{Name} stole some fish from another penguin! Hunger level decreased!"); //what a theif
                HungerLevel -= 1;
            }
            else if (p == 3 && !_canLeadGang)
            {
                Console.WriteLine($"{Name} has formed a  gang! Watch out!");
                _canLeadGang = true;
            }
            else
            {
                Console.WriteLine($"{Name} and their gang are plotting to take over the dragon fishing hole.");
            }
        }

        public void Steal(Dragon dragon)
        {
            if (BraveryLevel > dragon.FrightenLevel)
            {
                Console.WriteLine($"{Name} has stolen fish from {dragon.Name}!");
            }
            else
            {
                Console.WriteLine("The penguins were too scared to steal from the dragon!");
            }
        }
    }

    public class Dragon : Animal
    {
        public override string Name { get; set; }
        public int FireLevel { get; set; }
        public int FrightenLevel { get; set; }
        public double TreasureHoardAmount { get; set; }
        private bool _alert = false;

        public override void Update()
        {
            _alert = false;
            int d = Random.Shared.Next(1, 4);

            if (d == 1)
            {
                Console.WriteLine($"{Name} is alert and watchful for intruders.");
                _alert = true;
            }
            else if (d == 2)
            {
                Console.WriteLine($"{Name} sneezed a puff of fire and singed the fence causing some damage.");
            }
            else
            {
                Console.WriteLine($"{Name} was angry and roared, causing nearby penguins to panic and flee the scene!"); //that'll scare the fish thief
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

    public class Enclosure
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        private List<Animal> animalSquad;

        public Enclosure(string name)
        {
            Name = name;
            animalSquad = new List<Animal>();
        }

        public void AddAnimal(Animal critter)
        {
            animalSquad.Add(critter);
            Console.WriteLine($"{critter.Name} has joined {Name}! 🎉");
        }

        public void RemoveAnimal(Animal critter)
        {
            animalSquad.Remove(critter);
            Console.WriteLine($"{critter.Name} has left {Name}... 😢 byebye");
        }

        public void RunEnclosureStep()
        {
            Console.WriteLine($"\nUpdating {Name}...");
            foreach (Animal critter in animalSquad)
            {
                critter.Update();
            }
            HandleShenanigans();
        }

        private void HandleShenanigans()
        {
            if (animalSquad.Count > 1)
                Console.WriteLine("Some... intresting animal things are happening 🐧🔥");
        }

        public List<Animal> GetAnimals() => animalSquad;
    }

    public static class RandomEvents
    {
        private static Random rng = new Random();

        public static void MaybeTriggerEvent(Enclosure enclosure)
        {
            int roll = rng.Next(1, 101);
            if (roll <= 10)
            {
                Console.WriteLine($"🐟a FISH FRENZY has hit {enclosure.Name}! Penguins have gone crazy!");
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
    }
}