
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoxContoller : MonoBehaviour
{
    //script for the prefab of the tile

    public T_TargetGridManager targetGManager;
    bool correctlyPlaced = false;
    public BoxType boxType;
    private Vector3Int lastPositionCell;

    private void Awake()
    {
        Tilemap tileMap = targetGManager.targetTileMap; //tilemap where the target tile are
        SnapToGrid(tileMap); // to snap the boxes in the center of the cell where they are (so they are perfectly centered and avoid colisions or other errors)
    }

    private void Update()
    {
        bool isNowCorrect = targetGManager.isTargetCorrect(transform.position, boxType); //we get if the box is now correct placed, in case the player moves the box

        if(!isNowCorrect && correctlyPlaced)  //if now is not correct but it was before, means the player moved it to another tile (not the target)
        {
            correctlyPlaced = false;
            targetGManager.boxRemoved(lastPositionCell);
        }
        else if (isNowCorrect && !correctlyPlaced) // if its now correct placed but it wasn't before it means the player moved it where it belongs
        {
            correctlyPlaced = true;
            lastPositionCell = targetGManager.targetTileMap.WorldToCell(transform.position);
            targetGManager.boxCorrectlyPlaced(lastPositionCell);
        }
    }

    private void SnapToGrid(Tilemap tileMap)
    {
        Vector3Int cell = tileMap.WorldToCell(transform.position); // we get the position of the cell where the box is
        transform.position = tileMap.GetCellCenterWorld(cell); // we transform the box position to the center of that cell
    }
}
