using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public GameObject[] guns; 
    private int currentGunIndex = 0;

    void Start()
    {
        SelectGun(currentGunIndex);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            NextGun();
        }
        else if (scroll < 0f)
        {
            PreviousGun();
        }
    }

    void NextGun()
    {
        currentGunIndex = (currentGunIndex + 1) % guns.Length;
        SelectGun(currentGunIndex);
    }

    void PreviousGun()
    {
        currentGunIndex--;
        if (currentGunIndex < 0)
            currentGunIndex = guns.Length - 1;

        SelectGun(currentGunIndex);
    }

    void SelectGun(int index)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            bool isSelected = (i == index);

            MeshRenderer mesh = guns[i].GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.enabled = isSelected;

            guns[i].SetActive(isSelected);
        }
    }

    public GameObject GetCurrentGun()
    {
        return guns[currentGunIndex];
    }
}
