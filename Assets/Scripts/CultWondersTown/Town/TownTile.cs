using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTileController {

    int remaining_priest;
    int remaining_magic;
    int remaining_gold;
    int remaining_worker;
    int remaining_cult;

    WonderTile priest;
    WonderTile magic;
    WonderTile gold;
    WonderTile worker;
    WonderTile cult;

    public enum TownTile_type { priest, magic, gold, worker, cult};

    public TownTileController()
    {
        remaining_priest = 2;
        remaining_magic = 2;
        remaining_gold = 2;
        remaining_worker = 2;
        remaining_cult = 2;
    }

    public void SetUp()
    {

        priest = new WonderTile(new SingleIncome(0, 0, 1, 0), 9, null);
        magic = new WonderTile(new SingleIncome(0, 0, 0, 8), 6, null); ;
        gold = new WonderTile(new SingleIncome(6, 0, 1, 0), 5, null); ;
        worker = new WonderTile(new SingleIncome(0, 2, 1, 0), 7, null);
        cult = new WonderTile(new SingleIncome(0, 0, 0, 0), 8, new CultIncome(1, 1, 1, 1)); ;
    }

    public WonderTile Get_TownTile(TownTile_type type)
    {
        switch (type)
        {
            case TownTile_type.priest:
                if (remaining_priest > 0) { remaining_priest--; return priest; } break;
            case TownTile_type.gold:
                if (remaining_gold > 0) { remaining_gold--; return gold; } break;
            case TownTile_type.magic:
                if (remaining_magic > 0) { remaining_magic--; return magic; } break;
            case TownTile_type.worker:
                if (remaining_worker > 0) { remaining_worker--; return worker; } break;
            case TownTile_type.cult:
                if (remaining_cult > 0) { remaining_cult--; return cult; } break;
        }
        return null;
    }
}
