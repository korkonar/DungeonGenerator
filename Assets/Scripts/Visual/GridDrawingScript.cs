using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawingScript : MonoBehaviour
{
    public bool Draw2D;

    public struct lines{
        public int maxIndex;
        public Vector2[] xA;
        public Vector2[] yA;
    }

    public Material gridMat;

    lines thelines;
    int numCellsX;

    [Range(0.0f, 1.0f)]
    public float offsetX;
    [Range(0.0f, 1.0f)]
    public float offsetY;

    public DrawPlot plotInf;
    void Start()
    {
        numCellsX=plotInf.numCells;
        int aSize=2*(numCellsX+1);

        thelines = new lines();
        thelines.xA=new Vector2[aSize];
        thelines.yA=new Vector2[aSize];

        thelines.maxIndex=-1;
        float cellSize=1.0f;
        //print(cellSize);
        for(int i=0;i<=numCellsX;i++){
            thelines.maxIndex++;

            float pos=(cellSize*i)+offsetY;
            if(pos>numCellsX)pos-=numCellsX;

            thelines.xA[thelines.maxIndex]=new Vector2(0,pos);
            thelines.yA[thelines.maxIndex]=new Vector2(numCellsX,pos);
        }
        for(int i=0;i<=numCellsX;i++){
            thelines.maxIndex++;

            float pos=(cellSize*i)+offsetX;
            if(pos>numCellsX)pos-=numCellsX;

            thelines.xA[thelines.maxIndex]=new Vector2(pos,0);
            thelines.yA[thelines.maxIndex]=new Vector2(pos,numCellsX);
        }

    }

    public void UpdateGrid(){
        numCellsX=plotInf.numCells;
        int aSize=2*(numCellsX+1);
        thelines = new lines();
        thelines.xA=new Vector2[aSize];
        thelines.yA=new Vector2[aSize];

        thelines.maxIndex=-1;
        float cellSize=1.0f;
        //print(cellSize);
        for(int i=0;i<=numCellsX;i++){
            thelines.maxIndex++;

            float pos=(cellSize*i)+offsetY;
            if(pos>numCellsX)pos-=numCellsX;

            thelines.xA[thelines.maxIndex]=new Vector2(0,pos);
            thelines.yA[thelines.maxIndex]=new Vector2(numCellsX,pos);
        }
        for(int i=0;i<=numCellsX;i++){
            thelines.maxIndex++;

            float pos=(cellSize*i)+offsetX;
            if(pos>numCellsX)pos-=numCellsX;

            thelines.xA[thelines.maxIndex]=new Vector2(pos,0);
            thelines.yA[thelines.maxIndex]=new Vector2(pos,numCellsX);
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnPostRender()
    {
        if(Draw2D){
            if (!gridMat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            GL.PushMatrix();
            GL.LoadOrtho();
    
            //draw grid
            gridMat.SetPass(0); 
    
            GL.Begin(GL.LINES);
            if(thelines.maxIndex>=0){
                for(int i=0;i<=thelines.maxIndex;i++){
                    //GL.Color(Color.red);
                    GL.Vertex3(thelines.xA[i].x/numCellsX,thelines.xA[i].y/numCellsX,0);
                    GL.Vertex3(thelines.yA[i].x/numCellsX,thelines.yA[i].y/numCellsX,0);
                }
            }
            GL.End();
    
            GL.PopMatrix();
        }
    }
}
