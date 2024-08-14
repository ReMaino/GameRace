using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StartManager : MonoBehaviour
{
    public Transform[] spawners;
    public int maxCars = 8;

    public void GoRace(List<Racer> racers)
    {
        foreach (Racer racer in racers)
        {
            racer.StartRacer();
        }
    }

    private void SpawnAI (int pos)
    {
        PhotonNetwork.Instantiate("ToyotaAI", spawners[pos].position, spawners[pos].rotation, 0);
    }

    public void SpawnAIs(int countRacers)
    {
        for (int i = countRacers; i < maxCars; i++)
        {
            SpawnAI(i);
        }
    }

}
