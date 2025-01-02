using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawNoiseMap(float[,] noiseMap){

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colormap = new Color[width * height];
        for (int i = 0; i < height; i++){
            for (int j = 0; j < width; j++){
                colormap[i * width + j] = Color.Lerp(Color.black, Color.white, noiseMap[i,j]);
            }
        }
        texture.SetPixels(colormap);
        texture.Apply();
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
   
}
