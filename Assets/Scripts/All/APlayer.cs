using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class APlayer : ALife
{
    [SerializeField] TMP_Text lifeTxt;

    private void Start()
    {
        lifeTxt.text = Health.ToString();
    }

    public override bool TakeDamage(int amountDamage)
    {
        var isDead = base.TakeDamage(amountDamage);

        lifeTxt.text = Health.ToString();

        return isDead;
    }

    private void Update()
    {
        if (transform.rotation.eulerAngles != Vector3.zero)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
