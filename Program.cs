using NLog;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
string ticketFilePath = Directory.GetCurrentDirectory() + "\\tickets.csv";

logger.Info("Program started");

Ticket ticket = new Ticket
{
    ticketID = 1,
    summary = "This is a bug ticket",
    status = "Open",
    priority = "High",
    submitter = "Drew Kjell",
    assigned = "John Smith",
    watching = new List<string> { "Drew Kjell", "John Smith", "Bill Jones" }
};
Console.WriteLine(ticket.Display());
TicketFile ticketFile = new TicketFile(ticketFilePath);

logger.Info("Program ended");
