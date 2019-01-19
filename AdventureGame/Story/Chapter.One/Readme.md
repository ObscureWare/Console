# Chapter one - technicals

Here are resources for chapter one as well as common objects definitions (perhaps should move them to some base project?).

Following chapters should be able to refer to this chapter, and so on in the chain. 
Yet nothing in lower-number chapter should depend on higher number chapters.

There are classes that:
 - build map
 - build containers
 - setup characters, including dialogs and variables
 - construct items
 - construct puzzles (locks, terminals)
 - setup game events triggers
 - provide entry point
 - etc.

 Game will be in Polish so there will be a lot of interesting challenges in command parsing, dialogs and story descriptors 
 (mainly due to sex determination of the hero, but not only).
 I would like to make possibility to use both "biorę/podnoszę" and "weź/podnieś" words that will result with same operation attempt.
 Also matching and finding of items in area / inventory should make it possible with and without item unique names and adjectives.

 Commands should be able to ask player for more details to resolve target ambiguity - "talk to who?", "equip which knife?" etc.

 Also, entering terminal or dialog should switch to another communication model:
 Rather selecting dialog options than entering them.
 Navigating through menu and password entering near terminal. 
 Or using items - but this might be done with "use sth on terminal" insted of "use terminal" before.

# Game ideas

Player probably will finish the whole story within few in-world weeks, therefore I assume no character progress, learning new skills, etc.

Also, player is not able to hack terminals or lock-pick locks - he or she never learned such skills. 
Perhaps companions met during the story will posses such skills, but normally he must rely on acquiring keys or passwords or key-cards.

# Story plot (with technical implications)

Hero (or heroine) was raised in small village (NAME!), one of three villages in the valley 
[determine proper sizes that would be able to provide for over 1000 years].
(There is total of around 150 people [perhaps should determine proper roles for self-sustained society] in the valley,
yet of course game will not present them all or let hero interact with them.)

All villages are Amish-like - no electricity, simple tools, simple jobs and life - fishing, farming, breeding (sheep, cows), some hunting 
(in the forests there are some wild, small animals like heir or ), smithy, carpentry, etc.

The available materials are wood, iron (mainly recycled), coal (mainly charcoal), products of animals and crops (cotton, cannabis etc.).

The mountains are not passable, and everyone knows it. Hero can try some climbing, but cannot reach more then several meters. 
Beyond that point rock is too steep and solid to use simple tools and ropes he can obtain.

In the first chapter hero should just travel through the valley not yet knowing of his purpose. Game must provide him with temporary goals,
to avoid being lost and bored. Perhaps some passages / rooms be blocked with "story-doors" saying things like:
"you cannot go there yet" or "you have no business in going there" or "you must something important do elsewhere".
Such doors should automatically open (and perhaps close) according to specific game states.

Anyway after several quests, an plots here should be able to obtain grandfather's journal and in another village - 
the key-card to the hidden terminal on one of the rock walls near the third visit. Key-card reveals terminal but password is required.

He should be able to find some notes held by grandchildren of the long-deceased priest. That know something, but more like a legend or myth.

Finally, when player enters the password, the large, stone doors behind terminal open, where in the futuristic corridor 
he will met some companion (man from the village from the valley behind mountains? Yet their doors were always open, he was just checking here from time to time).
And figure out, that this might look like escape route from the valley. Together they decide to find more with other doors there are there.

And this concludes chapter one.