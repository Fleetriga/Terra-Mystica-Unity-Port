using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    Profile playerProfile;
    ProfileHistory profileHistory;
    [SerializeField] TMP_InputField newProfileName;

    string PATH;

    public Profile PlayerProfile { get { return playerProfile; } set { playerProfile = value; } }
    public ProfileHistory ProfileHist { get { return profileHistory; } set { profileHistory = value; } }

    private void Awake()
    {
        PATH = Application.persistentDataPath;
        LoadHistory();
        LoadProfile();
    }

    void SaveHistory()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = new FileStream(PATH + "History.dat", FileMode.Create);
            bf.Serialize(file, profileHistory);
        }
        catch (Exception e)
        {
            Debug.Log("SaveHistory() failed");
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }

    }

    void LoadHistory()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(PATH + "History.dat", FileMode.Open);
            profileHistory = (ProfileHistory)bf.Deserialize(file);
        }
        catch (Exception e)
        {
            Debug.Log("LoadHistory() failed");

            //Possible failed because it doesnt exist. If so create default
            CreateDefaultProfile();
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void LoadProfile()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(PATH + profileHistory.LastPlayerProfile + ".dat", FileMode.Open);
            playerProfile = (Profile)bf.Deserialize(file);
        }
        catch (Exception e)
        {
            Debug.Log("LoadProfile() failed");
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void SaveProfile(Profile p)
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = new FileStream(PATH+p.ProfileName+".dat", FileMode.Create);

            bf.Serialize(file, p);

            profileHistory.LastPlayerProfile = p.ProfileName;
            SaveHistory();
        }
        catch (Exception e)
        {
            Debug.Log("SaveProfile() failed");
            Debug.Log(e.ToString());
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void CreateDefaultProfile()
    {
        playerProfile = new Profile(0, "New Player" + UnityEngine.Random.Range(1, 10));
        SaveProfile(playerProfile);

        profileHistory = new ProfileHistory(playerProfile.ProfileName);
        SaveHistory();
    }

    public void CreateNewProfile()
    {
        playerProfile = new Profile(0, newProfileName.text);
        SaveProfile(playerProfile);
    }
}
