using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Match3 : MonoBehaviour
{
    private enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Gameplay")]
    public int width = 9;
    public int height = 9;
    [SerializeField] private Difficulty difficulty = Difficulty.Easy;
    [Tooltip("Medium and Hard only")] public float blockChance = 0.1f;
    [Tooltip("Hard only")] public float hazardChance = 0.02f;
    public int targetScore = 1000;
    public int totalMoves = 20;
    public int pieceValue = 1;

    [Header("Assets")]
    public GameObject gameBoard;
    public GameObject gemPrefab;
    public Sprite[] pieces;

    [Header("Statistics")]
    public int score = 0;
    public int moves = 0;
    public int multiplier = 1;

    Gem[,] board;
    GemPiece selectedPiece;

    #region FUNCTION
    public void BuildGame()
    {
        board = new Gem[width, height];

        score = 0;
        moves = 0;
        multiplier = 1;

        InitializeBoard();
        InstantiateBoard();

        CleanBoard();
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
                Gem gem = board[row, col];
                gem.piece = Instantiate(gemPrefab, gameBoard.transform).GetComponent<GemPiece>();
                gem.piece.manager = this;
                gem.piece.Initialize(gem.location, getSpriteFromType(gem.type), xScale, yScale);
            }
        }
    }

    public void LockBoard(bool locked)
    {
        GetComponent<GraphicRaycaster>().enabled = !locked;
    }

    private Gem BuildGem(int row, int col)
    {
        GemType type = ChooseRandomType(); //default as blank
        return new Gem(type, new Point(row, col));
    }

    private GemType ChooseRandomType()
    {
        int type = 0; //default as blank
        switch (difficulty)
        {
            case Difficulty.Hard: //chance of choosing hazard
                if (UnityEngine.Random.value <= hazardChance)
                {
                    type = (int)GemType.Hazard;
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
                type = UnityEngine.Random.Range((int)GemType.Square, (int)GemType.Star + 1);
                break;
        }

        return (GemType)type;
    }

    private void AddScore(int value)
    {
        score += value * multiplier;
    }

    private void CleanBoard()
    {
        List<Gem> matches = CheckBoard();
        if (matches.Count == 0)
        {
            LockBoard(false);
            multiplier = 1; //reset multiplier
            return; //No matches to clean
        }

        LockBoard(true);
        //Reset the matched gems
        foreach (Gem gem in matches)
        {
            //Add score
            if (gem.type == GemType.Hazard)
                AddScore(-pieceValue);
            else
                AddScore(pieceValue);

            //Move column down
            MoveToTop(gem);
        }

        //Check for Chains
        multiplier++;
        StartCoroutine(WaitToClean(1f));
    }

    IEnumerator WaitToClean(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        CleanBoard();
    }

    private void MoveToTop(Gem gem)
    {
        Point target = gem.location;
        Point top = new Point(gem.location.x, 0);
        float xScale = 960f / width;
        float yScale = 960f / height;

        //move target gem up
        for (int i = target.y; i > 0; i--)
        {
            Point a = new Point(target.x, i);
            Point b = new Point(target.x, i - 1); //last loop will be y = 0
            //swap board gem up
            SwapGems(a, b);
            //update gem points
            getGemAtPoint(a).location = a;
            getGemAtPoint(a).piece.SetPoint(a);
            getGemAtPoint(b).location = b;
            getGemAtPoint(b).piece.SetPoint(b);
            StartCoroutine(getGemAtPoint(a).piece.FallToPos());
        }
        //reset the top gem
        getGemAtPoint(top).type = ChooseRandomType();
        getGemAtPoint(top).piece.Initialize
            (top, getSpriteFromType(getGemAtPoint(top).type), xScale, yScale);
    }

    public void SwapPiece(GemPiece piece)
    {
        if (selectedPiece == null)
        { //First piece selected
            selectedPiece = piece;
            selectedPiece.selection.enabled = true;
        }
        else //Second piece selected
        {
            Point selectedLoc = selectedPiece.location;
            Point pieceLoc = piece.location;

            //Same piece => deselect
            if (selectedLoc == piece.location)
            {
                selectedPiece.selection.enabled = false;
                selectedPiece = null;
            }
            //Different piece => check up range
            else if (selectedLoc + Point.up == pieceLoc ||
                selectedLoc + Point.down == pieceLoc ||
                selectedLoc + Point.left == pieceLoc ||
                selectedLoc + Point.right == pieceLoc)
            {
                //Increment moves and deselect
                moves++;
                selectedPiece.selection.enabled = false;
                //swap gems in board
                SwapGems(selectedLoc, pieceLoc);
                //play "animation" then check board
                StartCoroutine(WaitForSwap(selectedPiece, piece));
                selectedPiece = null;
            }
        }
    }

    private void SwapGems(Point a, Point b)
    {
        var temp = board[a.x, a.y];
        board[a.x, a.y] = board[b.x, b.y];
        board[b.x, b.y] = temp;
        getGemAtPoint(a).location = a;
        getGemAtPoint(a).piece.SetPoint(a);
        getGemAtPoint(b).location = b;
        getGemAtPoint(b).piece.SetPoint(b);
    }

    IEnumerator WaitForSwap(GemPiece a, GemPiece b)
    {
        float swapSpeed = 5f;
        //play swaps
        a.StartCoroutine(a.FallToPos(swapSpeed));
        //wait the swap
        yield return b.StartCoroutine(b.FallToPos(swapSpeed));
        //Clean up matches
        CleanBoard();
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

    GemType getTypeAtPoint(Point p) { return board[p.x, p.y].type; }
    Gem getGemAtPoint(Point p) { return board[p.x, p.y]; }
    Sprite getSpriteFromType(GemType type)
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


    private List<Gem> CheckBoard()
    {
        List<Gem> matchedGems = new List<Gem>();
        for (int col = 0; col < height; col++)
        {
            for (int row = 0; row < width; row++)
            {
                if (board[row, col].type < GemType.Square) continue; //Ignore blocks and hazards
                List<Gem> match = ConnectedGems(board[row, col]);
                if (match != null && match.Count >= 3)
                {
                    JoinLists(ref matchedGems, match);
                }
            }
        }

        return matchedGems;
    }

    //return false for out of bounds, other wise if two gems match
    bool checkGem(Point location, Point direction)
    {
        Point dest = location + direction;
        //Check Bounds
        if (dest.x >= width || dest.x < 0
            || dest.y >= height || dest.y < 0)
            return false;
        //Check match at destination
        return getGemAtPoint(location).type == getGemAtPoint(dest).type;
    }

    //Check for a gem for matches in all directions, recursive for each direction as matches found
    List<Gem> ConnectedGems(Gem point)
    {
        List<Gem> connected = new List<Gem>();
        //check all directions
        for (int i = 0; i < directions.Length; i++)
        {
            //Check matches in a line
            List<Gem> fill = new List<Gem>();
            CheckDirection(ref fill, point.location, i);
            //more than 2 matched symbols will add to total list
            if (fill.Count >= 3)
                JoinLists(ref connected, fill);
        }
        if (connected.Count == 0) connected = null;
        return connected;
    }

    //Recursively check in a direction
    void CheckDirection(ref List<Gem> matches, Point point, int dir)
    {
        //Ignore Blocks
        if (getGemAtPoint(point).type == GemType.Block)
            return; 

        //Add current point as a matchable
        matches.Add(getGemAtPoint(point));

        //Check match in next spot
        if (checkGem(point, directions[dir]))
        {
            //Add any matching points in that direction
            //to the connected list
            CheckDirection(ref matches, point + directions[dir], dir);
        }
    }

    //Adds Gem from add to target, ignoring repeat locations
    void JoinLists(ref List<Gem> target, List<Gem> add)
    {
        if (add == null) return; //Do nothing

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
