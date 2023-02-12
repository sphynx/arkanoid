using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


[System.Serializable]
public class TileEvent : UnityEvent<TileBase> { }

[System.Serializable]
public class StringEvent : UnityEvent<string> { }



/*
 * We need at least events for the sounds:
 * 
 * - BrickHit(Tile)
 * - BonusPickedUp
 * - PadHit
 * 
 * 
*/