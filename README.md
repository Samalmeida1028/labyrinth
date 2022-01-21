## [Labyrinth][game]
A roguelite dungeon crawler developed by Caleb Scopteski, Patrick Walsh, Sam Almeida, and Ethan Ferrabelo for [LOWREZJAM 2021][lowrez]


<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#developers">Developers</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#level-generation">Level Generation</a>
      <ul>
        <li><a href="#the-algorithm">The Algorithm</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

## Developers

**Caleb Scopetski**
Programmer and Designer, utilized procedural generation techniques to develop level generation and entity spawning systems. 
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][calebgithub]

**Samuel Almeida**
Programmer and Designer, developed the basis for the enemy/combat, interaction and inventory systems within the game
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][samgithub]

**Patrick Walsh**
Programmer and Designer, joining the project late, took charge of the audio for Labyrinth. 
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][patgithub]

**Ethan Ferrabelo**
Programmer and Designer, most notably worked on the Enemy Pathfinding Logic and Animation System for Labyrinth.
<br />
[<img width="22px" src="https://cdn-icons-png.flaticon.com/512/270/270798.png" />][ethangithub]

## Development
Us 4 had two weeks to create a game within the limits of 64x64 resolution. As fans of the rougelite genre we decided to endeavour on one of our own. Using assets created by Krishna Palacio, we programmed a 3 floor 1 boss rogutelite dungeon game in C# using the Unity Engine. This being our second game jam ever, we still had to face constant challenges due to our ignorance towards game development in Unity. However, we believe we did an extradonary job, and as a group, are extremely proud of what we created. There are many MANY areas of this game that could be better, but the two-week development journey was an experience we will be grateful for. 

## Game Jam results
Labyrinth ended up coming #143 out of 337 differnet submissions. We excleed in some aspects, but lacked in others. Learning a lot about Teamwork, Programming, and Game Design, we consider this game jam an extremely fun expereince where we all walked away knowing a little more about programming than we did beforehand. 

You can check out the game on its [itch.io page][game]

## Level Generation
A key aspect of any roguelite game is procedural generation. Without procedural generation players would just play the same map over and over again, and no one likes that right? (****cough*** ***cough*** MOBA players*).

### _The Algorithm_
For Labyrinth we used [**Binary Space Partitioning**](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation) (hereafter referred to as **BSP**) to procedurally generate dungeon rooms. **BSP** is just a method for dividing a large area up into smaller pieces, which for the case of Labyrinth happens to be dividing a dungeon into many smaller rooms.

**BSP** is a recursive binary tree algorithim that works like this:
```
  1. Pick a random direction (horizontal or vertical)
  2. Pick a random position (x for vertical y for horizontal)
  3. Split the space into two sub-spaces along that position line
  4. Repeat the process on the sub-spaces
```
#### **Splits 1 and 2:**
<div class="row">
  <img src="https://sites.google.com/site/jicenospam/dungeon_bsp1.png" alt="Rogue Basin Example 1" width=375 height=225>
  <img src="https://sites.google.com/site/jicenospam/dungeon_bsp2.png" alt="Rogue Basin Example 2" width=375 height=225> 
</div>

##### Image Source: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

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

