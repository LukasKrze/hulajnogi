using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ScooterRentalApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<Client> userManager)
        {
            if (context.Roles.FirstOrDefault(r => r.Name == SystemRoles.Administrator) == null)
            {
                context.Add(new IdentityRole { Name = SystemRoles.Administrator, NormalizedName = SystemRoles.Administrator });
                context.SaveChanges();
            }

            var adminAccount = "Admin25@scooterrentalapp.pl";

            var user = userManager.FindByNameAsync(adminAccount).GetAwaiter().GetResult();

            if (user == null)
            {
                user = new Client
                {
                    UserName = adminAccount,
                    Email = adminAccount,
                    EmailConfirmed = true,
                    FirstName = "Konto",
                    LastName = "Administracyjne",
                    PhoneNumber = string.Empty
                };

                var result = userManager.CreateAsync(user, adminAccount).GetAwaiter().GetResult();
            }

            if (!userManager.IsInRoleAsync(user, SystemRoles.Administrator).GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(user, SystemRoles.Administrator).GetAwaiter().GetResult();
            }

            var sampleUsers = ReadResource("ScooterRentalApp.Seed.Users.csv");

            foreach (var sampleUser in sampleUsers)
            {
                user = userManager.FindByNameAsync(sampleUser[0]).GetAwaiter().GetResult();

                if (user == null)
                {
                    user = new Client
                    {
                        UserName = sampleUser[0],
                        Email = sampleUser[0],
                        EmailConfirmed = true,
                        FirstName = sampleUser[2],
                        LastName = sampleUser[3],
                        PhoneNumber = sampleUser[4]
                    };

                    var result = userManager.CreateAsync(user, sampleUser[1]).GetAwaiter().GetResult();
                }

            }
            ScooterCategory childCategory = context.ScooterCategories.FirstOrDefault(c => c.Name == "Dla dzieci");
            ScooterCategory adultCategory = context.ScooterCategories.FirstOrDefault(c => c.Name == "Dla dorosłych");
            
            if(childCategory == null)
            {
                childCategory = new ScooterCategory { Name = "Dla dzieci" };
                context.ScooterCategories.Add(childCategory);
                context.SaveChanges();
            }

            if (adultCategory == null)
            {
                adultCategory = new ScooterCategory { Name = "Dla dorosłych" };
                context.ScooterCategories.Add(adultCategory);
                context.SaveChanges();
            }

            var sampleScooters = ReadResource("ScooterRentalApp.Seed.Scooters.csv");

            foreach (var sampleScooter in sampleScooters)
            {
                if (sampleScooter[1] == "Electric" && !context.Scooters.OfType<ElectricScooter>().Any(s => s.SerialNumber == sampleScooter[7]))
                {
                    var pricing = new Pricing
                    {
                        From = DateTime.MinValue,
                        PricePerUnit = decimal.Parse(sampleScooter[9])
                    };

                    var scooter = new ElectricScooter
                    {
                        Model = sampleScooter[0],
                        BatteryCapacity = float.Parse(sampleScooter[2]),
                        HasKickstand = sampleScooter[4] == "TRUE",
                        MaxSpeed = float.Parse(sampleScooter[5]),
                        Range = float.Parse(sampleScooter[6]),
                        SerialNumber = sampleScooter[7],
                        YearOfProduction = int.Parse(sampleScooter[8]),
                        Pricings = new List<Pricing> { pricing },
                        Category = sampleScooter[10] == "1" ? childCategory :adultCategory,
                        Description = sampleScooter[11],
                        Picture = sampleScooter[12]
                    };

                    pricing.Scooter = scooter;

                    context.Pricings.Add(pricing);
                    context.Scooters.Add(scooter);
                    context.SaveChanges();
                }

                if (sampleScooter[1] == "Manual" && !context.Scooters.OfType<ManualScooter>().Any(s => s.SerialNumber == sampleScooter[7]))
                {
                    var pricing = new Pricing
                    {
                        From = DateTime.MinValue,
                        PricePerUnit = decimal.Parse(sampleScooter[9])
                    };

                    var manualScooter = new ManualScooter
                    {
                        Model = sampleScooter[0],
                        WheelSize = float.Parse(sampleScooter[3]),
                        HasKickstand = sampleScooter[4] == "TRUE",
                        MaxSpeed = float.Parse(sampleScooter[5]),
                        Range = float.Parse(sampleScooter[6]),
                        SerialNumber = sampleScooter[7],
                        YearOfProduction = int.Parse(sampleScooter[8]),
                        Pricings = new List<Pricing> { pricing },
                        Category = sampleScooter[10] == "1" ? childCategory : adultCategory,
                        Description = sampleScooter[11],
                        Picture = sampleScooter[12]
                    };

                    pricing.Scooter = manualScooter;

                    context.Pricings.Add(pricing);
                    context.Scooters.Add(manualScooter);
                    context.SaveChanges();
                }
            }
        }

        static List<string[]> ReadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            List<string[]> result = new List<string[]>();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                    result.Add(reader.ReadLine().Split(";").ToArray());

            }
            return result;

        }
    }
}
