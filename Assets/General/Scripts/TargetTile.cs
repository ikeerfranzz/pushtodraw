using UnityEngine;
using UnityEngine.Tilemaps;
using static BoxContoller;

// scriptable tile
[CreateAssetMenu(menuName = "Target Tiles")]
public class TargetTile : Tile
{
    public BoxType BoxTag; // tag for the tile 
}
