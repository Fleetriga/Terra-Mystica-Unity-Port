using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Profile
{
    private int totalWins;
    private string profileName;

    public int TotalWins { get { return totalWins; } set { totalWins = value; } }
    public string ProfileName { get { return profileName; } set { profileName = value; } }


    public Profile(int wins, string _name)
    {
        TotalWins = wins;
        ProfileName = _name;
    }

    public Profile()
    {

    }

}
