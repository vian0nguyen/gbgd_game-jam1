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

### Tag Key
  
  Your guide to all the tags in the game and what their data is used for.
  
  *  Prototype tag (~): Currently tells the console that the tag contains whatever text comes after it. Mainly used to test the tag system and tag parsing system.  
  
## Dialogue Formatting
When writing dialogue lines, try to break things up by sentence/natural pause so that all the text can fit on screen.  The text will scroll the next line of dialogue automatically, so no need to fit all the dialogue on one line.
