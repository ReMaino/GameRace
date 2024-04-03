using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacePosition : MonoBehaviour
{
    public Text positionText;
    public Transform playerTransform;
    public Transform[] checkpoints; // массив всех чекпоинтов на трассе
    public int totalPlayers = 4; // общее количество игроков

    void Update()
    {
        int playerPosition = CalculatePlayerPosition();
        positionText.text = playerPosition + "/" + totalPlayers;
    }

    private int CalculatePlayerPosition()
    {
        float playerDistanceToFinish = GetDistanceToFinish(); // получаем расстояние игрока до финиша

        int playerPosition = 1; // по умолчанию позиция игрока 1

        for (int i = 0; i < totalPlayers; i++)
        {
            float distanceToFinish = Vector3.Distance(playerTransform.position, checkpoints[checkpoints.Length - 1].position); // расстояние другого игрока до финиша

            if (distanceToFinish < playerDistanceToFinish) // если другой игрок ближе к финишу
            {
                playerPosition++;
            }
        }

        return playerPosition;
    }

    private float GetDistanceToFinish()
    {
        float distance = Vector3.Distance(playerTransform.position, checkpoints[checkpoints.Length - 1].position); // расстояние от игрока до последнего чекпоинта
        return distance;
    }
}