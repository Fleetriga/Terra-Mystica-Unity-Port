using System;

[Serializable]
public class ProfileHistory
{
    private string lastPlayerProfile;
    public string LastPlayerProfile { get { return lastPlayerProfile; } set { lastPlayerProfile = value; } }

	public ProfileHistory()
	{

	}
    public ProfileHistory(string lastprofile)
    {
        lastPlayerProfile = lastprofile;
    }
}
