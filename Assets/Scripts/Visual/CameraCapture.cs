using System.IO;
using UnityEngine;
 
public class CameraCapture : MonoBehaviour
{
    public int fileCounter;
    public KeyCode screenshotKey;
    private Camera Camera;
    public RenderTexture text;
    
    private int[,] iterations;
    private float[,] fits;
    static int ii=0;
    static int currI=0;

    void Start(){
        Camera=this.GetComponent<Camera>();

        fits=new float[HouseGraph.numberOfHouses+1,1100];

        System.GC.KeepAlive(fits);
    }
 
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            Capture(0,0,0,0);
        }
    }
 
    public void Capture(int i, float points, int iteration, float time)
    {
        
        Camera.targetTexture=text;
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = Camera.targetTexture;
 
        Camera.Render();
 
        Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
 
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
 
        File.WriteAllBytes("./images/"+"EndPlan"+(int)points+"_"+iteration+"_"+time+ ".png", bytes);
        fileCounter++;
        Camera.targetTexture=null;
    }

    public void CaptureFrame(int run,float points, int iteration, bool im){

        if(currI!=run){
            currI=run;
            ii=0;
        }
        fits[run,ii]=points;
        ii++;

        if(im){
            Camera.targetTexture=text;
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = Camera.targetTexture;
    
            Camera.Render();
    
            Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
    
            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
    
            File.WriteAllBytes("C:/Users/BigPC/Desktop/GoodResults"+ "/Frames/"+run+"_"+points+"_"+iteration+ ".png", bytes);
            fileCounter++;
            Camera.targetTexture=null;
        }
        
    }
}