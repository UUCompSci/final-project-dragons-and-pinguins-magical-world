using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Entities;
public abstract class Animal
{
  public Guid Id { get; private set; } = Guid.NewGuid();
  public abstract string Name { get; set; }
  private double _energy;
  private string _mood;

  public abstract void UpdateBehavior();
}

public class Penguin : Animal
{
  public override string Name { get; set; }
  public int HungerLevel { get; set; }
  public int MischiefLevel { get; set; }
  public int BraveryLevel { get; set; }
  public bool _canLeadGang = false;

  public override void UpdateBehavior()
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

  public override void UpdateBehavior()
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
      Console.WriteLine($"{Name} sneezed a puff of fire and singed the fence casuing {FireLevel} points of damage to it!");
      Enclosure._enclosureHP -= FireLevel;
    }
    else
    {
      Console.WriteLine($"{Name} was angery and roared causing the nearby penguins to panic and flee!");
      FrightenLevel += 1;
    }
  }
}