using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownFoundingBonusManager : MonoBehaviour{

    int remaining_priest;
    int remaining_magic;
    int remaining_gold;
    int remaining_worker;
    int remaining_cult;

    FavourBonus priest;
    FavourBonus magic;
    FavourBonus gold;
    FavourBonus worker;
    FavourBonus cult;

    public enum TownTiletype { priest, magic, gold, worker, cult, NOTHING};

    public TownFoundingBonusManager()
    {
        remaining_priest = 2;
        remaining_magic = 2;
        remaining_gold = 2;
        remaining_worker = 2;
        remaining_cult = 2;
    }

    public void SetUp()
    {

        priest = new FavourBonus(new SingleIncome(0, 0, 1, 0, 0), 9, null);
        magic = new FavourBonus(new SingleIncome(0, 0, 0, 8, 0), 6, null); ;
        gold = new FavourBonus(new SingleIncome(6, 0, 1, 0, 0), 5, null); ;
        worker = new FavourBonus(new SingleIncome(0, 2, 1, 0, 0), 7, null);
        cult = new FavourBonus(new SingleIncome(0, 0, 0, 0, 0), 8, new CultIncome(1, 1, 1, 1)); ;
    }

    public FavourBonus Get_TownTile(TownTiletype type)
    {
        switch (type)
        {
            case TownTiletype.priest:
                if (remaining_priest > 0) { remaining_priest--; return priest; } break;
            case TownTiletype.gold:
                if (remaining_gold > 0) { remaining_gold--; return gold; } break;
            case TownTiletype.magic:
                if (remaining_magic > 0) { remaining_magic--; return magic; } break;
            case TownTiletype.worker:
                if (remaining_worker > 0) { remaining_worker--; return worker; } break;
            case TownTiletype.cult:
                if (remaining_cult > 0) { remaining_cult--; return cult; } break;
        }
        return null;
    }


    public void ReduceTownTile(TownTiletype type)
    {
        switch (type)
        {
            case TownTiletype.priest:
                remaining_priest--; break;
            case TownTiletype.gold:
                remaining_gold--; break;
            case TownTiletype.magic:
                remaining_magic--; break;
            case TownTiletype.worker:
                remaining_worker--; break;
            case TownTiletype.cult:
                remaining_cult--; break;
        }
    }
}
