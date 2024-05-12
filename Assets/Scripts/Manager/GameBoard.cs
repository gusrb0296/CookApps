using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Cell_Class

/// <summary>
/// GameBoard를 이루는 한칸 한칸의 Cell
/// </summary>
public class Cell
{
    private int cell_x, cell_y;

    Character characterInCell;


    private bool isOccupied;
    public bool IsOccupied { get { return isOccupied; } set { } }


    private Vector3 cell_position;
    public Vector3 position => cell_position;

    public Cell(Vector3 offset, Vector3 size, int x, int y)
    {
        Vector3 dx = new Vector3(size.x, 0, 0);
        Vector3 dy = new Vector3(0, size.y, 0);
        cell_x = x;
        cell_y = y;
        cell_position = offset + cell_x * dx + cell_y * dy;
    }

    public void SetCharacter(Character character)
    {
        if (character == null)
        {
            isOccupied = false;
        }
        else
        {
            characterInCell = character;
            isOccupied = true;
        }
    }

    //    public Cannon GetCannon()
    //    {
    //        if (m_isOccupied)
    //        {
    //            return m_cannon;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    public void SetIsOccupied(bool value)
    {
        isOccupied = value;
    }
}

#endregion


public class GameBoard : MonoBehaviour
{
    [SerializeField] Button btn_test;
    [SerializeField] GameObject redDot;

    // 좌측 하단을 시작으로 우측 상단까지 Board를 그림
    public Transform leftBottom, rightTop;

    //private int[] dx = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
    //private int[] dy = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 };

    private Cell[,] m_gameBoard;
    private int m_width, m_height;

    public int Width => m_width;
    public int Height => m_height;

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

    Cell GetCell(float x, float y)
    {
        int _x = Mathf.RoundToInt((x - m_offset.x) / m_cellSize.x);
        int _y = Mathf.RoundToInt((y - m_offset.y) / m_cellSize.y);
        _x = Mathf.Clamp(_x, 0, m_width - 1);
        _y = Mathf.Clamp(_y, 0, m_height - 1);

        if (!m_gameBoard[_x, _y].IsOccupied)
            return m_gameBoard[_x, _y];
        else
            return null;
    }

    //public List<Cell> GetAdjacentCells(Vector3 v)
    //{
    //    return GetAdjacentCells(v.x, v.y);
    //}

    private int[] dx = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
    private int[] dy = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 };

    //List<Cell> GetAdjacentCells(float x, float y)
    //{
    //    int _x = Mathf.RoundToInt((x - m_offset.x) / m_cellSize.x);
    //    int _y = Mathf.RoundToInt((y - m_offset.y) / m_cellSize.y);
    //    _x = Mathf.Clamp(_x, 0, m_width - 1);
    //    _y = Mathf.Clamp(_y, 0, m_height - 1);
    //    List<Cell> cells = new List<Cell>();
    //    for (int i = 0; i < 8; i++)
    //    {
    //        if (_x + dx[i] < 0 || _y + dy[i] < 0 || _x + dx[i] >= m_width || _y + dy[i] >= m_height) continue;


    //        cells.Add(m_gameBoard[_x + dx[i], _y + dy[i]]);
    //    }
    //    return cells;
    //}

    private List<Cell> GetAdjacentCells(Vector2 position)
    {
        int x = Mathf.RoundToInt((position.x - m_offset.x) / m_cellSize.x);
        int y = Mathf.RoundToInt((position.y - m_offset.y) / m_cellSize.y);
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < 4; i++)  // 4 방향만 고려 (상하좌우)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx >= 0 && ny >= 0 && nx < m_width && ny < m_height)
            {
                cells.Add(m_gameBoard[nx, ny]);
            }
        }

        return cells;
    }

    public List<Cell> FindPath(Cell startCell, List<Vector2> targetPositions)
    {
        // 목표 셀로 변환
        List<Cell> targetCells = new List<Cell>();
        foreach (Vector2 pos in targetPositions)
        {
            Cell cell = GetCell(pos);
            if (cell != null && !cell.IsOccupied)
                targetCells.Add(cell);
        }

        // BFS 초기화
        Queue<Cell> queue = new Queue<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        queue.Enqueue(startCell);
        cameFrom[startCell] = null;

        // BFS 실행
        Cell current = null;
        while (queue.Count > 0)
        {
            current = queue.Dequeue();

            // 목표 도달 시 중단
            if (targetCells.Contains(current))
                break;

            foreach (Cell neighbor in GetAdjacentCells(new Vector2(current.position.x, current.position.y)))
            {
                if (neighbor != null && !cameFrom.ContainsKey(neighbor) && !neighbor.IsOccupied)
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // 경로 재구성
        List<Cell> path = new List<Cell>();
        if (targetCells.Contains(current))
        {
            while (current != null)
            {
                path.Add(current);
                MakeRedDot(current.position);
                current = cameFrom[current];
            }
            path.Reverse();
        }

        return path;
    }

    public void MakeRedDot(Vector3 pos)
    {
        Instantiate(redDot, pos, Quaternion.identity);
    }


    #region DrawGizmos

    // 게임을 시작하고, 에디터 상으로 hierarchy의 GameBoard를 클릭하면 그리드가 보입니다.
    void OnDrawGizmosSelected()
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

    #endregion
}