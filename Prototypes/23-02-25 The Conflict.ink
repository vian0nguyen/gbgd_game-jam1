//Intro
Good thing you're here! The last guy was awful. #LeaderDuck
Absolutely terrible! #NerdDuck
A complete failure. #EmoDuck
And he never tried any cake! #HungryDuck

Leader Duck: As you may know, in this town, cake is VERY important. #LeaderDuck
Hungry Duck: So that makes us also VERY important. #HungryDuck
Because we're the cake ingredient suppliers. #Ducks
Heh, heh. #EmoDuck
What? #LeaderDuck
We're about to become even more important. #EmoDuck
Close your beak! We're here to talk about delivery. Remember the script! #LeaderDuck
Ahem. Since cake is SO important, we have lots of top-secret cake business. Can we trust you to keep it down? #LeaderDuck
* You can count on me!
Great. We believe you, completely. Therefore, this first delivery is not a test. Can you send this Ingredient Package to the Toad Gang? #LeaderDuck
Paranoid Duck: I'm sure you've heard of Toad's reputation. So, please, show us you can be delicate around him. #NerdDuck
We won't wish you luck, because it won't help you! #HungryDuck
Bye. (Grins creepily) #EmoDuck

-> First_Toad_Meet


=== First_Toad_Meet ===

Finally! You're here! We've  got 3 cakes out for delivery. #Tiger
* Uh, I'm already delivering something.
    What?? -> deliver_ingredients_firsttime #Crab
* [Is this the Toad Gang?]
    Bzz! (Obviously! There's only one!) #Bee
    And the best in town. -> deliver_ingredients_firsttime #Tiger
+ This is from the Ducks. -> deliver_ingredients_firsttime
    
=== deliver_ingredients_firsttime ===
<>Oh, must be the ingredients, then. #Crab
Big Boss is busy. You can leave it with us. #Crab
* Here, I'll put it in your small hand.
-> deliver_ingredients_firsttime_conclusion

= deliver_ingredients_firsttime_conclusion
So you're not going to deliver our cakes? Disappointing. #Tiger
Bzz! (What a loser!) #Bee
Bzz. (Here's the payment, loser. Bring it back to the ducks.)  #Bee
* [What about my payment?] ->ask_payment
= ask_payment
Thats Ducks' business, not ours. Now, get out of here. We're busy. #Tiger

-> Payment_To_Ducks


=== Payment_To_Ducks ===
You're back. Did anything happen? #Ducks
Did Toad yell at you? #Ducks
* [No, I didn't even talk to him.] -> plan_figured_out
= plan_figured_out
Hey, Leader? #EmoDuck
Huh? #LeaderDuck
I figured it out! I figured out our PLAN! (Quackles) #EmoDuck
FINALLY! #Ducks
Excellent work. But we can't talk about it HERE. #LeaderDuck
We'll show the Toad Gang! #HungryDuck
What are the details?? #NerdDuck
The plan - the PLAN - is to - #EmoDuck
Bind your beaks! We'll talk about this in private. Nerd Duck? #LeaderDuck
Oh, right, Phase 1! Uhh, here you go. #NerdDuck
* [(Take the Totally Uninteresting Package (TUP).)] -> tup_obtained
= tup_obtained
Take this TUPperware to Grandma Shrew. Ask her to follow the instructions inside. #NerdDuck
And, whatever you do, don't let the Toad Gang see you. #Ducks
Now, shoo! ->Tupperware_To_Gma #Ducks 

=== Tupperware_To_Gma ===

Oh, a delivery? For me? But I didn't order anything. #Grandma
* [This is from Duck Gang. (Give Grandma the TUPperware.)] -> speculation_with_Gma
= speculation_with_Gma
Hmm, a note with instructions? How do I open this thing? "Do not let them see ..." Oh! #Grandma
Wait. But surely this must be from Toad Gang? #Grandma
* [No, this is definitely from the Ducks!] -> gma_follows_instructions
* [Something is weird here.] -> gma_follows_instructions
= gma_follows_instructions
"Now do the rest in private ..." Hmm! These ducks sure are bossy. You stay right there. #Grandma
... #Grandma
Let's see, what do I say now? "Go." Yes, tell them "go!" #Grandma
Hmmph. You kids are always getting me involved. This will lead to trouble! #Grandma

->END

=== Toad_Scorns_Ducks ===
So, this is the new delivery pig. #Toad
The ducks are second-rate cake fakes. #Toad
-> END