using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject placedManager = new GameObject("PlacedManager");
                instance = placedManager.AddComponent<GameManager>();
            }

            return instance;
        }

        set
        {
            if (instance == null)
            {
                instance = value;
            }
        }
    }

    public GameCanvas Canvas { get; private set; }

    public PlayerData PlayerData { get; private set; }


    private PlacedTypeSO TestObj;
    private PlacedTypeSO Chest;


    private void Awake()
    {
        Instance = this;

        PlayerData = PlayerData.Instance;

        TestObj = Resources.Load<PlacedTypeSO>("TestObj");
        Chest = Resources.Load<PlacedTypeSO>("Chest");


    }

    public void SetCanvas(GameCanvas canvas)
    {
        this.Canvas = canvas;

/*      //вписывать объекты сюда
 *      
        PlacedObject.Create(TestObj, canvas.cells[1, 1]);
        PlacedObject.Create(TestObj, canvas.cells[1, 2]);
        PlacedObject.Create(TestObj, canvas.cells[1, 3]);
        PlacedObject.Create(TestObj, canvas.cells[1, 4]);
        PlacedObject.Create(TestObj, canvas.cells[2, 1]);
        PlacedObject.Create(Chest, canvas.cells[3, 3]);
        PlacedObject.Create(TestObj, canvas.cells[3, 4]);
        PlacedObject.Create(TestObj, canvas.cells[4, 1]);
        PlacedObject.Create(TestObj, canvas.cells[4, 2]);
        PlacedObject.Create(TestObj, canvas.cells[4, 3]);
        PlacedObject.Create(TestObj, canvas.cells[4, 4]);
        PlacedObject.Create(TestObj, canvas.cells[5, 1]);
        PlacedObject.Create(TestObj, canvas.cells[6, 1]);
        PlacedObject.Create(TestObj, canvas.cells[6, 2]);
        PlacedObject.Create(TestObj, canvas.cells[6, 3]);
        PlacedObject.Create(TestObj, canvas.cells[6, 4]);*/
        PlacedObject.Create(Chest, canvas.cells[3, 3]);
    }

    //создание объектов в игре, объект встаёт в ближайшую свободную клетку
    public void CreateObject(PlacedTypeSO placedType, Vector2Int point, Transform creator = null)
    {
        if ((point.x < 0 || point.x >= 7) || (point.y < 0 || point.y >= 7))
            point = new Vector2Int(UnityEngine.Random.Range(0, 7), UnityEngine.Random.Range(0, 7));

        Vector2Int freeCell = FindFreeSpace(point);

        if (freeCell != new Vector2Int(-1, -1))
        {
            PlacedObject.Create(placedType, Canvas.cells[freeCell.x, freeCell.y], creator);
        }
    }

    //перемещение объекта в ближайшую свободную клетку
    public bool RandomMoveObject(PlacedObject placedObject, Vector2Int point)
    {
        Vector2Int freeCell = FindFreeSpace(point);
        
        if (freeCell != new Vector2Int(-1, -1))
        {
            Canvas.cells[freeCell.x, freeCell.y].MoveHereObject(placedObject);
            return true;
        }

        return false;
    }

    //поиск ближайшей свободной клетки, возвращает (-1,-1) если её нет
    public Vector2Int FindFreeSpace(Vector2Int refPoint)
    {
        if ((refPoint.x < 0 || refPoint.x >= 7) || (refPoint.y < 0 || refPoint.y >= 7)) return new Vector2Int(-1, -1);

        /* // с использованием Dictionary и Linq, находит лишь одну клетку
        Dictionary<Vector2Int, float> distanceToFreePoints = new Dictionary<Vector2Int, float>();

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                if (Canvas.cells[i, j].PlacedObject == null)
                    distanceToFreePoints.Add(new Vector2Int(i, j), Vector2Int.Distance(refPoint, new Vector2Int(i, j)));

        if (distanceToFreePoints.Count == 0) return new Vector2Int(-1,-1);

        Vector2Int result = distanceToFreePoints.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;

        return result;
        */

        //с Comparable классом, возможны несколько ближайших вариантов, выбирает рандомно
        List<DistanceToFreePoints> distanceToFreePoints = new List<DistanceToFreePoints>();

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
                if (Canvas.cells[i, j].PlacedObject == null)
                    distanceToFreePoints.Add(new DistanceToFreePoints(new Vector2Int(i, j), Vector2Int.Distance(refPoint, new Vector2Int(i, j))));

        if (distanceToFreePoints.Count == 0) return new Vector2Int(-1, -1);

        distanceToFreePoints.Sort();

        List<Vector2Int> results = new List<Vector2Int>();

        foreach(DistanceToFreePoints distance in distanceToFreePoints)
        {
            if (distanceToFreePoints[0] == distance)
                results.Add(distance.Point);
        }

        return results [UnityEngine.Random.Range(0, results.Count)];
    }

    private class DistanceToFreePoints : IComparable<DistanceToFreePoints>
    {
        public Vector2Int Point { get; private set; }
        public float Distance { get; private set; }

        public DistanceToFreePoints(Vector2Int point, float distance)
        {
            this.Point = point;
            this.Distance = distance;
        }

        public int CompareTo(DistanceToFreePoints other)
        {
            return Distance.CompareTo(other.Distance);
        }
    }
}
