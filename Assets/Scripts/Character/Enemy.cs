using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    Cell currentCell;

    void Start()
    {
        currentCell = GameManager.Instance.gameBoard.GetCell(transform.position);
        GameManager.Instance.cell_EnemyPos = currentCell;
    }


}
