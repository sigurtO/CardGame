using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    private TurnManager turnManager;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI buttonText;


    private void OnEnable()
    {
        turnManager.OnPlayerTurnEnded += EndTurn; // subscribe to the event when player turn ends
        turnManager.OnEnemyTurnEnded += EnemyTurnEnded; // subscribe to the event when enemy turn ends
    }

    private void OnDisable()
    {
        turnManager.OnPlayerTurnEnded -= EndTurn;
        turnManager.OnEnemyTurnEnded -= EnemyTurnEnded;

    }


    private void EndTurn()
    {
       buttonText.text = "EnemyTurn"; // update the button text to indicate it's the player's turn
        button.interactable = false;
    }

    private void EnemyTurnEnded()
    {
        buttonText.text = "End Turn"; // update the button text to indicate it's the player's turn
        button.interactable = true;
    }


}
