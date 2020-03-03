using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    int xSize;
    int zSize;

    /*public float perlinStrength = 2f;*/

    Texture2D Heightmap;
    Color32[] imgArray;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();

        //AHN3OuderlijkHuisGREY
        Heightmap = Resources.Load<Texture2D>("AHN3OuderlijkHuisGREY");
        imgArray = Heightmap.GetPixels32(0);
        
        xSize = Heightmap.width;
        zSize = Heightmap.height;
        Debug.Log("xSize: " + xSize.ToString() + " zSize: " + zSize.ToString());

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[xSize * zSize];

        float y;
        for(int index = 0, z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                if(z != zSize)
                {
                    Color32 pixel = new Color32(0,0,0,255);

                    try
                    {
                     pixel = imgArray[x + z * xSize];
                    }
                    catch
                    {
                        Debug.Log("x: " + x.ToString() + " z: " + z.ToString() + " xSize: " + xSize.ToString() + " x + z * xSize: " + (x + z * xSize).ToString()); 
                    }
                    y = pixel.g * 0.5f;
                    vertices[index] = new Vector3(x, y, z);
                    index++;
                }
            }
        }
        
        //check if 2x2 vertices or more
        if((xSize * zSize) >= 4)
        {
            // For every square there are 2 triangles, resulting in 6 points
            triangles = new int[((xSize - 1) * (zSize - 1)) * 6];

            int vert = 0;
            int tris = 0;
            for(int z = 0; z < (zSize - 1); z++)
            {
                for (int x = 0; x < (xSize - 1); x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + xSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + xSize;
                    triangles[tris + 4] = vert + xSize + 1;
                    triangles[tris + 5] = vert + 0;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }
        else
        {
            Debug.Log("Wrong file dimensions, image needs to be atleast 2x2 pixels!");
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    // Use this function to show all vertices
    /*void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for(int index = 0; index < vertices.Length; index++)
        {
            Gizmos.DrawSphere(vertices[index], .1f);
        }
    }*/
}
