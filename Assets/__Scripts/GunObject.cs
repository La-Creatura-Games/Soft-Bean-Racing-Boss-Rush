using UnityEngine;

[CreateAssetMenu(menuName = "Gun", fileName = "New Gun")]
public class GunObject : ScriptableObject
{

    public Rigidbody2D bulletPrefab;
    public float bulletSpeedScale = 30;

}
