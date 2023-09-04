using UnityEngine;
using static Game;
using static MovePlate;

public class ChessPiece : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public int xBoard = -1;
    public int yBoard = -1;

    const float offset = -3.5f;

    private Player player;

    public Sprite darkQueen, darkKing, darkBishop, darkKnight, darkRook, darkPawn;
    public Sprite lightQueen, lightKing, lightBishop, lightKnight, lightRook, lightPawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "Dark Queen": this.GetComponent<SpriteRenderer>().sprite = darkQueen; player = Player.Dark; break;
            case "Dark King": this.GetComponent<SpriteRenderer>().sprite = darkKing; player = Player.Dark; break;
            case "Dark Bishop": this.GetComponent<SpriteRenderer>().sprite = darkBishop; player = Player.Dark; break;
            case "Dark Knight": this.GetComponent<SpriteRenderer>().sprite = darkKnight; player = Player.Dark; break;
            case "Dark Rook": this.GetComponent<SpriteRenderer>().sprite = darkRook; player = Player.Dark; break;
            case "Dark Pawn": this.GetComponent<SpriteRenderer>().sprite = darkPawn; player = Player.Dark; break;
                
            case "Light Queen": this.GetComponent<SpriteRenderer>().sprite = lightQueen; player = Player.Light; break;
            case "Light King": this.GetComponent<SpriteRenderer>().sprite = lightKing; player = Player.Light; break;
            case "Light Bishop": this.GetComponent<SpriteRenderer>().sprite = lightBishop; player = Player.Light; break;
            case "Light Knight": this.GetComponent<SpriteRenderer>().sprite = lightKnight; player = Player.Light; break;
            case "Light Rook": this.GetComponent<SpriteRenderer>().sprite = lightRook; player = Player.Light; break;
            case "Light Pawn": this.GetComponent<SpriteRenderer>().sprite = lightPawn; player = Player.Light; break;
        }
    }

    public void SetCoords()
    {

        this.transform.position = new Vector3(xBoard+offset, yBoard+offset, 0);
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("Plate");

        for (int i = 0; i < movePlates.Length; i++) Destroy(movePlates[i]);
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "Dark Queen":
            case "Light Queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "Dark Knight":
            case "Light Knight":
                LMovePlate();
                break;
            case "Dark Bishop":
            case "Light Bishop":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "Dark King":
            case "Light King":
                SurrondMovePlate();
                break;
            case "Dark Rook":
            case "Light Rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "Dark Pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "Light Pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement,int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y)==null)
        {
            SpawnMovePlate(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<ChessPiece>().player != player)
        {
            SpawnAttackPlate(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurrondMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard -1);
        PointMovePlate(xBoard -1, yBoard - 1);
        PointMovePlate(xBoard -1, yBoard );
        PointMovePlate(xBoard -1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x,y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                SpawnMovePlate(x, y);
            } else if (cp.GetComponent<ChessPiece>().player != player)
            {
                SpawnAttackPlate(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x,y))
        {
            if (sc.GetPosition(x,y) == null)
            {
                SpawnMovePlate(x, y);
            }

            if (sc.PositionOnBoard(x+1,y) && sc.GetPosition(x+1,y) != null && sc.GetPosition(x+1, y).GetComponent<ChessPiece>().player !=player)
            {
                SpawnAttackPlate(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<ChessPiece>().player != player)
            {
                SpawnAttackPlate(x - 1, y);
            }
        }
    }

    public void SpawnMovePlate(int x,int y)
    {
        GameObject mp = Instantiate(movePlate, new Vector3(x+offset, y+offset, 0), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();

        mpScript.reference = gameObject;
        mpScript.SetCoords(x, y);
    }

    public void SpawnAttackPlate(int x, int y)
    {
        GameObject mp = Instantiate(movePlate, new Vector3(x + offset, y + offset, 0), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();

        mpScript.reference = gameObject;
        mpScript.SetCoords(x, y);
        mpScript.act = Move.Attack;
    }
}
