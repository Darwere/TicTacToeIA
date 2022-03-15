using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Move
{
    public int row, col;
};

public class IAManager : MonoBehaviour
{
    [SerializeField] private Type myPawn;

    private Type opponent;

    private void Start()
    {
        if (myPawn == Type.cross)
            opponent = Type.circle;
        else
            opponent = Type.cross;
    }
    // Update is called once per frame
    void Update()
    {
        if(myPawn == GameManager.Turn)
        {
            Move move = FindBestMove();
            GameManager.PutPawn(new Vector2Int(move.row, move.col), myPawn);
        }
    }

    private bool IsMovesLeft()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (GameManager.PawnBoard[i, j] == Type.empty)
                    return true;
        return false;
    }

    private int Evaluate()
    {
        Type[,] board = GameManager.PawnBoard;
        // Checking for Rows for X or O victory.
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] &&
                board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == myPawn)
                    return +10;
                else if (board[row, 0] == opponent)
                    return -10;
            }
        }

        // Checking for Columns for X or O victory.
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] &&
                board[1, col] == board[2, col])
            {
                if (board[0, col] == myPawn)
                    return +10;

                else if (board[0, col] == opponent)
                    return -10;
            }
        }

        // Checking for Diagonals for X or O victory.
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == myPawn)
                return +10;
            else if (board[0, 0] == opponent)
                return -10;
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == myPawn)
                return +10;
            else if (board[0, 2] == opponent)
                return -10;
        }

        // Else if none of them have won then return 0
        return 0;
    }

    private int MinMax(int depth, bool myTurn)
    {
        int score = Evaluate();

        // If Maximizer has won the game
        // return his/her evaluated score
        if (score == 10)
            return score;

        // If Minimizer has won the game
        // return his/her evaluated score
        if (score == -10)
            return score;

        // If there are no more moves and
        // no winner then it is a tie
        if (IsMovesLeft() == false)
            return 0;

        // If this maximizer's move
        if (myTurn)
        {
            int best = -1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (GameManager.PawnBoard[i, j] == Type.empty)
                    {
                        // Make the move
                        GameManager.PawnBoard[i, j] = myPawn;

                        // Call minimax recursively and choose
                        // the maximum value
                        best = Mathf.Max(best, MinMax(depth + 1, !myTurn));

                        // Undo the move
                        GameManager.PawnBoard[i, j] = Type.empty;
                    }
                }
            }
            return best;
        }

        // If this minimizer's move
        else
        {
            int best = 1000;

            // Traverse all cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty
                    if (GameManager.PawnBoard[i, j] == Type.empty)
                    {
                        // Make the move
                        GameManager.PawnBoard[i, j] = opponent;

                        // Call minimax recursively and choose
                        // the minimum value
                        best = Mathf.Min(best, MinMax(depth + 1, !myTurn));

                        // Undo the move
                        GameManager.PawnBoard[i, j] = Type.empty;
                    }
                }
            }
            return best;
        }
    }

    private Move FindBestMove()
    {
        Type[,] board = GameManager.PawnBoard;
        int bestVal = -1000;
        Move bestMove = new Move();
        bestMove.row = -1;
        bestMove.col = -1;

        // Traverse all cells, evaluate minimax function
        // for all empty cells. And return the cell
        // with optimal value.
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Check if cell is empty
                if (board[i, j] == Type.empty)
                {
                    // Make the move
                    board[i, j] = myPawn;

                    // compute evaluation function for this
                    // move.
                    int moveVal = MinMax(0, false);

                    // Undo the move
                    board[i, j] = Type.empty;

                    // If the value of the current move is
                    // more than the best value, then update
                    // best/
                    if (moveVal > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveVal;
                    }
                }
            }
        }

        return bestMove;
    }
}
