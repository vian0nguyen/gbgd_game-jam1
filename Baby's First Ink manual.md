# Ink in Unity Documentation
This manual will explain how to format Ink documents for this game. 
_(this document is a constant work in progress as we add more text/data to pull from the dialogue)_

1. Download [inky](https://www.inklestudios.com/ink/) from their website to help write dialogue content

## Tags
To write tags **in Ink**, simply write 

`# + tag marker + whatever data you want the game to use.`

ie: `My hat is blue. #!Jimmy`

The console will print something like "The person speaking is Jimmy" because of the "!" marker that tells the game the name of the speaker

In the **Unity inspector** 
1. go to the DialogueManager object > Dialogue Writer component
2. scroll down till you see "Tags"
3. add one to the "Size" slot
4. write in the new marker in the "marker" slot and whatever functions are called when that tag is called (drag and drop in the inspector).

With names, make sure the names are EXACTLY the same as whatever you're referencing (cases included)

### Tag Key
  
  Your guide to all the tags in the game and what their data is used for.
  
  *  Prototype tag (~): Currently tells the console that the tag contains whatever text comes after it. Mainly used to test the tag system and tag parsing system.  
  *  Set Quest tag (!): Sets the main questline for the dialogue.  Put name of quest scriptable object after the tag to specify which quest you want (and make sure said quest is in the "Quests" array in the QuestManager.
  *  Move Canvas tag (^): Moves world canvas to where the player is
  *  Choose Speaking Sprite (*): Chooses speaking sprite if the character is a MULTI NPC object. Put name of character after the tag to specify which one in the group is speaking (make sure the name is in the child objects of the multi NPC)
  *  New Arc tag (>): Enables arc to be advanced once this dialogue was over (can be placed anywhere in the ink file)
  *  Move Canvas to NPC tag (%): Moves world canvas to the NPC that's speaking
  
## Dialogue Formatting
When writing dialogue lines, try to break things up by sentence/natural pause so that all the text can fit on screen.  The text will scroll the next line of dialogue automatically, so no need to fit all the dialogue on one line.


## NPC Setup
When adding dialogue for NPC's, don't add them to the NPC's in the scene.  Instead, go to Assets/Scripts/ScriptableObjects/Quests and look for which questline you want to add the dialogue to, and then look for which character, which arc, and where in the dialogue set you want to add the dialogue to.

1. Go into QuestManager in the scene
2. Click on the field in the Quests array to find the scriptable object
3. Make sure NPC’s are listed; if not, add the npc from the scene to the Npcs array, and hit Update Names
4. Click on scriptable object in the Project tab
5. Each Element under the “Dialogue Arcs” is the arc, and that arc contains a dialogue set, expand the lists until you get to Dialogue Set
6. Put file into Dialogue Set
  
  * Dialogue Arcs will cycle through all the files in the Dialogue Set until it hits Recurring
  * Recurring dialogue is what plays once you’ve finished all the important talking (filler dialogue/hints) <- Make sure that there is a recurring dialogue file or else the dialogue will break (the dialogue set can be left blank, though).
  * If it’s multiple NPC’s, just tag the character that is the child object of the multi NPC object
  * Need different recurring dialogues per quest arc for each NPC.
  * Make 1 dialogue arc per story scene

**When testing a dialogue, go to the questManager in the scene, then under Current Quest, add the quest scriptable object you added dialogue for.  Once complete, remember to remove that quest scriptable from the Current Quest Value, as the player will have that set when they talk to the mailbox.**
