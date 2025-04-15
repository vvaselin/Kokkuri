using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    float angle; // Šp“x
    [SerializeField] 
    float speed; // ‘¬“x
    Vector3 velocity; // ˆÚ“®—Ê

    void Start()
    {
        // X•ûŒü‚ÌˆÚ“®—Ê‚ğİ’è‚·‚é
        velocity.x = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

        // Y•ûŒü‚ÌˆÚ“®—Ê‚ğİ’è‚·‚é
        velocity.z = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

        // ’e‚ÌŒü‚«‚ğİ’è‚·‚é
        float yAngle = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);

        // 5•bŒã‚Éíœ
        Destroy(gameObject, 5.0f);
    }
    void Update()
    {
        // –ˆƒtƒŒ[ƒ€A’e‚ğˆÚ“®‚³‚¹‚é
        transform.position += velocity * Time.deltaTime;
    }

    // Šp“x‚Æ‘¬“x‚ğİ’è‚·‚éŠÖ”
    public void Init(float input_angle, float input_speed)
    {
        angle = input_angle;
        speed = input_speed;
    }

    //Wall’Ê‚è‰ß‚¬‚½‚çÁ‚·
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Wall")) Destroy(gameObject);
    }
}