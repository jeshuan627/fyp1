using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public Button startButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        //Button btn = startButton.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    protected GameManager()
    {
        GameState = GameState.Start;
        CanSwipe = false;
    }

    public GameState GameState { get; set; }

    public bool CanSwipe { get; set; }

    /*public void TaskOnClick()
    {
        this.GameState = GameState.Start;
    }*/

    public void Die()
    {
            UIManager.Instance.SetStatus(Constants.StatusEndGame);
            this.GameState = GameState.End;
            PlayfabManager updatescore1 = new PlayfabManager();
            updatescore1.SendLeaderboard((int)UIManager.Instance.score);
            ChangeScene ending = new ChangeScene();
            ending.LeaderboardClick();
    }


}



