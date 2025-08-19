using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [System.Serializable]
    public class GunData
    {
        public string name;
        public GameObject gunObject;
        public bool unlocked = false;
        public int level = 0;
    }

    public List<GunData> guns = new List<GunData>();
    private int currentGunIndex = 0;

    void Start()
    {
        if (guns.Count > 0)
        {
            guns[0].unlocked = true;
            guns[0].level = 1;
            SelectGun(0);
        }
    }

    void Update()
    {
        if (guns.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            NextGun();
        else if (scroll < 0f)
            PreviousGun();
    }

    void NextGun()
    {
        int startIndex = currentGunIndex;
        do
        {
            currentGunIndex = (currentGunIndex + 1) % guns.Count;
        } while (!guns[currentGunIndex].unlocked && currentGunIndex != startIndex);

        SelectGun(currentGunIndex);
    }

    void PreviousGun()
    {
        int startIndex = currentGunIndex;
        do
        {
            currentGunIndex--;
            if (currentGunIndex < 0) currentGunIndex = guns.Count - 1;
        } while (!guns[currentGunIndex].unlocked && currentGunIndex != startIndex);

        SelectGun(currentGunIndex);
    }

    void SelectGun(int index)
    {
        for (int i = 0; i < guns.Count; i++)
        {
            bool isSelected = (i == index && guns[i].unlocked);

            MeshRenderer mesh = guns[i].gunObject.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
                mesh.enabled = isSelected;

            guns[i].gunObject.SetActive(isSelected);
        }
    }

    public Gun GetCurrentGunScript()
    {
        if (guns.Count == 0) return null;
        return guns[currentGunIndex].gunObject.GetComponent<Gun>();
    }

    public GameObject GetCurrentGun()
    {
        return guns[currentGunIndex].unlocked ? guns[currentGunIndex].gunObject : null;
    }

    public List<GunData> GetLockedGuns()
    {
        List<GunData> locked = new List<GunData>();
        for (int i = 0; i < guns.Count; i++)
        {
            if (!guns[i].unlocked)
                locked.Add(guns[i]);
        }
        return locked;
    }

    public List<GunData> GetUnlockedGuns()
    {
        List<GunData> unlocked = new List<GunData>();
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i].unlocked)
                unlocked.Add(guns[i]);
        }
        return unlocked;
    }

    public void UnlockGun(int index)
    {
        if (index >= 0 && index < guns.Count)
        {
            guns[index].unlocked = true;
            guns[index].level = 1;
            SelectGun(index);
        }
    }

    public void UpgradeGun(int index)
    {
        if (index >= 0 && index < guns.Count && guns[index].unlocked)
        {
            if (guns[index].level < 3) // max level 3
            {
                guns[index].level++;
                Debug.Log($"{guns[index].gunObject.name} upgraded to level {guns[index].level}");
            }
            else
            {
                Debug.Log($"{guns[index].gunObject.name} is already at max level!");
            }
        }
    }
}
