using UnityEngine;
using System.Collections.Generic;

public class TerrainController : MonoBehaviour
{
    public static TerrainController instance;
    public int[] randomSpawnGasPoint;
	public int roadToShowGas;
    [SerializeField]
    private float length = 100;
    [SerializeField]
    private float laneWidth = 5.8f;
    [SerializeField]
    private int lanes = 4;
    [SerializeField]
    private CarController player;

    
    Transform[] terrains;
    [SerializeField]
    public bool oneWay = false;
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private GameObject shieldPrefab;
    public GameObject gasPrefab;
	public GameObject desertRoadPrefab,snowRoadPrefab;
    private Coin coin;
    private Shield shield;
    private Queue<Transform> terrainQueue;
    private Vector3 lastPotition;
    private int roadPassed = 0;
    public int roadCount = 0;
	public GameObject snowEffect;
	int selectedMap;
	GameObject roadGameObject;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
		selectedMap=SavedDataManager.getCurrentSeledtedMap();
	//	selectedMap=0;
		switch (selectedMap){
		case 0:
			roadGameObject= Instantiate(desertRoadPrefab) as GameObject;
			snowEffect.SetActive(false);

			break;
		case 1:
			roadGameObject= Instantiate(snowRoadPrefab) as GameObject;

			snowEffect.SetActive(true);

			break;

		}
		terrains= new Transform[roadGameObject.transform.childCount];
		for (int i=0;i<roadGameObject.transform.childCount;i++){
			terrains[i]=roadGameObject.transform.Find("road"+i);
		}
		if (player == null) player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<CarController>();
		terrainQueue = new Queue<Transform>();
		for (int i = 0; i < terrains.Length; i++)
		{
			terrainQueue.Enqueue(terrains[i]);
			if (i > 0)
			{
				lastPotition = terrains[i - 1].position;
				lastPotition.z += length;
			}
		}
        lastPotition = terrains[terrains.Length - 1].position;

        GameObject coinObj = Instantiate(coinPrefab) as GameObject;
        coin = coinObj.GetComponent<Coin>();
        GameObject shieldObj = Instantiate(shieldPrefab) as GameObject;
        shield = shieldObj.GetComponent<Shield>();
    }
    void Update()
    {
        if (terrainQueue.Peek().position.z < Camera.main.transform.position.z - length)
        {

            Transform terrain = terrainQueue.Dequeue();
            lastPotition.z += length;
            terrain.position = lastPotition;
            terrainQueue.Enqueue(terrain);
            roadPassed++;
            roadCount++;
            if (roadCount % 5 == 0 && coin.transform.position.z < player.transform.position.z)
            {
                PutCoin(100, 15);
            }
            if (roadCount % 9 == 0 && shield.transform.position.z < player.transform.position.z)
            {
                PutShield(100, 25);
            }
			if (roadPassed >= roadToShowGas)
            {
                roadPassed = 0;
                int gasPos = randomSpawnGasPoint[Random.Range(0, 3)];
                GameObject gasObject = Instantiate(gasPrefab, new Vector3(gasPos,1.2f, terrain.position.z), Quaternion.identity) as GameObject;
            }
        }
    }

    public int GetPlayerLane()
    {
        return GetLane(player.transform.position.x);
    }

    public int GetLane(float x)
    {
        return (int)(x / laneWidth + laneWidth / 2);
    }

    public void PutCar(BotCar car, int lane, float z)
    {
        float x = GetPosition(lane);
        Vector3 pos = car.transform.position;
        pos.z = z;
        pos.x = x;
        car.transform.position = pos;

        bool moveForward = (x > 0) || oneWay;
        car.moveDirection = moveForward ? Vector3.forward : -Vector3.forward;
        Vector3 angle = car.transform.eulerAngles;
        angle.y = moveForward ? 0 : 180;
        car.transform.eulerAngles = angle;
        car.Active();
    }

    public float GetPosition(int currentLane)
    {
        currentLane = Mathf.Clamp(currentLane, 1, lanes);
        return (currentLane - lanes / 2 - 0.5f) * laneWidth;
    }

    public int GetTargetLane(BotCar car)
    {
        int carLane = GetLane(car.transform.position.x);
        int dir = GetPlayerLane() - carLane;
        if (dir == 0) return 0;
        int target = carLane + ((dir > 0) ? 1 : -1);
        if (oneWay)
        {
            return target;
        }
        else
        {
            return (carLane - lanes / 2 - 0.5f) * (target - lanes / 2 - 0.5f) > 0 ? target : 0;
        }
    }

    void PutCoin(float distance)
    {
        float posX = GetPosition(Random.Range(1, lanes + 1));
        coin.Put(posX, player.transform.position.z + distance);
    }

    void PutCoin(float distance, float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 100);
        if (Random.Range(0, 100) < percentage)
        {
            PutCoin(distance);
        }
    }

    void PutShield(float distance)
    {
        float posX = GetPosition(Random.Range(1, lanes + 1));
        shield.Put(posX, player.transform.position.z + distance);
    }

    void PutShield(float distance, float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 100);
        if (Random.Range(0, 100) < percentage)
        {
            PutShield(distance);
        } 
    }
}
