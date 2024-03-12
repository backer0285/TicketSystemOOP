using Microsoft.VisualBasic.FileIO;
using NLog;
public class TicketFile
{
    public string filePath { get; set; }
    public List<Ticket> Tickets { get; set; }

    public TicketFile(string ticketFilePath)
    {
        filePath = ticketFilePath;
        NLog.Logger logger = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

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
}
