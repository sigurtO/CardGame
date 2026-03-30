using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum TargetType { EnemyOnly, AllyOnly, Anyone }

[CreateAssetMenu(menuName = "Card Game/New Card")]
public class CardData : ScriptableObject
{
    [Header("Card info")]
    [SerializeField] public string cardName;
    [SerializeField] public string description;
    [SerializeField] public int manaCost;
    [SerializeField] public Sprite image;


    [Header("Card Effects")]
    [SerializeField]
    private List<EffectPayload> effects;
    [SerializeField] 
    public TargetType targetType;
    public List<EffectPayload> Effects => effects; // read only
}
