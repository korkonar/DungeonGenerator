using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGraph : MonoBehaviour
{
	public static int ID=0;
	public static Dictionary<int, Room> allRooms;
	public static Dictionary<int, Room> allRoomsTest;

	public static int[,] roomAdjacencyMatrix;

	public static List<Floor> floors;

	public int type;

	public bool drawing;
	public static float boudingSize;
	public static float boundingRatio;

	public GameObject pref;
	public static float[,] scores;

	public static int numberOfHouses=0;

	#region weightsDiversity
		static float Wadjacencies=0.5f;
		static float Wsizes=0.1f;
		static float Wratios=2.0f;
		static float Wcorners=1.0f;
		static float Wbounding=.5f;
		static float WboundingRatio=1.5f;
	#endregion

	//structure to store all connections without duplicates
	public struct connection{
		public int roomA;
		public int roomB;
	}
	
	void Awake()
	{
		scores=new float[numberOfHouses,3];

		floors =new List<Floor>();
		allRooms= new Dictionary<int, Room>();
		allRoomsTest=new Dictionary<int, Room>();
		CellGridScript.init=true;
		if(!drawing){
			switch(type){
				case 0:
					
					Room hroom1= new Room(6.0f,9.0f,0.5f,1.4f,"Living",0,Room.roomType.living);
					Room hroom2= new Room(5.0f,6.0f,0.5f,1.4f,"Kitchen",1,Room.roomType.kitchen);//
					Room hroom3= new Room(5.0f,6.0f,0.5f,1.4f,"bed1",2,Room.roomType.bedroom);//
					Room hroom31= new Room(5.0f,6.0f,0.5f,1.4f,"bed2",3,Room.roomType.bedroom);//
					Room hroom4= new Room(4f,6.0f,0.5f,1.4f,"extra1",4,Room.roomType.extra);//
					Room hroom5=new Room(4f,6.0f,0.5f,1.4f,"Bath",5,Room.roomType.bathroom);//

					
					Room hroom11= new Room(6.0f,9.0f,0.5f,1.4f,"Living",0,Room.roomType.living);
					Room hroom22= new Room(5.0f,6.0f,0.5f,1.4f,"Kitchen",1,Room.roomType.kitchen);//
					Room hroom33= new Room(5.0f,6.0f,0.5f,1.4f,"bed1",2,Room.roomType.bedroom);//
					Room hroom331= new Room(5.0f,6.0f,0.5f,1.4f,"bed2",3,Room.roomType.bedroom);//
					Room hroom44= new Room(4.0f,6.0f,0.5f,1.4f,"extra1",4,Room.roomType.extra);//
					Room hroom55=new Room(4.0f,6.0f,0.5f,1.4f,"Bath",5,Room.roomType.bathroom);//

					allRooms.Add(hroom1.ID,hroom1);
					allRooms.Add(hroom2.ID,hroom2);
					allRooms.Add(hroom3.ID,hroom3);
					allRooms.Add(hroom31.ID,hroom31);
					allRooms.Add(hroom4.ID,hroom4);
					allRooms.Add(hroom5.ID,hroom5);
					allRoomsTest.Add(hroom1.ID,hroom11);
					allRoomsTest.Add(hroom2.ID,hroom22);
					allRoomsTest.Add(hroom3.ID,hroom33);
					allRoomsTest.Add(hroom31.ID,hroom331);
					allRoomsTest.Add(hroom4.ID,hroom44);
					allRoomsTest.Add(hroom55.ID,hroom55);
				break;
				case 1:
					Room room1= new Room(50.0f,60.0f,0.5f,1.5f,"Living",0,Room.roomType.living);
					Room room2= new Room(40.0f,50.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);//
					Room room3= new Room(40.0f,50.0f,0.5f,1.5f,"bed1",2,Room.roomType.bedroom);//
					Room room31= new Room(40.0f,50.0f,0.5f,1.5f,"bed2",3,Room.roomType.bedroom);//
					Room room4= new Room(10.0f,20.0f,0.5f,1.5f,"extra1",4,Room.roomType.extra);//
					Room room5=new Room(10.0f,20.0f,0.5f,1.5f,"Bath",5,Room.roomType.bathroom);//

					Room room11= new Room(50.0f,60.0f,0.5f,1.5f,"Living",0,Room.roomType.living);
					Room room22= new Room(40.0f,50.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);//
					Room room33= new Room(40.0f,50.0f,0.5f,1.5f,"bed",2,Room.roomType.bedroom);//
					Room room331= new Room(40.0f,50.0f,0.5f,1.5f,"bed2",3,Room.roomType.bedroom);//
					Room room44= new Room(10.0f,20.0f,0.5f,1.5f,"extra1",4,Room.roomType.extra);//
					Room room55=new Room(10.0f,20.0f,0.5f,1.5f,"Bath",5,Room.roomType.bathroom);//

					allRooms.Add(room1.ID,room1);
					allRooms.Add(room2.ID,room2);
					allRooms.Add(room3.ID,room3);
					allRooms.Add(room31.ID,room31);
					allRooms.Add(room4.ID,room4);
					allRooms.Add(room5.ID,room5);
					allRoomsTest.Add(room1.ID,room11);
					allRoomsTest.Add(room2.ID,room22);
					allRoomsTest.Add(room3.ID,room33);
					allRoomsTest.Add(room31.ID,room331);
					allRoomsTest.Add(room4.ID,room44);
					allRoomsTest.Add(room55.ID,room55);
				break;
				case 2:
					Room livng= new Room(30.0f,40.0f,0.5f,1.5f,"Living",0,Room.roomType.living);
					Room kitch= new Room(20.0f,30.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);
					Room bed1= new Room(15.0f,25.0f,0.5f,1.5f,"bed1",2,Room.roomType.bedroom);
					Room bed2= new Room(15.0f,25.0f,0.5f,1.5f,"bed2",3,Room.roomType.bedroom);
					Room bath=new Room(10.0f,15.0f,0.5f,1.5f,"Bath",4,Room.roomType.bathroom);
					Room util=new Room(5.0f,15.0f,0.5f,1.5f,"Util",5,Room.roomType.extra);
					Room garag=new Room(30.0f,40.0f,0.5f,1.5f,"Garage",6,Room.roomType.garage);
					Room bed3= new Room(20.0f,30.0f,0.5f,1.5f,"MasterBed",7,Room.roomType.bedroom);
					Room masterbath=new Room(10.0f,20.0f,0.5f,1.5f,"Bath",8,Room.roomType.bathroom);

					Room livngt= new Room(30.0f,40.0f,0.5f,1.5f,"Living",0,Room.roomType.living);
					Room kitcht= new Room(20.0f,30.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);
					Room bed1t= new Room(15.0f,25.0f,0.5f,1.5f,"bed1",2,Room.roomType.bedroom);
					Room bed2t= new Room(15.0f,25.0f,0.5f,1.5f,"bed2",3,Room.roomType.bedroom);
					Room batht=new Room(10.0f,15.0f,0.5f,1.5f,"Bath",4,Room.roomType.bathroom);
					Room utilt=new Room(5.0f,15.0f,0.5f,1.5f,"Util",5,Room.roomType.extra);
					Room garagt=new Room(30.0f,40.0f,0.5f,1.5f,"Garage",6,Room.roomType.garage);
					Room bed3t= new Room(20.0f,30.0f,0.5f,1.5f,"MasterBed",7,Room.roomType.bedroom);
					Room masterbatht=new Room(10.0f,20.0f,0.5f,1.5f,"Bath",8,Room.roomType.bathroom);

					allRooms.Add(0,livng);
					allRooms.Add(1,kitch);
					allRooms.Add(2,bed1);
					allRooms.Add(3,bed2 );
					allRooms.Add(4,bath);
					allRooms.Add(5,util);
					allRooms.Add(6,garag);
					allRooms.Add(7,bed3 );
					allRooms.Add(8,masterbath);
					allRoomsTest.Add(0,livngt);
					allRoomsTest.Add(1,kitcht);
					allRoomsTest.Add(2,bed1t);
					allRoomsTest.Add(3,bed2t );
					allRoomsTest.Add(4,batht);
					allRoomsTest.Add(5,utilt);
					allRoomsTest.Add(6,garagt);
					allRoomsTest.Add(7,bed3t );
					allRoomsTest.Add(8,masterbatht);
				break;
				case 3:
				
					Room LIVING= new Room(30.0f,40.0f,0.5f,1.5f,"Livingroom",0,Room.roomType.living);
					Room KITCH= new Room(30.0f,40.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);
					Room TOIL= new Room(5.0f,15.0f,0.5f,1.5f,"Toilet",2,Room.roomType.toilet);
					Room BED1= new Room(20.0f,30.0f,0.5f,1.5f,"MasterBed",3,Room.roomType.bedroom);
					Room BATH1=new Room(10.0f,20.0f,0.5f,1.5f,"Bath1",4,Room.roomType.bathroom);
					Room PANT=new Room(5.0f,10.0f,0.5f,1.5f,"Pantrie",5,Room.roomType.extra);
					Room BED2= new Room(20.0f,30.0f,0.5f,1.5f,"Bed1",6,Room.roomType.bedroom);
					Room BED3= new Room(20.0f,30.0f,0.5f,1.5f,"Bed2",7,Room.roomType.bedroom);
					Room BATH2=new Room(10.0f,15.0f,0.5f,1.5f,"Bath2",8,Room.roomType.bathroom);
					Room BED4= new Room(20.0f,30.0f,0.5f,1.5f,"MaterBed2",9,Room.roomType.bedroom);
					Room Closet= new Room(5.0f,15.0f,0.5f,1.5f,"Closet",10,Room.roomType.ensuite);
					Room GARA= new Room(20.0f,40.0f,0.5f,1.5f,"Garage",11,Room.roomType.garage);

					Room LIVINGt= new Room(30.0f,40.0f,0.5f,1.5f,"Livingroom",0,Room.roomType.living);
					Room KITCHt= new Room(30.0f,40.0f,0.5f,1.5f,"Kitchen",1,Room.roomType.kitchen);
					Room TOILt= new Room(5.0f,15.0f,0.5f,1.5f,"Toilet",2,Room.roomType.toilet);
					Room BED1t= new Room(20.0f,30.0f,0.5f,1.5f,"MasterBed",3,Room.roomType.bedroom);
					Room BATH1t=new Room(10.0f,20.0f,0.5f,1.5f,"Bath1",4,Room.roomType.bathroom);
					Room PANTt=new Room(5.0f,10.0f,0.5f,1.5f,"Pantrie",5,Room.roomType.extra);
					Room BED2t= new Room(20.0f,30.0f,0.5f,1.5f,"Bed1",6,Room.roomType.bedroom);
					Room BED3t= new Room(20.0f,30.0f,0.5f,1.5f,"Bed2",7,Room.roomType.bedroom);
					Room BATH2t=new Room(10.0f,15.0f,0.5f,1.5f,"Bath2",8,Room.roomType.bathroom);
					Room BED4t= new Room(20.0f,30.0f,0.5f,1.5f,"MaterBed2",9,Room.roomType.bedroom);
					Room Closett= new Room(5.0f,15.0f,0.5f,1.5f,"Closet",10,Room.roomType.ensuite);
					Room GARAt= new Room(20.0f,40.0f,0.5f,1.5f,"Garage",11,Room.roomType.garage);

					allRooms.Add(LIVING.ID,LIVING);
					allRooms.Add(KITCH.ID,KITCH);
					allRooms.Add(TOIL.ID,TOIL);
					allRooms.Add(BED1.ID,BED1);
					allRooms.Add(BATH1.ID,BATH1);
					allRooms.Add(PANT.ID,PANT);
					allRooms.Add(BED2.ID,BED2);
					allRooms.Add(BED3.ID,BED3);
					allRooms.Add(BATH2.ID,BATH2);
					allRooms.Add(BED4.ID,BED4);
					allRooms.Add(Closet.ID,Closet);
					allRooms.Add(GARA.ID,GARA);
					allRoomsTest.Add(LIVINGt.ID,LIVINGt);
					allRoomsTest.Add(KITCHt.ID,KITCHt);
					allRoomsTest.Add(TOILt.ID,TOILt);
					allRoomsTest.Add(BED1t.ID,BED1t);
					allRoomsTest.Add(BATH1t.ID,BATH1t);
					allRoomsTest.Add(PANTt.ID,PANTt);
					allRoomsTest.Add(BED2t.ID,BED2t);
					allRoomsTest.Add(BED3t.ID,BED3t);
					allRoomsTest.Add(BATH2t.ID,BATH2t);
					allRoomsTest.Add(BED4t.ID,BED4t);
					allRoomsTest.Add(Closett.ID,Closett);
					allRoomsTest.Add(GARAt.ID,GARAt);
				break;
			}

			roomAdjacencyMatrix=new int[allRooms.Count,allRooms.Count];
			
			//UpdateRAM();
			DivideIntoFloors();
			GrowthMethod.initialized++;
			boudingSize=0;
			foreach(Room r in allRooms.Values){
				boudingSize+=r.maxSize;
			}	
			//print(boudingSize);

		}
	}

	public void SaveHouse(float fitScore, float reqScore,int ite){
		
        scores[ID,0]=fitScore;
		scores[ID,1]=reqScore;
		scores[ID,2]=ite;

		int i=0;
		foreach(Room r in allRooms.Values){
			int corners=0;
			r.currentAdj.Clear();
			GameObject gg=Instantiate(pref);
			gg.name=ID+"_"+i;
			SaveScript sc=gg.GetComponent<SaveScript>();
			sc.adj=new List<int>();
			sc.fitScore=fitScore;
			sc.qualityScore=reqScore;

			foreach(Vector2Int v in r.outerCells){
				Vector4 walls=CellGridScript.CellArray[v.x,v.y].wallSide;
				if(walls.x==1){
					if(CellGridScript.CellArray[v.x,v.y+1].roomId<99){
						if(!sc.adj.Contains(CellGridScript.CellArray[v.x,v.y+1].roomId)){
							sc.adj.Add(CellGridScript.CellArray[v.x,v.y+1].roomId);
						}
					}
				}
				if(walls.y==1){
					if(CellGridScript.CellArray[v.x,v.y-1].roomId<99){
						if(!sc.adj.Contains(CellGridScript.CellArray[v.x,v.y-1].roomId)){
							sc.adj.Add(CellGridScript.CellArray[v.x,v.y-1].roomId);
						}
					}
				}
				if(walls.z==1){
					if(CellGridScript.CellArray[v.x+1,v.y].roomId<99){
						if(!sc.adj.Contains(CellGridScript.CellArray[v.x+1,v.y].roomId)){
							sc.adj.Add(CellGridScript.CellArray[v.x+1,v.y].roomId);
						}
					}
				}
				if(walls.w==1){
					if(CellGridScript.CellArray[v.x-1,v.y].roomId<99){
						if(sc.adj.Contains(CellGridScript.CellArray[v.x-1,v.y].roomId)){
							sc.adj.Add(CellGridScript.CellArray[v.x-1,v.y].roomId);
						}
					}
				}
				if(walls.x==1 && walls.w==1)corners++;
				if(walls.y==1 && walls.z==1)corners++;
				if(walls.y==1 && walls.w==1)corners++;
				if(walls.x==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
				if(walls.x==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.z==1 && CellGridScript.CellArray[v.x-1,v.y+1].roomId==r.ID))corners++;
				if(walls.y==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.w==1 && CellGridScript.CellArray[v.x+1,v.y-1].roomId==r.ID))corners++;
				if(walls.y==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.z==1 && CellGridScript.CellArray[v.x-1,v.y-1].roomId==r.ID))corners++;
				if(walls.z==1 && (CellGridScript.CellArray[v.x+1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x+1,v.y+1].roomId==r.ID))corners++;
				if(walls.z==1 && (CellGridScript.CellArray[v.x+1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x+1,v.y-1].roomId==r.ID))corners++;
				if(walls.w==1 && (CellGridScript.CellArray[v.x-1,v.y+1].wallSide.y==1 && CellGridScript.CellArray[v.x-1,v.y+1].roomId==r.ID))corners++;
				if(walls.w==1 && (CellGridScript.CellArray[v.x-1,v.y-1].wallSide.x==1 && CellGridScript.CellArray[v.x-1,v.y-1].roomId==r.ID))corners++;
			}
			r.corners=corners;
			
			sc.currSize=r.currSize;
			sc.currRatio=r.currRatio;
			sc.corners=r.corners;
			sc.boudingSize=boudingSize;
			sc.boudingSize=boundingRatio;

			i++;
		}
		ID++;
    }

	public void Reset(){
		Floor.idGenerator=0;
		Awake();
	}

	public void DivideIntoFloors()
	{
		List<int> rooms=new List<int>();
		for(int i=0;i<allRooms.Count;i++){
			rooms.Add(i);
		}
		Floor f = new Floor(rooms);
		floors.Add(f);
	}
	
}