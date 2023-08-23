using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float dmage;
    private Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
       rigid.velocity = (transform.forward) * 10;
        StartCoroutine(remove());
    }
    private IEnumerator remove() //시간 지나면 오브젝트수가 너무 많아서 렉걸림 그래서 일단 나중에 수정
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") other.GetComponent<Player>().Hit(dmage);

        //if (other.tag != "Monster") Destroy(gameObject);
    }

}
