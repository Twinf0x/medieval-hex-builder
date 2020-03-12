using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDescription", menuName = "Custom/BuildingDescription", order = 1)]
public class BuildingDescriptionData : ScriptableObject
{
    public string title;
    public string description;
}
