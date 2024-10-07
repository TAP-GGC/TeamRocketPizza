[System.Serializable]
public class EmailElement
{
    public string Text;     // The content of the email element
    public bool IsPhishing; // Whether this element is a phishing attempt
    public string elementMessage; // The message to display when the element is clicked, this is to explain to the user why this object is phishing or not.

    public EmailElement(string text, bool isPhishing, string elementMessage)
    {
        this.Text = text;
        this.IsPhishing = isPhishing;
        this.elementMessage = elementMessage;
    }
    
}