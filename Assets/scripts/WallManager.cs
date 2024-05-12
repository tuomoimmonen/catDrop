using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform rightWall;
    [SerializeField] Transform leftWall;
    [SerializeField] Camera cam;

    void Start()
    {
        FixWalls();
    }

    private void FixWalls()
    {
        //cast to float, int gives weird results
        float aspectRatio = (float)Screen.height / Screen.width;

        //unity cam ortho size 5 = 5 units from middle to top or bottom
        float halfScreenWidth = cam.orthographicSize / aspectRatio;

        rightWall.position = new Vector3(halfScreenWidth + 0.5f, 0, 0); //assumes the wall is 1 unit wide
        leftWall.position = new Vector3(-halfScreenWidth - 0.5f, 0, 0); //assumes the wall is 1 unit wide
    }

}
