using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCPManager : MonoBehaviour
{

    public int CarNumber;
    public int cpCrossed = 0;
    public int CarPosition;

    public RaceManage raceManager;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CP"))
        {
            cpCrossed += 1;
            raceManager.CarCollectedCP(CarNumber, cpCrossed);
        }
    }

}
