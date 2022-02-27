using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    public Text dialogueText;
    public Text moveDescription;
    public GameObject actionSelector;
    public GameObject moveSelector;

    public List<Text> actionTexts;
    public List<Text> moveTexts;

    public int lettersPerSecond;
    public Color highlightedColor;
    
    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogueText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
    }

    public void EnableMoveDescription(bool enabled)
    {
        moveDescription.enabled = enabled;
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == selectedAction)
                actionTexts[i].color = highlightedColor;
            else
                actionTexts[i].color = Color.white;
        }
    }

    public void UpdateMoveSelection(int selectedMove)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.white;
        }

        switch (selectedMove)
        {
            case 0:
                moveDescription.text = "> COST:  5 MP\n> FIRE type spell that increases heat.";
                break;
            case 1:
                moveDescription.text = "> COST:  5 MP\n> ICE type spell that decreases heat.";
                break;
            case 2:
                moveDescription.text = "> COST:  5 MP\n> Curative magic that restores HP.";
                break;
            case 3:
                moveDescription.text = "> COST:  0 MP\n> Blunt attack that restores some MP.";
                break;
        }
    }

    public void SetMoveNames()
    {
        moveTexts[0].text = "Fireball";
        moveTexts[1].text = "Icebolt";
        moveTexts[2].text = "CureWounds";
        moveTexts[3].text = "ManaTap";
    }
}
