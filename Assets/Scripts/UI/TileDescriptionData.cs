using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDescription", menuName = "Custom/TileDescription", order = 0)]
public class TileDescriptionData : ScriptableObject 
{
    public string title;
    public Sprite icon;
}
