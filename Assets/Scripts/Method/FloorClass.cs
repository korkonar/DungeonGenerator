using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor
{
    public int floorId;
    public static int idGenerator=0;
    public List<int> rooms;
    public List<int> CrossFloorRooms;
    public CellGridScript.cell[,] FinishedFloorCellArray;

    public Floor(List<int> frooms){
        floorId=idGenerator++;
        rooms=frooms;
    }

    

}