using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public void StraightLevelClick()
    {
        SceneManager.LoadScene("straightPathsLevel");
    }

    public void RotatedLevelClick()
    {
        SceneManager.LoadScene("rotatedPathsLevel");
    }

    public void LeaderboardClick()
    {
        SceneManager.LoadScene("leaderboard");
    }

    public void TestingClick()
    {
        SceneManager.LoadScene("testing");
    }

    public void LoginClick()
    {
        SceneManager.LoadScene("login");
    }
}
