## [Labyrinth][game]
A roguelite dungeon crawler developed in Unity by Caleb Scopteski, Patrick Walsh, Sam Almeida, and Ethan Ferrabelo for [LOWREZJAM 2021][lowrez]


<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about">About</a></li>
    <li>
      <a href="#level-generation">Level Generation</a>
      <ul>
        <li><a href="#the-algorithm">The Algorithm</a></li>
        <li><a href="#the-dungeon-tree">The Dungeon Tree</a></li>
        <li><a href="#the-rooms">The Rooms</a></li>
        <li><a href="#the-hallways">The Hallways</a></li>
        <li><a href="#populating-the-rooms">Populating the Rooms</a></li>
      </ul>
    </li>
    <li><a href="#developers">Developers</a></li>
    <li><a href="#game-jam-results">Game Jame Results</a></li>
  </ol>
</details>

## About
Labyrinth was created as a submission for the [LOWREZJAM 2021][lowrez]. Our goal with making Labyrinth was to expand our knowledge of programming, game development, and gain experience working on a longer-term group programming project. As a team we had 2 weeks to create a game within the limits of 64x64 resolution. As fans of the roguelite genre we decided to endeavour on one of our own. Using assets created by Krishna Palacio, we programmed a 3 floor 1 boss floor roguelite dungeon crawler game in C# using the Unity Engine. 

Check out the game on itch.io! [itch.io page][game]

## Level Generation 
A key aspect of any roguelite game is procedural generation. Without procedural generation players would just play the same map over and over again, and no one likes that right? (****cough*** ***cough*** MOBA players*).

