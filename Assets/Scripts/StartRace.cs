using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRace : MonoBehaviour
{
    private void Start()
    {
        int enable = PlayerPrefs.GetInt("RaceBot");
        enable = 0;
        PlayerPrefs.SetInt("RaceBot", enable);

        StartCoroutine(Race());
    }

    public void StartRacing(List<Racer> racers)
    {
        foreach (Racer racer in racers)
        {
            racer.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
        }
    }

    private IEnumerator Race()
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.GetComponent<RCC_AICarController>().enabled = false; // true
        this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = false;
    }

    private void Update()
    {
        int enable = PlayerPrefs.GetInt("RaceBot");

        if (enable == 1)
        {
            this.gameObject.GetComponent<RCC_AICarController>().enabled = false;
            this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;

            Destroy(this, 5);
        }

        if (enable == 2)
        {
            this.gameObject.GetComponent<RCC_AICarController>().enabled = true;
            this.gameObject.GetComponent<RCC_CarControllerV3>().enabled = true;
        }
    }
}
