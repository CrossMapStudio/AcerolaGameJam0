using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    SpriteRenderer _renderer;

    #region Channels
    [SerializeField] private Combat_Channel _channel;
    #endregion

    private Rigidbody2D _RB;
    private Vector2 TargetPosition;
    [SerializeField] private float Health = 100f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _RB = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        _RB.MovePosition((Vector2)_RB.transform.position + (TargetPosition * Time.deltaTime));
    }

    public void Update()
    {
        TargetPosition = Vector3.Lerp(TargetPosition, Vector2.zero, Time.deltaTime * 20f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.tag == "Weapon")
        {
            _channel.OnEventRaised.AddListener(Take_Damage);
            _renderer.color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            _channel.OnEventRaised.RemoveListener(Take_Damage);
            _renderer.color = Color.red;
        }
    }

    public void Take_Damage(Vector2 direction, float damage)
    {
        float dot = Vector2.Dot(direction, ((Vector2)transform.position - PlayerController.Get_Controller.Get_PlayerRB.position).normalized);
        Debug.Log("Hit Dot Product: " + dot);

        if (dot >= .2f || Vector2.Distance(transform.position, PlayerController.Get_Controller.Get_PlayerRB.position) <= .3f)
        {
            TargetPosition = ((Vector2)transform.position - PlayerController.Get_Controller.Get_PlayerRB.position).normalized * 12f;
            Health -= damage;
            Debug.Log("Damage: " + damage);
        }

        if (Health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
