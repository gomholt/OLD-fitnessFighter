using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningSystem : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 1;
    public int enemiesSpawned = 0;


    public void FillRoomWithEnemies(GameObject room)
    {
        if (!room.GetComponent<RoomProperties>().clear)
        {
            numberOfEnemies = Random.Range(1, 5);
            //room.GetComponent<RoomProperties>().enemiesInRoom = numberOfEnemies;
            enemiesSpawned = 0;
            for (int i = 0; i < numberOfEnemies; i++)
            {
                float xCoord = room.transform.position.x;
                float zCoord = room.transform.position.z;
                SpawnEnemies(room, xCoord, zCoord);
            }
        }
    }

    void SpawnEnemies(GameObject room, float xCoord, float zCoord)
    {
        GameObject enemy;
        float min_X = xCoord - (room.GetComponent<RoomProperties>().xScale / 2 - 40);
        float max_X = xCoord + (room.GetComponent<RoomProperties>().xScale / 2 - 40);

        float min_Z = zCoord - (room.GetComponent<RoomProperties>().yScale / 2 - 30);
        float max_Z = zCoord + (room.GetComponent<RoomProperties>().yScale / 2 - 30);

        bool validSpawnPoint = false;
        Vector3 spawnPoint = new Vector3();
        //while (enemiesSpawned != numberOfEnemies)
        {
            spawnPoint = new Vector3(Random.Range(min_X, max_X), 0.5f, Random.Range(min_Z, max_Z));

            validSpawnPoint = CheckIfLocationIsValid(room, spawnPoint);
            if (validSpawnPoint)
            {
                enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                enemy.GetComponent<EnemyController>().assignedRoom = gameObject.GetComponent<EnemySpawningSystem>();
                enemy.transform.parent = room.transform;
                enemiesSpawned += 1;
            }
        }
    }


    bool CheckIfLocationIsValid(GameObject room, Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(new Vector3(room.transform.position.x, room.transform.position.y + 50, room.transform.position.z),new Vector3(room.GetComponent<RoomProperties>().xScale,5,room.GetComponent<RoomProperties>().yScale), room.transform.rotation);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 center = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float depth = colliders[i].bounds.extents.z;

            float leftExtent = center.x - width;
            float rightExtent = center.x + width;
            float lowerExtent = center.z - depth;
            float upperExtent = center.z + depth;

            if (position.x >= leftExtent && position.x <= rightExtent)
            {
                if (position.z >= lowerExtent && position.z <= upperExtent)
                {
                    return false;
                }
            }

        }

        return true;
    }
}
