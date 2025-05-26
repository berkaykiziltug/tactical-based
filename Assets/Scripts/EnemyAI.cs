using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        //Enemy will count down 2 seconds and will end its turn.
        timer = 2f;
    }

    private void Update()
    {
        
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
