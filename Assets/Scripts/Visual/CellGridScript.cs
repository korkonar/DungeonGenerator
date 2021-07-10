using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGridScript : MonoBehaviour
{
    public bool Draw2D;

    public DrawPlot plotInf;
    public GridDrawingScript gridInf;

    public static int currFloorId=555;
    public static bool init;

    public enum cellLocation{
        Outside,
        Border,
        Inside,
        Inner
    }
    public class cell{
        public cellLocation location;
        public int roomId;
        public Vector4 wallSide;// use this a bool, x:up, y:down, z:Right, w:Left
    }

    public static cell[,] CellArray;

    List<Vector3> outsidePoints=new List<Vector3>();
    public Material mat;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    public static int maxInnerCells;
    public static int totCells;
    float bestOffSetX;
    float bestOffSetY;
    public bool drawing;
    // Start is called before the first frame update
    void Start()
    {
        //initialize CellArray
        StartCoroutine(LateStart(0.5f));
        
    }

    public IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CellArray = new cell[plotInf.numCells,plotInf.numCells];
        for(int ix=0;ix<plotInf.numCells;ix++){
            for(int iy=0;iy<plotInf.numCells;iy++){
                CellArray[ix,iy]=new cell();
                CellArray[ix,iy].location=0;
                CellArray[ix,iy].roomId=99;
                CellArray[ix,iy].wallSide=Vector4.zero;
            }
        }
        maxInnerCells=0;
        for(int i=0;i<20;i++){
            for(int j=0;j<20;j++){
                gridInf.offsetX=i*0.05f;
                gridInf.offsetY=j*0.05f;
                gridInf.UpdateGrid();
                initializeGrid();
            }
        }
        setInnerCells();
        GrowthMethod.initialized++;
        totCells=plotInf.numCells*plotInf.numCells;
    }

    public IEnumerator LateStart2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        for(int ix=0;ix<plotInf.numCells;ix++){
            for(int iy=0;iy<plotInf.numCells;iy++){
                CellArray[ix,iy].location=0;
                CellArray[ix,iy].roomId=99;
                CellArray[ix,iy].wallSide=Vector4.zero;
            }
        }
        initializeGrid();
        setInnerCells();
        GrowthMethod.initialized++;
        totCells=plotInf.numCells*plotInf.numCells;
    }

    public void ResetArray(){
        StartCoroutine(LateStart2(0.0f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //initializeGrid();
    }

    void setInnerCells(){
        for(int xi=0; xi<plotInf.numCells;xi++){
            for(int yi=0; yi<plotInf.numCells;yi++){
                if(CellArray[xi,yi].location==cellLocation.Inside){
                    float disttemp=99999.9f;
                    Vector2 gridVectemp= new Vector2(0,0);
                    for(int i=0; i<plotInf.plot.Length;i++){
                        //check if it is the last point, if so reuse first point
                        gridVectemp= new Vector2(xi+gridInf.offsetX,yi+gridInf.offsetY);
                        if(i+1<plotInf.plot.Length){
                            float dist=distanceFromLine(plotInf.plot[i],plotInf.plot[i+1],gridVectemp);
                            if(disttemp>dist){
                                disttemp=dist;
                            }
                        }else{
                            float dist=distanceFromLine(plotInf.plot[i],plotInf.plot[0],gridVectemp);
                            if(disttemp>dist){
                                disttemp=dist;
                            }
                        }
                    }
                    //print(disttemp);
                    if(disttemp>plotInf.numCells/5f){
                        CellArray[xi,yi].location=cellLocation.Inner;
                    }
                }
            }
        }
    }
    void initializeGrid(){
        //i9nitializes the grid based on the plot, will be used by the ground floor
        for(int ix=0;ix<plotInf.numCells;ix++){
            for(int iy=0;iy<plotInf.numCells;iy++){
                CellArray[ix,iy].location=0;
            }
        }
        int[,] a= new int[plotInf.numCells,plotInf.numCells];
        
        for(int i=0; i<plotInf.plot.Length;i++){
            //check if it is the last point, if so reuse first point
            if(i+1<plotInf.plot.Length){
                //for each x
                    //And y
                        //If the distance is smaller than one cell
                            //Check if in the range of the segment ADD IT TO BORDER
                        //IF is to the right of the line COUNT IT
                for(int xi=0; xi<plotInf.numCells;xi++){
                    for(int yi=0; yi<plotInf.numCells;yi++){
                        Vector2 gridVec= new Vector2(xi+gridInf.offsetX,yi+gridInf.offsetY);
                        if(gridVec.x>plotInf.numCells)gridVec.x=-plotInf.numCells;
                        if(gridVec.y>plotInf.numCells)gridVec.y=-plotInf.numCells;
                        if(distanceFromLine(plotInf.plot[i],plotInf.plot[i+1],gridVec)<0.6f){
                            if((gridVec.x+1.0f>=plotInf.plot[i].x && gridVec.x-1.0f<=plotInf.plot[i+1].x) || (gridVec.x-1.0f<=plotInf.plot[i].x && gridVec.x+1.0f>=plotInf.plot[i+1].x)){
                                if((gridVec.y+1.0f>=plotInf.plot[i].y && gridVec.y-1.0f<=plotInf.plot[i+1].y) || (gridVec.y-1.0f<=plotInf.plot[i].y && gridVec.y+1.0f>=plotInf.plot[i+1].y)){
                                    CellArray[xi,yi].location=cellLocation.Border;
                                }
                            }
                        }else if(isRight(plotInf.plot[i],plotInf.plot[i+1],gridVec)){
                            //CellArray[xi,yi].location=cellLocation.Inside;
                            a[xi,yi]++;
                        }
                    }
                }
            }else{
                for(int xi=0; xi<plotInf.numCells;xi++){
                    for(int yi=0; yi<plotInf.numCells;yi++){
                        Vector2 gridVec= new Vector2(xi+gridInf.offsetX,yi+gridInf.offsetY);
                        if(gridVec.x>plotInf.numCells)gridVec.x=-plotInf.numCells;
                        if(gridVec.y>plotInf.numCells)gridVec.y=-plotInf.numCells;
                        if(distanceFromLine(plotInf.plot[i],plotInf.plot[0],gridVec)<0.6f){
                            if((gridVec.x+1.0f>=plotInf.plot[i].x && gridVec.x-1.0f<=plotInf.plot[0].x) || (gridVec.x-1.0f<=plotInf.plot[i].x && gridVec.x+1.0f>=plotInf.plot[0].x)){
                                if((gridVec.y+1.0f>=plotInf.plot[i].y && gridVec.y-1.0f<=plotInf.plot[0].y) || (gridVec.y-1.0f<=plotInf.plot[i].y && gridVec.y+1.0f>=plotInf.plot[0].y)){
                                    CellArray[xi,yi].location=cellLocation.Border;
                                }
                            }
                        }else if(isRight(plotInf.plot[i],plotInf.plot[0],gridVec)){
                            //CellArray[xi,yi].location=cellLocation.Inside;
                            a[xi,yi]++;
                        }
                    }
                } 
            }

        }
        //If the cells in a have a value of the number of line it means that all line had it to their right, so it is inside
        for(int ix=0;ix<plotInf.numCells;ix++){
            for(int iy=0;iy<plotInf.numCells;iy++){
                if(a[ix,iy]>=plotInf.plot.Length){
                    CellArray[ix,iy].location=cellLocation.Inside;
                }
            }
        }
        int count =countInsideCells();
        if(count>maxInnerCells){
            maxInnerCells=count;
            bestOffSetX=gridInf.offsetX;
            bestOffSetY=gridInf.offsetY;
            //print(count);
        }
    }

    bool isRight(Vector2 a, Vector2 b, Vector2 c){
        return ((c.x-a.x)*(b.y-a.y)-(c.y-a.y)*(b.x-a.x)) > 0;
    }

    float distanceFromLine(Vector2 A, Vector2 B, Vector2 P){
        return (Mathf.Abs(((B.y-A.y)*P.x)-((B.x-A.x)*P.y)+(B.x*A.y)-(B.y*A.x)))/(Mathf.Sqrt(((B.y-A.y)*(B.y-A.y))+((B.x-A.x)*(B.x-A.x))));
    }

    int countInsideCells(){
        int count=0;
        foreach(cell c in CellArray){
            if(c.location==cellLocation.Inside)count++;
        }
        return count;
    }

    void OnPostRender()
    {
        if(Draw2D){
            float cellSize=1.0f/plotInf.numCells;
            if (!mat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
    
            GL.PushMatrix();
            GL.LoadOrtho();
    
            //draw innerCells
            #region innerCells
               //mat.SetPass(0); 
               //GL.Begin(GL.LINES);
               //for(int ix=0;ix<plotInf.numCells;ix++){
               //    for(int iy=0;iy<plotInf.numCells;iy++){
               //        if(CellArray[ix,iy].location==cellLocation.Inside){
               //            GL.Vertex3((ix+gridInf.offsetX)*cellSize+(cellSize/2),(iy+gridInf.offsetY)*cellSize+(cellSize/2),1);
               //            GL.Vertex3((ix+gridInf.offsetX)*cellSize+(cellSize),(iy+gridInf.offsetY)*cellSize+(cellSize/2),1);
               //        }
               //    }
               //}
               //GL.End();
//  
               ////draw borderCells
               //mat2.SetPass(0); 
               //GL.Begin(GL.LINES);
               //for(int ix=0;ix<plotInf.numCells;ix++){
               //    for(int iy=0;iy<plotInf.numCells;iy++){
               //        if(CellArray[ix,iy].location==cellLocation.Border){
               //            GL.Vertex3((ix+gridInf.offsetX)*cellSize+(cellSize/2),(iy+gridInf.offsetY)*cellSize+(cellSize/2),1);
               //            GL.Vertex3((ix+gridInf.offsetX)*cellSize+(cellSize),(iy+gridInf.offsetY)*cellSize+(cellSize/2),1);
               //        }
               //    }
               //}
               //GL.End();
            #endregion
    
            //draw Rooms
            if(init){
                int i=0;
                foreach(int r in HouseGraph.allRooms.Keys){
                    switch(i){
                        case 0:
                            mat.SetPass(0); 
                        break;
                        case 1:
                            mat2.SetPass(0); 
                        break;
                        case 2:
                            mat3.SetPass(0); 
                        break;
                        case 3:
                            mat4.SetPass(0);
                        break;
                    }
                    i++;
                    if(i>=4)i=0;
                    GL.Begin(GL.LINES);
                    float fix=(cellSize/20);
                    List<Vector2Int> cells= HouseGraph.allRooms[r].outerCells;
                    foreach(Vector2Int c in cells){
                        //print(c);
                        if(CellArray[c.x,c.y].wallSide.x==1){
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize),((c.y+gridInf.offsetY)*cellSize+(cellSize))-fix,1);
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize+(cellSize)),((c.y+gridInf.offsetY)*cellSize+(cellSize))-fix,1);
                        }
                        if(CellArray[c.x,c.y].wallSide.y==1){
                            GL.Vertex3((c.x+gridInf.offsetX)*cellSize,((c.y+gridInf.offsetY)*cellSize)+fix,1);
                            GL.Vertex3((c.x+gridInf.offsetX)*cellSize+(cellSize),((c.y+gridInf.offsetY)*cellSize)+fix,1);
                        }
                        if(CellArray[c.x,c.y].wallSide.z==1){
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize+cellSize)-fix,(c.y+gridInf.offsetY)*cellSize,1);
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize+(cellSize))-fix,(c.y+gridInf.offsetY)*cellSize+(cellSize),1);
                        }
                        if(CellArray[c.x,c.y].wallSide.w==1){
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize)+fix,(c.y+gridInf.offsetY)*cellSize,1);
                            GL.Vertex3(((c.x+gridInf.offsetX)*cellSize)+fix,(c.y+gridInf.offsetY)*cellSize+(cellSize),1);
                        }
                    }
                    GL.Vertex3(((HouseGraph.allRooms[r].centerPoint.x+gridInf.offsetX)*cellSize),((HouseGraph.allRooms[r].centerPoint.y+gridInf.offsetY)*cellSize)+(cellSize/2),1);
                    GL.Vertex3(((HouseGraph.allRooms[r].centerPoint.x+gridInf.offsetX)*cellSize)+cellSize/2,(HouseGraph.allRooms[r].centerPoint.y+gridInf.offsetY)*cellSize+(cellSize/2),1);
                    GL.End();
                }
            }
            GL.PopMatrix();
        }
        
    }

}