```
public void generateHall(Dungeon lDungeon, Dungeon rDungeon)
    {
        
        if(!(lDungeon.failSplit||rDungeon.failSplit))
        { 
            Room lRoom = lDungeon.getRoom();
            Room rRoom = rDungeon.getRoom();


            Vector2 dirVector = lRoom.roomCenter-rRoom.roomCenter;
            int side = 0;
               
            /**
            *   Calculate which side of the room to create the corridor on
            **/
            if(Mathf.Abs(dirVector.x)>Mathf.Abs(dirVector.y))
            {
                
                //horizontal
                if(dirVector.x>0)
                {
                    //Left Side
                    side = 0;
                }
                else
                {
                    //Right Side
                    side = 2;
                }
            }
            else
            {
                //vertical
                if(dirVector.y<0)
                {
                    //Top Side
                    side = 1;
                }
                else
                {
                    //Bottom Side
                    side = 3;
                }

            }

            Vector2 vertRange = new Vector2(0,0);
            Vector2 horzRange = new Vector2(0,0);

            Vector2 startPos = new Vector2(0,0);
            Vector2 meetPos = new Vector2(0,0); 
            Vector2 endPos = new Vector2(0,0);
            
            //Calculate whether or not the hall will have to make a turn in order to reach the desired room
            bool turn = ((Mathf.Abs(dirVector.x) - (lRoom.roomDimensions.width/2 + rRoom.roomDimensions.width/2)) >=0 && (Mathf.Abs(dirVector.y) - (lRoom.roomDimensions.height/2 + rRoom.roomDimensions.height/2)) >=0 );

            //If the hall must turn, three points must be randomly generated, the start point, meet point and end point
            if(turn)
            {
                
                if(side == 0||side ==2)
                {
                    vertRange = new Vector2(lRoom.roomDimensions.yMin,lRoom.roomDimensions.yMax);
                    horzRange = new Vector2(rRoom.roomDimensions.xMin,rRoom.roomDimensions.xMax);

                    startPos = new Vector2((side==0 ? lRoom.roomDimensions.xMin : lRoom.roomDimensions.xMax),round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    meetPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),startPos.y);
                    endPos = new Vector2(meetPos.x,(dirVector.y>0 ? rRoom.roomDimensions.yMax : rRoom.roomDimensions.yMin));
                }
                else if (side ==1||side ==3)
                {
                    vertRange = new Vector2(rRoom.roomDimensions.yMin,rRoom.roomDimensions.yMax);
                    horzRange = new Vector2(lRoom.roomDimensions.xMin,lRoom.roomDimensions.xMax);
                    startPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),(side==1 ? lRoom.roomDimensions.yMax : lRoom.roomDimensions.yMin));
                    meetPos = new Vector2(startPos.x,round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    endPos = new Vector2((dirVector.x>0 ? rRoom.roomDimensions.xMax : rRoom.roomDimensions.xMin),meetPos.y);
                }
                drawHall(startPos,meetPos);
                drawHall(meetPos,endPos);
            }
            //If the hall doesn't have to turn only a start and end point are reandomly generated
            else
            {

                if(side == 0||side ==2)
                {
                    vertRange = new Vector2((lRoom.botLeft.y>rRoom.botLeft.y ? lRoom.botLeft.y : rRoom.botLeft.y),(lRoom.topRight.y>rRoom.topRight.y ? rRoom.topRight.y : lRoom.topRight.y));
                    startPos = new Vector2((side==0 ? lRoom.roomDimensions.xMin : lRoom.roomDimensions.xMax),round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    endPos = new Vector2((side==0 ? rRoom.roomDimensions.xMax : rRoom.roomDimensions.xMin),startPos.y);
                }
                else if (side ==1||side ==3)
                {
                    horzRange = new Vector2((lRoom.botLeft.x>rRoom.botLeft.x ? lRoom.botLeft.x : rRoom.botLeft.x),(lRoom.topRight.x>rRoom.topRight.x ? rRoom.topRight.x : lRoom.topRight.x));
                    startPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),(side==1 ? lRoom.roomDimensions.yMax : lRoom.roomDimensions.yMin));
                    endPos = new Vector2(startPos.x, (side==1 ? rRoom.roomDimensions.yMin : rRoom.roomDimensions.yMax));
                }
                drawHall(startPos,endPos);
            }
        }
    }
```
<img src="https://sites.google.com/site/jicenospam/dungeon_bsp5.png" alt="Rogue Basin Example 4" width=400 height=400>

##### Connecting rooms at the deepest layer of the tree: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

<img src="https://sites.google.com/site/jicenospam/dungeon_bsp6.png" alt="Rogue Basin Example 4" width=400 height=400>

##### Connecting rooms one depth up from the previous image: [roguebasin.com](http://www.roguebasin.com/index.php/Basic_BSP_Dungeon_generation)

### _Populating the Rooms_
After doing all the previous steps, populating the ```Rooms``` was a piece of cake! *(I wish...)*. In all seriousness, thanks to the previous preparations and the custom datatypes created, populating the ```Rooms``` with enemies, loot, etc. was made much easier. After assigning certain ```Rooms``` to be special types (start, end, shop) all that was left was to randomly spawn loot and enemies for the player to fight based off of a difficulty level that increased with each floor. ```Enemies``` and loot were spawned in a similar manner, they were confined towards the center of the room dimensions and a random spawn-point was picked. If the spawn-point already had an ```Enemy``` or loot on it, then another spawn-point would be chosen.

