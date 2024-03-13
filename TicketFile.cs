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
            // skip first line due to column headers
            sr.ReadLine();

            while (!sr.EndOfStream)
            {
                Ticket ticket = new Ticket();
                string line = sr.ReadLine();

                // handles comma embedded in a field bounded by quotes
                TextFieldParser parser = new TextFieldParser(new StringReader(line));
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                string[] ticketDetails = parser.ReadFields();
                ticket.ticketID = int.Parse(ticketDetails[0]);
                ticket.summary = ticketDetails[1];
                ticket.status = ticketDetails[2];
                ticket.priority = ticketDetails[3];
                ticket.submitter = ticketDetails[4];
                ticket.assigned = ticketDetails[5];
                ticket.watching = ticketDetails[6].Split('|').ToList();

                Tickets.Add(ticket);
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

    public void AddTicket(Ticket ticket)
    {
        try
        {
            ticket.ticketID = Tickets.Max(t => t.ticketID) + 1;
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{ticket.ticketID},{ticket.summary},{ticket.status},{ticket.priority},{ticket.submitter},{ticket.assigned},{string.Join("|",ticket.watching)}");
            sw.Close();
            Tickets.Add(ticket);
            logger.Info("Ticket ID {ID} added", ticket.ticketID);
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
        }
    }
}
