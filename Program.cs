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
        Ticket ticket = new Ticket();
        Console.WriteLine("Enter ticket summary");
        ticket.summary = Console.ReadLine();
        if (ticketFile.isUniqueSummary(ticket.summary))
        {
            string input;
            // TODO: handle blank entry

            Console.WriteLine("Enter ticket status");
            ticket.status = Console.ReadLine();
            Console.WriteLine("Enter ticket priority");
            ticket.priority = Console.ReadLine();
            Console.WriteLine("Enter ticket submitter");
            ticket.submitter = Console.ReadLine();
            Console.WriteLine("Enter the assigned handler");
            ticket.assigned = Console.ReadLine();

            do
            {
                Console.WriteLine("Enter a person watching this ticket (or done to quit)");
                input = Console.ReadLine();

                if (input != "done" && input.Length > 0)
                {
                    ticket.watching.Add(input);
                }
            } while (input != "done");

            if (ticket.watching.Count == 0)
            {
                ticket.watching.Add("(no watchers attached to ticket)");
            }

            ticketFile.AddTicket(ticket);
        }
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
