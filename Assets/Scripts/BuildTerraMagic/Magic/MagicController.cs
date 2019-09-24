using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {

    public enum Spell_Type { Priest, Worker, Gold, World_Bridge, World_Priest, World_Worker, World_Gold, World_Terraform1, World_Terraform2, NOTHING, BURN};

    Spell[] spell_list;
    List<Spell_Type> used_worldMagic;

    public MagicController()
    {
        used_worldMagic = new List<Spell_Type>();
    }

    public bool Check_WorldMagic_Used(Spell_Type st_)
    {
        foreach (Spell_Type st in used_worldMagic)
        {
            if (Equals(st, st_)) { return true; }
        }

        used_worldMagic.Add(st_);
        return false;

    }

    public void SetUpSpells()
    {
        spell_list = new Spell[] {
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

    public static bool Equals(Spell_Type t, Spell_Type t2)
    {
        return (int)t == (int)t2;
    }

    public static bool Equals(int t, Spell_Type t2)
    {
        return t == (int)t2;
    }

    public Spell GetSpell(int i)
    {
        return spell_list[i];
    }
}
