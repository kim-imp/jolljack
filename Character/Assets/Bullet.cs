using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //https://chanuklee0227.blogspot.com/2017/02/unity.html 참조
    public Vector3 FirePoint;
    public Vector3 TargetPoint;
    float FireAngle;

    private void Awake()
    {
        TargetPoint = GameObject.FindWithTag("Player").transform.position;
        FirePoint = this.transform.position;
        FireAngle = 60.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 velocity = GetVelocity(FirePoint, TargetPoint, FireAngle);
        SetVelocity(velocity);
        StartCoroutine(DestroyThis(2f));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 target = new Vector2(TargetPoint.x - transform.position.x, TargetPoint.y - transform.position.y);
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg + 180;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Destroy(this.gameObject);
        else if (collision.tag == "Ground")
            Destroy(this.gameObject);
    }

    IEnumerator DestroyThis(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float InitialAngle)
    {
        print("Fire");
        float gravity = Physics.gravity.magnitude;
        float angle = InitialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - targetPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }
}
