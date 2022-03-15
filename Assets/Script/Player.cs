using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Type pawnType;

    // Update is called once per frame
    void Update()
    {
        if (pawnType == GameManager.Turn)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Cell cell = hit.collider.gameObject.GetComponent<Cell>();
                    if (cell != null)
                    {
                        GameManager.PutPawn(cell.pos, pawnType);
                    }
                }
            }
        }
    }
}
