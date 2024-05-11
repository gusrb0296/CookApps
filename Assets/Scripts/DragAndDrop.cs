using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] SpriteRenderer img_Shadow;  // 어느 보드에 설치될 지 보여주기 위한 그림자

    [SerializeField] Character character;

    // 드래그&드랍 후 보드의 위치시킬 Cell
    Cell nearestCell;

    GameBoard gameBoard;

    private void Start()
    {
        gameBoard = GameManager.Instance.gameBoard;
    }


    // 드래그 시작할 때 그림자 ON
    private void OnMouseDown()
    {
        //그림자 인디케이터 활성화
        img_Shadow.gameObject.SetActive(true);


        // 이미 배치를 했었다면 배치한 곳의 배치 상태를 false
        if(nearestCell != null)
        {
            nearestCell.SetIsOccupied(false);
        }

    }

    void OnMouseDrag()
    {
        // 드래그 할 때 마우스 위치로 계속 이동
        transform.position = GetMousePos();

        // 그림자의 위치를 마우스 근처의 보드위로 배치        
        if(gameBoard.GetCell(transform.position) != null)        
        {
            nearestCell = gameBoard.GetCell(transform.position);
            img_Shadow.transform.position = nearestCell.position;
        }
        else
        {
            img_Shadow.transform.position = nearestCell.position;
        }
    }


    // 드래그 종료 후, 타일에 배치. 그림자 OFF
    void OnMouseUp()
    {
        transform.position = nearestCell.position;
        img_Shadow.gameObject.SetActive(false);
        nearestCell.SetCharacter(character);
    }

    // 화면의 마우스 위치를 반환함
    Vector3 GetMousePos()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePos;
    }
}
