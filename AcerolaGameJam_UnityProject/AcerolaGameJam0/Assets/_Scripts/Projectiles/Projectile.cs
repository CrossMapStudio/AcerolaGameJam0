using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float Speed, TrackingSpeed, TargetLifeTime;
    [SerializeField] private float DamageRadius;
    [SerializeField] private float Damage;
    [SerializeField] private LayerMask Damage_Layer;
    [HideInInspector]
    public Transform Target;
    private Vector2 Direction;
    [SerializeField] private Combat_Channel CombatChannel;
    [SerializeField] private CameraShake_Channel Camera_Channel;
    [SerializeField] private float Camera_Shake_Magnitude, Camera_Shake_Frequency, Camera_Shake_Smoothness;

    [SerializeField] private GameObject On_HitParticleEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Self_Destroy());
    }

    private void Update()
    {
        Direction = Vector2.Lerp(Direction, Target.position - rb.transform.position, Time.deltaTime * TrackingSpeed);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (Direction.normalized * Speed * Time.deltaTime));
        var col = Physics2D.OverlapCircle(transform.position, DamageRadius, Damage_Layer);
        if (col != null)
        {
            CombatChannel.RaiseEvent(Direction.normalized, Damage);
            Camera_Channel.RaiseEvent(Camera_Shake_Magnitude, Camera_Shake_Frequency, Camera_Shake_Smoothness);
            var clone = Instantiate(On_HitParticleEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public IEnumerator Self_Destroy()
    {
        //Spawn an Explosion ---
        yield return new WaitForSecondsRealtime(TargetLifeTime);
        var clone = Instantiate(On_HitParticleEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
