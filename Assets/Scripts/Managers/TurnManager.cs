using System;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;

public class TurnManager : MonoBehaviour
{
    //[SerializeField] // serialzed for testing
   // private GameState currentState;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private MatchController matchController;


    //events end turn
    public UnityAction OnEnemyTurnEnded;
    public static event Action OnPlayerTurnEnded;
    public UnityAction OnPlayerTurnStart;

    //event status Manager
    public static UnityAction OnTurnTick;


    //private void Start() // for testing
    //{
    //    BattleStart();
    //}
    public void BattleStart()
    {
        enemyManager.RollEnemyIntent();
        matchController.ShuffelCards(); //shuffel cards
        StartPlayerTurn(); //draw first hand
    }


public void StartPlayerTurn()
    {
        IStatusReciver playerStatuses = FindAnyObjectByType<Player>().GetComponent<IStatusReciver>();

        if (playerStatuses.GetTotalStatusAmount(StatusType.Mana) > 0)
        {
            int mana = playerStatuses.GetTotalStatusAmount(StatusType.Mana);
            manaManager.ResetManaForNewTurn(mana);
        }
        else
        {
            manaManager.ResetManaForNewTurn();
        }
        matchController.DrawCardOnPlayerTurn();
        OnPlayerTurnStart?.Invoke();
    }

    public void OnEndTurnButtonClicked() //end our turn aka start enemy turn
    {
        Debug.Log("--- PLAYER TURN ENDED ---");


        OnTurnTick?.Invoke(); //status manager tick for poison and regen and stuff

        StartCoroutine(enemyManager.ExecuteEnemyTurn(StartNewRound)); // attack
        OnPlayerTurnEnded?.Invoke(); //update ui //disable player ablity to play cards


        WaitForSeconds wait = new WaitForSeconds(1f);
        EndEnemyTurn();

    }

    private void StartNewRound()
    {
        Debug.Log("--- ENEMY TURN ENDED ---");
        enemyManager.RollEnemyIntent();
        StartPlayerTurn();
    }



    public void EndEnemyTurn() // call this after enemy has attacks
    {
        //if (currentState != GameState.EnemyTurn) return;
        //currentState = GameState.PlayerTurn;
        OnEnemyTurnEnded?.Invoke();
    }

}
