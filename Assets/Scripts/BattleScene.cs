using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    /* Pseudocode for the flow of battle states.
     * 
     *  BattleScene is entered on Encounter Trigger (enemy contact or interacting with enemy)
     *  v v v
     *  Scene Transition
     *  v v v
     *  BEGINNING OF BATTLESCENE CLASS
     *  
     *  =============================
     *  
     *  Start()
     *  > Instantiate HUD
     *  > Start music (looping)
     *  > Text appears procedurally/character-by-character
     *  
     *  Update()
     *  
     *  Key Press Code (Z/Accept)
     *  > if pressed/held in start screen before text has been displayed, display text faster
     *  > if pressed once all text has been displayed, instantiate Player Menu screen
     *  > if in Menu screen, do task correlating to currently hovered choice
     *  > if in other menus (win/defeat, etc) advance the text/game state
     *  
     *  Key Press Code (X/Back)
     *  > if pressed in start screen before text has been displayed, display text immediately
     *  > if pressed in Menu screen, go back to previous menu level when at a sublevel
     *  > if given a binary/yes-or-no dialogue/menu choice, move cursor to "no"
     *  
     *  Key Press Code (arrow keys)
     *  > if pressed in Menu screen, navigate menu options
     * 
     *  Menu()
     *  > state in which options Attack(), Item(), and Other() are displayed
     *  > using keystrokes, choose options to progress to next menu or next state
     *  > Attack opens Attack submenu with spells; if one is chosen, start Attack()
     *  
     *  Attack()
     *  > spell animation plays, text displayes, enemy takes damage/changes states
     *  > if enemy dead, go to Win()
     *  > if enemy alive, go to EnemyTurn()
     *  
     *  Item()
     *  > for now, a dummy option. empty inventory and will display a message as such,
     *  > then return to main menu
     *  
     *  Other()
     *  > choose Check() or Talk()
     *  
     *  EnemyTurn()
     *  > Enemy checks if Talk() has been visited and current state it is in, then acts accordingly
     *  > Enemy attempts attack or action w/ text + effect, player HP/status updated accordingly
     *  > If player health = 0, go to GameOver()
     *  > If player health > 0, go to Menu()
     *  
     *  Check()
     *  > Display stats, then go to EnemyTurn()
     *  
     *  Talk()
     *  > Display text conversation (may be one-sided). Enemy strategy could change
     *  
     *  Win()
     *  > Enemy fades out, display win text
     *  > Enemy removed from overworld scene, player moved back to overworld scene w/ transition
     *  
     *  GameOver()
     *  > GameOver text displayed, death animation/jingle/song plays
     *  > Continue option displayed (?) 
     */
}
