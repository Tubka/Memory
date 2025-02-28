using System.Windows;
using Memory.Database;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Memory.Services;

public partial class DatabaseServices
{
    public static bool CheckNameAlreadyExist(string name)
    {
        using (var context = new AppDbContext()) // Użycie EF DbContext
        {
            var existingCompany = context.Companies
                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            return existingCompany != null;
        }
    }

    public static List<MonthlyCheckbox> GetMonthlyCheckboxes(int companyId, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            var monthlyCheckboxes = context.MonthlyCheckboxes
                .Include(m => m.CheckboxTemplate) // Zawiera powiązany Template
                .Where(m => m.CompanyId == companyId && m.Date.Month == date.Month && m.Date.Year == date.Year)
                .ToList();


            return monthlyCheckboxes;
        }
    }

    public static Company AddCompany(string name)
    {
        using (var context = new AppDbContext())
        {
            if (context.Companies.Any(c => c.Name == name))
            {
                return null;
            }

            var company = new Company { Name = name };
            context.Companies.Add(company);
            context.SaveChanges();
            return company;
        }
    }

    public static List<Company> GetAllCompanies()
    {
        using (var context = new AppDbContext())
        {
            return context.Companies.ToList();
        }
    }

    public static CheckboxTemplate? AddCheckboxTemplate(int companyId, string templateName, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            // Sprawdzenie, czy firma istnieje
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                MessageBox.Show("Firma o podanym ID nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            // Sprawdzenie, czy checkbox o tej samej nazwie już istnieje dla firmy
            if (context.CheckboxTemplates.Any(ct => ct.CompanyId == companyId && ct.TemplateName == templateName))
            {
                MessageBox.Show("Checkbox o podanej nazwie już istnieje dla tej firmy.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            var checkboxTemplate = new CheckboxTemplate
            {
                CompanyId = companyId,
                TemplateName = templateName,
                CreatedAt = date,
                Order = 1
            };

            context.CheckboxTemplates.Add(checkboxTemplate);
            context.SaveChanges();
            return checkboxTemplate; // Zwracamy nowo dodany checkbox
        }
    }

    public static void AddCheckboxesIfNotExist(int companyId, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                throw new Exception("Firma o podanym ID nie istnieje.");
            }
            // Pobierz wszystkie szablony checkboxów dla danej firmy, które powinny być aktywne w danym miesiącu
            var templateIds = context.CheckboxTemplates
                .Where(t => t.CompanyId == companyId && 
                            t.CreatedAt.Year <= date.Year && t.CreatedAt.Month <= date.Month)
                .Select(t => t.Id)
                .ToList();

            if (!templateIds.Any())
            {
                return;
            }
            // Pobierz już istniejące checkboxy dla danego miesiąca i roku
            var existingCheckboxes = context.MonthlyCheckboxes
                .Where(mc => mc.CompanyId == companyId && mc.Date.Year == date.Year && mc.Date.Month == date.Month)
                .Select(mc => mc.TemplateId)
                .ToList();

            // Znajdź szablony, które nie mają jeszcze odpowiedniego MonthlyCheckbox w tym miesiącu
            var missingCheckboxes = templateIds
                .Where(tid => !existingCheckboxes.Contains(tid)) // Zapobiega duplikatom
                .Select(tid => new MonthlyCheckbox
                {
                    CompanyId = companyId,
                    TemplateId = tid,
                    Date = new DateTime(date.Year, date.Month, 1), // Zabezpieczenie - ustawiamy zawsze pierwszy dzień miesiąca
                    IsChecked = false
                })
                .ToList();

            if (missingCheckboxes.Any())
            {
                context.MonthlyCheckboxes.AddRange(missingCheckboxes);
                context.SaveChanges();
                MessageBox.Show("✅ Dodano nowe checkboxy!");
            }
            else
            {
                // MessageBox.Show("✅ Nie dodano nowych checkboxów – wszystkie są już w bazie.");
            }
        }
    }


    public static void AddDescription(int companyId, string text)
    {
        using (var context = new AppDbContext())
        {
            // Sprawdzenie, czy firma istnieje
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                throw new Exception("Firma o podanym ID nie istnieje.");
            }

            var description = new Description
            {
                CompanyId = companyId,
                Text = text,
                CreatedAt = DateTime.Now // Ustawienie aktualnej daty i czasu
            };

            context.Descriptions.Add(description);
            context.SaveChanges();

        }
    }
    public static Description GetLatestDescription(int companyId)
    {
        using (var context = new AppDbContext())
        {
            // Pobranie najnowszego wpisu dla danej firmy
            var latestDescription = context.Descriptions
                .Where(d => d.CompanyId == companyId)
                .OrderByDescending(d => d.CreatedAt) // Sortowanie malejące po dacie
                .FirstOrDefault();

            if (latestDescription == null)
            {
                throw new Exception($"Brak opisu dla firmy ID: {companyId}");
            }

            return latestDescription;
        }
    }
    public static MonthDescription AddMonthDescription(int companyId, int userId, string description, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                MessageBox.Show("Firma o podanym ID nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                MessageBox.Show("Użytkownik o podanym ID nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            var lastMonthDescription = context.MonthDescriptions
                .Where(md => md.CompanyId == companyId && md.Date.Year == date.Year && md.Date.Month == date.Month)
                .OrderByDescending(md => md.CreatedAt)  // Upewniamy się, że bierzemy ostatni zapis
                .FirstOrDefault();
            if (lastMonthDescription != null && lastMonthDescription.Description == description)
            {
                MessageBox.Show("Podany opis jest taki sam jak ostatni wpis. Nie dodano nowego opisu.", "Brak zmian", MessageBoxButton.OK, MessageBoxImage.Information);
                return null; // Zwracamy null, jeśli opis jest taki sam
            }

            var monthDescription = new MonthDescription
            {
                CompanyId = companyId,
                Date = new DateTime(date.Year, date.Month, 1), // Tylko miesiąc i rok
                Description = description,
                CreatedAt = DateTime.Now,
                UserId = userId  // Ustalamy użytkownika, który dodał opis
            };

            try
            {
                context.MonthDescriptions.Add(monthDescription);
                context.SaveChanges();
                MessageBox.Show("Opis zapisano pomyślnie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                return monthDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd przy zapisywaniu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }

    public static FuelDescription AddFuelDescription(int companyId, int userId, string description, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            var company = context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                MessageBox.Show("Firma o podanym ID nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                MessageBox.Show("Użytkownik o podanym ID nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }

            // var lastFuelDescription = context.FuelDescriptions
            //     .Where(md => md.CompanyId == companyId && Date.CompareDate(md.Date, date))
            //     .OrderByDescending(md => md.CreatedAt)  // Upewniamy się, że bierzemy ostatni zapis
            //     .FirstOrDefault();

            var Fuel2Description = new FuelDescription
            {
                CompanyId = companyId,
                Date = new DateTime(date.Year, date.Month, 1), // Tylko miesiąc i rok
                Description = description,
                CreatedAt = DateTime.Now,
                UserId = userId  // Ustalamy użytkownika, który dodał opis
            };
                context.FuelDescriptions.Add(Fuel2Description);
                context.SaveChanges();


            return Fuel2Description;
        }
    }
    public static FuelDescription? GetFuelDescription(int companyId, DateTime date)
    {
        using (var context = new AppDbContext())
        {
            var lastFuelDescription = context.FuelDescriptions
                .Where(md => md.CompanyId == companyId && md.Date.Year == date.Year && md.Date.Month == date.Month)
                .OrderByDescending(md => md.CreatedAt)
                .FirstOrDefault();

            return lastFuelDescription;
        }
    }


}

