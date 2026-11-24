using Gurrea.Services;


namespace Gurrea.Menus
{
    internal class ReportMenu
    {
        private readonly PersonService personService;

        private readonly ReportService reportService;

        public ReportMenu(PersonService personService)
        {
            this.personService = personService;
            reportService = new ReportService();
        }

        public void Run()
        {
            Console.Clear();

            string report = reportService.GenerateReport(
                personService.GetAllPeople());

            Console.WriteLine(report);
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }
    }
}