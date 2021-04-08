using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int damage = 20;
    public float shotCost = 5.0f;
    public GameObject projectile = null;
    public float projCost = 50.0f;
    public float projSpeed = 50.0f;
    public float effectTime = 0.1f;
    public float maxCapacity = 100;
    public float rechargeRate = 1;
    public AudioClip shotSounds;

    private float capacity;
    private float elapsedLaserTime = 0;
    private Vector3 targetPos;
    private Vector3 originPoint;
    private Collider barrel;
    private LineRenderer laser;
    private Slider chargeMeter;
    private float elapsedShootTime = 0;
    private ParticleSystem smoke;
    private ParticleSystem flash;
    private AudioSource soundSource;
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        capacity = maxCapacity;
        smoke = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        flash = transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>();
        chargeMeter = transform.GetChild(3).GetChild(0).GetComponent<Slider>();
        laser = GetComponent<LineRenderer>();
        barrel = transform.GetChild(2).GetComponent<Collider>();
        originPoint = flash.transform.position;
        laser.SetPosition(0, originPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (capacity < maxCapacity)
        {
            elapsedShootTime += Time.deltaTime;
            capacity += rechargeRate * elapsedShootTime * Time.deltaTime;
        }
        if (capacity > maxCapacity)
        {
            capacity = maxCapacity;
        }
        chargeMeter.value = capacity / maxCapacity;
        if (Input.GetMouseButtonDown(0) && capacity >= shotCost)
        {
            ShootLaser();
        }
        if (Input.GetMouseButtonDown(1) && capacity >= projCost)
        {
            ShootProjectile();
        }
        if (laser.enabled)
        {
            elapsedLaserTime += Time.deltaTime;
            if (elapsedLaserTime >= effectTime)
            {
                laser.enabled = false;
            }
        }
        else if (elapsedLaserTime > 0)
        {
            elapsedLaserTime = 0;
        }
        if (capacity <= 75 && smoke.isPlaying == false)
        {
            smoke.Play();
        }
        if (capacity >= 75 && smoke.isPlaying == true)
        {
            smoke.Stop();
        }
    }
    private void FixedUpdate()
    {
        originPoint = flash.transform.position;
        laser.SetPosition(0, originPoint);
    }
    public void ShootLaser()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 500);

        if (hitInfo.point == new Vector3(0,0,0))
        {
            targetPos = (Camera.main.transform.forward * 500) + transform.position;
        }
        else
        {
            targetPos = hitInfo.point;
            if (hitInfo.transform.GetComponent<HitDetector>() != null)
            {
                hitInfo.transform.GetComponent<HitDetector>().Hit(damage);
            }
        }
        laser.SetPosition(1, targetPos);
        laser.enabled = true;
        flash.Emit(1);
        soundSource.Play();

        capacity -= shotCost;
        elapsedShootTime = 0;
    }
    public void ShootProjectile()
    {
        GameObject tmp = Instantiate(projectile);
        tmp.transform.position = barrel.transform.position;
        tmp.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.VelocityChange);
        capacity -= projCost;
        elapsedShootTime = 0;
    }
}
