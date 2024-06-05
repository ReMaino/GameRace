using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManage : MonoBehaviour
{
    public GameObject CP;
    public GameObject CheckpointHolder;

    public Transform[] CheckpointsPosition; 
    public List<Racer> Racers; // Racers
//    public GameObject[] CheckpointsForeachCar; // его

    private int totalcars = 0;
    private int totalcheckpoints;

    public int totalLaps;

    public Text PositionText; // может быть
    public Text FinalText; // >
    public Text LapsText; // >

    public int GetLap(int carNumber)
    {
        return Racers[carNumber].Lap;
    }

    public int GetTotalCars()
    {
        return totalcars;
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }

    public GameObject SetCp()
    {
        CP = Instantiate(CP, CheckpointsPosition[0].position, CheckpointsPosition[0].rotation);
        CP.name = "CP" + Racers.Count;
        CP.layer = 15 + Racers.Count;

        return CP;
    }

    public void SetRacerPosition(Racer racer)
    {
        Racers.Add(racer);

        totalcars++;

        Racers[totalcars - 1].Car.GetComponent<CarCPManager>().CarPosition = totalcars;
        Racers[totalcars - 1].Car.GetComponent<CarCPManager>().CarNumber = totalcars - 1;
    }

    public bool FinishTrace(int carNumber)
    {
        return Racers[carNumber].Lap >= totalLaps; // поменяем
    }

    void Start()
    {
        totalcars = Racers.Count;
        totalcheckpoints = CheckpointHolder.transform.childCount;

        SetCheckpoints();
        SetCarPosition();
    }

    void SetCheckpoints()
    {
        CheckpointsPosition = new Transform[totalcheckpoints];

        for (int i = 0; i < totalcheckpoints; i++)
        {
            CheckpointsPosition[i] = CheckpointHolder.transform.GetChild(i).transform;
        }
    }

    void SetCheckpointForCar()
    {

    }

    void SetCarPosition()
    {

    }

    public void CarCollectedCP(int carNumber, int cpNumber)
    {
        cpNumber = cpNumber % CheckpointsPosition.Length;

        Racers[carNumber].Cp.transform.position = CheckpointsPosition[cpNumber].transform.position;
        Racers[carNumber].Cp.transform.rotation = CheckpointsPosition[cpNumber].transform.rotation;

        comparePositions(carNumber);

        if (cpNumber + 1 == totalcheckpoints)
        {
            Racers[carNumber].Lap++;

        }
    }

    void comparePositions(int carNumber)
    {
        if (Racers[carNumber].Car.GetComponent<CarCPManager>().CarPosition > 1)
        {
            GameObject currentCar = Racers[carNumber].Car;
            int currentCarPosition = currentCar.GetComponent<CarCPManager>().CarPosition;
            int currentCarCp = currentCar.GetComponent<CarCPManager>().cpCrossed;

            GameObject carInFront = null;
            int carInFrontPos = 0;
            int carInFrontCp = 0;

            for (int i = 0; i < totalcars; i++)
            {
                if (Racers[i].Car.GetComponent<CarCPManager>().CarPosition == currentCarPosition - 1)
                {
                    carInFront = Racers[i].Car;
                    carInFrontCp = carInFront.GetComponent<CarCPManager>().cpCrossed;
                    carInFrontPos = carInFront.GetComponent<CarCPManager>().CarPosition;
                    break;
                }
            }

            // this car has crossed ладнео не англичанин я
            if (currentCarCp > carInFrontCp)
            {
                currentCar.GetComponent<CarCPManager>().CarPosition = currentCarPosition - 1;
                carInFront.GetComponent<CarCPManager>().CarPosition = carInFrontPos + 1;

                Debug.Log("Car " + carNumber + " has over taken " + carInFront.GetComponent<CarCPManager>().CarNumber);
            }
        }
    }

}
