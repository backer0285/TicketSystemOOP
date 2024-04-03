﻿// have it display a default value for empty strings (ie NA or Unassigned)
// negative cost

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
    Console.WriteLine("3) Search Tickets");
    Console.WriteLine("Enter to quit");

    choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);

    if (choice == "1")
    {
        Console.WriteLine("1) Bug/Defect Ticket");
        Console.WriteLine("2) Enhancement Ticket");
        Console.WriteLine("3) Task Ticket");
        string menuChoice = Console.ReadLine();
        if (menuChoice == "1")
        {
            BugDefect ticket = new BugDefect();
            ticket = (BugDefect)AddPreliminaryTicket(ticket);

            Console.WriteLine("Enter severity");
            ticket.severity = Console.ReadLine();
            ticketFile.AddBugDefect(ticket);
        }
        else if (menuChoice == "2")
        {
            Enhancement ticket = new Enhancement();
            ticket = (Enhancement)AddPreliminaryTicket(ticket);

            Console.WriteLine("Enter software");
            ticket.software = Console.ReadLine();
            Console.WriteLine("Enter cost (decimal format)");
            string input = Console.ReadLine();
            input = input.Replace("$", "");
            input = input.Replace(",", "");
            if (Double.TryParse(input, out double cost))
            {
                ticket.cost = cost;
            }
            else
            {
                Console.WriteLine("Incorrectly formatted cost value: " + input + ".");
                logger.Error("Incorrectly formatted cost value: " + input + ".");
                break;
            }
            Console.WriteLine("Enter reason");
            ticket.reason = Console.ReadLine();
            Console.WriteLine("Enter estimate (hh:mm format)");
            input = Console.ReadLine();
            if (TimeSpan.TryParse(input, out TimeSpan estimate))
            {
                ticket.estimate = estimate;
            }
            else
            {
                Console.WriteLine("Incorrectly formatted estimate time value: " + input + ".");
                logger.Error("Incorrectly formatted estimate time value: " + input + ".");
                break;
            }
            ticketFile.AddEnhancement(ticket);
        }
        else if (menuChoice == "3")
        {
            Task ticket = new Task();
            ticket = (Task)AddPreliminaryTicket(ticket);

            Console.WriteLine("Enter project name");
            ticket.projectName = Console.ReadLine();
            Console.WriteLine("Enter due date (mm/dd/yyyy format)");
            string input = Console.ReadLine();
            if (DateOnly.TryParse(input, out DateOnly dueDate))
            {
                ticket.dueDate = dueDate;
            }
            else
            {
                Console.WriteLine("Incorrectly formatted date value: " + input + ".");
                logger.Error("Incorrectly formatted date value: " + input + ".");
                break;
            }
            ticketFile.AddTask(ticket);
        }
    }
    else if (choice == "2")
    {
        foreach (Ticket t in ticketFile.Tickets)
        {
            Console.WriteLine(t.Display());
        }
    }
    else if (choice == "3")
    {
        Console.WriteLine("1) Search by Status");
        Console.WriteLine("2) Search by Priority");
        Console.WriteLine("3) Search by Submitter");
        string searchMenuChoice = Console.ReadLine();
        string searchParameter = "";
        if (searchMenuChoice == "1")
        {
            while (searchParameter == "")
            {
                Console.WriteLine("Enter status to search");
                searchParameter = Console.ReadLine();
            }
            var searchResults = ticketFile.Tickets.Where(t => t.status.Contains(searchParameter, StringComparison.OrdinalIgnoreCase)).Select(t => t);
            Console.WriteLine();
            Console.WriteLine($"There are {searchResults.Count()} ticket(s) with \"{searchParameter}\" in the status:");
            Console.WriteLine();
            foreach (Ticket t in searchResults)
            {
                Console.WriteLine(t.Display());
            }
        }
        else if (searchMenuChoice == "2")
        {
            while (searchParameter == "")
            {
                Console.WriteLine("Enter priority to search");
                searchParameter = Console.ReadLine();
            }
            var searchResults = ticketFile.Tickets.Where(t => t.priority.Contains(searchParameter, StringComparison.OrdinalIgnoreCase)).Select(t => t);
            Console.WriteLine();
            Console.WriteLine($"There are {searchResults.Count()} ticket(s) with \"{searchParameter}\" in the priority:");
            Console.WriteLine();
            foreach (Ticket t in searchResults)
            {
                Console.WriteLine(t.Display());
            }
        }
        else if (searchMenuChoice == "3")
        {
            while (searchParameter == "")
            {
                Console.WriteLine("Enter submitter to search");
                searchParameter = Console.ReadLine();
            }
            var searchResults = ticketFile.Tickets.Where(t => t.submitter.Contains(searchParameter, StringComparison.OrdinalIgnoreCase)).Select(t => t);
            Console.WriteLine();
            Console.WriteLine($"There are {searchResults.Count()} ticket(s) with \"{searchParameter}\" in the submitter:");
            Console.WriteLine();
            foreach (Ticket t in searchResults)
            {
                Console.WriteLine(t.Display());
            }
        }
    }
} while (choice == "1" || choice == "2" || choice == "3");

logger.Info("Program ended");

// info all tickets have in common
Ticket AddPreliminaryTicket(Ticket ticket)
{
    Console.WriteLine("Enter ticket summary");
    ticket.summary = Console.ReadLine();
    if (ticketFile.isValidSummary(ticket.summary))
    {
        Console.WriteLine("Enter ticket status");
        ticket.status = Console.ReadLine();
        if (ticket.status == "")
        {
            ticket.status = "(no status attached to ticket)";
        }
        Console.WriteLine("Enter ticket priority");
        ticket.priority = Console.ReadLine();
        if (ticket.priority == "")
        {
            ticket.priority = "(no priority attached to ticket)";
        }
        Console.WriteLine("Enter ticket submitter");
        ticket.submitter = Console.ReadLine();
        if (ticket.submitter == "")
        {
            ticket.submitter = "(no submitter attached to ticket)";
        }
        Console.WriteLine("Enter the assigned handler");
        ticket.assigned = Console.ReadLine();
        if (ticket.assigned == "")
        {
            ticket.assigned = "(no one assigned to ticket)";
        }

        string input;
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
    }

    return ticket;
}