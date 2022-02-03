using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance;
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject placedManager = new GameObject("PlacedManager");
                instance = placedManager.AddComponent<PlayerData>();
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

    public event Action<int> OnScoreChanged;

    public PlayerDataSO Score { get; private set; }

    private void Awake()
    {
        Score = Resources.Load<PlayerDataSO>("Score");
    }

    private void Start()
    {
        ChangeScore(0);
    }

    public void ChangeScore(int cange)
    {
        Score.Integer += cange;
        OnScoreChanged?.Invoke(Score.Integer);
    }
}
