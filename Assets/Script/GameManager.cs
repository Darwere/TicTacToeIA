using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Type
{
    empty,
    circle,
    cross
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private int length;

    private Type[,] pawns;

    private Type turn = Type.circle;

    public static Type Turn => instance.turn;
    public static Type[,] PawnBoard => instance.pawns; 

    [SerializeField] private List<Vector2Int> dir;

    private void Awake()
    {
        instance = this;

        pawns = new Type[length, length];

        for(int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
                pawns[x, y] = Type.empty;
        }
    }

    private bool isEmpty(Vector2Int pos)
    {
        if (instance.pawns[pos.x, pos.y] == Type.empty)
            return true;
        else
            return false;
    }

    public static void PutPawn(Vector2Int pos, Type pawnType)
    {
        if (!instance.isEmpty(pos) || pawnType == Type.empty)
            return;
        else
        {
            Board.AddPawn(pos, pawnType);
            instance.pawns[pos.x, pos.y] = pawnType;
            if (!instance.Win(pos, pawnType))
            {
                if (instance.IsMovesLeft())
                {
                    if (instance.turn == Type.circle)
                        instance.turn = Type.cross;
                    else
                        instance.turn = Type.circle;
                }
                else
                {
                    Debug.Log("Draw");
                    instance.Restart();
                }
            }
            else
            {
                if (Turn == Type.circle)
                    Debug.Log("Circle win");
                else
                    Debug.Log("Cross win");
                instance.Restart();
            }      
        }
    }

    private bool Win(Vector2Int pos , Type pawnType)
    {
        int compteur = 0;
        for(int i = 0; i < dir.Count; i++)
        {
            if (i % 2 == 0)
                compteur = 0;

            while(CheckNeighBoor(pos + compteur * dir[i], pawnType, dir[i]))
                compteur++;

            if (compteur >= 3)
                return true;
        }
        return false;
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private bool CheckNeighBoor(Vector2Int pos, Type pawnType, Vector2Int dir)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < length && pos.y < length)
            if (pawns[pos.x, pos.y] == pawnType)
                return true;

        return false;
    }

    private bool IsMovesLeft()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (PawnBoard[i, j] == Type.empty)
                    return true;
        return false;
    }
}
