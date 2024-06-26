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
    public GameObject ReadyForm;
    public GameObject NotReadyForm;

    PhotonView pv;
    public GameObject Car;
    public GameObject Cp;
    public int Lap;

    public RaceManage raceManager;
    public CarCPManager cpManager;

    public bool isReady;

    public bool isAi = false;

    private void Awake()
    {
        raceManager = FindObjectOfType<RaceManage>();
        Car = gameObject;
        Cp = raceManager.SetCp();
        Lap = 0;
        raceManager.SetRacerPosition(gameObject.GetComponent<Racer>());
        cpManager = gameObject.GetComponent<CarCPManager>();

        isReady = isAi = gameObject.tag == "Enemy";

        PositionText = raceManager.PositionText;
        LapsText = raceManager.LapsText;
        FinalText = raceManager.FinalText;
        ReadyForm = raceManager.Ready;
        NotReadyForm = raceManager.NotReady;
        this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;

        if (!isAi)
        {
            pv = GetComponent<PhotonView>();
        }
    }

    public void SetReady()
    {
        if (!isAi && pv.IsMine)
        {
            if (ReadyForm.activeSelf == true)
            {
                NotReadyForm.SetActive(true);
                ReadyForm.SetActive(false);
            }
            else
            {
                NotReadyForm.SetActive(false);
                ReadyForm.SetActive(true);
            }

            isReady = !isReady;
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
