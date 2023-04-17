using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject submarine;
    [SerializeField] GameObject sword;
    private PlayerEventPublisher publisher;
    void Start()
    {
        publisher = new PlayerEventPublisher();
    }

    // Update is called once per frame
    private void Update()
    {
        publisher.updateSwordStatus(sword.activeInHierarchy);
    }

    public void MechToSub(){
        submarine.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SubToMech(){
        submarine.SetActive(true);
        this.gameObject.SetActive(false);
    }


    public void DrawSword()
    {
        sword.gameObject.SetActive(true);
    }

    public void SheathSword()
    {
        sword.gameObject.SetActive(false);
    }
}
