using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERACTION, PLAYERMOVE, ENEMYTURN, WON, LOST, BUSY }
public enum EnemyHeat { NORMAL, HOT, COLD }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleDialogueBox dialogueBox;

    public BattleHUD playerHUD;
    
    public BattleState state;
    public EnemyHeat temp;

    int currentAction;
    int currentMove;

    int dmgVal;
    int healVal;
    int mpVal;
    int chillTimer = 0;

    void Start()
    {
        state = BattleState.START;
        temp = EnemyHeat.NORMAL;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);

        dialogueBox.EnableMoveDescription(false);
        dialogueBox.SetMoveNames();

        yield return dialogueBox.TypeDialogue($">  {enemyUnit.unitName} blocks the way.");

        yield return new WaitForSeconds(2f);
        
        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PLAYERACTION;
        StartCoroutine(dialogueBox.TypeDialogue($">  What will {playerUnit.unitName} do?"));
        dialogueBox.EnableMoveDescription(false);
        dialogueBox.EnableMoveSelector(false);
        dialogueBox.EnableActionSelector(true);
        dialogueBox.EnableDialogueText(true);
    }

    void PlayerMove()
    {
        state = BattleState.PLAYERMOVE;
        dialogueBox.EnableActionSelector(false);
        dialogueBox.EnableDialogueText(false);
        dialogueBox.EnableMoveDescription(true);
        dialogueBox.EnableMoveSelector(true);        
    }

    IEnumerator PerformPlayerMove(int currentMove)
    {
        state = BattleState.BUSY;
        
        switch (currentMove)
        {
            case 0:
                if (playerUnit.currentMP >= 5)
                {
                    yield return dialogueBox.TypeDialogue($"> {playerUnit.unitName} hurls a Fireball.");
                    yield return new WaitForSeconds(1f);
                    yield return Fireball();
                }
                else
                {
                    yield return dialogueBox.TypeDialogue($"> Not enough mana to cast!");
                    yield return new WaitForSeconds(1f);
                    PlayerMove();
                }
                break;
            case 1:
                if (playerUnit.currentMP >= 5)
                {
                    yield return dialogueBox.TypeDialogue($"> {playerUnit.unitName} launches an Icebolt.");
                    yield return new WaitForSeconds(1f);
                    yield return Icebolt();
                }
                else
                {
                    yield return dialogueBox.TypeDialogue($"> Not enough mana to cast!");
                    yield return new WaitForSeconds(1f);
                    PlayerMove();
                }
                break;
            case 2:
                if (playerUnit.currentMP >= 5)
                {
                    yield return dialogueBox.TypeDialogue($"> {playerUnit.unitName} exhaustedly casts CureWounds.");
                    yield return new WaitForSeconds(1f);
                    yield return CureWounds();
                }
                else
                {
                    yield return dialogueBox.TypeDialogue($"> Not enough mana to cast!");
                    yield return new WaitForSeconds(1f);
                    PlayerMove();
                }
                break;
            case 3:
                yield return dialogueBox.TypeDialogue($"> {playerUnit.unitName} attempts a ManaTap.");
                yield return new WaitForSeconds(1f);
                yield return ManaTap();
                break;
        }
    }

    IEnumerator Examine()
    {
        state = BattleState.BUSY;

        dialogueBox.EnableActionSelector(false);

        yield return dialogueBox.TypeDialogue($"> Enemy Name: {enemyUnit.unitName}, Type: SLIME \n> Seems vuln. to quick changes in temperature.");

        yield return new WaitForSeconds(1.5f);
        
        PlayerAction();
    }

    private void Update()
    {
        if(state == BattleState.PLAYERACTION)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PLAYERMOVE)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                currentAction--;
        }

        dialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                //Examine
                StartCoroutine(Examine());
            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove == 0 || currentMove == 2)
                currentMove++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove == 1 || currentMove == 3)
                currentMove--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogueBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogueBox.EnableMoveSelector(false);
            dialogueBox.EnableMoveDescription(false);
            dialogueBox.EnableDialogueText(true);
            StartCoroutine(PerformPlayerMove(currentMove));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerAction();
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(healVal);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlayerMana()
    {
        playerUnit.AdjustMana(mpVal);
        playerHUD.SetMP(playerUnit.currentMP);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator Fireball()
    {
        state = BattleState.BUSY;
        mpVal = -5;

        yield return PlayerMana();
        //playerUnit.currentMP -= 5;
        //playerHUD.SetHP(playerUnit.currentHP);

        if (temp == EnemyHeat.NORMAL)
        {
            temp = EnemyHeat.HOT;
            dmgVal = playerUnit.damage + Random.Range(3, 5);
            bool isDead = enemyUnit.TakeDamage(dmgVal);            

            if(isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is scorched for {dmgVal} damage!\n> {enemyUnit.unitName} becomes tame.");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is scorched for {dmgVal} damage!\n> {enemyUnit.unitName} sizzles aggressively.");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
        else if (temp == EnemyHeat.HOT)
        {
            dmgVal = playerUnit.damage + Random.Range(-2, 0);
            bool isDead = enemyUnit.TakeDamage(dmgVal);

            if (isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is barely singed for {dmgVal} damage...\n> But it's just enough!");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is barely singed for {dmgVal} damage...");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
        else if (temp == EnemyHeat.COLD)
        {
            temp = EnemyHeat.NORMAL;
            dmgVal = 2*(playerUnit.damage + Random.Range(3, 5));
            bool isDead = enemyUnit.TakeDamage(dmgVal);

            if (isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is melted for {dmgVal} critical damage!\n> {enemyUnit.unitName} becomes tame.");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is melted for {dmgVal} critical damage!\n> {enemyUnit.unitName} limbers up again.");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
    }

    IEnumerator Icebolt()
    {
        state = BattleState.BUSY;
        mpVal = -5;

        yield return PlayerMana();

        if (temp == EnemyHeat.NORMAL)
        {
            temp = EnemyHeat.COLD;
            dmgVal = playerUnit.damage + Random.Range(3, 5);
            bool isDead = enemyUnit.TakeDamage(dmgVal);

            if (isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is frozen for {dmgVal} damage!\n> {enemyUnit.unitName} became tame.");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is frozen for {dmgVal} damage!\n> {enemyUnit.unitName} stiffens into a slime-cicle.");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
        else if (temp == EnemyHeat.HOT)
        {
            temp = EnemyHeat.NORMAL;
            dmgVal = 2*(playerUnit.damage + Random.Range(3, 5));
            bool isDead = enemyUnit.TakeDamage(dmgVal);

            if (isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is extinguished for {dmgVal} critical damage!\n> {enemyUnit.unitName} became tame.");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is extinguished for {dmgVal} critical damage!\n> {enemyUnit.unitName} gets cool-headed again.");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
        else if (temp == EnemyHeat.COLD)
        {
            dmgVal = playerUnit.damage + Random.Range(-4, -2);
            bool isDead = enemyUnit.TakeDamage(dmgVal);

            if (isDead)
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is barely frosted for {dmgVal} damage...\n> But it's just enough!");
                yield return new WaitForSeconds(2f);
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is barely frosted for {dmgVal} damage...");
                yield return new WaitForSeconds(2f);
                state = BattleState.ENEMYTURN;
                yield return EnemyTurn();
            }
        }
    }

    IEnumerator CureWounds()
    {
        state = BattleState.BUSY;
        mpVal = -5;

        yield return PlayerMana();

        healVal = playerUnit.damage + Random.Range(5, 10);
        yield return dialogueBox.TypeDialogue($"> {playerUnit.unitName} restores {healVal} hit points!");
        yield return PlayerHeal();
        state = BattleState.ENEMYTURN;
        yield return EnemyTurn();
    }

    IEnumerator ManaTap()
    {
        state = BattleState.BUSY;

        mpVal = Random.Range(10, 15);
        
        dmgVal = playerUnit.damage + Random.Range(-2, 0);
        bool isDead = enemyUnit.TakeDamage(dmgVal);

        if (isDead)
        {
            yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is prodded for {dmgVal} damage.\n> {playerUnit.unitName} restores {mpVal} MP!");
            yield return new WaitForSeconds(1f);
            yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} became tame.");
            yield return PlayerMana();
            //yield return new WaitForSeconds(2f);
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is prodded for {dmgVal} damage.\n> {playerUnit.unitName} restores {mpVal} MP!");
            yield return PlayerMana();
            //yield return new WaitForSeconds(2f);
            state = BattleState.ENEMYTURN;
            yield return EnemyTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        state = BattleState.BUSY;

        if (temp == EnemyHeat.NORMAL)
        {
            chillTimer = 0;
            dmgVal = enemyUnit.damage + Random.Range(-2, 2);
            yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} attacks.\n> {playerUnit.unitName} receives {dmgVal} damage!");            
        }
        else if(temp == EnemyHeat.HOT)
        {
            dmgVal = enemyUnit.damage + Random.Range(3, 5);
            yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} unleashes a heated wallop!\n> {playerUnit.unitName} suffers {dmgVal} damage!");
        }
        else if(temp == EnemyHeat.COLD)
        {
            dmgVal = 0;
            chillTimer++;

            if(chillTimer < 2)
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} is just chillin' there.");
            else
            {
                chillTimer = 0;
                temp = EnemyHeat.NORMAL;
                yield return dialogueBox.TypeDialogue($"> {enemyUnit.unitName} thawed out naturally!");
            }

        }

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(dmgVal);

        if (playerUnit.currentHP <= 0)
            playerUnit.currentHP = 0;

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERACTION;
            PlayerAction();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            StartCoroutine(dialogueBox.TypeDialogue($"> {playerUnit.unitName} won the battle!\n> END OF DEMO, THANKS FOR PLAYING!"));
        }
        else if(state == BattleState.LOST)
        {
            StartCoroutine(dialogueBox.TypeDialogue($"> {playerUnit.unitName} blacked out...\n> END OF DEMO, THANKS FOR PLAYING!"));
        }
    }}
