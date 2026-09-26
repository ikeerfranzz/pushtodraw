using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class T_TargetGridManager : MonoBehaviour
{
    // script that manages everything around the targettilemap (target position of the boxes)

    //variables
    public static T_TargetGridManager T_targetGridManager;
    public Tilemap targetTileMap; // tile map of the target tiles
    public int totalTargets = 0; // number of tiles where there needs to be a box
    public int correctBoxes = 0; //number of correct boxes placed
    public HashSet<Vector3Int> occupiedTargets = new HashSet<Vector3Int>();

    private void Awake()
    {
        T_targetGridManager = this;
        totalTargets = 0; // para reiniciar si cambian de escena o cualquier cosa (controlar errores)
        correctBoxes = 0;
        countTargets();
    }

    /*function that counts how many targets (tiles to put each box) there are*/
    public void countTargets()
    {
        foreach (Vector3Int position in targetTileMap.cellBounds.allPositionsWithin) // we look for all the cells in the tilemap (it gets the position of each cell)
        {
            TileBase tile = targetTileMap.GetTile(position); // we get what tile there's in that position
            if (tile is TargetTile) totalTargets++; // if that tile is a targettile we +1 the number of total targets, we do so we don't have to manually put the number of targets of each level
        }
    }

    /*function that counts every box placed in their tile and the win condition (all boxes are correcly placed)*/
    public void boxCorrectlyPlaced(Vector3Int cell)
    {
        if (occupiedTargets.Add(cell))
        {
            correctBoxes++;
            Debug.Log("Box correctly placed. There's " + correctBoxes + " boxes correctly placed of " + totalTargets + " boxes.");
            if (correctBoxes == totalTargets)
            {
                UIManager.UIManagerScript.showWinMenu();
                Debug.Log("ALL BOXES ARE CORRECTLY PLACED");
            }
            
        }
        //level completed!!!!!!******************************************************************************
    }

    /*function that is triggered when the a correctly placed box is moved from where it belongs*/
    public void boxRemoved(Vector3Int cell)
    { 
        if (occupiedTargets.Remove(cell))
        {
            correctBoxes--;
            Debug.Log("A box has been removed of it's place. There's " + correctBoxes + " boxes correctly placed of " + totalTargets + " boxes.");
        }
    }

    public TargetTile getTile(Vector3 position)
    {
        Vector3Int cell = targetTileMap.WorldToCell(position); //we get the cell the box it's on
        TileBase tile = targetTileMap.GetTile(cell); //we get the tile it's on the cell

        return tile as TargetTile;
    }

    public bool isTargetCorrect(Vector3 position, BoxType boxTag)
    {
        TargetTile tile = getTile(position);
        return tile != null && tile.BoxTag == boxTag;
    }
}
