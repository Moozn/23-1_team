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
    private IEnumerator remove() //�ð� ������ ������Ʈ���� �ʹ� ���Ƽ� ���ɸ� �׷��� �ϴ� ���߿� ����
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
