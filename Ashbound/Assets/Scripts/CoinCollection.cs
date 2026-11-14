using System.Collections;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [Header("Settings")]
    public int coinValue = 1;
    public AudioSource collectSound;
    public GameObject collectEffect;
    private Animator _animator;

    private bool collected;

    private void OnTriggerEnter(Collider other) => TryCollect(other);

    private void OnTriggerStay(Collider other) => TryCollect(other);

    private void Start()
    {
        _animator = GetComponent<Animator>();
        collectSound = GetComponent<AudioSource>();
    }
    private void TryCollect(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        ActionAnimationManager actions = other.GetComponentInParent<ActionAnimationManager>();
        if (actions == null || !actions.IsKicking) return;

        Collect();
    }

    private void Collect()
    {
        collected = true;

        GameManager.Instance.AddCoins(coinValue);

        

        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        _animator.SetBool("Open", true);
        collectSound.Play();
        Destroy(gameObject, 2);
    }

    IEnumerator FreezeFrames()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;

    }
}