using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPlot : MonoBehaviour
{
    public bool Draw2D;

    public Material plotMat;
    public Vector2[] plot;

    public int numCells;

    // Start is called before the first frame update
    void Awake()
    {
        //determine the size grid required
        Vector2 minVal= new Vector2(500,500);
        Vector2 maxVal= Vector2.zero;
        foreach( Vector2 v in plot){
            if(v.x<minVal.x)minVal.x=v.x;
            if(v.y<minVal.y)minVal.y=v.y;
            if(v.x>maxVal.x)maxVal.x=v.x;
            if(v.y>maxVal.y)maxVal.y=v.y;
        }

        print(minVal);
        print(maxVal);
        //Move the plot closer to 0,0
        if(minVal.x>23){
            do{
                for(int i=0;i<plot.Length;i++){
                    plot[i]=plot[i]-new Vector2Int(1,0);
                }
                minVal-=new Vector2(1,0);
                maxVal-=new Vector2(1,0);
            }while(minVal.x>3);
        }
        if(minVal.y>3){
            do{
                for(int i=0;i<plot.Length;i++){
                    plot[i]=plot[i]-new Vector2Int(0,1);
                }
                minVal-=new Vector2(0,1);
                maxVal-=new Vector2(0,1);
            }while(minVal.y>1);
        }

        int Xdist=(int)maxVal.x-(int)minVal.x;
        int Ydist=(int)maxVal.y-(int)minVal.y;

        if(Xdist>Ydist){
            numCells=Xdist+7;
            //numCells=Mathf.NextPowerOfTwo(numCells);
        }else{
            numCells=Ydist+7;
            //numCells=Mathf.NextPowerOfTwo(numCells);
        }
        //print(numCells);

    }

    // Update is called once per frame
    public void UpdatePlot()
    {
        //determine the size grid required
        Vector2 minVal= new Vector2(500,500);
        Vector2 maxVal= Vector2.zero;
        foreach( Vector2 v in plot){
            if(v.x<minVal.x)minVal.x=v.x;
            if(v.y<minVal.y)minVal.y=v.y;
            if(v.x>maxVal.x)maxVal.x=v.x;
            if(v.y>maxVal.y)maxVal.y=v.y;
        }

        print(minVal);
        print(maxVal);
        //Move the plot closer to 0,0
        if(minVal.x>5){
            do{
                for(int i=0;i<plot.Length;i++){
                    plot[i]=plot[i]-new Vector2Int(5,0);
                }
                minVal-=new Vector2(5,0);
                maxVal-=new Vector2(5,0);
            }while(minVal.x>5);
        }
        if(minVal.y>5){
            do{
                for(int i=0;i<plot.Length;i++){
                    plot[i]=plot[i]-new Vector2Int(0,5);
                }
                minVal-=new Vector2(0,5);
                maxVal-=new Vector2(0,5);
            }while(minVal.y>5);
        }

        int Xdist=(int)maxVal.x-(int)minVal.x;
        int Ydist=(int)maxVal.y-(int)minVal.y;

        if(Xdist>Ydist){
            numCells=Xdist+10;
        }else{
            numCells=Ydist+10;
        }
        //print(numCells);
    }

    void OnPostRender()
    {
        if(Draw2D){
            float cellSize=1.0f/numCells;
            if (!plotMat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
    
            GL.PushMatrix();
            GL.LoadOrtho();
    
            //draw plot
            plotMat.SetPass(0); 
            GL.Begin(GL.LINES); 
            for(int i=0;i<plot.Length;i++){
                //GL.Color(Color.red);
                GL.Vertex3(plot[i].x*cellSize+(cellSize/2),plot[i].y*cellSize+(cellSize/2),1);
                if(i+1<plot.Length){
                    GL.Vertex3(plot[i+1].x*cellSize+(cellSize/2),plot[i+1].y*cellSize+(cellSize/2),1);
                }else{
                    GL.Vertex3(plot[0].x*cellSize+(cellSize/2),plot[0].y*cellSize+(cellSize/2),1);
                }
            }
            GL.End();
    
            GL.PopMatrix();
        }
    }
}
