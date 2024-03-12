using NLog;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
string ticketFilePath = Directory.GetCurrentDirectory() + "\\tickets.csv";

logger.Info("Program started");

TicketFile ticketFile = new TicketFile(ticketFilePath);

string choice = "";
do
{
    Console.WriteLine("1) Add Ticket");
    Console.WriteLine("2) Display Tickets");
    Console.WriteLine("Enter to quit");

    choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);

    if (choice == "1")
    {
        // TODO: add ticket
    }
    else if (choice == "2")
    {
        foreach(Ticket t in ticketFile.Tickets)
        {
            Console.WriteLine(t.Display());
        }
    }
} while (choice == "1" || choice == "2");

logger.Info("Program ended");
