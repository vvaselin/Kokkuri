using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    float angle; // �p�x
    [SerializeField] 
    float speed; // ���x
    Vector3 velocity; // �ړ���

    void Start()
    {
        // X�����̈ړ��ʂ�ݒ肷��
        velocity.x = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

        // Y�����̈ړ��ʂ�ݒ肷��
        velocity.z = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

        // �e�̌�����ݒ肷��
        float yAngle = Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);

        // 5�b��ɍ폜
        Destroy(gameObject, 5.0f);
    }
    void Update()
    {
        // ���t���[���A�e���ړ�������
        transform.position += velocity * Time.deltaTime;
    }

    // �p�x�Ƒ��x��ݒ肷��֐�
    public void Init(float input_angle, float input_speed)
    {
        angle = input_angle;
        speed = input_speed;
    }

    //Wall�ʂ�߂��������
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Wall")) Destroy(gameObject);
    }
}