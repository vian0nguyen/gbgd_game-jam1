-> Intro


=== Intro ===
Hey, Delivery Pig! #*HungryDuck
Good thing you're here! The last one was awful. #*LeaderDuck
Absolutely terrible! #*NerdDuck
A complete failure. #*EmoDuck
Flour everywhere! #*HungryDuck
As you may know, in this neighborhood, cake is VERY important. #*LeaderDuck
So that makes us also VERY important. #*HungryDuck
Because we supply cake ingredients. #*Ducks
Heh. We're about to become EVEN more important. #*EmoDuck
Heh heh heh! #*HungryDuck
Hey! Emo Duck! Close your beak! We're here to talk about delivery. #*LeaderDuck
Remember the script! #*LeaderDuck
Ahem. #*LeaderDuck
Since cake is SO important, we have lots of top-secret cake business. Can we trust you to keep it zipped? #*LeaderDuck
* You can count on me!
Great. We believe you, completely. Therefore, this first delivery is not a test. Can you send this Ingredient Package to the Toad Gang? #*LeaderDuck
It's good pay! Just don't ask why we aren't doing it ourselves. #*HungryDuck
I'm sure you've heard of Toad's reputation. So, please, show us you can be delicate around him? #*NerdDuck
We won't wish you luck, because it won't help you! #*HungryDuck
Remember, your mission is to deliver ingredients to Toad Gang. Nothing else. #*LeaderDuck
Don't let them rope you into any other tasks. They can be nefarious. #*NerdDuck
/Grins creepily/ Bye. #*EmoDuck
-> First_Toad_Meet


=== First_Toad_Meet ===
Who's there? #Tiger
Bzz? (Did we hire you?) #Bee
* I'm delivering ingredients from the ducks.
    -> deliver_ingredients_firsttime
* Is this the Toad Gang?
    -> deliver_ingredients_firsttime

