using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class GrowthMethod : MonoBehaviour
{
    public CameraCapture capture;
    public static int initialized=0;
    public static CellGridScript.cell[,] CellArrayTest;

    int growingfloor=0;
    int phase=0;
    bool someMove;

    bool startedCoroutine=false;
    float currScore=0;
    int roomIndInFloor=0;

    int iterations=0;
    int badIterations;
    int maxBadIterations=1800; //10000
    bool done=false;
    //bool pic=false;
    int currRun=0;
    int NumRuns=0;
    int frames=20;
    int CapFrames=3000;
    int frameInt=0;

    //Simulated annealling
    float P;
    float T;//temperature
    float initT=20;
    float CoolingRate;
    public bool DebugFit ;
    bool first=true;
    int didntMove=0;

    #region FitFactors
        float MinSizeM=3.5f;//3.5
        float MaxSizeM=2.4f;//2.4
        float RatioM=2.5f;//1.5
        float AvDistM=15.0f;//12.4
        float RectM=2.5f;//2.5
        float ConnectM=1.2f;//1.4
        float OneWideM=7.2f;//8.2
        float FillM=3.0f;//3.0
        float FillMb=1.0f;//1.0
    #endregion

    static bool stole=false;
    static float prevToFill=0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        T=initT;
        CoolingRate=0.9995f;
        NumRuns=HouseGraph.numberOfHouses-1;
        //print(CoolingRate);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(initialized);
        
        if(initialized>=2){
            Floor f =HouseGraph.floors[growingfloor];
            if(CellGridScript.currFloorId!=f.floorId){
                RoomInitialPositions(f);
            }else{
                if(!done){  
                    HillClimbingGrowth(f);
                    //Debug.Break();
                }
            }
        }
    }

    void cooling(){
        T=(T*CoolingRate);
        //print(T);
    }

    void HillClimbingGrowth(Floor f){
        
        //frames++;
        //if(frames>CapFrames){
        //    frameInt++;
        //    if(frameInt>250){
        //        capture.CaptureFrame(currRun,currScore,iterations,true);
        //        frameInt=0;
        //    }else{
        //        capture.CaptureFrame(currRun,currScore,iterations,false);
        //    }
////
        //    frames=0;
        //    CapFrames++;
        //}

        for(int ix=0;ix<CellGridScript.CellArray.GetLength(0);ix++){
            for(int iy=0;iy<CellGridScript.CellArray.GetLength(1);iy++){
                CellArrayTest[ix,iy].location=CellGridScript.CellArray[ix,iy].location;
                CellArrayTest[ix,iy].roomId=CellGridScript.CellArray[ix,iy].roomId;
                CellArrayTest[ix,iy].wallSide=CellGridScript.CellArray[ix,iy].wallSide;
            }
        }
        foreach(Room r in HouseGraph.allRooms.Values){
            HouseGraph.allRoomsTest[r.ID].centerPoint=r.centerPoint;
            HouseGraph.allRoomsTest[r.ID].currRatio=r.currRatio;
            HouseGraph.allRoomsTest[r.ID].currSize=r.currSize;
            HouseGraph.allRoomsTest[r.ID].corners=r.corners;
            HouseGraph.allRoomsTest[r.ID].outerCells.Clear();
            foreach(Vector2Int v in r.outerCells){
                HouseGraph.allRoomsTest[r.ID].outerCells.Add(v);
            }

        }
        someMove=false;
        bool effective=false;
        int tested=0;
        int currRoom=f.rooms[roomIndInFloor];
        do{
            effective=false;
            #region EqualChances
                int rMove=Random.Range(0,3);
                stole=false;
                switch(rMove){
                    case 0:
                        //grow
                        effective=HouseGraph.allRoomsTest[currRoom].AddCell();
                        tested++;
                    break;
                    case 1:
                        //shrink
                        effective=HouseGraph.allRoomsTest[currRoom].RemoveCell();
                        tested++;
                    break;
                    case 2:
                        //steal
                        effective=HouseGraph.allRoomsTest[currRoom].StealCell();
                        if(effective)stole=true;
                        tested++;
                    break;
                    case 3:
                        //grow Non rectangular
                        tested++;
                    break;
                    case 4:
                        //exchange
                        tested++;
                    break;
                }
            #endregion

            #region GrowHigherChance
                //float val=Random.value;
                ////print(grow);
                //if(val<=grow){
                //    effective=HouseGraph.allRoomsTest[currRoom].AddCell();
                //}else if(val<=grow+0.33f){
                //    effective=HouseGraph.allRoomsTest[currRoom].RemoveCell();
                //}else{
                //    effective=HouseGraph.allRoomsTest[currRoom].StealCell();
                //}
            #endregion
            
            if(effective)someMove=true;
            if(tested>10){
                //print("didnt move");
                stole=false;
                didntMove++;
                break;
            }
        }while(!effective);
        if(effective){
            float newScore=calculateFitFunction(true);
            float PVal=Mathf.Exp((-Mathf.Abs(newScore-currScore)*1.5f)/(T));
            if(DebugFit)print(currRun+", "+iterations+": "+currScore+"<>"+newScore+" PVal:"+PVal);
            //print(PVal);
            if(currScore<newScore){
                badIterations=0;
                for(int ix=0;ix<CellGridScript.CellArray.GetLength(0);ix++){
                    for(int iy=0;iy<CellGridScript.CellArray.GetLength(1);iy++){
                        CellGridScript.CellArray[ix,iy].location=CellArrayTest[ix,iy].location;
                        CellGridScript.CellArray[ix,iy].roomId=CellArrayTest[ix,iy].roomId;
                        CellGridScript.CellArray[ix,iy].wallSide=CellArrayTest[ix,iy].wallSide;
                    }
                }
                foreach(Room r in HouseGraph.allRoomsTest.Values){
                    HouseGraph.allRooms[r.ID].centerPoint=r.centerPoint;
                    HouseGraph.allRooms[r.ID].currRatio=r.currRatio;
                    HouseGraph.allRooms[r.ID].currSize=r.currSize;
                    HouseGraph.allRooms[r.ID].corners=r.corners;
                    HouseGraph.allRooms[r.ID].outerCells.Clear();
                    foreach(Vector2Int v in r.outerCells){
                        HouseGraph.allRooms[r.ID].outerCells.Add(v);
                    }
                }
                currScore=newScore;
            }else{
                P=Random.value;
                badIterations++;
                if(P<PVal){
                    //print("force change");
                    for(int ix=0;ix<CellGridScript.CellArray.GetLength(0);ix++){
                        for(int iy=0;iy<CellGridScript.CellArray.GetLength(1);iy++){
                            CellGridScript.CellArray[ix,iy].location=CellArrayTest[ix,iy].location;
                            CellGridScript.CellArray[ix,iy].roomId=CellArrayTest[ix,iy].roomId;
                            CellGridScript.CellArray[ix,iy].wallSide=CellArrayTest[ix,iy].wallSide;
                        }
                    }
                    foreach(Room r in HouseGraph.allRoomsTest.Values){
                        HouseGraph.allRooms[r.ID].centerPoint=r.centerPoint;
                        HouseGraph.allRooms[r.ID].currRatio=r.currRatio;
                        HouseGraph.allRooms[r.ID].currSize=r.currSize;
                        HouseGraph.allRooms[r.ID].corners=r.corners;
                        HouseGraph.allRooms[r.ID].outerCells.Clear();
                        foreach(Vector2Int v in r.outerCells){
                            HouseGraph.allRooms[r.ID].outerCells.Add(v);
                        }
                    }
                    currScore=newScore;
                }
            }

            cooling();
            iterations++;
            //print(iterations);
            if(badIterations>=maxBadIterations){
                done=true;
                //if(!pic){
                //    capture.Capture(currRun,currScore,iterations,Time.unscaledTime);
                //    pic=true;
                //}
                if(currRun<NumRuns){
                    currRun++;
                    initialized=0;
                    //GetComponent<HouseGraph>().SaveHouse(currScore,1,iterations);
                    GetComponent<HouseGraph>().Reset();
                    GetComponent<CellGridScript>().ResetArray();
                    CellGridScript.init=false;
                    CellGridScript.currFloorId=555;
                    T=initT;
                    badIterations=0;
                    //pic=false;
                    iterations=0;
                    done=false;
                    startedCoroutine=false;
                }else{
                    //GetComponent<HouseGraph>().SaveHouse(currScore,1,iterations);
                    //CsvWriter.WriteFile(iterations,Time.unscaledTime);
                    GetComponent<Model>().Generate();
                    Time.fixedDeltaTime=0.016f;
                    print("Time for "+currRun+" runs with +-"+iterations+" iterations, Time: "+ Time.unscaledTime);
                    print("didnt Move:"+didntMove);
					print("DONE!");
                }
            }
        }
        roomIndInFloor++;
        if(roomIndInFloor>=f.rooms.Count){
            roomIndInFloor=0;
        }
    }

    public void newGen(){
        Time.fixedDeltaTime=0.00001f;
        initialized=0;
        //GetComponent<HouseGraph>().SaveHouse(currScore,1,iterations);
        GetComponent<HouseGraph>().Reset();
        GetComponent<CellGridScript>().ResetArray();
        CellGridScript.init=false;
        CellGridScript.currFloorId=555;
        T=initT;
        badIterations=0;
        //pic=false;
        iterations=0;
        done=false;
        startedCoroutine=false;
    }

    float calculateFitFunction(bool test){
        float toMinSize=0.0f;
        float toRatio=0.0f;
        float AvDist=0.0f;
        float ToConnectivity=0.0f;
        float ToRectangular=0.0f;
        float ToMaxSize=0.0f;
        float penalizeOneWide=0.0f;
        float ToFill=0.0f;
        float ToFillb=0.0f;
        int distances=0;

        Vector2Int minv= new Vector2Int(500,500);
        Vector2Int maxv= Vector2Int.zero;
        if(test){
            int size=0;
            foreach(Room r in HouseGraph.allRoomsTest.Values){
                size+=r.currSize;

                if(r.currSize< r.minSize){
                    toMinSize += (r.currSize-r.minSize);
                }else if(r.currSize >= r.minSize && r.currSize <= r.maxSize){
                    toMinSize += (r.currSize-r.minSize);
                    ToMaxSize += (r.maxSize-r.currSize);
                }else{  
                    ToMaxSize += (r.maxSize-r.currSize);
                }
                toRatio += (r.maxRatio-r.currRatio);
                
                foreach(Room r2 in HouseGraph.allRoomsTest.Values){
                    if(r2.ID!=r.ID){
                        if(r.connections.Contains(r2.ID)){
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint)*1.5f;
                            AvDist-=Mathf.Sqrt(r.maxSize);
                        }else{
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint);
                            AvDist-=Mathf.Sqrt(r.maxSize);
                        }
                        distances++;
                    }
                }
                int corners=0;
                float upsameCon=0.25f;
                float downsameCon=0.25f;
                float rightsameCon=0.25f;
                float leftsameCon=0.25f;
                float upsame=0.5f;
                float downsame=0.5f;
                float rightsame=0.5f;
                float leftsame=0.5f;

                foreach(Vector2Int v in r.outerCells){
                    if(v.x>maxv.x)maxv.x=v.x;
                    if(v.y>maxv.y)maxv.y=v.y;
                    if(v.x<minv.x)minv.x=v.x;
                    if(v.y<minv.y)minv.y=v.y;
                    Vector4 walls=CellArrayTest[v.x,v.y].wallSide;

                    if(walls.x==1){
                        if(CellArrayTest[v.x,v.y+1].roomId<99){
                            ToConnectivity+=1f/(upsameCon);
                            upsameCon+=0.25f;
                        }else{
                            ToConnectivity-=1f/upsame;
                            upsame+=0.75f;
                        }
                    }
                    if(walls.y==1){
                        if(CellArrayTest[v.x,v.y-1].roomId<99){
                            ToConnectivity+=1f/(downsameCon);
                            downsameCon+=0.30f;
                        }else{
                            ToConnectivity-=1f/downsame;
                            downsame+=0.75f;
                        }
                    }
                    if(walls.z==1){
                        if(CellArrayTest[v.x+1,v.y].roomId<99){
                            ToConnectivity+=1f/(rightsameCon);
                            rightsameCon+=0.30f;
                        }else{
                            ToConnectivity-=1f/rightsame;
                            rightsame+=0.75f;
                        }
                    }
                    if(walls.w==1){
                        if(CellArrayTest[v.x-1,v.y].roomId<99){
                            ToConnectivity+=1f/(leftsameCon);
                            leftsameCon+=0.30f;
                        }else{
                            ToConnectivity-=1f/leftsame;
                            leftsame+=0.75f;
                        }
                    }
                }

                foreach(Vector2Int v in r.outerCells){
                    Vector4 wallSide=CellArrayTest[v.x,v.y].wallSide;
                    if(wallSide.x==1){
                        if(CellArrayTest[v.x,v.y+1].roomId>=99 && CellArrayTest[v.x,v.y+2].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.y==1){
                        if(CellArrayTest[v.x,v.y-1].roomId>=99 && CellArrayTest[v.x,v.y-2].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.z==1){
                        if(CellArrayTest[v.x+1,v.y].roomId>=99 && CellArrayTest[v.x+2,v.y].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.w==1){
                        if(CellArrayTest[v.x-1,v.y].roomId>=99 && CellArrayTest[v.x-2,v.y].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }

                    if(wallSide.x+wallSide.y==2)penalizeOneWide--;
                    if(wallSide.z+wallSide.w==2)penalizeOneWide--;
                    if(wallSide.x==1 && wallSide.z==1)corners++;
                    if(wallSide.x==1 &&wallSide.w==1)corners++;
                    if(wallSide.y==1 && wallSide.z==1)corners++;
                    if(wallSide.y==1 && wallSide.w==1)corners++;
                    if(wallSide.x==1 && (CellArrayTest[v.x+1,v.y+1].wallSide.w==1 && CellArrayTest[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.x==1 && (CellArrayTest[v.x-1,v.y+1].wallSide.z==1 && CellArrayTest[v.x-1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellArrayTest[v.x+1,v.y-1].wallSide.w==1 && CellArrayTest[v.x+1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellArrayTest[v.x-1,v.y-1].wallSide.z==1 && CellArrayTest[v.x-1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellArrayTest[v.x+1,v.y+1].wallSide.y==1 && CellArrayTest[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellArrayTest[v.x+1,v.y-1].wallSide.x==1 && CellArrayTest[v.x+1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellArrayTest[v.x-1,v.y+1].wallSide.y==1 && CellArrayTest[v.x-1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellArrayTest[v.x-1,v.y-1].wallSide.x==1 && CellArrayTest[v.x-1,v.y-1].roomId==r.ID))corners++;
                }
                if(r.Type!=Room.roomType.hall){
                    ToRectangular+=-Mathf.Pow((corners-4),2);
                }else{
                    ToRectangular-=Mathf.Pow((corners-4),1.2f);
                }
                r.corners=corners;
            }
            AvDist=AvDist/distances;
            AvDist=-Mathf.Pow(AvDist,2);

            if(stole){
                ToFill=prevToFill;
            }else{
                int outsidecells=FindOusideCellsTest(minv.x,maxv.x,minv.y,maxv.y);
                ToFill=((((maxv.x+1)-(minv.x-1))+1)*(((maxv.y+1)-(minv.y-1))+1))-(outsidecells+size);
                prevToFill=ToFill;
            }
        
            //print(((((maxv.x+1)-(minv.x-1))+1)*(((maxv.y+1)-(minv.y-1))+1))+"-("+outsidecells+"+"+size+")="+ToFill);
        }else{
            int size=0;
            foreach(Room r in HouseGraph.allRooms.Values){
                size+=r.currSize;
                
                if(r.currSize< r.minSize){
                    toMinSize += (r.currSize-r.minSize);
                }else if(r.currSize >= r.minSize && r.currSize <= r.maxSize){
                    toMinSize += (r.currSize-r.minSize);
                    ToMaxSize += (r.maxSize-r.currSize);
                }else{  
                    ToMaxSize += (r.maxSize-r.currSize);
                }
                toRatio += (r.maxRatio-r.currRatio);
                
                foreach(Room r2 in HouseGraph.allRooms.Values){
                    if(r2.ID!=r.ID){
                        if(r.connections.Contains(r2.ID)){
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint)*1.5f;
                            AvDist-=Mathf.Sqrt(r.maxSize);
                        }else{
                            AvDist+=Vector2.Distance(r.centerPoint,r2.centerPoint);
                            AvDist-=Mathf.Sqrt(r.maxSize);
                        }
                        distances++;
                    }
                }
                int corners=0;
                float upsameCon=0.25f;
                float downsameCon=0.25f;
                float rightsameCon=0.25f;
                float leftsameCon=0.25f;
                float upsame=0.5f;
                float downsame=0.5f;
                float rightsame=0.5f;
                float leftsame=0.5f;

                foreach(Vector2Int v in r.outerCells){
                    if(v.x>maxv.x)maxv.x=v.x;
                    if(v.y>maxv.y)maxv.y=v.y;
                    if(v.x<minv.x)minv.x=v.x;
                    if(v.y<minv.y)minv.y=v.y;
                    Vector4 walls=CellGridScript.CellArray[v.x,v.y].wallSide;
                    if(walls.x==1){
                        if(CellGridScript.CellArray[v.x,v.y+1].roomId<99){
                            ToConnectivity+=1f/(upsameCon);
                            upsameCon+=0.25f;
                        }else{
                            ToConnectivity-=1f/upsame;
                            upsame+=0.75f;
                        }
                    }
                    if(walls.y==1){
                        if(CellGridScript.CellArray[v.x,v.y-1].roomId<99){
                            ToConnectivity+=1f/(downsameCon);
                            downsameCon+=0.25f;
                        }else{
                            ToConnectivity-=1f/downsame;
                            downsame+=0.75f;
                        }
                    }
                    if(walls.z==1){
                        if(CellGridScript.CellArray[v.x+1,v.y].roomId<99){
                            ToConnectivity+=1f/(rightsameCon);
                            rightsameCon+=0.25f;
                        }else{
                            ToConnectivity-=1f/rightsame;
                            rightsame+=0.75f;
                        }
                    }
                    if(walls.w==1){
                        if(CellGridScript.CellArray[v.x-1,v.y].roomId<99){
                            ToConnectivity+=1f/(leftsameCon);
                            leftsameCon+=0.25f;
                        }else{
                            ToConnectivity-=1f/leftsame;
                            leftsame+=0.75f;
                        }
                    }
                }

                foreach(Vector2Int v in r.outerCells){
                    Vector4 wallSide=CellGridScript.CellArray[v.x,v.y].wallSide;
                    if(wallSide.x==1){
                        if(CellGridScript.CellArray[v.x,v.y+1].roomId>=99 && CellGridScript.CellArray[v.x,v.y+2].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.y==1){
                        if(CellGridScript.CellArray[v.x,v.y-1].roomId>=99 && CellGridScript.CellArray[v.x,v.y-2].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.z==1){
                        if(CellGridScript.CellArray[v.x+1,v.y].roomId>=99 && CellGridScript.CellArray[v.x+2,v.y].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    if(wallSide.w==1){
                        if(CellGridScript.CellArray[v.x-1,v.y].roomId>=99 && CellGridScript.CellArray[v.x-2,v.y].roomId<99){
                            ToFillb-=0.2f;
                        }
                    }
                    
                    if(wallSide.x+wallSide.y==2)penalizeOneWide--;
                    if(wallSide.z+wallSide.w==2)penalizeOneWide--;
                    if(wallSide.x==1 && wallSide.z==1)corners++;
                    if(wallSide.x==1 &&wallSide.w==1)corners++;
                    if(wallSide.y==1 && wallSide.z==1)corners++;
                    if(wallSide.y==1 && wallSide.w==1)corners++;
                    if(wallSide.x==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.x==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.z==1 && CellGridScript.CellArray[v.x-1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.y==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.z==1 && CellGridScript.CellArray[v.x-1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.z==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x+1,v.y-1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x-1,v.y+1].roomId==r.ID))corners++;
                    if(wallSide.w==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x-1,v.y-1].roomId==r.ID))corners++;
                }
                if(r.Type!=Room.roomType.hall){
                    ToRectangular+=-Mathf.Pow((corners-4),2);
                }else{
                    ToRectangular-=Mathf.Pow((corners-4),1.2f);
                }
                r.corners=corners;
            }
            AvDist=AvDist/distances;
            AvDist=-Mathf.Pow(AvDist,2);

            //toFill Proper
            if(stole){
                ToFill=prevToFill;
            }else{
                int outsidecells=FindOusideCellsTest(minv.x,maxv.x,minv.y,maxv.y);
//
                ToFill=((((maxv.x+1)-(minv.x-1))+1)*(((maxv.y+1)-(minv.y-1))+1))-(outsidecells+size);
                prevToFill=ToFill;
            }
            //print(((((maxv.x+1)-(minv.x-1))+1)*(((maxv.y+1)-(minv.y-1))+1))+"-("+outsidecells+"+"+size+")="+ToFill);
        }

        if(DebugFit){
            print("MinSize: "+MinSizeM*toMinSize);
            print("MaxSize: "+MaxSizeM*ToMaxSize);
            print("Size: "+(MaxSizeM*ToMaxSize+MinSizeM*toMinSize));
            print("Ratio: "+RatioM*toRatio);
            print("AvDistance: "+AvDistM*AvDist);
            print("Corners: "+RectM*ToRectangular);
            print("Connectivity: "+ConnectM*ToConnectivity);
            print("OneWide: "+OneWideM*penalizeOneWide);
            print("Fill: "+FillM*(-ToFill));
            print("Fillb: "+FillMb*(-ToFillb));
        }
        float fit=(MinSizeM*toMinSize)+ //minSize
        (MaxSizeM*ToMaxSize)+           //maxSize
        (RatioM*toRatio)+               //ratio
        (AvDistM*AvDist)+               //Distance
        (ConnectM*ToConnectivity)+      //connectivity
        (RectM*ToRectangular)+          //room rectangularity
        (OneWideM*penalizeOneWide)+     //OneWide penalization
        (FillM*(-ToFill))+  
        (FillMb*(-ToFillb));          

        return fit;
    }

    int FindOusideCells(int minx,int maxx, int miny, int maxy){
        //print(minx+", "+maxx+", "+miny+", "+maxy);
        int count=0;
        Vector2Int vector=new Vector2Int(minx-1,miny-1);
       
        List<Vector2Int> neighbors=new List<Vector2Int>();
        List<Vector2Int> explored=new List<Vector2Int>();
        neighbors.Add(vector);
        explored.Add(vector);
        count++;
        do{
            Vector2Int v=neighbors[0];
            if(v.x+1<=maxx+1 && CellGridScript.CellArray[v.x+1,v.y].roomId>=99 && !explored.Contains(new Vector2Int(v.x+1,v.y))){
                neighbors.Add(new Vector2Int(v.x+1,v.y));
                explored.Add(new Vector2Int(v.x+1,v.y));
                count++;
            }
            if(v.x-1>=minx-1 && CellGridScript.CellArray[v.x-1,v.y].roomId>=99 && !explored.Contains(new Vector2Int(v.x-1,v.y))){
                neighbors.Add(new Vector2Int(v.x-1,v.y));
                explored.Add(new Vector2Int(v.x-1,v.y));
                count++;
            }
            if(v.y+1<=maxy+1 && CellGridScript.CellArray[v.x,v.y+1].roomId>=99 && !explored.Contains(new Vector2Int(v.x,v.y+1))){
                neighbors.Add(new Vector2Int(v.x,v.y+1));
                explored.Add(new Vector2Int(v.x,v.y+1));
                count++;
            }
            if(v.y-1>=miny-1 && CellGridScript.CellArray[v.x,v.y-1].roomId>=99 && !explored.Contains(new Vector2Int(v.x,v.y-1))){
                neighbors.Add(new Vector2Int(v.x,v.y-1));
                explored.Add(new Vector2Int(v.x,v.y-1));
                count++;
            }
            neighbors.Remove(v);
        }while(neighbors.Count>0);

        return count;
    }

    int FindOusideCellsTest(int minx,int maxx, int miny, int maxy){
        //print(minx+", "+maxx+", "+miny+", "+maxy);
        int count=0;
        Vector2Int vector=new Vector2Int(minx-1,miny-1);

        List<Vector2Int> neighbors=new List<Vector2Int>();
        List<Vector2Int> explored=new List<Vector2Int>();
        neighbors.Add(vector);
        explored.Add(vector);
        count++;
        do{
            Vector2Int v=neighbors[0];
            if(v.x+1<=maxx+1 && CellArrayTest[v.x+1,v.y].roomId>=99 && !explored.Contains(new Vector2Int(v.x+1,v.y))){
                neighbors.Add(new Vector2Int(v.x+1,v.y));
                explored.Add(new Vector2Int(v.x+1,v.y));
                count++;
            }
            if(v.x-1>=minx-1 && CellArrayTest[v.x-1,v.y].roomId>=99 && !explored.Contains(new Vector2Int(v.x-1,v.y))){
                neighbors.Add(new Vector2Int(v.x-1,v.y));
                explored.Add(new Vector2Int(v.x-1,v.y));
                count++;
            }
            if(v.y+1<=maxy+1 && CellArrayTest[v.x,v.y+1].roomId>=99 && !explored.Contains(new Vector2Int(v.x,v.y+1))){
                neighbors.Add(new Vector2Int(v.x,v.y+1));
                explored.Add(new Vector2Int(v.x,v.y+1));
                count++;
            }
            if(v.y-1>=miny-1 && CellArrayTest[v.x,v.y-1].roomId>=99 && !explored.Contains(new Vector2Int(v.x,v.y-1))){
                neighbors.Add(new Vector2Int(v.x,v.y-1));
                explored.Add(new Vector2Int(v.x,v.y-1));
                count++;
            }
            neighbors.Remove(v);
            
        }while(neighbors.Count>0);

        return count;
    }

    void RoomInitialPositions(Floor f){
        if(!startedCoroutine){
            //print("initializing");
            StartCoroutine(initPos(f));
            //print("initializing");
            if(first){
                CellArrayTest = new CellGridScript.cell[CellGridScript.CellArray.GetLength(0),CellGridScript.CellArray.GetLength(1)];
            }
            startedCoroutine=true;
            for(int ix=0;ix<CellGridScript.CellArray.GetLength(0);ix++){
                for(int iy=0;iy<CellGridScript.CellArray.GetLength(1);iy++){
                    if(first){
                        CellArrayTest[ix,iy]=new CellGridScript.cell();
                    }
                    CellArrayTest[ix,iy].location=0;
                    CellArrayTest[ix,iy].roomId=99;
                    CellArrayTest[ix,iy].wallSide=Vector4.zero;
                }
            }
            currScore=calculateFitFunction(false);
            first=false;
        }
    }

    IEnumerator initPos(Floor f){
        //TODO Implement actual logic
        List<int> placedRooms=new List<int>();
        List<Vector2Int> innerCells=new List<Vector2Int>();
		List<Vector2Int> usedCells=new List<Vector2Int>();

        for(int xi=2; xi<CellGridScript.CellArray.GetLength(0)-2;xi++){
            for(int yi=2; yi<CellGridScript.CellArray.GetLength(0)-2;yi++){
                if(CellGridScript.CellArray[xi,yi].location==CellGridScript.cellLocation.Inner){
                    innerCells.Add(new Vector2Int(xi,yi));
                }
            }
        }
		//print(innerCells.Count);

        //select cell in the border of the inner cells
        Vector2Int v=new Vector2Int(0,0);

        int rng =Random.Range(0,innerCells.Count);
        v=innerCells[rng];
        
		innerCells.Clear();
		
        //assign it to first room(always id 0)
        CellGridScript.CellArray[v.x,v.y].roomId=0;
        HouseGraph.allRooms[0].outerCells.Add(v);
        HouseGraph.allRooms[0].centerPoint=v;
        HouseGraph.allRooms[0].currSize=1;
        CellGridScript.CellArray[v.x,v.y].wallSide=Vector4.one;
		if((int)CellGridScript.CellArray[(v+Vector2Int.up).x,(v+Vector2Int.up).y].location>1){
			innerCells.Add(v+Vector2Int.up);
		}
		if((int)CellGridScript.CellArray[(v+Vector2Int.down).x,(v+Vector2Int.down).y].location>1){
			innerCells.Add(v+Vector2Int.down);
		}
		if((int)CellGridScript.CellArray[(v+Vector2Int.left).x,(v+Vector2Int.left).y].location>1){
			innerCells.Add(v+Vector2Int.left);
		}
		if((int)CellGridScript.CellArray[(v+Vector2Int.right).x,(v+Vector2Int.right).y].location>1){
			innerCells.Add(v+Vector2Int.right);
		}
		usedCells.Add(v);
		//print(innerCells.Count);

        placedRooms.Add(0);

        do{
            //go through all connections place them close
            foreach(Room r in HouseGraph.allRooms.Values){
                if(!placedRooms.Contains(r.ID)){
            
                    rng = Random.Range(0,innerCells.Count);
                    v = innerCells[rng];

                    CellGridScript.CellArray[v.x,v.y].roomId = r.ID;
                    HouseGraph.allRooms[r.ID].outerCells.Add(v);
                    HouseGraph.allRooms[r.ID].centerPoint = v;
                    HouseGraph.allRooms[r.ID].currSize = 1;
                    CellGridScript.CellArray[v.x,v.y].wallSide = Vector4.one;
					//print(v);
					usedCells.Add(v);
						
					if((int)CellGridScript.CellArray[(v+Vector2Int.up).x,(v+Vector2Int.up).y].location>1 && !usedCells.Contains(v+Vector2Int.up) && !innerCells.Contains(v+Vector2Int.up)){
						innerCells.Add(v+Vector2Int.up);
					}
					if((int)CellGridScript.CellArray[(v+Vector2Int.down).x,(v+Vector2Int.down).y].location>1 && !usedCells.Contains(v+Vector2Int.down)&& !innerCells.Contains(v+Vector2Int.down)){
						innerCells.Add(v+Vector2Int.down);
					}
					if((int)CellGridScript.CellArray[(v+Vector2Int.left).x,(v+Vector2Int.left).y].location>1 && !usedCells.Contains(v+Vector2Int.left)&& !innerCells.Contains(v+Vector2Int.left)){
						innerCells.Add(v+Vector2Int.left);
					}
					if((int)CellGridScript.CellArray[(v+Vector2Int.right).x,(v+Vector2Int.right).y].location>1 && !usedCells.Contains(v+Vector2Int.right)&& !innerCells.Contains(v+Vector2Int.right)){
						innerCells.Add(v+Vector2Int.right);
					}
					innerCells.Remove(v);					
                    placedRooms.Add(r.ID);
                          
                }
            }
            
        }while(placedRooms.Count< f.rooms.Count);
        CellGridScript.init=true;
        initialized++;
        CellGridScript.currFloorId=f.floorId;
		//Debug.Break();
		
        yield return null;
    }

    public void toggleDebugFit()=>DebugFit=!DebugFit;
}
