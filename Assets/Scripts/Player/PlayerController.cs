using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GunSystem gunManager;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

        Gun currentGun = gunManager.GetCurrentGun().GetComponent<Gun>();

        if (currentGun == null) return;

        currentGun.TickCooldown();

        if (currentGun.Automatic)
        {
            if (Input.GetMouseButton(0) && currentGun.CanShoot)
            {
                currentGun.Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentGun.CanShoot)
            {
                currentGun.Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentGun.Reload();
        }
    }
}
