using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    public static RaceManager raceManager;

    public GameObject CP;
    public GameObject checkpointHolder;

    public Transform[] checkpointsPosition; 
    public List<Racer> racers; // Racers

    private int totalcars = 0;
    private int totalcheckpoints;

    public int totalLaps;
    public int readyPlayers = 0;

    public StartManager startManager;

    public Text positionText; // может быть
    public Text finalText; // >
    public Text lapsText; // >
    public Button readyButton;
    public GameObject notReady;
    public GameObject ready;


    public int GetLap(int carNumber)
    {
        return racers[carNumber].lap;
    }

    public int GetTotalCars()
    {
        return totalcars;
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }

    public void SetReady(bool isReady)
    {
        readyPlayers = 0;

        foreach (Racer racer in racers)
        {
            if (racer.isReady)
            {
                readyPlayers++;
            }
        }
    }

    private void OnEnable()
    {
        if (RaceManager.raceManager == null)
        {
            RaceManager.raceManager = this;
        }
    }

    public GameObject SetCp()
    {
        CP = Instantiate(CP, checkpointsPosition[0].position, checkpointsPosition[0].rotation);
        CP.name = "CP" + racers.Count;
        CP.layer = 15 + racers.Count;

        return CP;
    }

    public void SetRacerPosition(Racer racer)
    {
        racers.Add(racer);

        totalcars++;

        racers[totalcars - 1].gameObject.GetComponent<CarCPManager>().carPosition = totalcars;
        racers[totalcars - 1].gameObject.GetComponent<CarCPManager>().carNumber = totalcars - 1;
    }

    public bool FinishTrace(int carNumber)
    {
        return racers[carNumber].lap >= totalLaps;
    }

    void Start()
    {
        totalcars = racers.Count;
        totalcheckpoints = checkpointHolder.transform.childCount;

        SetCheckpoints();
    }

    void SetCheckpoints()
    {
        checkpointsPosition = new Transform[totalcheckpoints];

        for (int i = 0; i < totalcheckpoints; i++)
        {
            checkpointsPosition[i] = checkpointHolder.transform.GetChild(i).transform;
        }
    }

    public void CarCollectedCP(int carNumber, int cpNumber)
    {
        cpNumber = cpNumber % checkpointsPosition.Length;

        racers[carNumber].CP.transform.position = checkpointsPosition[cpNumber].transform.position;
        racers[carNumber].CP.transform.rotation = checkpointsPosition[cpNumber].transform.rotation;

        comparePositions(carNumber);

        if (cpNumber + 1 == totalcheckpoints)
        {
            racers[carNumber].lap++;
        }
    }

    void comparePositions(int carNumber)
    {
        if (racers[carNumber].gameObject.GetComponent<CarCPManager>().carPosition > 1)
        {
            GameObject currentCar = racers[carNumber].gameObject;
            int currentCarPosition = currentCar.GetComponent<CarCPManager>().carPosition;
            int currentCarCp = currentCar.GetComponent<CarCPManager>().cpCrossed;

            GameObject carInFront = null;
            int carInFrontPos = 0;
            int carInFrontCp = 0;

            for (int i = 0; i < totalcars; i++)
            {
                if (racers[i].gameObject.GetComponent<CarCPManager>().carPosition == currentCarPosition - 1)
                {
                    carInFront = racers[i].gameObject;
                    carInFrontCp = carInFront.GetComponent<CarCPManager>().cpCrossed;
                    carInFrontPos = carInFront.GetComponent<CarCPManager>().carPosition;
                    break;
                }
            }

            if (currentCarCp > carInFrontCp)
            {
                currentCar.GetComponent<CarCPManager>().carPosition = currentCarPosition - 1;
                carInFront.GetComponent<CarCPManager>().carPosition = carInFrontPos + 1;

                // Debug.Log("Car " + carNumber + " has over taken " + carInFront.GetComponent<CarCPManager>().CarNumber);
            }
        }
    }

    private void Update()
    {
        // Debug.Log(totalcars + " : " + readyPlayers);

        if (totalcars != 0 && readyPlayers == totalcars)
        {
            startManager.SpawnAIs(totalcars);
            startManager.GoRace(racers);
        }
    }
}
