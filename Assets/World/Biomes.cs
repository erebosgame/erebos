using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Biomes : MonoBehaviour
{
    /*public Element currentBiome = Element.NoElement;
    public VolumeProfile fireProfile;
    public VolumeProfile earthProfile;
    public VolumeProfile waterProfile;
    public VolumeProfile airProfile;

    int GetMaxIndex(float[,,] map)
    {
        int maxIndex = 0;
        for (int i = 1; i < map.GetLength(2); i++)
        {
            if (map[0,0,i] > map[0,0,maxIndex])
            {
                maxIndex = i;
            }
        }
        return maxIndex;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentBiome();
    }

    void UpdateCurrentBiome()
    {
        RaycastHit hit;
        Physics.Raycast(Player.gameObject.transform.position, Vector3.down, out hit, 1000f, LayerMask.GetMask("BiomesData"));
        Terrain t = hit.collider.gameObject.GetComponent<Terrain>();
        Vector3 alphaPos = new Vector3();
        alphaPos.x = ((hit.point.x - t.gameObject.transform.position.x) / t.terrainData.size.x) * t.terrainData.alphamapWidth;
        alphaPos.z = ((hit.point.z - t.gameObject.transform.position.z) / t.terrainData.size.z) * t.terrainData.alphamapHeight;
        float[,,] map = (t.terrainData.GetAlphamaps((int)alphaPos.x, (int)alphaPos.z, 1, 1));
        Element newBiome = (Element)GetMaxIndex(map);
        /*
         *      Debug.Log(t.terrainData.terrainLayers[1].maskMapTexture.width +"" + t.terrainData.terrainLayers[1].maskMapTexture.height);
                Color color = (t.terrainData.terrainLayers[1].maskMapTexture.GetPixel((int)alphaPos.x, (int)alphaPos.z));
        
                Element newBiome = (Element) color.maxColorComponent;
         
        if (currentBiome != newBiome)
        {
            currentBiome = newBiome;
            switch(currentBiome)
            {
                case Element.NoElement:
                    Camera.main.GetComponent<Volume>().profile = null;
                    break;
                case Element.Fire:
                    Camera.main.GetComponent<Volume>().profile = fireProfile;
                    break;
                case Element.Earth:
                    Camera.main.GetComponent<Volume>().profile = earthProfile;
                    break;
                case Element.Water:
                    Camera.main.GetComponent<Volume>().profile = waterProfile;
                    break;
                case Element.Air:
                    Camera.main.GetComponent<Volume>().profile = airProfile;
                    break;
            }
        }
    } */
}