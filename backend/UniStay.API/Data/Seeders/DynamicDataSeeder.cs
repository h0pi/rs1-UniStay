using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniStay.API.Data.Models;
using UniStay.API.Data;


public static class DynamicDataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, CancellationToken ct = default)
    {
        // Ako već ima rola ili usera – pretpostavi da je baza popunjena i preskoči
        if (await db.Role.AnyAsync(ct) || await db.User.AnyAsync(ct))
            return;

        // ------------------------------------------------
        // 1. ROLES
        // ------------------------------------------------
        var roles = new List<Roles>
        {
            new Roles { RoleName = "Student" }, // ID = 1 ako je baza prazna
            new Roles { RoleName = "Admin"   }, // ID = 2
            new Roles { RoleName = "Employee"}  // ID = 3
        };

        db.Role.AddRange(roles);
        await db.SaveChangesAsync(ct);

        var studentRole = roles.Single(r => r.RoleName == "Student");
        var adminRole = roles.Single(r => r.RoleName == "Admin");
        var employeeRole = roles.Single(r => r.RoleName == "Employee");

        // ------------------------------------------------
        // 2. USERS (20 studenata, 2 admina, 5 employee)
        // ------------------------------------------------
        string defaultPassword = "Test123!"; // zadovoljava tvoj password validator
        string passwordHash = HashPassword(defaultPassword);

        var users = new List<Users>();

        // 10 muških studenata Ammar Hopovac
        for (int i = 0; i < 10; i++)
        {
            users.Add(new Users
            {
                FirstName = $"Ammar{i}",
                LastName = "Hopovac",
                Email = $"ammar{i}@gmail.com",
                Phone = "060 000 0000",
                DateOfBirth = new DateTime(2000, 1, 1).AddDays(i),
                Username = $"ammar{i}",
                PasswordHash = passwordHash,
                ProfileImage = string.Empty,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = studentRole
            });
        }

        // 10 studentica Emra Mujezin
        for (int i = 0; i < 10; i++)
        {
            users.Add(new Users
            {
                FirstName = $"Emra{i}",
                LastName = "Mujezin",
                Email = $"emra{i}@gmail.com",
                Phone = "060 000 0000",
                DateOfBirth = new DateTime(2000, 6, 1).AddDays(i),
                Username = $"emra{i}",
                PasswordHash = passwordHash,
                ProfileImage = string.Empty,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = studentRole
            });
        }

        // 2 admina Admin Admin0..1
        for (int i = 0; i < 2; i++)
        {
            users.Add(new Users
            {
                FirstName = $"Admin{i}",
                LastName = "Admin",
                Email = $"admin{i}@gmail.com",
                Phone = "060 000 0000",
                DateOfBirth = new DateTime(1990, 1, 1).AddYears(i),
                Username = $"admin{i}",
                PasswordHash = passwordHash,
                ProfileImage = string.Empty,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = adminRole
            });
        }

        // 5 employee-a sa bosanskim imenima
        string[] empFirst = { "Haris", "Lejla", "Mirza", "Amila", "Tarik" };
        string[] empLast = { "Kovačević", "Hadžić", "Marić", "Mehić", "Delić" };

        for (int i = 0; i < 5; i++)
        {
            users.Add(new Users
            {
                FirstName = $"Haris{i}",
                LastName = "Kovacevic",
                Email = $"haris[i].@gmail.com",
                Phone = "060 000 0000",
                DateOfBirth = new DateTime(1995, 3, 1).AddYears(i),
                Username = $"haris{i}",
                PasswordHash = passwordHash,
                ProfileImage = string.Empty,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Role = employeeRole
            });
        }

        db.User.AddRange(users);
        await db.SaveChangesAsync(ct);

        var allStudents = users.Where(u => u.Role == studentRole).ToList();
        var allAdmins = users.Where(u => u.Role == adminRole).ToList();

        // ------------------------------------------------
        // 3. ROOMS (50 soba) + BEDS + ASSIGNMENT 2 studenta / soba
        // ------------------------------------------------
        var rooms = new List<Rooms>();
        for (int i = 1; i <= 50; i++)
        {
            rooms.Add(new Rooms
            {
                RoomNumber = $"R{i:000}",
                Floor = (i - 1) / 10 + 1,   // po 10 soba po spratu
                MaxOccupancy = 2,
                Description = $"Soba broj {i}"
            });
        }

        db.Room.AddRange(rooms);
        await db.SaveChangesAsync(ct); // sada RoomID-ovi postoje

        // po 2 kreveta u svakoj sobi
        var beds = new List<Beds>();
        foreach (var room in rooms)
        {
            for (int b = 1; b <= 2; b++)
            {
                beds.Add(new Beds
                {
                    BedNumber = $"{room.RoomNumber}-B{b}",
                    Room = room
                });
            }
        }

        db.Bed.AddRange(beds);
        await db.SaveChangesAsync(ct);

        // dodjela kreveta studentima: 2 studenta po sobi (1 student = 1 krevet)
        var orderedBeds = beds.OrderBy(b => b.Room.RoomNumber).ToList();
        var bedAssignments = new List<BedAssignments>();

        int bedIndex = 0;
        foreach (var student in allStudents)
        {
            if (bedIndex >= orderedBeds.Count)
                break;

            var bed = orderedBeds[bedIndex++];
            bedAssignments.Add(new BedAssignments
            {
                Student = student,
                Bed = bed,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddMonths(6)
            });
        }

        db.BedAssignment.AddRange(bedAssignments);
        await db.SaveChangesAsync(ct);

        // ------------------------------------------------
        // 4. ANNOUNCEMENTS (3 komada od admina)
        // ------------------------------------------------
        var adminUser = allAdmins.First();

        var announcements = new List<Announcements>
        {
            new Announcements
            {
                Title = "Dobrodošlica u dom",
                Content = "Dragi studenti, dobrodošli u studentski dom UniStay. Molimo vas da pročitate kućni red na oglasnoj ploči.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                VisibleTo = "Student",
                CreatedByUserID = adminUser.UserID
            },
            new Announcements
            {
                Title = "Noć sporta u sali doma",
                Content = "U petak organizujemo noć sporta u fiskulturnoj sali doma. Fudbal, košarka i odbojka od 19:00 do 23:00.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                VisibleTo = "Student",
                CreatedByUserID = adminUser.UserID
            },
            new Announcements
            {
                Title = "Radna akcija čišćenja",
                Content = "U subotu pravimo zajedničku akciju čišćenja zajedničkih prostorija. Okupljanje u 10:00 u holu doma.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                VisibleTo = "Student",
                CreatedByUserID = adminUser.UserID
            }
        };

        db.Announcement.AddRange(announcements);
        await db.SaveChangesAsync(ct);

        // Ostale tabele (Applications, Faults, Payments...) možeš naknadno dopuniti po potrebi.
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
