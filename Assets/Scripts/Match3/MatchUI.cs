using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchUI : OpenUI
{
    public Match3 match;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;

    protected override void EnableUI(bool enable)
    {
        base.EnableUI(enable);
        //Build the board on enter
        if (enable) match.BuildGame();
        //destroy all gems on leave
        else
        {
            WipeBoard();
        }
    }

    protected override void Start()
    { //stops double initiliazing
        //base.Start();
    }

    private void Update() //Update UIs
    {
        scoreText.text = match.score.ToString() + "/" + match.targetScore.ToString();
        int movesLeft = (match.totalMoves - match.moves);
        movesText.text = movesLeft.ToString();
        if (movesLeft <= 0)
        {
            match.LockBoard(true);
        }
    }

    public void WipeBoard()
    {
        foreach (Transform child in match.gameBoard.transform)
            Destroy(child.gameObject);
    }
}