[`Level Generation Controller Source Code`](https://github.com/Samalmeida1028/lowResGame/blob/a2a0b3185cb3981d30ac67db3006e8eb17250761/GameBoy/Assets/Scripts/Level%20Generation/LevelGenerator.cs)

### _The Algorithm_
For Labyrinth we used [**Binary Space Partitioning**](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation) (hereafter referred to as **BSP**) to procedurally generate dungeon rooms. **BSP** is just a method for dividing a large area up into smaller pieces, which for the case of Labyrinth happens to be dividing a dungeon into many smaller rooms.

**BSP** is a recursive binary tree algorithim that works like this:
```
  1. Pick a random direction (horizontal or vertical)
  2. Pick a random position (x for vertical y for horizontal)
  3. Split the space into two sub-spaces along that position line
  4. Repeat the process on the sub-spaces
```
<div class="row">
  <img src="https://sites.google.com/site/jicenospam/dungeon_bsp1.png" alt="Rogue Basin Example 1" width=375 height=225>
  <img src="https://sites.google.com/site/jicenospam/dungeon_bsp2.png" alt="Rogue Basin Example 2" width=375 height=225> 
</div>

##### First two splits of the space *(Left- first split, Right- second split)*: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

### _The Dungeon Tree_
In order to implement this in Unity, we had to create a custom binary tree structure that could hold all of our data. Since the tree would be comprised of many sub-```Dungeons``` we decided to name our tree-nodes ```Dungeons```:
```
public class Dungeon
{
    public float width;
    public float height;
    public Vector2 botLeft;
    public Vector2 topRight;

    public int depth;

    public Dungeon parentDungeon;
    public Dungeon leftDungeon;
    public Dungeon rightDungeon;


    public Split split = null;

    public bool failSplit = false;

    public Room room = null;

    public Dungeon (Vector2 botLeft, Vector2 topRight, int depth);
    
    ...(getter and setter methods in source code)
```
A ```Dungeon``` Node stored various values related to its position and size in the world and also its parent and children dungeons. The ```Split``` refers to the line on which the current ```Dungeon``` would be split into two sub-```Dungeons``` shown here:
```
public class Split
{
    public float x;
    public float y;
    public bool horizontal;

    public Split(float x, float y, bool horizontal)
    {
        this.x=x;
        this.y=y;
        this.horizontal=horizontal;
    }
}
```
The main ```Dungeon``` Node would perform ```log(base 2)``` splits to achieve the desired number of rooms. For example after ```4``` splitting iterations the main ```Dungeon``` would be ```Split``` into ```16``` terminal Child-```Dungeon``` Nodes:

<img src="https://sites.google.com/site/jicenospam/dungeon_bsp3.png" alt="Rogue Basin Example 3" width=400 height=400>

##### Example Dungeon after 4 splitting iterations (dotted white lines are splits): [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

### _The Rooms_
Once the desired number of terminal nodes is achieved, the ```Room``` generation begins. In each terminal Child-```Dungeon``` Node the level generator script will attempt to generate a randomly sized ```Room``` (within certain specifications). If successful, a ```Room``` datatype will be stored within the node for use when instantiating the game objects.
```
public class Room
{
    public int roomNumber;
    public Rect roomDimensions;
    public Vector2 botLeft;
    public Vector2 topRight;
    public Vector2 roomCenter;
    

    public Room (int roomNumber, float width, float height, Vector2 botLeft, Vector2 topRight, Vector2 roomCenter){
        
        this.roomNumber = roomNumber;
        this.botLeft = botLeft;
        this.topRight = topRight;
        this.roomCenter = roomCenter;

        this.roomDimensions = new Rect(botLeft.x,botLeft.y,width,height);

    }

}
```
<img src="https://sites.google.com/site/jicenospam/dungeon_bsp4.png" alt="Rogue Basin Example 4" width=400 height=400>

##### Example room generation: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

### _The Hallways_
Now the game wouldn't be very fun if the player was unable to travel from ```Room``` to ```Room``` would it? Thats where the ```Hallways``` come in. By reversing back up the tree and connecting the ```Rooms``` in each ```Dungeon``` Node with a ```Hallway```, we were able to ensure that every ```Room``` would be connected. 

[`Hallway Generation Script Source Code`](https://github.com/Samalmeida1028/lowResGame/blob/a2a0b3185cb3981d30ac67db3006e8eb17250761/GameBoy/Assets/Scripts/Level%20Generation/LevelGenerator.cs#L328-L427)

<img src="https://sites.google.com/site/jicenospam/dungeon_bsp5.png" alt="Rogue Basin Example 4" width=400 height=400>

##### Connecting rooms at the deepest layer of the tree: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

<img src="https://sites.google.com/site/jicenospam/dungeon_bsp6.png" alt="Rogue Basin Example 4" width=400 height=400>

##### Connecting rooms one depth up from the previous image: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

### _Populating the Rooms_
After doing all the previous steps, populating the ```Rooms``` was a piece of cake! *(I wish...)*. In all seriousness, thanks to the previous preparations and the custom datatypes created, populating the ```Rooms``` with enemies, loot, etc. was made much easier. After assigning certain ```Rooms``` to be special types (start, end, shop) all that was left was to randomly spawn loot and enemies for the player to fight based off of a difficulty level that increased with each floor. ```Enemies``` and loot were spawned in a similar manner, they were confined towards the center of the room dimensions and a random spawn-point was picked. If the spawn-point already had an ```Enemy``` or loot on it, then another spawn-point would be chosen.

[`Room Population Script Source Code`](https://github.com/Samalmeida1028/lowResGame/blob/a2a0b3185cb3981d30ac67db3006e8eb17250761/GameBoy/Assets/Scripts/Level%20Generation/LevelGenerator.cs#L628-L751)

### _Wrapping Up_
All of the source code for Level Generation can be found here [```lowResGame/GameBoy/Assets/Scripts/Level Generation/```](https://github.com/Samalmeida1028/lowResGame/tree/main/GameBoy/Assets/Scripts/Level%20Generation)

That just about wraps up this brief summary of Labyrinth's level generation, now lets learn about the people behind this mediocre at best game. 

## Developers

**Caleb Scopetski**
Utilized procedural generation techniques to develop level generation and entity spawning systems. 
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][calebgithub]

**Samuel Almeida**
Developed the basis for the enemy/combat, interaction and inventory systems within the game
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][samgithub]

**Patrick Walsh**
Took charge of the audio for Labyrinth. 
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][patgithub]

**Ethan Ferrabelo**
Worked on the Enemy Pathfinding Logic and Animation System for Labyrinth.
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][ethangithub]

## Game Jam Results
Labyrinth ended up coming #143 out of 337 different submissions. Although we didn't place as high as we would've liked, we achieved our main goal of learning. Throughout the process we learned about working as a team on a larger programming project, the ins and outs of Unity, and efficiently using version control systems (Github).

Check out the game on itch.io! [itch.io page][game]

[game]: https://cscopetski.itch.io/labyrinth
[ethangithub]: https://github.com/eferrabelo1114
[samgithub]: https://github.com/Samalmeida1028
[patgithub]: https://github.com/pw42020
[calebgithub]: https://github.com/cscopetski
[lowrez]: https://itch.io/jam/lowrezjam-2021
