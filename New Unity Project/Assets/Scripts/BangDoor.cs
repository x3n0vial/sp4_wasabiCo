using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BangDoor : MonoBehaviour
{
    public Vector3 axis;
    public GameObject pivot;
    public Canvas bangDoorCanvas;


    public float FALL_SPEED = 50.0f;
    public float FALL_ACCEL = 40.0f;

    bool isFalling = false;
    bool isBanged = false;
    float theta = 0.0f;

    Image progressBar;

    float progress = 0.0f;
    float progress_vel = 0.0f;
    float progress_accel = 0.0f;
    public float decline_accel = -2.0f;
    public float push_accel = 8.0f;
    public float MaxPushAccel = 25.0f;

    Vector3 bar_ori_pos;
    float bar_pos_offset = 0.95f;
    float bar_full_scale = 1.9f;

    AudioSource audioSource;

    void Start()
    {
        progressBar = bangDoorCanvas.gameObject.transform.Find("ProgressBar").GetComponent<Image>();

        if (progressBar == null)
            Debug.Log("Lantern::Could not find Progress Bar Image Component from Lantern");
        bar_ori_pos = progressBar.transform.localPosition;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            progress_accel += push_accel;
            if (progress_accel > MaxPushAccel)
                progress_accel = MaxPushAccel;

            // Play Audio
            audioSource.Play();
        }

        if (bangDoorCanvas.gameObject.activeSelf)
        {
            if (progress_accel >= 0)
                progress_accel += decline_accel;
           
           
            progress_vel += progress_accel * Time.deltaTime;

            if (progress_vel < 0 && progress <= 0)
            {
                progress_accel = 0;
                progress_vel = 0;
                progress = 0;
            }
            else
            {
                progress += progress_vel * Time.deltaTime;
            }

          

            progressBar.transform.localPosition = new Vector3(bar_ori_pos.x + progress * bar_pos_offset,
                   bar_ori_pos.y, bar_ori_pos.z);
            progressBar.transform.localScale = new Vector3(progress * bar_full_scale,
               progressBar.transform.localScale.y, progressBar.transform.localScale.z);

            if (progress >= 1.0f)
            {
                isFalling = true;
                bangDoorCanvas.gameObject.SetActive(false);
                isBanged = true;
            }
        }

        if (isFalling)
        {
            FALL_SPEED += FALL_ACCEL * Time.deltaTime;
            float angle = FALL_SPEED * Time.deltaTime;

            transform.RotateAround(pivot.transform.position, axis, angle);
            theta += angle;
            if (theta >= 90.0f)
            {
                isFalling = false;
                transform.RotateAround(pivot.transform.position, axis, 90.0f - theta);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBanged)
        {
            bangDoorCanvas.gameObject.SetActive(true);
        }
    }
}
