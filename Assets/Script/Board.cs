using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Board : MonoBehaviour
{
    private static Board instance;

    [SerializeField] private int length;

    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private GameObject circle;

    [SerializeField] private GameObject cross;

    private GameObject[,] board;

    private void Awake()
    {
        instance = this;
        board = new GameObject[length, length];
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < length; x++)
        {

            for (int y = 0; y < length; y++)
            {
                board[x, y] = Instantiate(cellPrefab, transform.position + new Vector3(x, y), Quaternion.identity);
                Cell cell = board[x, y].GetComponent<Cell>();
                cell.pos = new Vector2Int(x, y);
            }

        }
    }


    public static void AddPawn(Vector2Int pos, Type pawnType)
    {
        if(pawnType == Type.circle)
            Instantiate(instance.circle, instance.board[pos.x, pos.y].transform.position, Quaternion.identity);
        else
            Instantiate(instance.cross, instance.board[pos.x, pos.y].transform.position, Quaternion.identity);
    }

}
