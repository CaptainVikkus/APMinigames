using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Match3 : MonoBehaviour
{
    private enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public int width = 9;
    public int height = 9;
    [SerializeField] private Difficulty difficulty = Difficulty.Easy;
    [Tooltip("Medium and Hard only")] public float blockChance = 0.1f;
    [Tooltip("Hard only")] public float hazardChance = 0.02f;
    public GameObject gameBoard;
    public GameObject gemPrefab;
    public Sprite[] pieces;
    Gem[,] board;

    #region FUNCTION
    public void BuildGame()
    {
        board = new Gem[width, height];

        InitializeBoard();
        InstantiateBoard();
    }

    private void InitializeBoard()
    {
        for (int col = 0; col < height; col++)
        {
            for (int row = 0; row < width; row++)
            {
                board[row, col] = BuildGem(row, col);
            }
        }
    }
    private void InstantiateBoard()
    {
        float xScale = 960f / width;
        float yScale = 960f / height;

        for (int col = 0; col < height; col++)
        {
            for (int row = 0; row < width; row++)
            {
                float x = (xScale * row);
                float y = (yScale * col);
                GemPiece piece = Instantiate(gemPrefab, gameBoard.transform).GetComponent<GemPiece>();
                Gem gem = board[row, col];
                piece.Initialize(gem, getSpriteFromType(gem.type), xScale, yScale);
            }
        }
    }

    private Sprite getSpriteFromType(GemType type)
    {
        switch (type)
        {
            case GemType.Hazard:
                return pieces[pieces.Length - 2];
            case GemType.Block:
                return pieces[pieces.Length - 1];
            case GemType.Square:
                return pieces[0];
            case GemType.Circle:
                return pieces[1];
            case GemType.Triangle:
                return pieces[2];
            case GemType.Star:
                return pieces[3];
            default:
                return pieces[0];
        }
    }

    private Gem BuildGem(int row, int col)
    {
        int type = 0; //default as blank
        switch (difficulty)
        {
            case Difficulty.Hard: //chance of choosing hazard
                if (UnityEngine.Random.value <= hazardChance)
                {
                    type = (int)GemType.Block;
                    break;
                }
                else
                    goto case Difficulty.Medium;
            case Difficulty.Medium: //chance of choosing block (increased by difficulty)
                if (UnityEngine.Random.value <= blockChance * (float)difficulty)
                {
                    type = (int)GemType.Block;
                    break;
                }
                else
                    goto default;
            default:
                //choose a random regular gem type
                type = UnityEngine.Random.Range((int)GemType.Square, (int)GemType.Triangle + 1);
                break;
        }
        Debug.Log("Gem Created: " + ((GemType)type).ToString());
        return new Gem((GemType)type, new Point(row, col));
    }

    #endregion

    #region LOGIC

    Point[] directions =
    {
        Point.up,
        Point.right,
        Point.down,
        Point.left
    };

    private void CheckBoard()
    {
        List<Gem> matchedGems = new List<Gem>();
        for (int col = 0; col < height; col++)
        {
            for (int row = 0; row < width; row++)
            {
                if (board[row, col].type < GemType.Square) continue; //Ignore blocks and hazards
                List<Gem> match = ConnectedGems(board[row, col]);
                if (match.Count >= 3)
                {
                    JoinLists(ref matchedGems, match);
                }
            }
        }
    }

    GemType getTypeAtPoint(Point p) { return board[p.x, p.y].type; }
    Gem getGemAtPoint(Point p) { return board[p.x, p.y]; }

    bool checkGem(Point location, Point direction)
    {
        Point dest = location + direction;
        //Check Bounds
        if (dest.x > width || dest.x < 0
            || dest.y > height || dest.y < 0)
            return false;
        //Check match at destination
        return getGemAtPoint(location) == getGemAtPoint(dest);
    }

    //Check for a gem for matches in all directions, recursive for each direction as matches found
    List<Gem> ConnectedGems(Gem point, int dir = -1)
    {
        List<Gem> connected = new List<Gem>();
        // dir == -1 indicates first call
        if (dir == -1)
        {   //check all directions
            for (int i = 0; i < directions.Length; i++)
            {
                List<Gem> fill = new List<Gem>();
                //Check match in directoin
                if (checkGem(point.location, directions[i]))
                {
                    //Add current point as a matchable
                    fill.Add(point);
                    //Add any matching points in that direction
                    //to the connected list
                    Point dest = point.location + directions[i];
                    JoinLists(ref connected,
                        ConnectedGems(getGemAtPoint(dest), i));

                    //Add fill to connected if match of 3 or more found in direction
                    if (fill.Count >= 3)
                        JoinLists(ref connected, fill);
                }
            }
        }
        //Check continuous direction
        else
        {
            //Check match in direction 
            if (checkGem(point.location, directions[dir]))
            {
                //Add current point as a matchable
                connected.Add(point);
                //Add any matching points in that direction
                //to the connected list
                Point dest = point.location + directions[dir];
                JoinLists(ref connected,
                    ConnectedGems(getGemAtPoint(dest), dir));
            }
            //Otherwise just return the connected list
        }

        return connected;
    }

    //Adds Gem from add to target, ignoring repeat locations
    void JoinLists(ref List<Gem> target, List<Gem> add)
    {
        foreach (Gem gem in add)
        {
            bool isUnique = true;
            //Check if the gem is already in target list
            for (int i = 0; i < target.Count; i++)
            {
                if (target[i].location == gem.location)
                {
                    isUnique = false;
                    break;
                }
            }
            //Add if unique
            if (isUnique)
                target.Add(gem);
        }
    }
    #endregion

}
