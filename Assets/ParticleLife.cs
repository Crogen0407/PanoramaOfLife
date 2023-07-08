using UnityEngine;

public class ParticleLife : MonoBehaviour
{
    bool isfirstCreate = true;

    void OnEnable()
    {
        if (isfirstCreate == true)
            isfirstCreate = false;
        else if(isfirstCreate == false)
            Invoke("Die", 3);
    }

    void Die()
	{
        PoolManager.Instance._objectManager.Push(tag, gameObject);
	}
}
