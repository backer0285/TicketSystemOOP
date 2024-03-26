using NLog;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
string ticketFilePath = Directory.GetCurrentDirectory() + "\\tickets.csv";

logger.Info("Program started");

BugDefect bd = new BugDefect
{
    ticketID = 100,
    summary = "index out of bounds error",
    status = "open",
    priority = "medium",
    submitter = "me",
    assigned = "you",
    watching = {"me", "them", "us"},
    severity = "low"
};

Console.WriteLine(bd.Display());

// TicketFile ticketFile = new TicketFile(ticketFilePath);

// string choice = "";
// do
// {
//     Console.WriteLine("1) Add Ticket");
//     Console.WriteLine("2) Display Tickets");
//     Console.WriteLine("Enter to quit");

//     choice = Console.ReadLine();
//     logger.Info("User choice: {Choice}", choice);

//     if (choice == "1")
//     {
//         Ticket ticket = new Ticket();
//         Console.WriteLine("Enter ticket summary");
//         ticket.summary = Console.ReadLine();
//         if (ticketFile.isValidSummary(ticket.summary))
//         {
//             Console.WriteLine("Enter ticket status");
//             ticket.status = Console.ReadLine();
//             if (ticket.status == "")
//             {
//                 ticket.status = "(no status attached to ticket)";
//             }
//             Console.WriteLine("Enter ticket priority");
//             ticket.priority = Console.ReadLine();
//             if (ticket.priority == "")
//             {
//                 ticket.priority = "(no priority attached to ticket)";
//             }
//             Console.WriteLine("Enter ticket submitter");
//             ticket.submitter = Console.ReadLine();
//             if (ticket.submitter == "")
//             {
//                 ticket.submitter = "(no submitter attached to ticket)";
//             }
//             Console.WriteLine("Enter the assigned handler");
//             ticket.assigned = Console.ReadLine();
//             if (ticket.assigned == "")
//             {
//                 ticket.assigned = "(no one assigned to ticket)";
//             }

//             string input;
//             do
//             {
//                 Console.WriteLine("Enter a person watching this ticket (or done to quit)");
//                 input = Console.ReadLine();

//                 if (input != "done" && input.Length > 0)
//                 {
//                     ticket.watching.Add(input);
//                 }
//             } while (input != "done");

//             if (ticket.watching.Count == 0)
//             {
//                 ticket.watching.Add("(no watchers attached to ticket)");
//             }

//             ticketFile.AddTicket(ticket);
//         }
//     }
//     else if (choice == "2")
//     {
//         foreach (Ticket t in ticketFile.Tickets)
//         {
//             Console.WriteLine(t.Display());
//         }
//     }
// } while (choice == "1" || choice == "2");

logger.Info("Program ended");
