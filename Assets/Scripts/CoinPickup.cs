using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSfx;
    [SerializeField] private int pointsPerCoin = 100;

    private bool _wasCollected = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !_wasCollected)
        {
            _wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointsPerCoin);
            AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
