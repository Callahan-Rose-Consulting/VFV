/*
 This class is called to accept an interview and set the day of which the interview will occur
 */
using UnityEngine;

public class AcceptInterview : MonoBehaviour
{
    public void AcceptAnInterview(int DayOfInterview) // -1 if you want the interview to be the next day -JP
    {
        if (DayOfInterview == -1)
        {
            /*if ((Calendar.GetCurrentDayIndex() + 1) > 6)
            {
                DayOfInterview = 0;
            }
            else
            {
                DayOfInterview = Calendar.GetCurrentDayIndex() + 1;
            }*/

            DayOfInterview = 4;

        }
        Calendar.InterviewAccepted = true;
        Calendar.DayOfInterview = DayOfInterview;
    }
}