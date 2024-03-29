# Unity Documentation notes for vian

## Making NPCs
Make sure a couple things:
* has a _trigger_ collider
* TAG (drop down) = NPC
* Has an NPC component
IF it's a one-off object that we want text to print when interacted with:
* (this might break something lol) but it doesn't need to be populated in QuestManager
* Fill it's NPC component directly with it's main dialogue and/or recurring text.
