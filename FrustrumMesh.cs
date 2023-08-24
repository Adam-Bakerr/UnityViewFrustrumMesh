using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(Camera))]
public class FrustrumMesh : MonoBehaviour
{
    Camera camera;

    MeshFilter filter;
    Mesh frustrumMesh;

    [SerializeField] Transform TargetOffset;

    Vector3[] CloseCorners;
    Vector3[] FarCorners;

    [SerializeField] float farDistance = 5f;
    float closeDistance = 5f;
    void Start()
    {
        filter = GetComponent<MeshFilter>();
        camera = GetComponent<Camera>();

        frustrumMesh = new Mesh();

        CalculateFrustrumVerticies();

        CreateMesh();
    }

    private void CalculateFrustrumVerticies()
    {
        closeDistance = camera.nearClipPlane;
        if(TargetOffset != null)
        {
            //Calculate Diffrence Between Camera and target Position
            Vector3 Offset = TargetOffset.position - camera.transform.position;
            closeDistance = Offset.z;
        }

        CloseCorners = new Vector3[4];
        camera.CalculateFrustumCorners(camera.rect, closeDistance, camera.stereoActiveEye, CloseCorners);


        //Calculate the far plane of the camera at a given distance
        FarCorners = new Vector3[4];
        camera.CalculateFrustumCorners(camera.rect, farDistance, camera.stereoActiveEye, FarCorners);
    }


    private void CreateMesh()
    {
        //Concat Verticies To Single Array
        Vector3[] FrustrumVerticies = new Vector3[8];
        CloseCorners.CopyTo(FrustrumVerticies,0);
        FarCorners.CopyTo(FrustrumVerticies, 4);

        //Basic Cube Indicies
        int[] indicies = new int[] { 0,2,3,0,1,2,7,6,4,6,5,4,4,1,0,4,5,1,3,6,7,3,2,6,1,6,2,1,5,6,4,3,7,4,0,3};

        //Apply Calculated Position to Local Mesh
        frustrumMesh.vertices = FrustrumVerticies;
        frustrumMesh.triangles = indicies;

        //Push Local Mesh To Filter
        filter.mesh = frustrumMesh;
    }
}
