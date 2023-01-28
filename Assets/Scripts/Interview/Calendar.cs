/*
 This static class is able to be called to get and alter information related to our calendar system.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calendar
{
    private static readonly List<string> DaysOfTheWeek = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    private static int WeekNumber = 1;
    public static int CurrentDay = 0;
    private static int SignificantActions = 0;
    public static bool InterviewAccepted = false;
    public static int DayOfInterview = -1;
    public static int DayOfEvent = 2;

    public static void IncreaseSignificantActions(int NumOfActions) { 
        SignificantActions += NumOfActions;
        if (SignificantActions > 2) 
            SignificantActions = 2;
    }

    public static int GetSignificationActions() { return SignificantActions; }

    public static void IncreaseDay()
    {

        CurrentDay++; this is what it should be
        


        // If the day of the week index is out of bounds set it to 0 or Sunday and increment the week
        if (CurrentDay > 6)
        {
            WeekNumber++;
            CurrentDay = 0;
        }

        SignificantActions = 0; // Clear any significant actions

        DayChanger.summary_messages[0] = "";
        DayChanger.summary_messages[1] = "";
        DayChanger.summary_messages[2] = "";

        /* Original Logic
                if (CurrentDay >= 7)
                {
                    CurrentDay = 0;
                }

                SignificantActions = 0;
                if (CurrentDay % 6 == 0)
                {
                    WeekNumber++;
                }
        */
        //Debug.Log("Increased Day");
        //Debug.Log(GetCurrentDay());
    }

    public static string GetCurrentDay() { return DaysOfTheWeek[CurrentDay]; }

    public static int GetCurrentDayIndex() { return CurrentDay; }

    public static int GetCurrentWeek() { return WeekNumber; }

    public static void SetCurrentWeek(int p_WeekNumber) { WeekNumber = p_WeekNumber; }
    public static void SetSignificationActions(int p_SignificantActions) {  SignificantActions = p_SignificantActions; }
}