using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Racer : MonoBehaviour
{
    public Text PositionText;
    public Text LapsText;
    public Text FinalText;

    PhotonView pv;
    public GameObject Car;
    public GameObject Cp;
    public int Lap;

    public RaceManage raceManager;
    public CarCPManager cpManager;

    public bool isAi = false;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManage>();
        Car = gameObject;
        Cp = raceManager.SetCp();
        Lap = 0;
        raceManager.SetRacerPosition(gameObject.GetComponent<Racer>());
        cpManager = gameObject.GetComponent<CarCPManager>();

        isAi = gameObject.tag == "Enemy";

        PositionText = raceManager.PositionText;
        LapsText = raceManager.LapsText;
        FinalText = raceManager.FinalText;

        if (!isAi)
        {
            pv = GetComponent<PhotonView>();
        }
    }

    private void Update()
    {
        if (!isAi && pv.IsMine)
        {
            PositionText.text = cpManager.CarPosition.ToString() + "/" + raceManager.GetTotalCars().ToString();
            LapsText.text = raceManager.GetLap(cpManager.CarNumber).ToString() + "/" + raceManager.GetTotalLaps().ToString();

            if (raceManager.FinishTrace(cpManager.CarNumber))
            {
                RCC.SetControl(gameObject.GetComponent<RCC_CarControllerV3>(), false);
                if (cpManager.CarPosition == 1)
                {
                    FinalText.text = "WIN! Вы заняли 1 место!";
                }
                else
                {
                    FinalText.text = "LOOSE! Вы заняли " + cpManager.CarPosition.ToString() + " место.";
                }
                this.enabled = false;
            }
        }
    }

}
