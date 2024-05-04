using UnityEngine;

public class UnitSoundTrigger : MonoBehaviour
{
    public void PlaySound()
    {
        string unitName = gameObject.name.Replace("(Clone)", "").Trim();
        SoundManager.instance.PlayAttackSound(unitName);
    }
}
