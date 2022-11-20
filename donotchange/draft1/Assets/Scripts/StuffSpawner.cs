using UnityEngine;
using System.Collections;

public class StuffSpawner : MonoBehaviour
{
    //points where stuff will spawn :)
    public Transform[] StuffSpawnPoints;
    //meat gameobjects
    public GameObject[] Bonus;
    //obstacle gameobjects
    public GameObject[] Obstacles;

    public bool RandomX = false;
    public float minX = -2f, maxX = 2f;

    // Use this for initialization
    void Start()
    {
        //first, let's decide whether we'll place an obstacle
        int obstacleIndex = -1;
        CreateObstacle(StuffSpawnPoints[1].position, Obstacles[Random.Range(0, Obstacles.Length)]);
        for (int i = 2; i < 14; i++)
            CreateCandy(StuffSpawnPoints[i].position, Bonus[Random.Range(0, Bonus.Length)]);

    }

    void CreateObstacle(Vector3 position, GameObject prefab)
    {
        if (RandomX) //true on the wide path, false on the rotated ones
            position += new Vector3(0, 0, 0);

        Instantiate(prefab, position, Quaternion.identity);
    }

    void CreateCandy(Vector3 position, GameObject prefab)
    {
        if (RandomX) //true on the wide path, false on the rotated ones
            position += new Vector3(0, 6, 0);

        Instantiate(prefab, position, Quaternion.identity);
    }


}
