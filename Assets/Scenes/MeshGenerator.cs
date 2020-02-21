using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    // Size of the grid
    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        // (xSize + 1) * (zSize + 1) the amount of vertices for the grid
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        

        // "<=" because zSize + 1 is your vertices amount
        for (int index = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[index] = new Vector3(x, y, z);
                index++;
            }
        }

        // create the array for the tiles (6 points for the two traingles)
        triangles = new int[xSize * zSize * 6];

        for (int vert = 0, tris = 0, z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
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
