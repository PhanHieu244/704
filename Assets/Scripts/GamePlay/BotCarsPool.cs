using UnityEngine;
using System.Collections.Generic;

public class BotCarsPool
{
    private List<BotCar> chunk;

    public void Create(BotCarSpawner.BotCarEntry[] cars)
    {
        chunk = new List<BotCar>();
        foreach(BotCarSpawner.BotCarEntry car in cars)
        {
            for (int i = 0; i < car.ammount; i++)
            {
                GameObject obj = MonoBehaviour.Instantiate(car.prefab) as GameObject;
                obj.SetActive(false);
                chunk.Add(obj.GetComponent<BotCar>());
            }
        }
    }
    public BotCar GetCar()
    {
        foreach (BotCar car in chunk)
        {
            if (car.isAvailable) return car;
        }
        return null;
    }
}
