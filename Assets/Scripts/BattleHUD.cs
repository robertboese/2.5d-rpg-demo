using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text HPtext;
    public Text MPtext;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        HPtext.text = "HP:  " + unit.currentHP;
        MPtext.text = "MP:  " + unit.currentMP;
    }

    public void SetHP(int hp)
    {
        HPtext.text = "HP:  " + hp;
    }

    public void SetMP(int mp)
    {
        MPtext.text = "MP:  " + mp;
    }
}
