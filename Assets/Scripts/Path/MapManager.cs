using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; } // Simple Singleton for easy access

    [SerializeField] private RestSiteManager restSiteManager; // We will build this next!

    [SerializeField] private CurrentRunState runState;

    // Drag the very first row of clickable nodes into this list!
    [SerializeField] private List<MapNode> startingNodes = new List<MapNode>();

    // A master list of ALL nodes on the map so we can find them by ID
    [SerializeField] private List<MapNode> allNodes = new List<MapNode>();

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
        // 1. First, lock EVERY node on the map. Trust no one.
        foreach (MapNode node in allNodes)
        {
            node.SetInteractable(false);
        }

        // 2. Are we at the start of a run?
        if (string.IsNullOrEmpty(runState.currentNodeID))
        {
            // Unlock the bottom row!
            foreach (MapNode node in startingNodes)
            {
                node.SetInteractable(true);
            }
            return;
        }

        // 3. We are in the middle of a run! Find out where we are standing.
        MapNode currentNode = FindNodeByID(runState.currentNodeID);

        if (currentNode != null)
        {
            // Unlock ONLY the connected children!
            foreach (MapNode childNode in currentNode.nextNodes)
            {
                childNode.SetInteractable(true);
            }
        }
    }

    public void OnNodeClicked(MapNode clickedNode)
    {
        // 1. Always save our current location in the backpack
        runState.currentNodeID = clickedNode.nodeID;

        Debug.Log($"[MapManager] Clicked Node: {clickedNode.nodeID} of type {clickedNode.nodeType}");

        // 2. THE ROUTER: Where do we go?
        switch (clickedNode.nodeType)
        {
            case NodeType.Encounter:
                // Normal combat route
                runState.nextEncounter = clickedNode.GetEncounter();
                SceneManager.LoadScene("BattleScene");
                break;

            case NodeType.RestSite:
                // Pop up the campfire UI! (No scene load needed)
                restSiteManager.ShowRestSite();
                break;

            case NodeType.Shop:
                Debug.LogWarning("Shop not built yet!");
                break;
        }
    }

    // Called by the MapNode when the player clicks it
    public void SetCurrentNode(MapNode clickedNode)
    {
        // Save the ID so we know where we are when we return from battle
        runState.currentNodeID = clickedNode.nodeID;

        // Save the Encounter so the Battle Scene knows what to spawn
        runState.nextEncounter = clickedNode.GetEncounter();
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