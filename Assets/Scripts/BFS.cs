using System;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MapController mapController = new MapController();
        // 시작 좌표와 목적지 좌표를 매개변수로 넘겨준다.
        mapController.BFS(0, 0, 5, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MapController
{
    // 탐색할 맵이다.
    public int[,] maps = new int[,]
    {
        {0, 0, 0, 0, 0, 0 },
        {0, 0, 1, 0, 0, 0 },
        {0, 0, 1, 0, 0, 0 },
        {0, 0, 1, 0, 0, 0 },
        {0, 0, 1, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0 }
    };

    // 상 하 좌 우 방향을 나타낸다.
    public int[,] direction = new int[,]
    {
        { -1,  0 },
        {  1,  0 },
        {  0, -1 },
        {  0,  1 },
    };

    // 한번 들른 길인지 체크한다.
    public bool[,] checkRoad = null;

    // 탐색
    public void BFS(int y, int x, int targetY, int targetX)
    {
        // 길 체크 플래그를 초기화해준다.
        ClearCheckRoad();

        // 가장 빠른길을 담을 노드를 선언한다.
        BFSNode bestNode = null;

        // 탐색하기 위한 BFSNode형 Queue를 만든다.
        // 시작지점을 가리키는 Node를 만들어서 담는다.
        // 시작지점은 한번 들른 길로서 체크한다.
        Queue<BFSNode> queue = new Queue<BFSNode>();
        queue.Enqueue(new BFSNode(y, x, null));
        checkRoad[y, x] = true;

        // 더이상 탐색할게 없을 때까지 루프한다.
        while (queue.Count > 0)
        {
            // 노드를 가져온다.
            BFSNode node = queue.Dequeue();

            // 현재 노드가 목표 지점일 경우
            if (node.Y == targetY && node.X == targetX)
            {
                // 가장 빠른길이 아직 없거나
                // 현재 노드가 가장 빠른길일 경우 bestNode에 담는다.
                // 이후 여기에 들어오는 Node들과 bestNode를 비교하여 가장 빠른 길을 담는다.
                if (bestNode == null || (bestNode.PrevCount > node.PrevCount))
                    bestNode = node;
            }

            // 상 하 좌 우를 체크한다.
            for (int i = 0; i < direction.GetLength(0); i++)
            {
                // 현재 노드의 위치 + (상or하or좌or우)
                int dy = node.Y + direction[i, 0];
                int dx = node.X + direction[i, 1];

                // 좌표가 맵의 범위안에 있으며
                // 갈 수 있는 길이고
                // 한 번도 들른적 없는 길인 경우
                if (CheckMapRange(dy, dx) && CheckMapWay(dy, dx) && !checkRoad[dy, dx])
                {
                    // 찾은 길은 노드를 만들어 Queue에 담는다.
                    // 현재 노드는 찾은 길의 이전 노드로서 담는다.
                    BFSNode searchNode = new BFSNode(dy, dx, node);
                    queue.Enqueue(searchNode);

                    // 이 좌표는 한 번 들른 길로 체크한다.
                    checkRoad[dy, dx] = true;
                }
            }
        }

        while (bestNode.PrevCount > 0)
        {
            Console.WriteLine(string.Format("[{0},{1}]", bestNode.Y, bestNode.X));

            // bestNode를 순회하며 해당 좌표를 콘솔로 찍어준다.

            // [5,3]->[5,2]->[5,1]->[5,0]->[4,0]->[3,0]->[2,0]->[1,0] 순으로
            bestNode = bestNode.PrevNode;
        }
    }

    private bool CheckMapRange(int y, int x)
    {
        return (y >= 0 && y < maps.GetLength(0) &&
            (x >= 0 && x < maps.GetLength(1)));
    }

    private bool CheckMapWay(int y, int x)
    {
        return maps[y, x] == 0;
    }

    private void ClearCheckRoad()
    {
        checkRoad = new bool[maps.GetLength(0), maps.GetLength(1)];
        for (int i = 0; i < checkRoad.GetLength(0); ++i)
        {
            for (int j = 0; j < checkRoad.GetLength(1); ++j)
                checkRoad[i, j] = false;
        }
    }
}

public class BFSNode
{
    public int X { get; }
    public int Y { get; }
    public BFSNode PrevNode { get; }
    public int PrevCount { get; }

    public BFSNode(int y, int x, BFSNode prevNode)
    {
        Y = y;
        X = x;
        PrevNode = prevNode;

        if (PrevNode == null)
        {
            // 이전 노드가 없으면 시작지점이기 때문에 Count는 0이다.
            PrevCount = 0;
        }
        else
        {
            // 이전 노드가 있다면 이전노드의 '이전노드 갯수' + 1을 해준다.
            // 목표지점에 해당하는 노드는 최종적으로
            // 시작지점에서 목표지점까지의 노드 수가 담기게 된다.
            PrevCount = PrevNode.PrevCount + 1;
        }
    }
}