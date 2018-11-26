using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSpawning : MonoBehaviour {

   
    public static List<GameObject> Adjust(List<GameObject> spawnedVeget,float InGround)
    {
        for (int i = 0; i < spawnedVeget.Count; i++)
        {

            int layer = 1 << 8;

            layer = ~layer;


            RaycastHit ray;

            if (Physics.Raycast(spawnedVeget[i].transform.position, spawnedVeget[i].transform.TransformDirection(Vector3.down), out ray, Mathf.Infinity, layer))
            {
                //Debug.DrawRay(spawnedVeget[i].transform.position, spawnedVeget[i].transform.TransformDirection(Vector3.down) * ray.distance, Color.yellow);
               //Debug.Log("hit");

                spawnedVeget[i].transform.Translate(0, -ray.distance -InGround, 0);
            }
            else
            {
                //Debug.DrawRay(spawnedVeget[i].transform.position, spawnedVeget[i].transform.TransformDirection(Vector3.down) * 1000, Color.white);
                //Debug.Log("not hit");
            }
        }

        return spawnedVeget;
    }
}
