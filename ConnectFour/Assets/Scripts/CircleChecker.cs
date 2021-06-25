using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleChecker : MonoBehaviour
{

    /*
    public enum CheckDirections { Horizontal = 0, Vertical = 1 };

    private CircleCollider2D _selfCollider;

    private Vector2[] _horizontalDirections = new[] { Vector2.left, Vector2.right };

    private void Start()
    {
        _selfCollider = transform.gameObject.GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector2.right * 1.0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 1.0f, Color.red);
    }

    public void CheckAllNeighboursForPlayer(GameManager.Players player)
    {
        for (int i = 0; i < _horizontalDirections.Length; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _horizontalDirections[i], 1.0f);

            for (int j = 0; j < hits.Length; j++)
            {
                hits[j].collider.GetComponent<CircleChecker>().CheckNeighbourInDirection(_horizontalDirections[i], player);
            }
        }
    }

    public void CheckNeighbourInDirection(Vector2 direction, GameManager.Players player) 
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1.0f);
        
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != _selfCollider && hits[i].collider.tag == gameObject.tag)
            {
                //Debug.Log(player.ToString() + " in direction " + direction.ToString() + " " + gameObject.name + " hit " + hits[i].collider.name + " wich is " + hits[i].collider.tag);

                if(player == GameManager.Players.Player1) 
                {
                    GameManager.Instance.Player1Matches++;
                }
                else
                {
                    GameManager.Instance.Player2Matches++;
                }

                hits[i].collider.GetComponent<CircleChecker>().CheckNeighbourInDirection(direction, player);

                break;
            }
            else if(hits[i].collider != _selfCollider && hits[i].collider.tag != gameObject.tag) 
            {
                //Debug.Log("not enough matches");

                if (player == GameManager.Players.Player1)
                {
                    //if(GameManager.Instance.Player1Matches < 4)
                        GameManager.Instance.Player1Matches = 0;
                }
                else
                {
                    //if (GameManager.Instance.Player2Matches < 4)
                        GameManager.Instance.Player2Matches = 0;
                }
            }
        }

        if(player == GameManager.Players.Player1)
            Debug.Log(player.ToString() + " with circle" + gameObject.name + " mactches in direction " + direction.ToString() + ": " + GameManager.Instance.Player1Matches);
        else
            Debug.Log(player.ToString() + " with circle" + gameObject.name + " mactches in direction " + direction.ToString() + ": " + GameManager.Instance.Player2Matches);
    }
    */
}
