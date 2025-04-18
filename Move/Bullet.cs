using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    float angle; // 角度
    [SerializeField] 
    float speed; // 速度
    Vector3 velocity; // 移動量

    void Start()
    {
        // X方向の移動量を設定する
        velocity.x = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

        // Y方向の移動量を設定する
        velocity.z = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

        // 弾の向きを設定する
        float yAngle = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);

        // 5秒後に削除
        Destroy(gameObject, 5.0f);
    }
    void Update()
    {
        // 毎フレーム、弾を移動させる
        transform.position += velocity * Time.deltaTime;
    }

    // 角度と速度を設定する関数
    public void Init(float input_angle, float input_speed)
    {
        angle = input_angle;
        speed = input_speed;
    }

    //Wall通り過ぎたら消す
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Wall")) Destroy(gameObject);
    }
}