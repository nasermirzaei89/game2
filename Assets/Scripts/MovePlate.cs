using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    public GameObject reference = null;

    int xBoard;
    int yBoard;

    public enum Move
    {
        Normal,
        Attack
    }

    public Move act;

    public void Start()
    {
        if (act == Move.Attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (act == Move.Attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(xBoard, yBoard);

            Destroy(cp);
        }

        controller.GetComponent<Game>().ClearPosition(reference.GetComponent<ChessPiece>().xBoard, reference.GetComponent<ChessPiece>().yBoard);

        reference.GetComponent<ChessPiece>().xBoard = xBoard;
        reference.GetComponent<ChessPiece>().yBoard = yBoard;
        reference.GetComponent<ChessPiece>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<ChessPiece>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        xBoard= x;
        yBoard = y;
    }
}
