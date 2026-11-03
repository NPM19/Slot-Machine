# Slot-Machine
This is a showcase project of Slot Machine Simulation.

--------------------------------------------
Instruction to Run the WebGL Playable build:
--------------------------------------------
1) Open terminal or powershell window in the 'WebGL playable' Folder, where the 'index.html' file is located. 
2) Make sure you have python installed on your system. you can check install status by typing the cmd "python --version" and hit enter(mine is displayed Python 3.13.1). if python is not installed, install it and make sure to tick "Add python.exe to PATH" checkbox during installation then check status again.
3) Now to the cmd prompt or powershell window open in the WebGL playable folder copy/paste command "python -m http.server 8000" press "enter".
4) Open chrome browser and copy/paste "localhost:8000" on tab and press "enter".
5) Game should start running, and you can play the game.

==================
Unity Project Log:
==================

Project Brief:
-------------
This project is about showcasing the working mechanism of Slot Machine architecture in a very simplest way.

Technical Walkthrough:
----------------------
- I have tried to keep everything as simple as I could by writing/keeping code files and code small and simple. 

- the main slot machine processing starts in SlotMachine Controller class, the slot machine states have been implemented using the state pattern using enums to hightlight main states. 

- ReelManager class handles the working and handling of reels. the SN Controller only calls the appropriate methods on it when the state of slot machine demands it.

- Callback functions are also used in the code design to keep classes loosely coupled with each other whenever necessary.

- Almost all the classes implement private fields and expose either public property or method to use that field to enforce encapsulation.

- Slot symbol gameobject is used to represent symbols on the slot machine reels, and each symbol gameobject has Symbol class component attached to it. that has information of weight of drawing that symbol on spin, payout multiplier, and ID of the symbol that is used to determine whether two symbol objects represent same symbol icon or not. eg. all cherry objects will have same ID.

- Timers are used to determine when reels will start to stop. and the time interval between each reels to show result once by one on each reel.  

- The result of each spin is predetermined when player uses the spin, the machine is just creating the illusion of luck based draw and invoking the atmospheric suspense by animating the reels spin and after a predefined time starts stopping the reel at a sequential seperate time interval. lastly UI information is updated to show the spin information for that spin.  

**************
BONUS FEATURE:
**************
The playable build only has win reward for when all symbols in the slots are same. 
but in inside the project I have also implemeted win reward for even when more than half the symbols on slot matches. you can see it in WinEvaluater.cs file inside the Assets/Scripts folder. and test the project in unity editor by simply uncommenting the commented parts of that file that are inside CalculateWinAmount() function and the necessary codes associated with those codes inside. then simply run the game in editor and you will see rewards will be rewarded for 2 same symbols in a resultant spin.