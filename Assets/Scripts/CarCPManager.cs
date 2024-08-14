using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCPManager : MonoBehaviour
{

    public int carNumber;
    public int cpCrossed = 0;
    public int carPosition;

    public RaceManager raceManager;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CP"))
        {
            cpCrossed += 1;
            raceManager.CarCollectedCP(carNumber, cpCrossed);
        }
    }

}
