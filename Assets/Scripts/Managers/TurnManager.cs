using UnityEngine;
using UnityEngine.Events;

//public enum GameState { BattleStart, PlayerTurn, EnemyTurn, Victory, Defeat }
public class TurnManager : MonoBehaviour
{
    //[SerializeField] // serialzed for testing
   // private GameState currentState;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private MatchController matchController;


    //events end turn
    public UnityAction OnEnemyTurnEnded;
    public UnityAction OnPlayerTurnEnded;
    public UnityAction OnPlayerTurnStart;

    //event status Manager
    public UnityAction OnTurnTick;


    private void Start() // for testing
    {
        BattleStart();
    }
    public void BattleStart()
    {
        enemyManager.RollEnemyIntent();
        matchController.ShuffelCards(); //shuffel cards
        StartPlayerTurn(); //draw first hand
    }

    public void StartPlayerTurn()
    {
        manaManager.ResetManaForNewTurn();
        matchController.DrawCardOnPlayerTurn();

        OnPlayerTurnStart?.Invoke();

        //matchController.
        //draw cards etc
    }

    public void OnEndTurnButtonClicked()
    {
        Debug.Log("--- PLAYER TURN ENDED ---");


        OnTurnTick?.Invoke(); //status manager tick for poison and regen and stuff

        StartCoroutine(enemyManager.ExecuteEnemyTurn(StartNewRound)); // attack
        OnPlayerTurnEnded?.Invoke(); //update ui //disable player ablity to play cards


        WaitForSeconds wait = new WaitForSeconds(1f); // wait for 1 second before starting enemy turn, adjust as needed
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
