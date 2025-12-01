using Microsoft.EntityFrameworkCore;
using Entities;

using (Zoo z = new())
{
    bool created = z.Database.EnsureCreated();
    Console.WriteLine($"Database created: {created}");

}