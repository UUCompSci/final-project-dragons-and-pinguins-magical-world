using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Entities;

public abstract class Animal
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public abstract string Name { get; set; }
  private double _energy;
  private string _mood;

  public abstract void Update();
  public double EnergyLevel
  {
    get { return _energy; }
    set { _energy = value; }
  }

  public string CurrentMood
  {
    get { return _mood; }
    set { _mood = value; }
  }
}

public class Penguin : Animal
{
  public override string Name { get; set; }
  public int HungerLevel { get; set; }
  public int MischiefLevel { get; set; }
  public int BraveryLevel { get; set; }
  public bool _canLeadGang = false;

  public override void Update()
  {
    int p = Random.Shared.Next(1, 4);

    if (p == 1)
    {
      Console.WriteLine($"{Name} wandered into a different encloser.");
    }
    else if (p == 2)
    {
      Console.WriteLine($"{Name} stole some fish from another peguin! Hunger level decreased!");
      HungerLevel -= 1;
    }
    else if (p == 3 & _canLeadGang != true)
    {
      Console.WriteLine($"{Name} has formed a mini gang!");
      _canLeadGang = true;
    }
    else
    {
      Console.WriteLine($"{Name} and their gang are plotting on how to take over the dragons fishing hole.");
    }
  }

  public void Steal(Dragon dragon)
  {
    if (BraveryLevel > dragon.FrightenLevel)
    {
      Console.Write($"{Name} has stolen fish from {dragon.Name}");
    }      
    else
    {
      Console.WriteLine("The penguins were to scared to steal from the dragon!");        
    }
  }
}

public class Dragon : Animal
{
  public override string Name { get; set; }
  public int FireLevel { get; set; }
  public int FrightenLevel { get; set; }
  public double _treasureHoardAmount;
  public bool _alert = false;

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
      Console.WriteLine($"{Name} was angery and roared causing the nearby penguins to panic and flee!");
      FrightenLevel += 1;
    }
  }
}

// Kate's part
public class Enclosure
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  private string enclosureName;
  private List<Animal> animalSquad;

  public Enclosure(string name)
  {
    enclosureName = name;
    animalSquad = new List<Animal>();
  }

  public string Name { get { return enclosureName; } }

  public void AddAnimal(Animal critter)
  {
    animalSquad.Add(critter);
    Console.WriteLine($"{critter.Name} has joined {enclosureName}! 🎉");
  }

  public void RemoveAnimal(Animal critter)
  {
    animalSquad.Remove(critter);
    Console.WriteLine($"{critter.Name} has left {enclosureName}... 😢");
  }

  //let the animals do their thing
  public void RunEnclosureStep()
  {
    Console.WriteLine($"\nUpdating {enclosureName}...");
    foreach (Animal critter in animalSquad)
    {
      critter.Update(); //polymorphic MAGIC
    }

    HandleShenanigans();
  }

  private void HandleShenanigans()
  {
    //placeholder for chaos between dragons and penguins
    if (animalSquad.Count > 1)
    {
      Console.WriteLine("Some animal things are happening! 🐧🔥");
    }
  }

  public List<Animal> GetAnimals()
  {
    return animalSquad;
  }
}

//RANDOM EVENTS
public static class RandomEvents
{
  private static Random rng = new Random();

  public static void MaybeTriggerEvent(Enclosure enclosure)
  {
    int roll = rng.Next(1, 101); // Roll 1-100
    if (roll <= 10)
    {
      Console.WriteLine($"🐟 Fish Frenzy hits {enclosure.Name}! Penguins are bouncing off the walls!");
    }
    else if (roll <= 15)
    {
      Console.WriteLine($"🔥 Dragon Fire Cough in {enclosure.Name}! Watch out, penguins!");
    }
    //add more ridiculous events here
  }
}

//world manager
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
    Console.WriteLine($"{critter.Name} waddled/flew from {from.Name} to {to.Name}!");
  }

  public void AdvanceTime()
  {
    foreach (Enclosure e in allEnclosures)
    {
      e.RunEnclosureStep();
      RandomEvents.MaybeTriggerEvent(e);
    }
  }

  //database magic (placeholder)
  public void SaveZoo()
  {
    Console.WriteLine("💾 Saving the zoo to the mysterious database...");
  }

  public void LoadZoo()
  {
    Console.WriteLine("📂 Loading the zoo from the mysterious database...");
  }

  public List<Enclosure> GetEnclosures()
  {
    return allEnclosures;
  }
}