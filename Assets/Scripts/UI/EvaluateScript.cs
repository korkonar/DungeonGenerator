using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluateScript : MonoBehaviour
{

    #region FitFactors
        float MinSizeM=3.0f;
        float MaxSizeM=2.4f;
        float RatioM=1.5f;
        float AvDistM=12.4f;
        float RectM=1.8f;
        float ConnectM=1.6f;
        float OneWideM=3.4f;
        float FillM=2.6f;
        float ToBound=0.25f;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fitFunction(){
        float toMinSize=0.0f;
        float toRatio=0.0f;
        float AvDist=0.0f;
        float ToConnectivity=0.0f;
        float ToRectangular=0.0f;
        float ToMaxSize=0.0f;
        float penalizeOneWide=0.0f;
        float ToFill=0.0f;
        float ToBoundingBox=0.0f;
        int distances=0;
        
        Vector2Int minv= new Vector2Int(500,500);
        Vector2Int maxv= Vector2Int.zero;
        foreach(Room r in HouseGraph.allRooms.Values){
                if(r.currSize<r.minSize){
                    toMinSize+=(r.currSize-r.minSize)*1.2f;
                }else if(r.currSize>r.minSize && r.currSize<r.maxSize){
                    toMinSize+=(r.currSize-r.minSize)/2;
                    ToMaxSize+=(r.maxSize-r.currSize)/2;
                }else{
                    ToMaxSize+=(r.maxSize-r.currSize)*1.5f;
                }
                if(r.currRatio>r.maxRatio){
                    toRatio+=Mathf.Pow((r.maxRatio-r.currRatio),3);
                }else{
                    toRatio+=1;
                }
                foreach(Room r2 in HouseGraph.allRooms.Values){
                    if(r2.ID!=r.ID){
                        if(r.connections.Contains(r2.ID)){
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint)*1.5f;
                            AvDist-=Mathf.Sqrt(r.minSize);
                        }else{
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint);
                            AvDist-=Mathf.Sqrt(r.minSize);
                        }
                        distances++;
                    }
                }
                int corners=0;
                float same=0.5f;
                foreach(Vector2Int v in r.outerCells){
                    if(v.x>maxv.x)maxv.x=v.x;
                    if(v.y>maxv.y)maxv.y=v.y;
                    if(v.x<minv.x)minv.x=v.x;
                    if(v.y<minv.y)minv.y=v.y;
                    Vector4 walls=CellGridScript.CellArray[v.x,v.y].wallSide;
                    if(walls.x==1){
                        if(r.connections.Contains(CellGridScript.CellArray[v.x,v.y+1].roomId)){
                            ToConnectivity+=2.5f/same;
                        }else if(CellGridScript.CellArray[v.x,v.y+1].roomId<99){
                            ToConnectivity+=1f/(same);
                        }
                    }
                    if(walls.y==1){
                        if(r.connections.Contains(CellGridScript.CellArray[v.x,v.y-1].roomId)){
                            ToConnectivity+=2.5f/same;
                        }else if(CellGridScript.CellArray[v.x,v.y-1].roomId<99){
                            ToConnectivity+=1f/(same);
                        }
                    }
                    if(walls.z==1){
                        if(r.connections.Contains(CellGridScript.CellArray[v.x+1,v.y].roomId)){
                            ToConnectivity+=2.5f/same;
                        }else if(CellGridScript.CellArray[v.x+1,v.y].roomId<99){
                            ToConnectivity+=1f/(same);
                        }
                    }
                    if(walls.w==1){
                        if(r.connections.Contains(CellGridScript.CellArray[v.x-1,v.y].roomId)){
                            ToConnectivity+=2.5f/same;
                        }else if(CellGridScript.CellArray[v.x-1,v.y].roomId<99){
                            ToConnectivity+=1f/(same);
                        }
                    }
                    same+=0.25f;
                }

                foreach(Vector2Int v in r.outerCells){
                    Vector4 wallSide=CellGridScript.CellArray[v.x,v.y].wallSide;
                    if(wallSide.x==1){
                        if(CellGridScript.CellArray[v.x,v.y+1].roomId>=99 && CellGridScript.CellArray[v.x,v.y+2].roomId<99){
                            ToFill-=0.2f;
                        }
                    }
                    if(wallSide.y==1){
                        if(CellGridScript.CellArray[v.x,v.y-1].roomId>=99 && CellGridScript.CellArray[v.x,v.y-2].roomId<99){
                            ToFill-=0.2f;
                        }
                    }
                    if(wallSide.z==1){
                        if(CellGridScript.CellArray[v.x+1,v.y].roomId>=99 && CellGridScript.CellArray[v.x+2,v.y].roomId<99){
                            ToFill-=0.2f;
                        }
                    }
                    if(wallSide.w==1){
                        if(CellGridScript.CellArray[v.x-1,v.y].roomId>=99 && CellGridScript.CellArray[v.x-2,v.y].roomId<99){
                            ToFill-=0.2f;
                        }
                    }
                    if(wallSide.x+wallSide.y==2)penalizeOneWide--;
                    if(wallSide.z+wallSide.w==2)penalizeOneWide--;
                    if(wallSide.x==1 && wallSide.z==1)corners++;
                    if(wallSide.x==1 &&wallSide.w==1)corners++;
                    if(wallSide.y==1 && wallSide.z==1)corners++;
                    if(wallSide.y==1 && wallSide.w==1)corners++;
                    if(wallSide.x==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.x==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.z==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.z==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                }
                ToRectangular+=-Mathf.Pow((corners-4),2);
            }
            AvDist=AvDist/distances;
            AvDist=-Mathf.Pow(AvDist,2);

        int boundingSise=(maxv.x-minv.x)*(maxv.y-minv.y);
        if(boundingSise<=HouseGraph.boudingSize){
            ToBoundingBox=25+(HouseGraph.boudingSize-boundingSise);
        }else{
            ToBoundingBox=25-(boundingSise-HouseGraph.boudingSize);
        }

        //print("MinSize: "+MinSizeM*toMinSize);
        transform.GetChild(2).GetComponent<Text>().text="MinSize:"+(MinSizeM*toMinSize);
        //print("MaxSize: "+MaxSizeM*ToMaxSize);
        transform.GetChild(3).GetComponent<Text>().text="MaxSize:"+(MaxSizeM*ToMaxSize);
        //print("Size: "+(MaxSizeM*ToMaxSize+MinSizeM*toMinSize));
        transform.GetChild(4).GetComponent<Text>().text="Total Size:"+(MaxSizeM*ToMaxSize+MinSizeM*toMinSize);
        //print("Ratio: "+RatioM*toRatio);
        transform.GetChild(5).GetComponent<Text>().text="Ratio:"+(RatioM*toRatio);
        //print("AvDistance: "+AvDistM*AvDist);
        transform.GetChild(6).GetComponent<Text>().text="Distance:"+(AvDistM*AvDist);
        //print("Corners: "+RectM*ToRectangular);
        transform.GetChild(7).GetComponent<Text>().text="Corners"+(RectM*ToRectangular);
        //print("Connectivity: "+ConnectM*ToConnectivity);
        transform.GetChild(8).GetComponent<Text>().text="Connectivity:"+(ConnectM*ToConnectivity);
        //print("OneWide: "+OneWideM*penalizeOneWide);
        transform.GetChild(9).GetComponent<Text>().text="One Wide:"+(OneWideM*penalizeOneWide);
        //print("Fill: "+FillM*ToFill);
        transform.GetChild(10).GetComponent<Text>().text="Fill:"+(FillM*ToFill);
        print(ToBoundingBox);
        
        float fit=(MinSizeM*toMinSize)+ //minSize
        (MaxSizeM*ToMaxSize)+           //maxSize
        (RatioM*toRatio)+               //ratio
        (AvDistM*AvDist)+               //Distance
        (ConnectM*ToConnectivity)+      //connectivity
        (RectM*ToRectangular)+          //room rectangularity
        (OneWideM*penalizeOneWide)+     //OneWide penalization
        (FillM*ToFill);                 //Fill holes

        transform.GetChild(1).GetComponent<Text>().text=""+(fit);
    
    }

}
