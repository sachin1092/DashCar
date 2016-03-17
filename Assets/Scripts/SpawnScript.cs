using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject powerup;

    float timeElapsed = 0;
    float spawnCycle = 0.8f;
    bool spawnPowerup = true;

    void Update()
    {
        int[] x = new int[3] { -4, 0, 4 };
        timeElapsed += Time.deltaTime;
        if (timeElapsed > spawnCycle)
        {
            GameObject temp;
            if (spawnPowerup)
            {
                temp = (GameObject)Instantiate(powerup);
                
                Vector3 pos = temp.transform.position;
               
                temp.transform.position = new Vector3(x[Random.Range(0, 3)], pos.y, pos.z);
            }
            else
            {
                temp = (GameObject)Instantiate(obstacle);
                Vector3 pos = temp.transform.position;
                temp.transform.position = new Vector3(x[Random.Range(0, 3)], pos.y, pos.z);
                //temp.transform.rotation = new Quaternion(90, 0, 0, 1);
            }

            timeElapsed -= spawnCycle;
            spawnPowerup = !spawnPowerup;
        }
    }
}