using UnityEngine;
using System.Collections.Generic;

public class ObjectSlicer : MonoBehaviour
{
    private bool isSliced = false;
    private Mesh originalMesh; // Store the original mesh
    private Mesh slicedMesh;   // Store the sliced mesh

    public void SliceObject()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogWarning("ObjectSlicer: MeshFilter not found on the target object.");
            return;
        }

        if (!isSliced)
        {
            // Save the original mesh
            originalMesh = meshFilter.mesh;

            // Create a copy of the mesh to modify
            Mesh mesh = Instantiate(originalMesh);
            meshFilter.mesh = mesh;

            // Get current vertices and triangles
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            // Create lists to store the visible and hidden triangles
            List<int> hiddenTriangles = new List<int>();
            List<int> visibleTriangles = new List<int>();

            // Iterate over the triangles
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v1 = vertices[triangles[i]];
                Vector3 v2 = vertices[triangles[i + 1]];
                Vector3 v3 = vertices[triangles[i + 2]];

                // Check if any vertex is above Y=0
                if (v1.y > 0 || v2.y > 0 || v3.y > 0)
                {
                    // Add to hidden triangles
                    hiddenTriangles.Add(triangles[i]);
                    hiddenTriangles.Add(triangles[i + 1]);
                    hiddenTriangles.Add(triangles[i + 2]);
                }
                else
                {
                    // Add to visible triangles
                    visibleTriangles.Add(triangles[i]);
                    visibleTriangles.Add(triangles[i + 1]);
                    visibleTriangles.Add(triangles[i + 2]);
                }
            }

            // Set submeshes
            mesh.subMeshCount = 2;
            mesh.SetTriangles(visibleTriangles.ToArray(), 0); // Visible triangles
            mesh.SetTriangles(hiddenTriangles.ToArray(), 1);  // Hidden triangles

            // Recalculate mesh data
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            isSliced = true;
        }
        else
        {
            // Restore the original mesh
            meshFilter.mesh = originalMesh;
            isSliced = false;
        }
    }
}