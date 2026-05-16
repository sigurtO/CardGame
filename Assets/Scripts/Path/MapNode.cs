using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum NodeType
{
    Encounter, //battle
    RestSite, //heal
    Shop, // maybe add later
    Mystery // maybe add later
}

public class MapNode : MonoBehaviour
{
    [Header("The Encounter")]
    [SerializeField] private EncounterDefinition myEncounter;

    [Header("Map Connections")]
    public List<MapNode> nextNodes = new List<MapNode>();

    [Header("Node Identity")]
    public string nodeID; // to member where we are in the run
    public NodeType nodeType = NodeType.Encounter;

    [Header("UI Components")]
    [SerializeField] private Button nodeButton;
    [SerializeField] private Image nodeIcon;


    private void Awake()
    {
        if (nodeButton == null) nodeButton = GetComponent<Button>();
    }

    public void SetInteractable(bool isClickable) //node on or off
    {
        nodeButton.interactable = isClickable;

        nodeIcon.color = isClickable ? Color.white : Color.gray;
    }
    

    public EncounterDefinition GetEncounter() => myEncounter;
}