using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {

    public enum SpellType { Priest, Worker, Gold, World_Bridge, World_Priest, World_Worker, World_Gold, World_Terraform1, World_Terraform2, NOTHING, BURN};

    Spell[] spellList;
    List<SpellType> usedWorldMagic;

    public MagicController()
    {
        usedWorldMagic = new List<SpellType>();
    }

    public bool CheckWorldMagicUsed(SpellType st_)
    {
        foreach (SpellType st in usedWorldMagic)
        {
            if (Equals(st, st_)) { return true; }
        }

        return false;
    }

    public void DisableWorldMagic(SpellType st_)
    {
        usedWorldMagic.Add(st_);
    }

    public void SetUpSpells()
    {
        spellList = new Spell[] {
            new Spell(0, 0, 1, 0, 5, 0), //1 priest
            new Spell(0, 1, 0, 0, 3, 0), //1 worker
            new Spell(1, 0, 0, 0, 1, 0), //1 gold
            new Spell(0, 0, 0, 0, 3, 0), //world bridge gives nothing
            new Spell(0, 0, 1, 0, 3, 0), //world priest 
            new Spell(7, 0, 0, 0, 4, 0), //world gold
            new Spell(0, 2, 0, 0, 4, 0), //world worker
            new Spell(0, 0, 0, 0, 4, 1), //world shovel
            new Spell(0, 0, 0, 0, 6, 2), //world shovel2
            new Spell(0, 0, 0, 0, 0, 0), //Does nothing
            new Spell(0, 0, 0, 0, 0, 0)
        };
    }

    public static bool Equals(SpellType t, SpellType t2)
    {
        return (int)t == (int)t2;
    }

    public static bool Equals(int t, SpellType t2)
    {
        return t == (int)t2;
    }

    public Spell GetSpell(int i)
    {
        return spellList[i];
    }

    public void ResetWorldMagic()
    {
        usedWorldMagic = new List<SpellType>();
    }
}
