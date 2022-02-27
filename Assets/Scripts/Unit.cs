using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int damage;

    public int maxHP;
    public int currentHP;

    public int maxMP;
    public int currentMP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int heal)
    {
        currentHP += heal;

        if (currentHP > maxHP)
            currentHP = maxHP;
    }
    
    public void AdjustMana(int mp)
    {
        currentMP += mp;

        if (currentMP > maxMP)
            currentMP = maxMP;
    }
}
