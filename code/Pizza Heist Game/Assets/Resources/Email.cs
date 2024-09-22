[System.Serializable]
public class Email
{
    public EmailElement Subject;
    public EmailElement SenderEmail;
    public EmailElement SenderName;
    public EmailElement Body;
    public EmailElement Link;

    // You can add more elements like links, attachments, etc.
    public Email(EmailElement subject, EmailElement senderEmail, EmailElement senderName, EmailElement body, EmailElement link)
    {
        Subject = subject;
        SenderEmail = senderEmail;
        SenderName = senderName;
        Body = body;
        Link = link;
    }
    
}