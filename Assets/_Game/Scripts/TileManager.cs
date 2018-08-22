using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    int rowSize;

    public float moveSpeed = 0.5f;

    public List<TileController> tiles;

    public bool inputLock = false;

    private void Start()
    {
        rowSize = (int) Mathf.Sqrt(tiles.Count);

        ShuffleBoard();
    }


    void ShuffleBoard() {

        for (int i = 0; i < 100; i++) {
            TilePressed(tiles[Random.Range(0, tiles.Count)] );
        }
    }

    public void TilePressed(TileController tile) {

        if (inputLock)
            return;

        // Debug.Log("Tile pressed: " + tile.gameObject.name);

        //SwitchTiles(tiles[2], tiles[1]);

        TileController emptyNeighbour = EmptyNeighbour(tile);

        if (emptyNeighbour != null)
            SwitchTiles(tile, emptyNeighbour);

    }

    void SwitchTiles(TileController tile1, TileController tile2) {
        inputLock = true;

        Vector3 pos = tile1.gameObject.transform.position;

        LeanTween.move(tile1.gameObject, tile2.gameObject.transform.position, moveSpeed)
                 .setEase(LeanTweenType.easeInCubic)
                 .setOnComplete(() => { 
                            inputLock = false; 
        });
        LeanTween.move(tile2.gameObject, pos, moveSpeed).setEase(LeanTweenType.easeInCubic);

       // tile1.gameObject.transform.position = tile2.gameObject.transform.position;
       // tile2.gameObject.transform.position = pos;

        int index1 = TileIndex(tile1);
        int index2 = TileIndex(tile2);

        tiles[index1] = tile2;
        tiles[index2] = tile1;

    }

    TileController EmptyNeighbour(TileController tile) {
        
        foreach( TileController t in Neighbours(tile)) {
            if (t.empty)
                return t;
        }

        return null;
    }


    List<TileController> Neighbours(TileController tile) {
        int index = TileIndex(tile);
        List<TileController> neighbours = new List<TileController>();

        int over = index - rowSize;
        if(InRange(over))
            neighbours.Add(tiles[over]);

        int under = index + rowSize;
        if (InRange(under))
            neighbours.Add(tiles[under]);

        int right = index + 1;
        if (InRange(right) && (index + 1) % rowSize != 0)
            neighbours.Add(tiles[right]);

        int left = index - 1;
        if (InRange(left) && index % rowSize != 0)
            neighbours.Add(tiles[left]);
        
        return neighbours;
    
    }

    bool InRange(int index) {
        return (index >= 0 && index < tiles.Count);
    }


    int TileIndex(TileController tile) {

        return tiles.IndexOf(tile);
    }


}
