using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    [Header("General")]
    [SerializeField] Tilemap map;
    [SerializeField] Tilemap sections;

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

    bool currentlyGenerating;

    [Header("Land Gen")]
    [SerializeField] Tile good;
    [SerializeField] Tile bad;
    [SerializeField] Tile white;
    [SerializeField] int branchLoopCount;
    [SerializeField] int branchNumKept;
    [SerializeField] int idealTileCount;
    [SerializeField] int hardTileMin;
    [SerializeField] int hardTileCap; //Technically there can be one extra
    int branchLoopCur; //Current index in the branch loop

    [Header("Section Gen")]

    [SerializeField] int sectionWidth;
    [SerializeField] int sectionWidthVariance;
    [SerializeField] int sectionHeight;
    [SerializeField] int sectionHeightVariance;
    
    [Header("Public Info")]

    [HideInInspector] public List<List<Vector2Int>> sectionLists;
    [HideInInspector] public bool[,] waterArray;
    [HideInInspector] public Building[,] buildingArray;

    public static MapManager reference;

    void Awake()
    {
        reference = this;

        mapLeft = -mapWidth / 2;
        mapRight = mapWidth / 2;
        mapTop = mapHeight / 2;
        mapBottom = -mapHeight / 2;

        //This is only here for testing purposes
        waterArray = new bool[mapWidth,mapHeight];
        buildingArray = new Building[mapWidth,mapHeight];
    }

    void Start()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (map.GetTile<Tile>(new(mapLeft+i,mapTop-j-1)).gameObject != null)
                {
                    waterArray[i,j] = true;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!currentlyGenerating)
            {
                StartCoroutine(GenMap(result =>{}));
            } else
            {
                Debug.Log("Currently Generating");
            }
            
        }
    }

    public Vector2Int TransformPosToTilemapPos(Vector2 transformPos)
    {
        return new((int)(transformPos.x-0.5f) , ((int)(transformPos.y-0.5f) ));
    }

    public Vector2Int TilemapPosToArrayPos(Vector2Int tilemapPos)
    {
        return new(tilemapPos.x - mapLeft, -(tilemapPos.y + 1 - mapTop));
    }

    public bool IsArrayPosValid(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < mapWidth && pos.y < mapHeight);
    }

    public bool IsBuildingPosEmpty(Vector2Int pos)
    {
        return buildingArray[pos.x,pos.y] == null;
    }

    ///This is all map/section gen stuff

    public IEnumerator GenMap(Action<bool> callbackOnFinish)
    {
        currentlyGenerating = true;

        ////Generate Sections
        sectionLists = new();

        //Int array to represent the map, starts as all blank
        int[,] sectionArray = new int[mapWidth,mapHeight];

        //Generate Section Centers
        int numWidthCenters = mapWidth / sectionWidth;
        int numHeightCenters = mapHeight / sectionHeight;

        for (int i = 0; i < numWidthCenters; i++)
        {
            for (int j = 0; j < numHeightCenters; j++)
            {
                int sectionNum = (i*numHeightCenters)+j;

                int k = (sectionWidth / 2) + (i * sectionWidth) + UnityEngine.Random.Range(-sectionWidthVariance,sectionWidthVariance+1);
                int l = (sectionHeight / 2) + (j * sectionHeight) + UnityEngine.Random.Range(-sectionHeightVariance,sectionHeightVariance+1);

                sectionArray[k,l] = sectionNum+1; //Section number plus 1
                sectionLists.Add(new List<Vector2Int>());
                sectionLists[sectionNum].Add(new(k,l));
                SetTile(sections, k, l, white, (sectionNum+1) / 25f *3 % 1f); //set with color
            }
        }

        //Expand to all adjacent tiles each step going
        //through the array regularly then in reverse
        int sectionCount = sectionLists.Count;
        LinkedList<Vector2Int>[] sectionDeques = new LinkedList<Vector2Int>[sectionCount];
        for (int i = 0; i < sectionCount; i++)
        {
            sectionDeques[i] = new LinkedList<Vector2Int>();
            sectionDeques[i].AddLast(sectionLists[i][0]); // start with section center
        }

        bool tilesAdded = true;

        while (tilesAdded)
        {
            tilesAdded = false;

            //Go forward through sections
            for (int i = 0; i < sectionCount; i++)
            {
                LinkedList<Vector2Int> newDeque = new();

                foreach (Vector2Int tile in sectionDeques[i])
                {
                    List<Vector2Int> neighbors = GetPositionsAtDistance(tile, 1);

                    foreach (Vector2Int neighbor in neighbors)
                    {
                        if (IsTileValid(neighbor.x, neighbor.y) && sectionArray[neighbor.x, neighbor.y] == 0)
                        {
                            sectionArray[neighbor.x, neighbor.y] = i+1;
                            newDeque.AddLast(neighbor);
                            tilesAdded = true;
                            SetTile(sections, neighbor.x, neighbor.y, white, (i+1) / 25f *3 % 1f); //set with color
                        }
                    }
                }

                sectionDeques[i] = newDeque;
            }

            yield return null;

            if (!tilesAdded) break;
            tilesAdded = false;

            // Reverse pass
            for (int i = sectionCount - 1; i >= 0; i--)
            {
                LinkedList<Vector2Int> newDeque = new();

                foreach (Vector2Int tile in sectionDeques[i])
                {
                    List<Vector2Int> neighbors = GetPositionsAtDistance(tile, 1); // distance 1 neighbors

                    foreach (Vector2Int neighbor in neighbors)
                    {
                        if (IsTileValid(neighbor.x, neighbor.y) && sectionArray[neighbor.x, neighbor.y] == 0)
                        {
                            sectionArray[neighbor.x, neighbor.y] = i+1;
                            newDeque.AddLast(neighbor);
                            tilesAdded = true;
                            SetTile(sections, neighbor.x, neighbor.y, white, (i+1) / 25f *3 % 1f); //set with color
                        }
                    }
                }

                sectionDeques[i] = newDeque;
            }

            yield return null;
        }

        /*
        //i is distance, j is section num
        for(int i = 1; tilesAdded; i++)
        {
            tilesAdded = false;
            for (int j = 0; j < sectionCount; j++)
            {
                List<Vector2Int> newTiles = GetPositionsAtDistance(sectionLists[j][0],i);
                for (int k = 0; k < newTiles.Count; k++)
                {
                    if (IsTileValid(newTiles[k].x,newTiles[k].y) && sectionArray[newTiles[k].x,newTiles[k].y] == 0)
                    {
                        sectionArray[newTiles[k].x,newTiles[k].y] = j;
                        tilesAdded = true;
                        SetTile(newTiles[k].x,newTiles[k].y,good,j/25f);
                    }
                }
            }

            yield return null;

            if (!tilesAdded) {break;}
            tilesAdded = false;
            i++;

            for (int j = sectionCount-1; j >= 0; j--)
            {
                List<Vector2Int> newTiles = GetPositionsAtDistance(sectionLists[j][0],i);
                for (int k = 0; k < newTiles.Count; k++)
                {
                    if (IsTileValid(newTiles[k].x,newTiles[k].y) && sectionArray[newTiles[k].x,newTiles[k].y] == 0)
                    {
                        sectionArray[newTiles[k].x,newTiles[k].y] = j;
                        tilesAdded = true;
                        SetTile(newTiles[k].x,newTiles[k].y,good,j/25f);
                    }
                }
            }

            yield return null;
        }*/

        ////Generate Tiles
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
            if (!tilePlaced && RandomTileCanBePlaced(totalTilesPlaced))
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
                        SetTile(map, i,j,good);
                        break;

                    case TileTypes.Water:
                        SetTile(map, i,j,bad);
                        break;
                }

            }
        }

        waterArray = new bool[mapWidth,mapHeight];
        buildingArray = new Building[mapWidth,mapHeight];

        currentlyGenerating = false;
        callbackOnFinish(true);

    }

    List<Vector2Int> GetPositionsAtDistance(Vector2Int start, int distance)
    {
        List<Vector2Int> output = new();

        for (int i = -distance; i <= distance; i++)
        {
            output.Add(new(start.x+i , start.y-(distance-Math.Abs(i))));
            //Add bottom point if there is a height difference
            if (distance-Math.Abs(i) != 0)
            {
                output.Add(new(start.x+i , start.y+(distance-Math.Abs(i))));
            }
        }

        return output;
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

    bool RandomTileCanBePlaced(int totalTilesPlaced)
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

    void SetTile(Tilemap target, int x, int y, Tile type)
    {
        //y is subtracted since tilemap numbers go upwards as they increase
        target.SetTile(new(mapLeft+x,mapTop-y-1), type);
    }

    void SetTile(Tilemap target, int x, int y, Tile type, float colorVal)
    {
        //y is subtracted since tilemap numbers go upwards as they increase
        target.SetTile(new(mapLeft+x,mapTop-y-1), type);
        target.SetTileFlags(new(mapLeft+x,mapTop-y-1), TileFlags.None);
        target.SetColor(new(mapLeft+x,mapTop-y-1), Color.HSVToRGB(colorVal, 1, 1));
    }

    /*
    public IEnumerator OldGenMap(Action<bool> callbackOnFinish)
    {
        currentlyGenerating = true;
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
        callbackOnFinish(true);

    }
    */
}