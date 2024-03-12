public class Ticket
{
    public int ticketID { get; set; }
    public string summary { get; set; }
    public string status { get; set; }
    public string priority { get; set; }
    public string submitter { get; set; }
    public string assigned { get; set; }
    public List<string> watching { get; set; }

    public Ticket()
    {
        watching = new List<string>();
    }

    public string Display()
    {
        return $"ID: {ticketID}\nSummary: {summary}\nStatus: {status}\nPriority: {priority}\nSubmitter: {submitter}\nAssigned: {assigned}\nWatching: {string.Join(", ", watching)}\n";
    }
}
