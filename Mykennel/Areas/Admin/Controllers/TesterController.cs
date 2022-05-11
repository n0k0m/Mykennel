using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mykennel.Data;
using Mykennel.Models;
using Mykennel.Utility;
using Mykennel.Utility.TestData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TesterController : Controller
    {

        private readonly ILogger<TesterController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TesterController(
            ILogger<TesterController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment hostEnvironment

            )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var userCount = _context.Users.Count();
            if (userCount > 0)
            { ViewData["UserCount"] = userCount;} else
            { ViewData["UserCount"] = "0"; }

            var breedCount = _context.Breeds.Count();
            var countryCount = _context.Countries.Count();
            if (breedCount > 0)
            {
                ViewData["BreedCount"] = breedCount;
                ViewData["CountryCount"] = countryCount;
            }
            else
            { 
              ViewData["BreedCount"] = "0"; 
              ViewData["CountryCount"] = "0";
            }

            var kennelCount = _context.Kennels.Count();
            if (kennelCount > 0)
            { @ViewData["KennelCount"] = kennelCount;} else
            { @ViewData["KennelCount"] = "0"; }

            var dogsCount = _context.Dogs.Count();
            var littersCount = _context.Litters.Count();
            var puppiesCount = _context.Puppies.Count();
            if (dogsCount > 0)
            { @ViewData["DogCount"] = dogsCount; }
            else
            { @ViewData["DogCount"] = "0"; }
            if (littersCount > 0)
            { @ViewData["LitterCount"] = littersCount; }
            else
            { @ViewData["LitterCount"] = "0"; }
            if (puppiesCount > 0)
            { @ViewData["PuppyCount"] = puppiesCount; }
            else
            { @ViewData["PuppyCount"] = "0"; }

            // Fájlok és mappák kiíratása
            string webRootPath = _hostEnvironment.WebRootPath;
            string uploadFolder = Path.Combine(webRootPath, "kennels");

            if (Directory.Exists(uploadFolder))
            {
                string[] folders = Directory.GetDirectories(uploadFolder, "*", SearchOption.TopDirectoryOnly);
                return View(folders);
            }
            string[] noFolders = { };
            return View(noFolders);
        }

        public async Task<IActionResult> CreateUsers()
        {
            if (_context.ApplicationUsers.Count() == 1)
            {
                
                string[] Emails = 
                    { "alhajj@me.com",
                    "pkplex@yahoo.com",
                    "mcast@yahoo.ca",
                    "gastown@mac.com",
                    "kiddailey@comcast.net",
                    "kevinm@yahoo.ca",
                    "lipeng@yahoo.com",
                    "gregh@att.net",
                    "mhoffman@yahoo.ca",
                    "chunzi@me.com",
                    "niknejad@me.com",
                    "psichel@gmail.com",
                    "magusnet@gmail.com",
                    "crimsane@outlook.com",
                    "jusdisgi@sbcglobal.net",
                    "leviathan@verizon.net",
                    "yxing@outlook.com",
                    "natepuri@aol.com",
                    "moxfulder@verizon.net",
                    "rafasgj@live.com"
                };

                List<ApplicationUser> users = new List<ApplicationUser>();
                for (int i = 0; i < 20; i++)
                {
                    users.Add(
                    new ApplicationUser
                    {
                        UserName = Emails[i],
                        Email = Emails[i],
                        Role = SD.Role_User_Breeder
                    }
                    );
                }

                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "Pass1?");
                    await _userManager.AddToRoleAsync(user, SD.Role_User_Breeder);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateBreedsCountries()
        {
            // Fajták létrehozása
            if (_context.Breeds.Count() < 1)
            {
                List<Breed> breeds = new List<Breed>();
                breeds.Add(new Breed { BreedId = 166, Name = "German Shepherd Dog", OriginalName = "Deutscher Schaeferhund" });
                breeds.Add(new Breed { BreedId = 149, Name = "Bulldog", OriginalName = "Bulldog" });
                breeds.Add(new Breed { BreedId = 250, Name = "Havanese", OriginalName = "Bichon Havanais" });
                breeds.Add(new Breed { BreedId = 148, Name = "Dachshund", OriginalName = "Dachshund" });
                breeds.Add(new Breed { BreedId = 111, Name = "Golden Retriever", OriginalName = "Golden Retriever" });
                breeds.Add(new Breed { BreedId = 86, Name = "Yorkshire Terrier", OriginalName = "Yorkshire Terrier" });

                foreach (var breed in breeds)
                {
                    _context.Add(breed);
                    await _context.SaveChangesAsync();
                }

                // Országok létrehozása
                List<Country> countries = new List<Country>();
                countries.Add(new Country { CountryId = 40, CountryName = "Austria" });
                countries.Add(new Country { CountryId = 276, CountryName = "Germany" });
                countries.Add(new Country { CountryId = 348, CountryName = "Hungary" });
                countries.Add(new Country { CountryId = 616, CountryName = "Poland" });

                foreach (var country in countries)
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateKennels()
        {
            // Kennelek létrehozása
            if (_context.Kennels.Count() < 1 && _context.Users.Count() > 1)
            {
                Random random = new Random();
                var users = _context.Users.Where(u => u.Email != "admin@admin.eu").ToList();
                var usersCount = _context.Users.Count();
                var countries = _context.Countries.ToList();
                var countriesCount = _context.Countries.Count();

                int i = 0;
                foreach (var user in users)
                {
                    Kennel kennel = new Kennel()
                    {
                        KennelName = TestKennels.KennelName[i],
                        URLName = TestKennels.URLName[i],
                        City = TestKennels.City[random.Next(0, TestKennels.City.Count())],
                        PostalCode = TestKennels.PostalCode[random.Next(0, TestKennels.PostalCode.Count())],
                        Address = TestKennels.Address[random.Next(0, TestKennels.Address.Count())],
                        ShortDescription = TestKennels.ShortDescription[random.Next(0, TestKennels.ShortDescription.Count())],
                        Description = TestKennels.Description[random.Next(0, TestKennels.Description.Count())],
                        Logo = TestKennels.Logo[random.Next(0, TestKennels.Logo.Count())],
                        ContactPerson = TestKennels.ContactPerson[random.Next(0, TestKennels.ContactPerson.Count())],
                        Country = countries[random.Next(0, countriesCount)],
                        ApplicationUser= user as ApplicationUser
                    };
                    i++;
                    
                    _context.Add(kennel);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateRest()
        {
            if (_context.Dogs.Count() < 1)
            {
                try
                {
                    Random random = new Random();
                    var kennels = _context.Kennels.ToList();
                    var breeds = _context.Breeds.ToList();
                    var breedsCount = _context.Breeds.Count();

                    foreach (var kennel in kennels)
                    {
                        // Kiválasztok pár fajtát
                        for (int i = 0; i < 3; i++)
                        {
                            var breed = breeds[random.Next(0, breedsCount)];
                            // Fajtánként random mennyiségű kutyát
                            for (int j = 0; j < random.Next(0, 15); j++)
                            {
                                Dog dog = new Dog()
                                {
                                    RegNumber = TestDogs.RegNumber[random.Next(0, TestDogs.RegNumber.Count())],
                                    Born = GetRandomDate(),
                                    Name = TestDogs.Name[random.Next(0, TestDogs.Name.Count())],
                                    Sex = TestDogs.Sex[random.Next(0, TestDogs.Sex.Count())],
                                    Description = TestDogs.Description[random.Next(0, TestDogs.Description.Count())],
                                    TitlesGenetics = TestDogs.TitlesGenetics[random.Next(0, TestDogs.TitlesGenetics.Count())],
                                    Status = TestDogs.Status[random.Next(0, TestDogs.Status.Count())],
                                    // DogImage = TestDogs.DogImage[random.Next(0, TestDogs.DogImage.Count())],
                                    DogImage = @"\images\testImages\dogs\" + breed.BreedId + @"\" + random.Next(1, 11) + @".jpg",
                                    Breed = breed,
                                    Kennel = kennel,
                                };

                                _context.Add(dog);
                                await _context.SaveChangesAsync();
                            }

                            // Frissítem kennelen és fajtán belül 
                            var dbDogs = _context.Dogs.Where(d => d.Breed == breed && d.Kennel == kennel).ToList();
                            var dbMaleDogs = dbDogs.Where(d => d.Sex == Sex.Male).ToList();
                            var dbFemaleDogs = dbDogs.Where(d => d.Sex == Sex.Female).ToList();
                            if (dbDogs.Any())
                            {
                                foreach (var dbDog in dbDogs)
                                {
                                    int? RandomOrNull;
                                    if (dbMaleDogs.Any())
                                    {
                                        RandomOrNull = GetRandomOrNull(dbMaleDogs.Count());
                                        dbDog.Father = RandomOrNull == null ? null : dbMaleDogs[(int)RandomOrNull];
                                    }
                                    if (dbFemaleDogs.Any())
                                    {
                                        RandomOrNull = GetRandomOrNull(dbFemaleDogs.Count());
                                        dbDog.Mother = RandomOrNull == null ? null : dbFemaleDogs[(int)RandomOrNull];
                                    }
                                    _context.Update(dbDog);
                                    await _context.SaveChangesAsync();
                                }
                            }

                            // Ha van legalább egy szülőpár
                            if (dbMaleDogs.Any() && dbFemaleDogs.Any())
                            {
                                // Létrehozom az almokat is, hogy még kennelen és fajtán belül fel tudjam tölteni
                                for (int k = 0; k < random.Next(0, 4); k++)
                                {
                                    Litter litter = new Litter()
                                    {
                                        Name = TestLitters.Name[random.Next(0, TestLitters.Name.Count())],
                                        Date = GetRandomDate(),
                                        Kennel = kennel,
                                        Father = dbMaleDogs[random.Next(0, dbMaleDogs.Count())],
                                        Mother = dbFemaleDogs[random.Next(0, dbFemaleDogs.Count())]
                                    };

                                    _context.Add(litter);
                                    await _context.SaveChangesAsync();

                                    // Létrehozom hozzá a kiskutyákat is
                                    for (int l = 0; l < random.Next(0, 10); l++)
                                    {
                                        int? RandomOrNull = GetRandomOrNull(dbDogs.Count());
                                        Puppy puppy = new Puppy()
                                        {
                                            Name = TestPuppies.Name[random.Next(0, TestPuppies.Name.Count())],
                                            Sex = TestPuppies.Sex[random.Next(0, TestPuppies.Sex.Count())],
                                            Bookable = TestPuppies.Bookable[random.Next(0, TestPuppies.Bookable.Count())],
                                            Aim = TestPuppies.Aim[random.Next(0, TestPuppies.Aim.Count())],
                                            Description = TestPuppies.Description[random.Next(0, TestPuppies.Description.Count())],
                                            Litter = litter,
                                            Dog = RandomOrNull == null ? null : dbDogs[(int)RandomOrNull]
                                        };
                                        _context.Add(puppy);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }

                    }
                } catch (Exception errorMsg)
                {
                    TempData["ErrorMessage"] = errorMsg.ToString();
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteKennels()
        {
            // Hogy biztosan sikerüjön a törlés meg kell szüntetni a külső kulcsokat
            var puppies = _context.Puppies.ToList();
            var litters = _context.Litters.ToList();
            var dogs = _context.Dogs.ToList();
            var kennels = _context.Kennels.ToList();
            try
            {
                foreach (var puppy in puppies)
                {
                    _context.Puppies.Remove(puppy);
                    await _context.SaveChangesAsync();
                }

                foreach (var litter in litters)
                {
                    _context.Litters.Remove(litter);
                    await _context.SaveChangesAsync();
                }

                foreach (var dog in dogs)
                {
                    dog.Father = null;
                    dog.Mother = null;
                    _context.Update(dog);
                    await _context.SaveChangesAsync();

                    _context.Dogs.Remove(dog);
                    await _context.SaveChangesAsync();
                }

                foreach (var kennel in kennels)
                {
                    _context.Kennels.Remove(kennel);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception errorMsg)
            {
                TempData["ErrorMessage"] = errorMsg.ToString();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAll()
        {
            var users = _context.Users.ToList();
            var breeds = _context.Breeds.ToList();
            var countries = _context.Countries.ToList();
            try
            {
                await DeleteKennels();

                foreach (var breed in breeds)
                {
                    _context.Breeds.Remove(breed);
                    await _context.SaveChangesAsync();
                }

                foreach (var country in countries)
                {
                    _context.Countries.Remove(country);
                    await _context.SaveChangesAsync();
                }

                foreach (var user in users)
                {
                    if (user.Email != "admin@admin.eu")
                    {
                        await _userManager.DeleteAsync(user);
                    }
                }
            }
            catch (Exception errorMsg)
            {
                TempData["ErrorMessage"] = errorMsg.ToString();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteImages() 
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            string uploadFolder = Path.Combine(webRootPath, "kennels");

            try
            {
                if (Directory.Exists(uploadFolder))
                {
                    DirectoryInfo di = new DirectoryInfo(uploadFolder);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(uploadFolder);
                }
            }
            catch (Exception errorMsg)
            {
                TempData["ErrorMessage"] = errorMsg.ToString();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public int? GetRandomOrNull(int max)
        {
            Random random = new Random();
            int?[] randomOrNull = { random.Next(0, max), null };
            return randomOrNull[random.Next(0, 2)];
        }

        public DateTime GetRandomDate()
        {
            Random random = new Random();

            DateTime minDate = new DateTime(2015, 1, 1, 10, 0, 0);
            DateTime maxDate = new DateTime(2022, 1, 1, 10, 0, 0);
            //Random.Next in .NET is non-inclusive to the upper bound (@NickLarsen)
            int minutesDiff = Convert.ToInt32(maxDate.Subtract(minDate).TotalMinutes + 1);
            return minDate.AddMinutes(random.Next(1, minutesDiff));
            }
        }
    }