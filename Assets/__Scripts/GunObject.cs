using UnityEngine;

[CreateAssetMenu(menuName = "Gun", fileName = "New Gun")]
public class GunObject : ScriptableObject
{

    public Rigidbody2D bulletPrefab;
    public float bulletSpeed = 30;
    public float fireRate = 3;

}
