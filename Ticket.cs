public abstract class Ticket
{
    public int ticketID { get; set; }
    string _summary;
    public string summary
    {
        get
        {
            return this._summary;
        }
        set
        {
            this._summary = CorrectForCommas(value);
        }
    }
    string _status;
    public string status
    {
        get
        {
            return this._status;
        }
        set
        {
            this._status = CorrectForCommas(value);
        }
    }
    string _priority;
    public string priority
    {
        get
        {
            return this._priority;
        }
        set
        {
            this._priority = CorrectForCommas(value);
        }
    }    
    string _submitter;
    public string submitter
    {
        get
        {
            return this._submitter;
        }
        set
        {
            this._submitter = CorrectForCommas(value);
        }
    } 
    string _assigned;
    public string assigned
    {
        get
        {
            return this._assigned;
        }
        set
        {
            this._assigned = CorrectForCommas(value);
        }
    }
    public List<string> watching { get; set; }

    public Ticket()
    {
        watching = new List<string>();
    }

    public virtual string Display()
    {
        return $"ID: {ticketID}\nSummary: {summary}\nStatus: {status}\nPriority: {priority}\nSubmitter: {submitter}\nAssigned: {assigned}\nWatching: {string.Join(", ", watching)}\n";
    }

    // method to check for embedded commas and surround with quotes if necessary
    public string CorrectForCommas(string stringToCheck)
    {
        if (stringToCheck.Contains(','))
        {
            stringToCheck = "\"" + stringToCheck + "\"";
        }
        return stringToCheck;
    }
}

public class BugDefect : Ticket
{
    string _severity;
    public string severity
    {
        get
        {
            return this._severity;
        }
        set
        {
            this._severity = CorrectForCommas(value);
        }
    }
    public override string Display()
    {
        return $"ID: {ticketID}\nSummary: {summary}\nStatus: {status}\nPriority: {priority}\nSubmitter: {submitter}\nAssigned: {assigned}\nWatching: {string.Join(", ", watching)}\nSeverity: {severity}\n";
    }
}

public class Enhancement : Ticket{
    string _software;
    public string software
    {
        get
        {
            return this._software;
        }
        set
        {
            this._software = CorrectForCommas(value);
        }
    }
    public double cost { get; set; }
    string _reason;
    public string reason
    {
        get
        {
            return this._reason;
        }
        set
        {
            this._reason = CorrectForCommas(value);
        }
    }
    string _estimate;
    public string estimate
    {
        get
        {
            return this._estimate;
        }
        set
        {
            this._estimate = CorrectForCommas(value);
        }
    }
    public override string Display()
    {
        return $"ID: {ticketID}\nSummary: {summary}\nStatus: {status}\nPriority: {priority}\nSubmitter: {submitter}\nAssigned: {assigned}\nWatching: {string.Join(", ", watching)}\nSoftware: {software}\nCost: {cost}\nReason: {reason}\nEstimate: {estimate}\n";
    }
}
