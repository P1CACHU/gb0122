using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string username => _username;
    public string mail => _mail;
    public string pass => _pass;

    private string _username;
    private string _mail;
    private string _pass;
    
    public void UpdateUsername(string newUsername)
    {
        _username = newUsername;
    }

    public void UpdateEmail(string newMail)
    {
        _mail = newMail;
    }

    public void UpdatePass(string newPass)
    {
        _pass = newPass;
    }
}