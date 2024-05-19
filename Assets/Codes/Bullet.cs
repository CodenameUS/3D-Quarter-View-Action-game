using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            // .. 땅에 닿으면 3초뒤 삭제
            Destroy(gameObject, 3);
        }
        else if(collision.gameObject.tag == "Wall")
        {
            // .. 벽에 닿으면 삭제
            Destroy(gameObject);
        }
    }
}
    