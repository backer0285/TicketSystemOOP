using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using NLog;
public class TicketFile
{
    public string filePath { get; set; }
    public List<Ticket> Tickets { get; set; }
    private static NLog.Logger logger = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

    public TicketFile(string ticketFilePath)
    {
        filePath = ticketFilePath;
        Tickets = new List<Ticket>();

        try
        {
            StreamReader sr = new StreamReader(filePath);
            // skip first line due to column headers, toggle on if needed
            // sr.ReadLine();
            int lineNumber = 1;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                // handles comma embedded in a field bounded by quotes
                TextFieldParser parser = new TextFieldParser(new StringReader(line));
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");
                string[] ticketDetails = parser.ReadFields();

                // determines type of ticket, not very scaleable in present implementation
                if (ticketDetails.Length == 8)
                {
                    BugDefect ticket = WriteBugDefect(ticketDetails, lineNumber);
                    Tickets.Add(ticket);
                }
                else if (ticketDetails.Length == 11)
                {
                    Enhancement ticket = WriteEnhancement(ticketDetails, lineNumber);
                    Tickets.Add(ticket);
                }
                else if (ticketDetails.Length == 9)
                {
                    Task ticket = WriteTask(ticketDetails, lineNumber);
                    Tickets.Add(ticket);
                }
                else
                {
                    Console.WriteLine("Incorrect number of fields on entry: " + lineNumber);
                    logger.Error("Incorrect number of fields on entry: " + lineNumber);
                }

                lineNumber++;
            }
            sr.Close();
            logger.Info("Tickets in file {Count}", Tickets.Count);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    public bool isValidSummary(string summary)
    {
        if (Tickets.ConvertAll(t => t.summary.ToLower()).Contains(summary.ToLower()))
        {
            logger.Info("Duplicate ticket summary {Summary}", summary);
            return false;
        }
        if (summary == "")
        {
            logger.Info("Blank summary submitted.");
            return false;
        }
        return true;
    }

    public void AddBugDefect(BugDefect ticket)
    {
        try
        {
            if (Tickets.Count > 0)
            {
                ticket.ticketID = Tickets.Max(t => t.ticketID) + 1;
            }
            else
            {
                ticket.ticketID = 1;
            }
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{ticket.ticketID},{ticket.summary},{ticket.status},{ticket.priority},{ticket.submitter},{ticket.assigned},{string.Join("|", ticket.watching)},{ticket.severity}");
            sw.Close();
            Tickets.Add(ticket);
            logger.Info("Ticket ID {ID} added", ticket.ticketID);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    public void AddEnhancement(Enhancement ticket)
    {
        try
        {
            if (Tickets.Count > 0)
            {
                ticket.ticketID = Tickets.Max(t => t.ticketID) + 1;
            }
            else
            {
                ticket.ticketID = 1;
            }
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{ticket.ticketID},{ticket.summary},{ticket.status},{ticket.priority},{ticket.submitter},{ticket.assigned},{string.Join("|", ticket.watching)},{ticket.software},{ticket.cost},{ticket.reason},{ticket.estimate}");
            sw.Close();
            Tickets.Add(ticket);
            logger.Info("Ticket ID {ID} added", ticket.ticketID);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    public void AddTask(Task ticket)
    {
        try
        {
            if (Tickets.Count > 0)
            {
                ticket.ticketID = Tickets.Max(t => t.ticketID) + 1;
            }
            else
            {
                ticket.ticketID = 1;
            }
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{ticket.ticketID},{ticket.summary},{ticket.status},{ticket.priority},{ticket.submitter},{ticket.assigned},{string.Join("|", ticket.watching)},{ticket.projectName},{ticket.dueDate}");
            sw.Close();
            Tickets.Add(ticket);
            logger.Info("Ticket ID {ID} added", ticket.ticketID);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }

    public BugDefect WriteBugDefect(string[] ticketDetails, int lineNumber)
    {
        BugDefect ticket = new BugDefect();
        ticket = (BugDefect)WritePreliminaryTicket(ticket, ticketDetails);
        ticket.severity = ticketDetails[7];
        return ticket;
    }

    public Enhancement WriteEnhancement(string[] ticketDetails, int lineNumber)
    {
        Enhancement ticket = new Enhancement();
        ticket = (Enhancement)WritePreliminaryTicket(ticket, ticketDetails);
        ticket.software = ticketDetails[7];
        if (Double.TryParse(ticketDetails[8], out double cost))
        {
            ticket.cost = cost;
        }
        else
        {
            Console.WriteLine("Incorrectly formatted cost value: " + ticketDetails[8] + " on entry: " + lineNumber);
            logger.Error("Incorrectly formatted cost value: " + ticketDetails[8] + " on entry: " + lineNumber);
        }
        ticket.reason = ticketDetails[9];

        if (TimeSpan.TryParse(ticketDetails[10], out TimeSpan estimate))
        {
            ticket.estimate = estimate;
        }
        else
        {
            Console.WriteLine("Incorrectly formatted estimate time value: " + ticketDetails[10] + " on entry: " + lineNumber);
            logger.Error("Incorrectly formatted estimate time value: " + ticketDetails[10] + " on entry: " + lineNumber);
        }
        return ticket;
    }

    public Task WriteTask(string[] ticketDetails, int lineNumber)
    {
        Task ticket = new Task();
        ticket = (Task)WritePreliminaryTicket(ticket, ticketDetails);
        ticket.projectName = ticketDetails[7];
        if (DateOnly.TryParse(ticketDetails[8], out DateOnly dueDate))
        {
            ticket.dueDate = dueDate;
        }
        else
        {
            Console.WriteLine("Incorrectly formatted date value: " + ticketDetails[8] + " on entry: " + lineNumber);
            logger.Error("Incorrectly formatted date value: " + ticketDetails[8] + " on entry: " + lineNumber);
        }
        return ticket;
    }

    // info all tickets have in common
    public Ticket WritePreliminaryTicket(Ticket ticket, string[] ticketDetails)
    {
        ticket.ticketID = int.Parse(ticketDetails[0]);
        ticket.summary = ticketDetails[1];
        ticket.status = ticketDetails[2];
        ticket.priority = ticketDetails[3];
        ticket.submitter = ticketDetails[4];
        ticket.assigned = ticketDetails[5];
        ticket.watching = ticketDetails[6].Split('|').ToList();
        return ticket;
    }
}