```
public void populateRoom()
    {
        
        GameObject chest = chestPrefab;

        GameObject meleeEnemy = meleeEnemyPrefab;
        GameObject mageEnemy = mageEnemyPrefab;
        GameObject enemy;
        foreach (RoomObj room in roomList)
        {
            room.enemyCount = Random.Range(levelDifficulty,(int)Mathf.Round(levelDifficulty*3.25f));

            Vector2 min = room.botLeft;
            Vector2 max = room.topRight;
            
            bool pickChestSpawn = false;
            bool pickEnemySpawn = false;

            Vector3 chestSpawn = new Vector3(0,0,0);
            Vector3 enemySpawn = new Vector3(0,0,0);

            int chestCounter = 0;
            int enemyCounter = 0;

            if(room.isEndRoom==false){
                while(room.chestCount>0)
                {
                    pickChestSpawn=false;

                    while(!pickChestSpawn)
                    {   
                        if(chestCounter>((room.roomDimensions.width-2)*(room.roomDimensions.height-2)))
                        {
                            pickChestSpawn=true;
                        }
                        chestCounter++;
                        float x = round(Random.Range(min.x+2*tilePixelCount,max.x-tilePixelCount));
                        float y = round(Random.Range(min.y+2*tilePixelCount,max.y-tilePixelCount));

                        if(grid[(int)(x/tilePixelCount),(int)(y/tilePixelCount)]==0)
                        {
                            pickChestSpawn = true;
                            chestSpawn = new Vector3(x,y,0);
                        }
                    }

                    if(pickChestSpawn&&room.chestCount>0)
                    {
                        grid[(int)(chestSpawn.x/tilePixelCount),(int)(chestSpawn.y/tilePixelCount)]=1;
                        chest.transform.GetChild(0).gameObject.GetComponent<ChestActiveItem>().tierVal = Random.Range(levelDifficulty,levelDifficulty*2f);
                        chest.transform.GetChild(0).gameObject.GetComponent<ChestActiveItem>().isShop=false;
                        Instantiate(chest,chestSpawn,Quaternion.identity);
                        numChest++;
                        room.chestCount--;
                        chestCounter=0;
                    }
                }   
            }
            while(room.enemyCount>0)
            {
                pickEnemySpawn = false;

                while(!pickEnemySpawn)
                {
                    if(enemyCounter>((room.roomDimensions.width-2)*(room.roomDimensions.height-2)))
                    {
                        pickEnemySpawn=true;
                    }
                    enemyCounter++;
                    float x = round(Random.Range(min.x+2*tilePixelCount,max.x-tilePixelCount));
                    float y = round(Random.Range(min.y+2*tilePixelCount,max.y-tilePixelCount));

                    if(grid[(int)(x/tilePixelCount),(int)(y/tilePixelCount)]==0)
                    {
                        pickEnemySpawn = true;
                        enemySpawn = new Vector3(x,y,0);
                    }

                    if(pickEnemySpawn&&room.enemyCount>0)
                    {
                        grid[(int)(enemySpawn.x/tilePixelCount),(int)(enemySpawn.y/tilePixelCount)]=1;
                        if(Random.Range(0,10)<=2)
                        {
                            enemy = mageEnemy;
                        }else{
                            enemy = meleeEnemy;
                        }
                        enemy.transform.GetChild(0).gameObject.GetComponent<EnemyScript>().enemyTier = Random.Range(1+levelDifficulty/2,levelDifficulty*1.2f);
                        Instantiate(enemy,enemySpawn,Quaternion.identity);
                        room.enemyCount--;
                        enemyCount++;
                        enemyCounter=0;
                    }
                }
            }

            for (float i =min.y; i<=max.y; i+=tilePixelCount)
            {
            
                for (float j = min.x; j<=max.x; j+=tilePixelCount)
                {     
                    if(i-min.y==0||j-min.x==0||i-min.y==tilePixelCount||j-min.x==tilePixelCount||i==max.y-tilePixelCount||j==max.x-tilePixelCount)
                    {
                        bool spawnChance = false;

                        if((i<max.y/4&&j<max.x/4) || ((i>(max.y-max.y/4)&&(j>(max.x-max.x/4)))))
                        {
                            spawnChance = Random.Range(0,10)<3;
                        }else if((i<max.y/2.5&&j<max.x/2.5) || ((i>max.y-max.y/2.5&&j>max.x-max.x/2.5)))
                        {
                            spawnChance = Random.Range(0,10)<1;
                        }else if((i<max.y/2&&j<max.x/2) ||(i>max.y/2&&j>max.x/2))
                        {
                            spawnChance = Random.Range(0,10)<0;
                        }
                        if(spawnChance&& grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]==0)
                        {   
                            grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]=1;
                            Instantiate(destructableObj,new Vector3(j,i,0),Quaternion.identity);
                        }
                    }
                }
            }  
        }
    }
}
```
That just about wraps up the brief summary of Labyrinth's level generation. Since we were already on the topic of spawning ```Enemies```, let's move on to ```Enemy``` Pathfinding.

## Enemy Pathfinding
A* poggers

[game]: https://cscopetski.itch.io/labyrinth
[ethangithub]: https://github.com/eferrabelo1114
[samgithub]: https://github.com/Samalmeida1028
[patgithub]: https://github.com/pw42020
[calebgithub]: https://github.com/cscopetski
[lowrez]: https://itch.io/jam/lowrezjam-2021