=== deliver_ingredients_firsttime ===
Oh, it's the ingredients. So they're not here to deliver our cakes. #Crab
Big Boss is busy. You can leave it with us. #Crab
Bzz? (Can we get them to deliver our cakes, too? That way, we won't have to pay!) #Bee
Yeah, you can do it, won't you? We don't bite. /Tiger grins./ #Tiger
* Sorry, I'm only here to deliver!
-> deliver_ingredients_firsttime_conclusion

= deliver_ingredients_firsttime_conclusion
Ugh, that's so disappointing. Again? #Tiger
Bzz! (What a loser!) #Bee
Whatever. They're not worth our time. #Crab
Oh, you're still here? #Crab
Bzz. (Here's the payment, loser. Bring it back to the ducks.) #Bee
You can go, now. #Tiger
-> Payment_To_Ducks


=== Payment_To_Ducks ===
You're back. What happened? #NerdDuck
Did Toad yell at you? #HungryDuck
* No, I didn't even talk to him. -> plan_figured_out
= plan_figured_out
You didn't see him? But he personally oversees all transactions! #HungryDuck
Hmm. Something's been keeping him occupied lately. #NerdDuck
Better to have him busy than paying attention to us. Since, you know - #LeaderDuck
Hey, Leader? #EmoDuck
Huh? #LeaderDuck
I figured it out! #EmoDuck
I figured out our PLAN! #EmoDuck
REALLY? #NerdDuck
TRULY? #HungryDuck
Yes! #EmoDuck
And the plan is - #EmoDuck
WAIT! Bind your beaks! #LeaderDuck
We can't talk about it HERE. #LeaderDuck
But you know what this means. Nerd Duck? #LeaderDuck
Oh, right, Phase 1! Uhh, here you go. #NerdDuck
Don't mind this. It's a Totally Uninteresting Package. #NerdDuck
* /Take the Totally Uninteresting Package (TUP)./ -> tup_obtained
= tup_obtained
Take this TUPperware to Grandma Shrew. Ask her to follow the instructions inside. #NerdDuck
And, whatever you do, don't let the Toad Gang see you. #LeaderDuck
Go, go! /Emo Duck quackles evily./ #EmoDuck
->Tupperware_To_Gma


=== Tupperware_To_Gma ===
Oh, a delivery? #Grandma
For me? But I didn't order anything. #Grandma
* This is from Duck Gang. /Give Grandma the TUPperware./ -> speculation_with_Gma
= speculation_with_Gma
Hmm, a note with instructions? How do I open this thing? "Do not let them see ..." Oh! #Grandma
Wait. You said Duck Gang. But surely this must be from Toad Gang? #Grandma
* No, this is definitely from the Ducks! -> gma_follows_instructions
= gma_follows_instructions
"Now do the rest in private ..." Hmm! These ducks sure are bossy. You stay right there. #Grandma
... #Grandma
Let's see, what do I say now? "Go." Yes, tell them "go!" #Grandma
Hmmph. You kids are always getting me involved. This will lead to trouble! #Grandma
->Gang_Spots_Player


=== Gang_Spots_Player ===
Hey, are you coming from Grandma's house? #Crab
* Uh oh!
    -> make_excuses
= make_excuses
With a skateboard and everything - why were you there? #Tiger
* I was [just skating by!] -
    -> forgone_conclusion
* I was [enjoying the view!] -
    -> forgone_conclusion
* I was [talking to Grandma!] -
    -> forgone_conclusion
= forgone_conclusion
Bzz! (Wait a minute - delivery gear!) #Bee
And they're tired and out of breath. #Tiger
That can only mean one thing. #Crab
You DID deliver our cakes, after all! #Crab
Head back to our facility, we've got more there! #Crab
* Uhh, I wasn't[ delivering your cakes to Grandma.] - -> crab_snap
= crab_snap
What are you waiting for? /Crab's claws snap menacingly./ #Crab
No time to waste. See you there. #Crab
->Go_to_Bakery


=== Go_to_Bakery ===
* Whoa, how did you all get here before me? -> toad_gang_flew_back
= toad_gang_flew_back
You'll never know our secret! #Tiger
Bzz! (It was me, they flew on my back!) #Bee
Hey! That was our SECRET! /Tiger pouts./ #Tiger
* It still doesn't make sense. How did they all fit on a tiny bee? -> confrontation
= confrontation
That's not what we're here to talk about. #Crab
It seems like you weren't entirely truthful to us. /Crab snaps his little claw./ #Crab
We talked to Grandma, and she says you delivered something to her. #Crab
Do you have something to tell us? #Crab
* I have no idea what you're talking about! -> crab_rages
* /Lie/ Uhh, yeah! I delivered - I delivered your cake! -> crab_rages
* No. I don't owe you anything. -> crab_rages
= crab_rages
She said that you came from the ducks and delivered - #Crab
A CAKE! #ToadGang
The ducks' note asked her not to tell us. #Crab
But Grandma told us anyway! #Bee
Because we're the boss. #Tiger
And you're gonna pay! #Tiger
But we're not the ones you should be afraid of. #Crab
Here he is: Toad! #Crab
-> Toad_Scorns_Ducks


=== Toad_Scorns_Ducks ===
So, this is the Ducks' new Delivery Pig. #Toad
Not much to look at. #Toad
/Toad clears his throat./ #Toad
The ducks are second-rate cake fakes. #Toad
They think it's easy to make cake. They think it's easy to support a neighborhood. #Toad
We're not afraid of them. They're going to fail anyway. #Toad
Many have tried. We remain. #Toad
* You're a bunch of bullies.
-> defending_neighborhood
= defending_neighborhood
Bullies? The rest of the city, they're the real bullies. Did you know? This neighborhood is the single largest haven in the WORLD. #Toad
The city is creeping in. Only we've kept it out. #Toad
You think this cake is just cake? Cake is what we send on holidays. Cake is what we gather to eat. #Toad
Others have tried to disrupt the cake before. They failed. #Crab
* Then why do you have to threaten me?
-> not_afraid_of_ducks
= not_afraid_of_ducks
We don't want to have to keep dealing with startups! It's better to make an example. #Tiger
/Crab snips his claws./ Sorry in advance. #Crab
Bzz. (We won't enjoy it THAT much, I swear!) #Bee
Believe us. You have to - there's too much at stake. #Toad
-> Ducks_Burst_In


=== Ducks_Burst_In ===
Stop right there, Toad! #LeaderDuck
* Are you here to save me?
-> grandma_told_ducks
= grandma_told_ducks
Toad, you overgrown frog! #EmoDuck
Grandma told us you were up to no good! #LeaderDuck
What? But Grandma was on OUR side! #Crab
Enough dragging this on! Gang, let's end this! #Toad
No, we end this. #NerdDuck
We have a secret weapon. #EmoDuck
And it's called - #HungryDuck
PIE! #NerdDuck
PIE! #EmoDuck
PIE! #HungryDuck
PIE! #LeaderDuck
Bzz! (No way, it can't be!) #Bee
Our only weakness! #Tiger
This can only mean one thing ... ! #Crab
WAR! #Toad
-> END