using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private RestSiteManager restSiteManager;

    [SerializeField] private CurrentRunState runState;

    [SerializeField] private List<MapNode> startingNodes = new List<MapNode>();

    [SerializeField] private List<MapNode> allNodes = new List<MapNode>(); //list of all nodes we can find via id

    private void Awake()
    {
        Instance = this;
    }

    public void PressForResetDev() // for testing
    {

        Debug.LogWarning("Developer triggered Run Reset!");
        runState.ResetRun();
        UpdateMapVisuals(); // Redraw the map!

    }

    private void Start()
    {
        UpdateMapVisuals();
    }

    public void UpdateMapVisuals()
    {
        foreach (MapNode node in allNodes) // lock all nodes
        {
            node.SetInteractable(false);
        }

        if (string.IsNullOrEmpty(runState.currentNodeID)) // if we are at start of run
        {
            foreach (MapNode node in startingNodes) // unlock only the starting nodes
            {
                node.SetInteractable(true);
            }
            return;
        }

        MapNode currentNode = FindNodeByID(runState.currentNodeID); // what Id do we have in our run state

        if (currentNode != null)
        {
            foreach (MapNode childNode in currentNode.nextNodes) // unlock the children of that node
            {
                childNode.SetInteractable(true);
            }
        }
    }

    public void OnNodeClicked(MapNode clickedNode)
    {
        runState.currentNodeID = clickedNode.nodeID; // save current location in runstate

        Debug.Log($"[MapManager] Clicked Node: {clickedNode.nodeID} of type {clickedNode.nodeType}");

        switch (clickedNode.nodeType)
        {
            case NodeType.Encounter: //normal combat
                runState.nextEncounter = clickedNode.GetEncounter();
                SceneManager.LoadScene("BattleScene");
                break;

            case NodeType.RestSite:
                restSiteManager.ShowRestSite();
                break;

            case NodeType.Shop:
                Debug.LogWarning("Shop not built yet!");
                break;
        }
    }

    public void SetCurrentNode(MapNode clickedNode) //call when player clicks
    {
        runState.currentNodeID = clickedNode.nodeID; // save ID

        runState.nextEncounter = clickedNode.GetEncounter(); // save encounter so battle scene knows what to load
    }

    private MapNode FindNodeByID(string id)
    {
        foreach (MapNode node in allNodes)
        {
            if (node.nodeID == id) return node;
        }
        return null;
    }
}