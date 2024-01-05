-> Gang_Spots_Player
=== Gang_Spots_Player ===
Hey, are you coming from Grandma's house? #*Crab
Only WE deliver to Grandma! #*Tiger
What's going on? #*Crab
* Uh oh! I [don't want trouble!]-
    -> make_excuses
= make_excuses
Bzz? (What were you doing there?) #*Bee
* I was [doing nothing at all, and I'm NOT being suspicious!] -
    -> forgone_conclusion
* I was [enjoying the view!] -
    -> forgone_conclusion
* I was [just, uhh, saying hi to Grandma.] -
    -> forgone_conclusion
= forgone_conclusion
Bzz! (Look at that delivery gear!) #*Bee
And the note in hand. #*Tiger
That can only mean one thing. #*Crab
Bzz. (You made a delivery to Grandma.) #*Bee
* Please [don't hurt me!] -
    -> gang_realization
* No[, I didn't, I swear!] -
    -> gang_realization
= gang_realization
Hooray! #*ToadGang
* Huh?
    -> mistaken_delivery
= mistaken_delivery
You DID deliver our cakes, after all! #*Crab
Less work for us! #*Tiger
Head back to our facility, we've got more cakes! #*Crab
* Right away!
    -> crab_snap
* Uhh, I wasn't [delivering your cakes to Grandma.] -
    -> crab_snap
= crab_snap
What are you waiting for? /Crab's claws snap menacingly./ #*Crab
Bzz. (No time to waste. See you there.) #*Bee
And we're going to talk to Grandma. #*Tiger
-> END