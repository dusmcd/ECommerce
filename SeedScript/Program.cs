using ECommerceAPI.Data;
using SeedScript;
using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<EcommerceAPIContext>();
string devConnectionStr = "Server=(localdb)\\MSSQLLocalDB;Database=EcommerceAPIContext-1";
optionsBuilder.UseSqlServer(devConnectionStr);


var context = new EcommerceAPIContext(optionsBuilder.Options);

SeedData seedData = new SeedData(context);
// run seed script
seedData.DoSomething();

Console.WriteLine("DB seeded");

