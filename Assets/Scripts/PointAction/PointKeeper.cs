using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointKeeper {

    List<PointBonus> reactive_temp;
    List<PointBonus> reactive_perm;
    List<PointBonus> accumulative_temp;
    List<PointBonus> accumulative_perm;

    public List<PointBonus> Accumulative_temp
    {
        get { return accumulative_temp;  }
        set { accumulative_temp = value; }
    }
    public List<PointBonus> Accumulative_perm
    {
        get { return accumulative_perm; }
        set { accumulative_perm = value; }
    }

    public PointKeeper()
    {
        reactive_perm = new List<PointBonus>();
        reactive_temp = new List<PointBonus>();
        accumulative_perm = new List<PointBonus>();
        accumulative_temp = new List<PointBonus>();
    }

    public void ResetTempBonuses()
    {
        reactive_temp = new List<PointBonus>();
        accumulative_temp = new List<PointBonus>();
    }

    PointBonus ContainsBonus(List<PointBonus> bonus_list, PointBonus.Action_Type at)
    {
        foreach (PointBonus pb in bonus_list)
        {
            if (pb.Equals(at)) { return pb; }
        }
        return null;
    }

    /**
     * Sends in point bonus vars, parses them and sends them down the pipeline
     */
    public void AddPointBonus(PointBonus pb_, bool temporary)
    {
        List<PointBonus> temp;
        if (PointBonus.CheckReactive(pb_.GetActionType()))
        {
            if (temporary) { temp = reactive_temp; } else { temp = reactive_perm; };
            AddBonus(pb_, temp);
        }
        else
        {
            if (temporary) { temp = accumulative_temp; } else { temp = accumulative_perm; };
            AddBonus(pb_, temp);
        }
    }

    /**
     * If a bonus exists in the list then just increase the amount of points recieved. Otherwise create a new entry.
     */
    void AddBonus(PointBonus pb_, List<PointBonus> list)
    {
        PointBonus temp = ContainsBonus(list, pb_.GetActionType());
        if (temp != null)
        {
            temp.AddPointBonus(pb_.pointBonus);
        }
        else
        {
            list.Add(pb_);
        }
    }

    public int GetPointBonus(PointBonus.Action_Type at_)
    {
        int totalPointBonus = 0;
        PointBonus temp;

        if (PointBonus.CheckReactive(at_)) //If the action type is reactive only bother checking the reactive type lists.
        {
            temp = ContainsBonus(reactive_temp, at_);
            if (temp != null) { totalPointBonus += temp.pointBonus; }

            temp = ContainsBonus(reactive_perm, at_);
            if (temp != null) { totalPointBonus += temp.pointBonus; }
        }
        else //Else it's accumulative so only check those list types.
        {
            temp = ContainsBonus(accumulative_temp, at_);
            if (temp != null) { totalPointBonus += temp.pointBonus; }

            temp = ContainsBonus(accumulative_perm, at_);
            if (temp != null) { totalPointBonus += temp.pointBonus; }
        }

        return totalPointBonus;
    }


}
