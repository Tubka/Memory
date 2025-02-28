using System.Windows.Controls;
using System.Windows.Data;

namespace Memory.Services
{
    public partial class CompanySelectionService 
    {
        // Lista danych do wyświetlenia w ComboBox
        public static void AddItemsToComboBox(ComboBox companyList)
        {
            var Companies = DatabaseServices.GetAllCompanies();
            // Tworzenie widoku kolekcji z filtrem
            if (Companies is null)
            {
                return;
            }

            // Użycie CollectionViewSource, aby stworzyć widok z filtrami
            var CompaniesView = CollectionViewSource.GetDefaultView(Companies);

            // Czyszczenie istniejących elementów w ComboBox
            companyList.Items.Clear();

            // Dodawanie nowych elementów do ComboBox
            foreach (var company in Companies)
            {
                companyList.Items.Add(company);
            }
        }

    }
}
