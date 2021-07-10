using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public int ID;
	
	public float minSize;
	public float maxSize;
	
	public float minRatio;
	public float maxRatio;
	
	public int currSize;
	public float currRatio;

	public int corners;

	public List<int> currentAdj;
	
	bool Multifloor;
	
	public string name;

	public enum roomType{
		hall,
		living,
		dining,
		kitchen,
		bedroom,
		toilet,
		bathroom,
		ensuite,
		stair,
		extra,
		garage
	}
	public roomType Type;
	public List<Vector2Int> outerCells;
	public Vector2 centerPoint;
	public List<int> connections;
	
	public Room(float minS,float maxS,float minR, float maxR, string n, int id, roomType type){
		ID=id;
		minSize=minS;
		maxSize=maxS;
		minRatio=minR;
		maxRatio=maxR;
		name=n;
		Type=type;
        connections=new List<int>();
        outerCells=new List<Vector2Int>();
		currSize=0;
		currRatio=1;
		currentAdj= new List<int>();
	}

    public Room(float minS,float maxS,float minR, float maxR, string n, int id, roomType type, int[] conn){
		ID=id;
		minSize=minS;
		maxSize=maxS;
		minRatio=minR;
		maxRatio=maxR;
		name=n;
		Type=type;
        connections=new List<int>();
        foreach(int i in conn){
            connections.Add(i);
        }
        outerCells=new List<Vector2Int>();
		currSize=0;
		currRatio=1;

		currentAdj= new List<int>();
	}
	
	public bool AddCell(){
		int cellChoice=Random.Range(0,outerCells.Count);
		Vector2Int v =outerCells[cellChoice];
		Vector4 aSides=GrowthMethod.CellArrayTest[v.x,v.y].wallSide;
		int rng=Random.Range(0,((int)aSides.x+(int)aSides.y+(int)aSides.z+(int)aSides.w));
		int good=0;

		if(aSides.x==1){
			if(good==rng){
				//addCellUp
				if((GrowthMethod.CellArrayTest[v.x,v.y+1].location==CellGridScript.cellLocation.Inside || GrowthMethod.CellArrayTest[v.x,v.y+1].location==CellGridScript.cellLocation.Inner) && GrowthMethod.CellArrayTest[v.x,v.y+1].roomId>=99){
					GrowthMethod.CellArrayTest[v.x,v.y+1].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.x=0;
					GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide=new Vector4(1,0,1,1);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x,v.y+2].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x,v.y+2].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y+2].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x,v.y+2));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x,v.y+1));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Add "+0);
					currSize++;
					currRatio=reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.y==1){
			if(good==rng){
				//addCellDown
				if((GrowthMethod.CellArrayTest[v.x,v.y-1].location==CellGridScript.cellLocation.Inside || GrowthMethod.CellArrayTest[v.x,v.y-1].location==CellGridScript.cellLocation.Inner) && GrowthMethod.CellArrayTest[v.x,v.y-1].roomId>=99){
					GrowthMethod.CellArrayTest[v.x,v.y-1].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.y=0;
					GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide=new Vector4(0,1,1,1);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x,v.y-2].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x,v.y-2].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y-2].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x,v.y-2));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x,v.y-1));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Add "+1);
					currSize++;
					currRatio=reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.z==1){
			if(good==rng){
				//addCellRight
				if((GrowthMethod.CellArrayTest[v.x+1,v.y].location==CellGridScript.cellLocation.Inside || GrowthMethod.CellArrayTest[v.x+1,v.y].location==CellGridScript.cellLocation.Inner) && GrowthMethod.CellArrayTest[v.x+1,v.y].roomId>=99){
					GrowthMethod.CellArrayTest[v.x+1,v.y].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.z=0;
					GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide=new Vector4(1,1,1,0);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x+2,v.y].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+2,v.y].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+2,v.y].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+2,v.y));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x+1,v.y));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Add "+2);
					currSize++;
					currRatio=reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.w==1){
			if(good==rng){
				//addCellLeft
				if((GrowthMethod.CellArrayTest[v.x-1,v.y].location==CellGridScript.cellLocation.Inside || GrowthMethod.CellArrayTest[v.x-1,v.y].location==CellGridScript.cellLocation.Inner) && GrowthMethod.CellArrayTest[v.x-1,v.y].roomId>=99){
					GrowthMethod.CellArrayTest[v.x-1,v.y].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.w=0;
					GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide=new Vector4(1,1,0,1);
					if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-2,v.y].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-2,v.y].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-2,v.y].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-2,v.y));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x-1,v.y));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Add "+3);
					currSize++;
					currRatio=reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		return false;
	}
	public bool RemoveCell(){
		if(currSize>1){
			int cellChoice=Random.Range(0,outerCells.Count);
			Vector2Int v =outerCells[cellChoice];

			//make sure that we are not removing a cell that would cut the room in two chunks
			if(!isCellJoint(GrowthMethod.CellArrayTest[v.x,v.y].wallSide, v)){
				Vector4 aSides=GrowthMethod.CellArrayTest[v.x,v.y].wallSide;
				GrowthMethod.CellArrayTest[v.x,v.y].wallSide=Vector4.zero;
				GrowthMethod.CellArrayTest[v.x,v.y].roomId=99;
				outerCells.Remove(v);
				if(aSides.x==0){
					GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.y=1;
					if(!outerCells.Contains(new Vector2Int(v.x,v.y+1))){
						outerCells.Add(new Vector2Int(v.x,v.y+1));
					}
				}
				if(aSides.y==0){
					GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.x=1;
					if(!outerCells.Contains(new Vector2Int(v.x,v.y-1))){
						outerCells.Add(new Vector2Int(v.x,v.y-1));
					}
				}
				if(aSides.z==0){
					GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.w=1;
					if(!outerCells.Contains(new Vector2Int(v.x+1,v.y))){
						outerCells.Add(new Vector2Int(v.x+1,v.y));
					}
				}
				if(aSides.w==0){
					GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.z=1;
					if(!outerCells.Contains(new Vector2Int(v.x-1,v.y))){
						outerCells.Add(new Vector2Int(v.x-1,v.y));
					}
				}
				//Debug.Log(name+" Remove "+v);
				currSize--;
				currRatio=reCalculateRatioTest(0,0);
				reCalculateCenterPoint();
				return true;	
			}
		}
		return false;
	}

	public bool StealCell(){
		int cellChoice=Random.Range(0,outerCells.Count);
		Vector2Int v =outerCells[cellChoice];
		Vector4 aSides=GrowthMethod.CellArrayTest[v.x,v.y].wallSide;
		int rng=Random.Range(0,sumOfWall(aSides));
		int good=0;

		if(aSides.x==1){
			if(good==rng){
				//addCellUp
				if(HouseGraph.allRoomsTest.ContainsKey(GrowthMethod.CellArrayTest[v.x,v.y+1].roomId) && !isCellJoint(GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide, new Vector2Int(v.x,v.y+1))){
					int stoleId=GrowthMethod.CellArrayTest[v.x,v.y+1].roomId;
					HouseGraph.allRoomsTest[stoleId].outerCells.Remove(new Vector2Int(v.x,v.y+1));
					GrowthMethod.CellArrayTest[v.x,v.y+1].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.x=0;
					GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide=new Vector4(1,0,1,1);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y+1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.w=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x+1,v.y+1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x+1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y+1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.z=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x-1,v.y+1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x-1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x,v.y+2].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x,v.y+2].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y+2].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x,v.y+2));
						}
					}else if(GrowthMethod.CellArrayTest[v.x,v.y+2].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x,v.y+2].wallSide.y=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x,v.y+2))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x,v.y+2));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y+1].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x,v.y+1));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}

					//Debug.Log(name+" Steal "+0);
					currSize++;
					HouseGraph.allRoomsTest[stoleId].currSize--;
					currRatio=reCalculateRatioTest(0,0);
					HouseGraph.allRoomsTest[stoleId].currRatio=HouseGraph.allRoomsTest[stoleId].reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					HouseGraph.allRoomsTest[stoleId].reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.y==1){
			if(good==rng){
				//addCellDown
				if(HouseGraph.allRoomsTest.ContainsKey(GrowthMethod.CellArrayTest[v.x,v.y-1].roomId) && !isCellJoint(GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide, new Vector2Int(v.x,v.y-1))){
					int stoleId=GrowthMethod.CellArrayTest[v.x,v.y-1].roomId;
					HouseGraph.allRoomsTest[stoleId].outerCells.Remove(new Vector2Int(v.x,v.y-1));
					GrowthMethod.CellArrayTest[v.x,v.y-1].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.y=0;
					GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide=new Vector4(0,1,1,1);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y-1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.w=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x+1,v.y-1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x+1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y-1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.z=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x-1,v.y-1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x-1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x,v.y-2].roomId==ID){
						GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x,v.y-2].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y-2].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x,v.y-2));
						}
					}else if(GrowthMethod.CellArrayTest[v.x,v.y-2].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x,v.y-2].wallSide.x=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x,v.y-2))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x,v.y-2));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y-1].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x,v.y-1));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Steal "+1);
					currSize++;
					HouseGraph.allRoomsTest[stoleId].currSize--;
					currRatio=reCalculateRatioTest(0,0);
					HouseGraph.allRoomsTest[stoleId].currRatio=HouseGraph.allRoomsTest[stoleId].reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					HouseGraph.allRoomsTest[stoleId].reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.z==1){
			if(good==rng){
				//addCellRight
				if(HouseGraph.allRoomsTest.ContainsKey(GrowthMethod.CellArrayTest[v.x+1,v.y].roomId) && !isCellJoint(GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide, new Vector2Int(v.x+1,v.y))){
					int stoleId=GrowthMethod.CellArrayTest[v.x+1,v.y].roomId;
					HouseGraph.allRoomsTest[stoleId].outerCells.Remove(new Vector2Int(v.x+1,v.y));
					GrowthMethod.CellArrayTest[v.x+1,v.y].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.z=0;
					GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide=new Vector4(1,1,1,0);
					if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y-1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x+1,v.y-1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x+1,v.y-1].wallSide.x=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x+1,v.y-1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x+1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+1,v.y+1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x+1,v.y+1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x+1,v.y+1].wallSide.y=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x+1,v.y+1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x+1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x+2,v.y].roomId==ID){
						GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide.z=0;
						GrowthMethod.CellArrayTest[v.x+2,v.y].wallSide.w=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x+2,v.y].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x+2,v.y));
						}
					}else if(GrowthMethod.CellArrayTest[v.x+2,v.y].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x+2,v.y].wallSide.w=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x+2,v.y))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x+2,v.y));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x+1,v.y].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x+1,v.y));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Steal "+2);
					currSize++;
					HouseGraph.allRoomsTest[stoleId].currSize--;
					currRatio=reCalculateRatioTest(0,0);
					HouseGraph.allRoomsTest[stoleId].currRatio=HouseGraph.allRoomsTest[stoleId].reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					HouseGraph.allRoomsTest[stoleId].reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		if(aSides.w==1){
			if(good==rng){
				//addCellLeft
				if(HouseGraph.allRoomsTest.ContainsKey(GrowthMethod.CellArrayTest[v.x-1,v.y].roomId) && !isCellJoint(GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide,new Vector2Int(v.x-1,v.y))){
					int stoleId=GrowthMethod.CellArrayTest[v.x-1,v.y].roomId;
					HouseGraph.allRoomsTest[stoleId].outerCells.Remove(new Vector2Int(v.x-1,v.y));
					GrowthMethod.CellArrayTest[v.x-1,v.y].roomId=ID;
					GrowthMethod.CellArrayTest[v.x,v.y].wallSide.w=0;
					GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide=new Vector4(1,1,0,1);
					if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.y=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.x=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y-1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x-1,v.y-1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x-1,v.y-1].wallSide.x=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x-1,v.y-1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x-1,v.y-1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.x=0;
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.y=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-1,v.y+1));
						}
					}else if(GrowthMethod.CellArrayTest[v.x-1,v.y+1].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x-1,v.y+1].wallSide.y=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x-1,v.y+1))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x-1,v.y+1));
						}
					}
					if(GrowthMethod.CellArrayTest[v.x-2,v.y].roomId==ID){
						GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide.w=0;
						GrowthMethod.CellArrayTest[v.x-2,v.y].wallSide.z=0;
						if(sumOfWall(GrowthMethod.CellArrayTest[v.x-2,v.y].wallSide)==0){
							outerCells.Remove(new Vector2Int(v.x-2,v.y));
						}
					}else if(GrowthMethod.CellArrayTest[v.x-2,v.y].roomId==stoleId){
						GrowthMethod.CellArrayTest[v.x-2,v.y].wallSide.z=1;
						if(!HouseGraph.allRoomsTest[stoleId].outerCells.Contains(new Vector2Int(v.x-2,v.y))){
							HouseGraph.allRoomsTest[stoleId].outerCells.Add(new Vector2Int(v.x-2,v.y));
						}
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x-1,v.y].wallSide)!=0){
						outerCells.Add(new Vector2Int(v.x-1,v.y));
					}
					if(sumOfWall(GrowthMethod.CellArrayTest[v.x,v.y].wallSide)==0){
						outerCells.Remove(v);
					}
					//Debug.Log(name+" Steal "+3);
					currSize++;
					HouseGraph.allRoomsTest[stoleId].currSize--;
					currRatio=reCalculateRatioTest(0,0);
					HouseGraph.allRoomsTest[stoleId].currRatio=HouseGraph.allRoomsTest[stoleId].reCalculateRatioTest(0,0);
					reCalculateCenterPoint();
					HouseGraph.allRoomsTest[stoleId].reCalculateCenterPoint();
					return true;
				}
			}
			good++;
		}
		return false;
	}

	bool isCellJoint(Vector4 v, Vector2Int vv){
		int sum=sumOfWall(v);
		if(sum==1){
			if(v.x==1 && (GrowthMethod.CellArrayTest[vv.x-1,vv.y-1].roomId!=ID || GrowthMethod.CellArrayTest[vv.x+1,vv.y-1].roomId!=ID))return true;
			if(v.y==1 && (GrowthMethod.CellArrayTest[vv.x-1,vv.y+1].roomId!=ID || GrowthMethod.CellArrayTest[vv.x+1,vv.y+1].roomId!=ID))return true;
			if(v.z==1 && (GrowthMethod.CellArrayTest[vv.x-1,vv.y-1].roomId!=ID || GrowthMethod.CellArrayTest[vv.x-1,vv.y+1].roomId!=ID))return true;
			if(v.w==1 && (GrowthMethod.CellArrayTest[vv.x+1,vv.y-1].roomId!=ID || GrowthMethod.CellArrayTest[vv.x+1,vv.y+1].roomId!=ID))return true;
		}
		else if(sum==2){
			if(v.x==1 && v.y==1){
				return true;
			}else if(v.z==1 && v.w==1){
				return true;
			}
			if(v.x==1 && v.z==1 && (GrowthMethod.CellArrayTest[vv.x,vv.y-1].wallSide.w==1 || GrowthMethod.CellArrayTest[vv.x-1,vv.y].wallSide.y==1))return true;
			if(v.x==1 && v.w==1 && (GrowthMethod.CellArrayTest[vv.x,vv.y-1].wallSide.z==1 || GrowthMethod.CellArrayTest[vv.x+1,vv.y].wallSide.y==1))return true;
			if(v.y==1 && v.z==1 && (GrowthMethod.CellArrayTest[vv.x,vv.y+1].wallSide.w==1 || GrowthMethod.CellArrayTest[vv.x-1,vv.y].wallSide.x==1))return true;
			if(v.y==1 && v.w==1 && (GrowthMethod.CellArrayTest[vv.x,vv.y+1].wallSide.z==1 || GrowthMethod.CellArrayTest[vv.x+1,vv.y].wallSide.x==1))return true;
		}else if(sum==4)return true;
		return false;
	}

	int sumOfWall(Vector4 v){
		return (int)v.x+(int)v.y+(int)v.z+(int)v.w;
	}

	int selectBestSidetoGrowRect(){
		//TODO add that in aSides it increases the number based on priority instead of only being yes/no, and take better take into accound ratio
		//fully remove ratio condition only apply it as priority

		Vector4 aSides=new Vector4(1,1,1,1);
		//check Available sides
		foreach(Vector2Int c in outerCells){
			if(CellGridScript.CellArray[c.x,c.y].wallSide.x==1){
				if(CellGridScript.CellArray[c.x,c.y+1].location==CellGridScript.cellLocation.Border || CellGridScript.CellArray[c.x,c.y+1].location==CellGridScript.cellLocation.Outside || CellGridScript.CellArray[c.x,c.y+1].roomId<99){
					//if the cell is in the border or belongs to a different room, we cant grow that way
					aSides.x=0;
				}
			}
			if(CellGridScript.CellArray[c.x,c.y].wallSide.y==1){
				if(CellGridScript.CellArray[c.x,c.y-1].location==CellGridScript.cellLocation.Border || CellGridScript.CellArray[c.x,c.y-1].location==CellGridScript.cellLocation.Outside || CellGridScript.CellArray[c.x,c.y-1].roomId<99){
					//if the cell is in the border or belongs to a different room, we cant grow that way
					aSides.y=0;
				}
			}
			if(CellGridScript.CellArray[c.x,c.y].wallSide.z==1){
				if(CellGridScript.CellArray[c.x+1,c.y].location==CellGridScript.cellLocation.Border || CellGridScript.CellArray[c.x+1,c.y].location==CellGridScript.cellLocation.Outside || CellGridScript.CellArray[c.x+1,c.y].roomId<99){
					//if the cell is in the border or belongs to a different room, we cant grow that way
					aSides.z=0;
				}
			}
			if(CellGridScript.CellArray[c.x,c.y].wallSide.w==1){
				if(CellGridScript.CellArray[c.x-1,c.y].location==CellGridScript.cellLocation.Border || CellGridScript.CellArray[c.x-1,c.y].location==CellGridScript.cellLocation.Outside || CellGridScript.CellArray[c.x-1,c.y].roomId<99){
					//if the cell is in the border or belongs to a different room, we cant grow that way
					aSides.w=0;
				}
			}
		}
		Debug.Log("Possible expansion for "+name+": "+aSides);
		//from the available sides check which can be made in terms of change in size
		//Check maxRatio, and size not broken
		//Debug.Log("check RATIO for side selection");
		int countAddedSize=0;
        float ratioX,ratioY,ratioZ,ratioW=0;
		if(aSides.x==1){
			countAddedSize=0;
			//check for new side
			foreach(Vector2Int c in outerCells){
				if(CellGridScript.CellArray[c.x,c.y].wallSide.x==1)countAddedSize++;
			}
			ratioX=reCalculateRatio(0,1);
			if(ratioX<=maxRatio){
				aSides.x+=2;
			}
			if(currSize+countAddedSize>maxSize || ratioX>=maxRatio*2){
				aSides.x=0;
			}
		}
		if(aSides.y==1){
			countAddedSize=0;
			foreach(Vector2Int c in outerCells){
				if(CellGridScript.CellArray[c.x,c.y].wallSide.y==1)countAddedSize++;
			}
			ratioY=reCalculateRatio(0,1);
			if(ratioY<=maxRatio){
				aSides.y+=2;
			}
			if(currSize+countAddedSize>maxSize || ratioY>=maxRatio*2){
				aSides.y=0;
			}
		}
		if(aSides.z==1){
			countAddedSize=0;
			foreach(Vector2Int c in outerCells){
				if(CellGridScript.CellArray[c.x,c.y].wallSide.z==1)countAddedSize++;
			}
			ratioZ=reCalculateRatio(1,0);
			if(ratioZ<=maxRatio){
				aSides.z+=2;
			}
			if(currSize+countAddedSize>maxSize || ratioZ>=maxRatio*2){
				aSides.z=0;
			}
		}
		if(aSides.w==1){
			countAddedSize=0;
			foreach(Vector2Int c in outerCells){
				if(CellGridScript.CellArray[c.x,c.y].wallSide.w==1)countAddedSize++;
			}
			ratioW=reCalculateRatio(1,0);
			if(ratioW>=maxRatio){
				aSides.w+=2;
			}
			if(currSize+countAddedSize>maxSize || ratioW>=maxRatio*2){
				aSides.w=0;
			}
		}
		Debug.Log("after size check:"+aSides);

		if(aSides.x+aSides.y+aSides.z+aSides.w==0)return 0;
		//knowing which side is possible, select the one more logical to fullfill room connections
		//first get direction vector for each connection
		List<Vector2> dir=new List<Vector2>();
		foreach(int con in connections){
			float tempDist=9999.9f;
			dir.Add(new Vector2(0,0));
			foreach(Vector2Int v in HouseGraph.allRooms[con].outerCells){
				foreach(Vector2Int c in outerCells){
					Vector2 dirTemp=c-v;
					if(CellGridScript.CellArray[c.x,c.y].wallSide.x==1){
						if(CellGridScript.CellArray[c.x,c.y+1].roomId==con){
							dir[dir.Count-1]=Vector2.zero;
							break;
						}
					}else if(CellGridScript.CellArray[c.x,c.y].wallSide.y==1){
						if(CellGridScript.CellArray[c.x,c.y-1].roomId==con){
							dir[dir.Count-1]=Vector2.zero;
							break;
						}
					}else if(CellGridScript.CellArray[c.x,c.y].wallSide.z==1){
						if(CellGridScript.CellArray[c.x+1,c.y].roomId==con){
							dir[dir.Count-1]=Vector2.zero;
							break;
						}
					}else if(CellGridScript.CellArray[c.x,c.y].wallSide.w==1){
						if(CellGridScript.CellArray[c.x-1,c.y].roomId==con){
							dir[dir.Count-1]=Vector2.zero;
							break;
						}
					}
					if(tempDist>dirTemp.magnitude){
						dir[dir.Count-1]=dirTemp;
					}
				}
			}
		}
		//remove does that cannot be expanded towards and those who are already connected
		List<Vector2> toRemove=new List<Vector2>();
		for(int j=0;j<dir.Count;j++){
			if(dir[j]!=Vector2.zero){
				if(dir[j].x*dir[j].x>dir[j].y*dir[j].y){
					//goes left or right
					if(dir[j].x>0){
						//goes left
						if(aSides.w<1.0f){
							toRemove.Add(dir[j]);
						}
					}else{
						//goes right
						if(aSides.z<1.0f){
							toRemove.Add(dir[j]);
						}
					}
				}else{
					//goes up or down
					if(dir[j].y>0){
						//goes down
						if(aSides.y<1.0f){
							toRemove.Add(dir[j]);
						}
					}else{
						//goes up
						if(aSides.x<1.0f){
							toRemove.Add(dir[j]);
						}
					}
				}
			}else{
				toRemove.Add(dir[j]);
			}
		}
		foreach(Vector2 v in toRemove){
			dir.Remove(v);
		}
		//Debug.Log("Possible dir with connections:"+dir.Count);
		//select the closest one
		if(dir.Count>0){
			int selected=0;
			float smallest=9999.9f;
			for(int i =0;i<dir.Count;i++){
				if(smallest>dir[i].magnitude){
					selected=i;
					smallest=dir[i].magnitude;
				}
			}
			//Debug.Log(dir[selected]);
			if(dir[selected].x*dir[selected].x>dir[selected].y*dir[selected].y){
				//goes left or right
				if(dir[selected].x>0){
					//goes left
					aSides.w++;
				}else{
					//goes right
					aSides.z++;
				}
			}else if(dir[selected].x*dir[selected].x<dir[selected].y*dir[selected].y){
				//goes up or down
				if(dir[selected].y>0){
					//goes down
					aSides.y++;
				}else{
					//goes up
					aSides.x++;
					
				}
			}
		}
		float max=0;
		int sel=0;
		int numEq=0;
		for(int i=0; i<4;i++){
			switch(i){
				case 0:
					if(aSides.x>max){
						max=aSides.x;
						sel=0;
						numEq=0;
					}else if(aSides.x==max){
						numEq++;
					}
				break;
				case 1:
					if(aSides.y>max){
						max=aSides.y;
						sel=1;
						numEq=0;
					}else if(aSides.y==max){
						numEq++;
					}
				break;
				case 2:
					if(aSides.z>max){
						max=aSides.z;
						sel=2;
						numEq=0;
					}else if(aSides.z==max){
						numEq++;
					}
				break;
				case 3:
					if(aSides.w>max){
						max=aSides.w;
						sel=3;
						numEq=0;
					}else if(aSides.w==max){
						numEq++;
					}
				break;
			}
		}
		if(numEq==0){
			switch(sel){
				case 0:
					return 1;
				case 1:
					return 2;
				case 2:
					return 3;
				case 3:
					return 4;
			}
		}

		Debug.Log("Random selection");
		//select a random available direction
		int good=0;
		int rng=Random.Range(0,numEq);
		for(int i=0;i<4;i++){
			switch(i){
				case 0:
					if(aSides.x==max){
						if(good==rng){
							return 1;
						}
						good++;
					}
				break;
				case 1:
					if(aSides.y==max){
						if(good==rng){
							return 2;
						}
						good++;
					}
				break;
				case 2:
					if(aSides.z==max){
						if(good==rng){
							return 3;
						}
						good++;
					}
				break;
				case 3:
					if(aSides.w==max){
						if(good==rng){
							return 4;
						}
						good++;
					}
				break;
			}
		}

		return 0;//0:cant,1:up,2:down,3:right,:4:Left
	}
	
	public float reCalculateRatio(int xAdd, int yAdd){
		int x=xAdd;
		int y=yAdd;
		foreach(Vector2Int v in outerCells){
			if(CellGridScript.CellArray[v.x,v.y].wallSide.x==1){
				x++;
			}
			if(CellGridScript.CellArray[v.x,v.y].wallSide.z==1){
				y++;
			}
		}
		if(x>y){
			return x/y;
		}else{
			return y/x;
		}
	}

	float reCalculateRatioTest(int xAdd, int yAdd){
		int x=xAdd;
		int y=yAdd;

		foreach(Vector2Int v in outerCells){
			if(GrowthMethod.CellArrayTest[v.x,v.y].wallSide.x==1){
				x++;
			}
			if(GrowthMethod.CellArrayTest[v.x,v.y].wallSide.z==1){
				y++;
			}
		}

		if(x>y){
			return x/y;
		}else{
			return y/x;
		}
	}

	public void reCalculateCenterPoint(){
		Vector2 center=Vector2.zero;
		foreach(Vector2Int v in outerCells){
			center+=v;
		}	
		center=center/outerCells.Count;
		centerPoint=new Vector2(center.x,center.y);
	}
    
}