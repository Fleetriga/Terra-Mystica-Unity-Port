using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic {
    int tier_one;
    int tier_two;
    int tier_three;

    int max_magic;

	public Magic(int starting_one, int starting_two, int starting_three)
    {
        tier_one = starting_one;
        tier_two = starting_two;
        tier_three = starting_three;

        max_magic = 12;
    }

    public void UseSpell(int cost)
    {
        tier_three -= cost;
        tier_one += cost;
    }

    public void BurnMagic(int burn)
    {
        if (tier_two >= burn && tier_two >= 2)
        {
            tier_two -= burn * 2; //Burn and send one to tier 3 
            max_magic -= burn; //Always reduce maximum as it has been burned;
            tier_three += burn; //But only if there's one to send, stops user from burning his last and creating a free magic

            if (tier_three > max_magic) { tier_three = max_magic; } //Make sure tier 3 didn't go over the max;
        }
    }

    public void AddMagic(int income)
    {
        income -= tier_one;
        if (income < 0)
        {
            tier_two += (tier_one + income);
            tier_one -= (tier_one + income);
        }
        else
        {
            tier_two += tier_one;
            tier_one = 0;

            //repeat steps
            income -= tier_two;
            if (income < 0)
            {
                tier_three += (tier_two + income);
                tier_two -= (tier_two + income);
            }
            else
            {
                tier_three += tier_two;
                tier_two = 0;
            }
        }
    }

    public int[] GetTiers()
    {
        return new int[] { tier_one, tier_two, tier_three};
    }
}
