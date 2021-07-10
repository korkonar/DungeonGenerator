using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public GameObject Container;
    [Space]
    [Header("Pieces")]
    public GameObject NONE;
    public GameObject UP;
    public GameObject DOWN;
    public GameObject RIGHT;
    public GameObject LEFT;
    public GameObject UP_DOOR;
    public GameObject DOWN_DOOR;
    public GameObject LEFT_DOOR;
    public GameObject RIGHT_DOOR;
    public GameObject UP_RIGHT;
    public GameObject UP_LEFT;
    public GameObject DOWN_RIGHT;
    public GameObject DOWN_LEFT;
    public GameObject UP_DOWN;
    public GameObject LEFT_RIGHT;
    public GameObject UP_LEFT_DOWN;
    public GameObject UP_RIGHT_DOWN;
    public GameObject DOWN_LEFT_RIGHT;
    public GameObject UP_LEFT_RIGHT;
    public GameObject ALL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate(){
        
        foreach (Transform child in Container.transform) {
            GameObject.Destroy(child.gameObject);
        }

        int minx=100,miny =100;
        int maxx=-10,maxy =-10;

        List<int> rooms= new List<int>();
        for(int x=0;x<CellGridScript.CellArray.GetLength(0);x++){
            for(int y=0;y<CellGridScript.CellArray.GetLength(1);y++){
                if(CellGridScript.CellArray[x,y].roomId<99){
                    if(x<minx)minx=x;
                    else if(x>maxx)maxx=x;
                    if(y<miny)miny=y;
                    else if(y>maxy)maxy=y;
                    int nw=sumOfWall(CellGridScript.CellArray[x,y].wallSide);
                    switch(nw){
                        case 0:
                            Instantiate(NONE,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                        break;
                        case 1:
                            if(CellGridScript.CellArray[x,y].wallSide.x==1){
                                Instantiate(UP,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.y==1){
                                Instantiate(DOWN,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.z==1){  
                                Instantiate(RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.w==1){
                                Instantiate(LEFT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }
                        break;
                        case 2:
                            if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.z==1){
                                Instantiate(UP_RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.w==1){
                                Instantiate(UP_LEFT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                 if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.y==1 && CellGridScript.CellArray[x,y].wallSide.z==1){  
                                Instantiate(DOWN_RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.y==1 && CellGridScript.CellArray[x,y].wallSide.w==1){
                                Instantiate(DOWN_LEFT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                                 if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.y==1){
                                Instantiate(UP_DOWN,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.z==1 && CellGridScript.CellArray[x,y].wallSide.w==1){
                                Instantiate(LEFT_RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                                 if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }
                        break;
                        case 3:
                            if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.z==1 && CellGridScript.CellArray[x,y].wallSide.w==1){
                                Instantiate(UP_LEFT_RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                                 if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.z==1 && CellGridScript.CellArray[x,y].wallSide.y==1){
                                Instantiate(UP_RIGHT_DOWN,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.x==1 && CellGridScript.CellArray[x,y].wallSide.w==1  && CellGridScript.CellArray[x,y].wallSide.y==1){  
                                Instantiate(UP_LEFT_DOWN,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y+1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y+1].roomId)){
                                        Instantiate(UP_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y+1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                                 if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }else if(CellGridScript.CellArray[x,y].wallSide.y==1 && CellGridScript.CellArray[x,y].wallSide.w==1 && CellGridScript.CellArray[x,y].wallSide.z==1){
                                Instantiate(DOWN_LEFT_RIGHT,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x,y-1].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x,y-1].roomId)){
                                        Instantiate(DOWN_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x,y-1].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x+1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x+1,y].roomId)){
                                        Instantiate(RIGHT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x+1,y].roomId);
                                    }
                                }
                                if(CellGridScript.CellArray[x,y].roomId!=CellGridScript.CellArray[x-1,y].roomId){
                                    if(!rooms.Contains(CellGridScript.CellArray[x-1,y].roomId)){
                                        Instantiate(LEFT_DOOR,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                                        rooms.Add(CellGridScript.CellArray[x-1,y].roomId);
                                    }
                                }
                            }
                        break;
                        case 4:
                            Instantiate(ALL,new Vector3(x*3,0,y*3),Quaternion.identity,Container.transform);
                        break;
                    }
                }
            }
        }

        Camera.main.transform.position=new Vector3(((minx+maxx)*3)/2,Camera.main.transform.position.y,(((miny+maxy)*3)/2)-10);

    }

    int sumOfWall(Vector4 v){
		return (int)v.x+(int)v.y+(int)v.z+(int)v.w;
	}
}
