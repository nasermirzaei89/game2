using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject chessPiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerDark = new GameObject[16];
    private GameObject[] playerLight = new GameObject[16];

    public enum Player
    {
        Light,
        Dark
    }

    private Player currentPlayer;

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    private bool isGameOver = false;

    public bool IsGameOver()
    {
        return isGameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLight = new GameObject[]
        {
            Create("Light Rook", 0,0),Create("Light Knight", 1,0),Create("Light Bishop", 2,0),Create("Light Queen", 3,0),
            Create("Light King", 4,0),Create("Light Bishop", 5,0),Create("Light Knight", 6,0),Create("Light Rook", 7,0),
            Create("Light Pawn", 0,1),Create("Light Pawn", 1,1),Create("Light Pawn", 2,1),Create("Light Pawn", 3,1),
            Create("Light Pawn", 4,1),Create("Light Pawn", 5,1),Create("Light Pawn", 6,1),Create("Light Pawn", 7,1),
        };



        playerDark = new GameObject[]
        {
            Create("Dark Rook", 0,7),Create("Dark Knight", 1,7),Create("Dark Bishop", 2,7),Create("Dark Queen", 3,7),
            Create("Dark King", 4,7),Create("Dark Bishop", 5,7),Create("Dark Knight", 6,7),Create("Dark Rook", 7,7),
            Create("Dark Pawn", 0,6),Create("Dark Pawn", 1,6),Create("Dark Pawn", 2,6),Create("Dark Pawn", 3,6),
            Create("Dark Pawn", 4,6),Create("Dark Pawn", 5,6),Create("Dark Pawn", 6,6),Create("Dark Pawn", 7,6),
        };

        for (int i = 0; i < playerLight.Length; i++)
        {
            SetPosition(playerLight[i]);
            SetPosition(playerDark[i]);
        }
    }

    public GameObject Create(string name,int x, int y)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(x, y, 0), Quaternion.identity);
        ChessPiece cm = obj.GetComponent<ChessPiece>();

        cm.name = name;
        cm.xBoard = x;
        cm.yBoard = y;

        cm.Activate();

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessPiece cm = obj.GetComponent<ChessPiece>();

        positions[cm.xBoard, cm.yBoard] = obj;
    }

    public void ClearPosition(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        return x >= 0 && y >= 0 && x < positions.GetLength(0) && y < positions.GetLength(1);
    }

    public void NextTurn()
    {
        if (currentPlayer == Player.Light)
        {
            currentPlayer = Player.Dark;
        } else
        {
            currentPlayer = Player.Light;
        }
    }

    public void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            isGameOver = false;

            SceneManager.LoadScene("Chess");
        }
    }
}
