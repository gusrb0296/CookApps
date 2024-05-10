using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public Transform leftBottom, rightTop;

    private int[] dx = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
    private int[] dy = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 };

    private Cell[,] m_gameBoard;
    private int m_width, m_height;
    public int Width
    {
        get { return m_width; }
    }
    public int Height
    {
        get { return m_height; }
    }
    private Vector3 m_offset;
    private Vector3 m_cellSize;


    void Start()
    {
        Generate(leftBottom, rightTop, 10, 5);
    }

  
    public void Generate(Transform leftBottom, Transform rightTop, int width, int height)
    {
        m_offset = leftBottom.position;
        m_cellSize = new Vector3((rightTop.position.x - leftBottom.position.x) / width, (rightTop.position.y - leftBottom.position.y) / height);
        m_offset += (m_cellSize / 2);

        m_gameBoard = new Cell[width, height];
        m_width = width;
        m_height = height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                m_gameBoard[i, j] = new Cell(m_offset, m_cellSize, i, j);
            }
        }

    }

    public Cell GetCell(Vector2 pos)
    {
        return GetCell(pos.x, pos.y);
    }

    public Cell GetCell(float x, float y)
    {
        int _x = Mathf.RoundToInt((x - m_offset.x) / m_cellSize.x);
        int _y = Mathf.RoundToInt((y - m_offset.y) / m_cellSize.y);
        _x = Mathf.Clamp(_x, 0, m_width - 1);
        _y = Mathf.Clamp(_y, 0, m_height - 1);
        return m_gameBoard[_x, _y];
    }

    public List<Cell> GetAdjacentCells(Vector3 v)
    {
        return GetAdjacentCells(v.x, v.y);
    }

    public List<Cell> GetAdjacentCells(float x, float y)
    {
        int _x = Mathf.RoundToInt((x - m_offset.x) / m_cellSize.x);
        int _y = Mathf.RoundToInt((y - m_offset.y) / m_cellSize.y);
        _x = Mathf.Clamp(_x, 0, m_width - 1);
        _y = Mathf.Clamp(_y, 0, m_height - 1);
        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < 8; i++)
        {
            if (_x + dx[i] < 0 || _y + dy[i] < 0 || _x + dx[i] >= m_width || _y + dy[i] >= m_height) continue;
            cells.Add(m_gameBoard[_x + dx[i], _y + dy[i]]);
        }
        return cells;
    }


    private void OnDrawGizmosSelected()
    {
        try
        {
            Debug.DrawLine(leftBottom.position, new Vector3(leftBottom.position.x, rightTop.position.y), Color.red);
            Debug.DrawLine(leftBottom.position, new Vector3(rightTop.position.x, leftBottom.position.y), Color.red);
            Debug.DrawLine(new Vector3(leftBottom.position.x, rightTop.position.y), rightTop.position, Color.red);
            Debug.DrawLine(new Vector3(rightTop.position.x, leftBottom.position.y), rightTop.position, Color.red);

            Vector3 dx = new Vector3(m_cellSize.x, 0, 0);
            Vector3 dy = new Vector3(0, m_cellSize.y, 0);
            for (int i = 0; i < m_width; i++)
            {
                Debug.DrawLine(m_offset + i * dx, m_offset + i * dx + (m_height - 1) * dy);
            }
            for (int j = 0; j < m_height; j++)
            {
                Debug.DrawLine(m_offset + j * dy, m_offset + j * dy + (m_width - 1) * dx);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}

public class Cell
{
    private int m_x, m_y;
    private Vector3 m_position;
    private bool m_isOccupied;
    public bool IsOccupied { get { return m_isOccupied; } }
    public Vector3 position
    {
        get { return m_position; }
    }

    public Cell(Vector3 offset, Vector3 size, int x, int y)
    {
        Vector3 dx = new Vector3(size.x, 0, 0);
        Vector3 dy = new Vector3(0, size.y, 0);
        m_x = x;
        m_y = y;
        m_position = offset + m_x * dx + m_y * dy;
    }

    //public void SetCannon(Cannon cannon)
    //{
    //    if (cannon == null)
    //    {
    //        m_isOccupied = false;
    //    }
    //    else
    //    {
    //        m_cannon = cannon;
    //        m_isOccupied = true;
    //    }
    //}

    //public Cannon GetCannon()
    //{
    //    if (m_isOccupied)
    //    {
    //        return m_cannon;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
}