using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 6f;                  
    public float m_ExplosionRadius = 5f;

    private float m_OriginalPitch;
    private float m_PitchRange = 0.3f;

    public Rigidbody owner;


    private void Start()
    {
        m_OriginalPitch = m_ExplosionAudio.pitch;

        if (GetComponentInParent<TankShooting>().weaponType == 1)
        {
            m_ExplosionRadius = 2f;
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = 10f;
        }
        if (GetComponentInParent<TankShooting>().weaponType == 3)
        {
            m_ExplosionRadius = 10f;
        }
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        for(int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if(!targetRigidbody)
            {
                continue;
            }
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if(!targetHealth)
            {
                continue;
            }
            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);
        }
        if (GetComponentInParent<TankShooting>().weaponType == 1)//tiny explosions for triple shot
        {
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = .1f;
            m_ExplosionParticles.Emit(emitParams, 10);
            m_ExplosionAudio.volume = .3f;
        }
        if (GetComponentInParent<TankShooting>().weaponType == 2)//play the regular explosion for cannon shots
        {
            m_ExplosionParticles.Play();
        }
        if (GetComponentInParent<TankShooting>().weaponType == 3)//huge explosions for artillery shots
        {
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = 10f;
            m_ExplosionParticles.Emit(emitParams, 10);
            m_ExplosionAudio.volume = 1.3f;
        }
        m_ExplosionParticles.transform.parent = null;
        //m_ExplosionParticles.Play();
        m_ExplosionAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
        m_ExplosionAudio.Play();
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}