using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private List<Cell> cellsList;

    [SerializeField]
    public Text Score;
    public Cell[,] cells { get; private set; }

    //поскольку в планах нет маштабируемости вся работа с клетками упрощена
    private void Awake()
    {
        cells = new Cell[7, 7];

        foreach (Cell cell in cellsList)
        {
            cells[cell.Position.x, cell.Position.y] = cell;
        }

        GameManager.Instance.SetCanvas(this);
        PlayerData.Instance.OnScoreChanged += ScoreRefresh;
    }

    private void ScoreRefresh(int score)
    {
        Score.text = score.ToString();
    }

}
