[System.Serializable]
public class EmailElement
{
    public string Text;     // The content of the email element
    public bool IsPhishing; // Whether this element is a phishing attempt

    public EmailElement(string text, bool isPhishing)
    {
        Text = text;
        IsPhishing = isPhishing;
    }
}