[System.Serializable]
public class Emailv2
{
    public string Subject;
    public string SenderEmail;
    public string SenderName;
    public string Body;
    public bool IsPhishing;
    public string phishingExplanation;

    public Emailv2(string subject, string senderEmail, string senderName, string body, bool isPhishing, string phishingExplanation)
    {
        this.Subject = subject;
        this.SenderEmail = senderEmail;
        this.SenderName = senderName;
        this.Body = body;
        this.IsPhishing = isPhishing;
        this.phishingExplanation = phishingExplanation;
    }

    
    
}