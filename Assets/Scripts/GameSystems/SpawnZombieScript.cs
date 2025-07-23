using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombieScript : MonoBehaviour
{
    
    public GameObject zombiePrefab;
    public float spawnInterval = 2f;
    private int currentZombies;

    public Transform upRight;
    public Transform upLeft;

    public Transform downright;
    public Transform downLeft;

    public Transform leftup;
    public Transform leftDown;

    public Transform rightUp;
    public Transform RightDown;

    public void SpawnZombie(int maxZombies)
    {
        if (currentZombies >= maxZombies)
            return;

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                spawnPos = Vector3.Lerp(upLeft.position, upRight.position, Random.value);
                spawnRot = Quaternion.LookRotation(Vector3.back);
                break;

            case 1:
                spawnPos = Vector3.Lerp(downLeft.position, downright.position, Random.value);
                spawnRot = Quaternion.LookRotation(Vector3.forward);
                break;

            case 2:
                spawnPos = Vector3.Lerp(leftDown.position, leftup.position, Random.value);
                spawnRot = Quaternion.LookRotation(Vector3.right);
                break;

            case 3:
                spawnPos = Vector3.Lerp(RightDown.position, rightUp.position, Random.value);
                spawnRot = Quaternion.LookRotation(Vector3.left);
                break;
        }

        Instantiate(zombiePrefab, spawnPos, spawnRot);
        currentZombies++;
    }

    public void ZombieDied()
    {
        currentZombies = Mathf.Max(0, currentZombies - 1);
    }
}
