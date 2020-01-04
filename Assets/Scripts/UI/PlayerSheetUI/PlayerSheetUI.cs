using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSheetUI : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] Text[] moneyWorkerPriest;
    [SerializeField] Text[] magicTiers;
    [SerializeField] Text points;
    [SerializeField] TextMeshProUGUI playerName;

    public void RegularUpdate(PlayerStatistics ps)
    {
        //update with PlayerStatistics
        moneyWorkerPriest[0].text = ps.PlayerGold.ToString();
        moneyWorkerPriest[1].text = ps.PlayerWorkers.ToString();
        moneyWorkerPriest[2].text = ps.PlayerPriests.ToString();
    }

    public void TierMagicUpdate(PlayerStatistics ps)
    {
        magicTiers[0].text = ps.tierOne.ToString();
        magicTiers[1].text = ps.tierTwo.ToString();
        magicTiers[2].text = ps.tierThree.ToString();
    }

    public void PointsUpdate(PlayerStatistics ps)
    {
        points.text = ps.PlayerPoints.ToString();
    }

    public void ActionUpdate()
    {

    }

    public void SetUp(PlayerStatistics ps)
    {
        playerName.text = ps.GetComponent<PlayerNetworked>().ProfileName;
        portrait.sprite = GetComponent<FactionImageHolder>().FactionImages[(int)ps.GetComponent<PlayerNetworked>().PlayerFaction];
        RegularUpdate(ps);
        PointsUpdate(ps);
        TierMagicUpdate(ps);
    }
}
