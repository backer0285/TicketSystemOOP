using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
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

logger.Info("Program ended");
