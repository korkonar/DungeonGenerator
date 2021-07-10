using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintManager : MonoBehaviour
{
    public RoomsUIScript rooms;
    private Room currRoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl)){
            int x = (int)(Input.mousePosition.x/(1000.0f/CellGridScript.CellArray.GetLength(0)));
            int y = (int)(Input.mousePosition.y/(1000.0f/CellGridScript.CellArray.GetLength(0)));
            //print((x-1)+", "+(y-1));
            
            if(CellGridScript.CellArray[x-1,y-1].location>(CellGridScript.cellLocation)1){
                CellGridScript.CellArray[x-1,y-1].roomId=currRoom.ID;
                CellGridScript.CellArray[x-1,y-1].wallSide=new Vector4(1,1,1,1);
                bool outer=false;
                if(CellGridScript.CellArray[x-1,y-2].roomId==currRoom.ID){
                    CellGridScript.CellArray[x-1,y-2].wallSide.x=0;
                    CellGridScript.CellArray[x-1,y-1].wallSide.y=0;
                }else outer=true;
                if(CellGridScript.CellArray[x-1,y].roomId==currRoom.ID){
                    CellGridScript.CellArray[x-1,y].wallSide.y=0;
                    CellGridScript.CellArray[x-1,y-1].wallSide.x=0;
                }else outer=true;
                if(CellGridScript.CellArray[x-2,y-1].roomId==currRoom.ID){
                    CellGridScript.CellArray[x-2,y-1].wallSide.z=0;
                    CellGridScript.CellArray[x-1,y-1].wallSide.w=0;
                }else outer=true;
                if(CellGridScript.CellArray[x,y-1].roomId==currRoom.ID){
                    CellGridScript.CellArray[x,y-1].wallSide.w=0;
                    CellGridScript.CellArray[x-1,y-1].wallSide.z=0;
                }else outer=true;
                if(!currRoom.outerCells.Contains(new Vector2Int(x-1,y-1)) && outer){
                    currRoom.outerCells.Add(new Vector2Int(x-1,y-1));
                }
                currRoom.currSize++;
                currRoom.reCalculateCenterPoint();
                currRoom.currRatio=currRoom.reCalculateRatio(0,0);
            }
            //update viewer
            for(int i=0;i<RoomsUIScript.roomsGO.Count;i++){
                if(RoomsUIScript.roomsGO[i].transform.GetChild(4).GetComponent<Toggle>().isOn){
                    RoomsUIScript.roomsGO[i].transform.GetChild(8).GetComponent<Text>().text="Size: "+currRoom.currSize;
                    RoomsUIScript.roomsGO[i].transform.GetChild(9).GetComponent<Text>().text="Ratio: "+currRoom.currRatio;
                    break;
                }
            }
        }
    }

    public void ChangeRoomToPaint(){
        for(int i=0;i<RoomsUIScript.roomsGO.Count;i++){
            if(RoomsUIScript.roomsGO[i].transform.GetChild(4).GetComponent<Toggle>().isOn){
                currRoom=HouseGraph.allRooms[i];
                break;
            }
        }
    }
}
