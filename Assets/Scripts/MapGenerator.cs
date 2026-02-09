using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TestMapGen : MonoBehaviour
{

    public Tilemap tilemap;
    public Tile good;
    public Tile bad;

    [SerializeField]
    int mapWidth;
    [SerializeField]
    int mapHeight;

    int mapLeft;
    int mapRight;
    int mapTop;
    int mapBottom;

    enum TileTypes
    {
        Water,
        Land,
    }

    [SerializeField] int branchLoopCount;
    [SerializeField] int branchNumKept;
    [SerializeField] int idealTileCount;
    [SerializeField] int hardTileMin;
    [SerializeField] int hardTileCap; //Technically there can be one extra

    bool currentlyGenerating;

    int branchLoopCur; //Current index in the branch loop

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapLeft = -mapWidth / 2;
        mapRight = mapWidth / 2;
        mapTop = mapHeight / 2;
        mapBottom = -mapHeight / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tilemap.SetTile(RandomTilePos(), good);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            tilemap.SetTile(RandomTilePos(), bad);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!currentlyGenerating)
            {
                currentlyGenerating = true;
                StartCoroutine(nameof(GenMap));
            } else
            {
                Debug.Log("Currently Generating");
            }
            
        }
    }

    Vector3Int RandomTilePos() //Just for testing
    {
        return new Vector3Int(UnityEngine.Random.Range(mapLeft, mapRight), UnityEngine.Random.Range(mapBottom, mapTop), 0);
    }


    IEnumerator GenMap()
    {
        //Array to represent the map, starts as all water
        TileTypes[,] mapArray = new TileTypes[mapWidth,mapHeight];
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                mapArray[i,j] = TileTypes.Water;
            }
        }

        //Queue of coordinates
        List<Vector2Int> coordinateQueue = new();
        List<Vector2Int> branchCoordinateQueue = new();

        int totalTilesPlaced = 0;
        int recentLoopsTried = 0;

        System.Random seed = new System.Random();

        ////1. Spawn start Square
        int startWidth = (mapWidth % 2 == 1) ? 3 : 4;
        int startHeight = (mapHeight % 2 == 1) ? 3 : 4;

        for (int i = 0; i < startWidth; i++)
        {
            for (int j = 0; j < startHeight; j++)
            {
                AddTile(((mapWidth-1)/2)-1+i,((mapHeight-1)/2)-1+j,TileTypes.Land,coordinateQueue,mapArray);
            }
        }
        //Shuffle list
        for (int i = coordinateQueue.Count - 1; i > 0; i--)
        {
            int j = seed.Next(i + 1);
            (coordinateQueue[i], coordinateQueue[j]) = (coordinateQueue[j], coordinateQueue[i]);
        }
        

        ////2. Go through queue
        //Every placed square has a chance to expand itself
        while (coordinateQueue.Count > 0 || branchCoordinateQueue.Count > 0)
        {
            //Debug.Log("Iteration started");
            if (totalTilesPlaced >= hardTileCap)
            {
                break;
            }

            if (recentLoopsTried > 1000)
            {
                recentLoopsTried = 0;
                //Debug.Log("oneFrame");
                yield return null;
            }

            Vector2Int curCoords;
            if (branchLoopCur < Math.Min(branchLoopCount,totalTilesPlaced) && branchCoordinateQueue.Count > 0)
            {
                branchLoopCur++;
                int randomNum = UnityEngine.Random.Range(0,branchCoordinateQueue.Count);
                curCoords = branchCoordinateQueue[0];
                branchCoordinateQueue.RemoveAt(0);
            }
            else //Back to regular queue
            {
                //Add some of branch queue then get first from coord queue
                branchLoopCur = 0;
                for (int i = 0; i < Math.Min(branchNumKept,branchCoordinateQueue.Count); i++)
                {
                    coordinateQueue.Add(branchCoordinateQueue[i]);
                }
                //coordinateQueue.AddRange(branchCoordinateQueue);
                branchCoordinateQueue = new();

                curCoords = coordinateQueue[0];
                coordinateQueue.RemoveAt(0);
            }

            ///Skip if tile is already ground
            if (IsTileGround(curCoords.x,curCoords.y,mapArray))
            {
                continue;
            }

            bool tilePlaced = false;

            ///Count number of surrounding squares
            int surroundingGroundCount = 0;
            int subsequentGroundCount = 0;
            bool subsequentlyConnected = false; //For if 2+ subsequent blocks connect the target one
            List<int> foundGroundIndices = new();

            Vector2Int[] checkCoords = {new(-1,-1),new(0,-1),new(1,-1),new(1,0),new(1,1),new(0,1),new(-1,1),new(-1,0)};

            for (int i = 0; i < checkCoords.Length; i++)
            {

                if (IsTileGround(curCoords.x+checkCoords[i].x,curCoords.y+checkCoords[i].y,mapArray))
                {
                    surroundingGroundCount++;
                    foundGroundIndices.Add(i);
                    subsequentGroundCount++;
                    if (subsequentGroundCount > 1)
                    {
                        subsequentlyConnected = true;
                    }
                }
                else
                {
                    subsequentGroundCount = 0;
                }
                
            }

            //Check for end to start connection of check coords list
            if (subsequentGroundCount == 1)
            {
                if (IsTileGround(curCoords.x+checkCoords[0].x,curCoords.y+checkCoords[0].y,mapArray))
                {
                    subsequentlyConnected = true;
                }
            }

            ///A square surrounded on at least 5 adjacents gets filled
            if (surroundingGroundCount >= 5)
            {
                AddTile(curCoords.x,curCoords.y,TileTypes.Land,branchCoordinateQueue,mapArray);
                totalTilesPlaced++;
                tilePlaced = true;
            }

            ///Check chance to place block
            if (!tilePlaced && RandomTileCheck(totalTilesPlaced))
            {
                //Debug.Log("Random check passed");
                AddTile(curCoords.x,curCoords.y,TileTypes.Land,branchCoordinateQueue,mapArray);
                totalTilesPlaced++;
                tilePlaced = true;

                //Ensure is connected by 2 subsequent tiles
                //Otherwise add tile as needed
                if (!subsequentlyConnected)
                {
                    int randomIndex = foundGroundIndices[UnityEngine.Random.Range(0,foundGroundIndices.Count)];

                    randomIndex += UnityEngine.Random.Range(0,2) == 1 ? -1 : 1;

                    if (randomIndex < 0)
                    {
                        randomIndex = checkCoords.Length-1;
                    } 
                    else if (randomIndex >= checkCoords.Length)
                    {
                        randomIndex = 0;
                    }

                    AddTile(curCoords.x+checkCoords[randomIndex].x,curCoords.y+checkCoords[randomIndex].y,TileTypes.Land,branchCoordinateQueue,mapArray);
                    totalTilesPlaced++;
                }
            } else
            {
                //Debug.Log("Random check failed");
            }

            recentLoopsTried++;

        }


        ////3. Actually update tilemap
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {

                switch (mapArray[i,j])
                {
                    case TileTypes.Land:
                        SetTile(i,j,good);
                        break;

                    case TileTypes.Water:
                        SetTile(i,j,bad);
                        break;
                }

            }
        }

        currentlyGenerating = false;

    }

    void AddTile(int x, int y, TileTypes newType, List<Vector2Int> coordinateQueue, TileTypes[,] mapArray) //Always used with valid position
    {
        //Debug.Log("tile added");

        mapArray[x,y] = newType;

        ///Add nearby tiles to queue if tile was placed
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (IsTileValid(x+i,y+j) && !IsTileGround(x+i,y+j,mapArray))
                {
                    coordinateQueue.Add(new(x+i,y+j));
                    //Debug.Log("tile enqueued");
                }
            }
        }
    }

    bool RandomTileCheck(int totalTilesPlaced)
    {
        float randomNum = UnityEngine.Random.Range(0f,1f);
        //Debug.Log(""+randomNum +">"+(totalTilesPlaced*1f/hardTileCap));
        return totalTilesPlaced < hardTileMin || (randomNum > (totalTilesPlaced*1f/hardTileCap)); //TODO make this
    }

    bool IsTileValid(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < mapWidth && y < mapHeight);
    }

    bool IsTileGround(int x, int y, TileTypes[,] mapArray)
    {
        return IsTileValid(x,y) && mapArray[x,y] != TileTypes.Water;
    }

    void SetTile(int x, int y, Tile type)
    {
        //y is subtracted since tilemap numbers go upwards as they increase
        tilemap.SetTile(new Vector3Int(mapLeft+x,mapTop-y-1,0), type);
    }

}