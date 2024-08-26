using UnityEngine;
using System.Collections.Generic;

public class BotCarSpawner : MonoBehaviour
{
    public static BotCarSpawner instance;

    [System.Serializable]
    public class BotCarEntry
    {
        public GameObject prefab;
        public int ammount = 1;
    }

    [SerializeField]
    private int maxLane = 4;
    [SerializeField]
    private float laneWidth = 5.8f;
    [SerializeField]
    private BotCarEntry[] botCars;
    [SerializeField]
    private float distance = 30;
    [SerializeField]
    private float maxDistance = 100;
    private BotCarsPool pool;
    [SerializeField]
    Vector3 latestPosition = Vector3.zero;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitCarsPool();
        latestPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        latestPosition.z += 200;
    }
    void Update()
    {
       if (latestPosition.z - GameObject.FindGameObjectWithTag("Player").transform.position.z < maxDistance) RandomSpawn();
    }

    public float GetPosition(int currentLane)
    {
        currentLane = Mathf.Clamp(currentLane, 1, 4);
        return (currentLane - maxLane / 2 - 0.5f) * laneWidth;
    }

    void InitCarsPool()
    {
        pool = new BotCarsPool();
        pool.Create(botCars);
    }


    void RandomSpawn()
    {
        BotCar car = pool.GetCar();
        if (latestPosition.z == 0) latestPosition.z += 100;
        if (car != null)
        {
            Vector3 pos = latestPosition;
            pos.z += distance;
            latestPosition = pos;
            TerrainController.instance.PutCar(car, Random.Range(1, maxLane + 1), pos.z);
        }
    }
}
