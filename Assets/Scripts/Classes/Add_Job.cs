using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Job : MonoBehaviour
{
    // Name of programmer: Austin Greear
    //Purpose: Script solely responsible for adding a job to the static resume class. 
    //Also marks a failed interview variable in the resume static class
    public Job job;

    public void Add_Job_To_Resume()
    {
        Resume.current_job = job;

        Resume.current_job_progress = 0;

        GameManager.instance.update_job();
    }

    public void Fail_Interview()
    {
        Resume.failed_interview_job = job;
    }
}
