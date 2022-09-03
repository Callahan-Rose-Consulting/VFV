/*
 This class holds lists of experiences that can be referenced during an interview.
 */
using System.Collections.Generic;

public static class Resume
{
    //MasterExperiences is meant to contain ALL experiences the player can possibly have
    //It is intended for the player to make selections from the MasterExperiences list and have it be organized into the sections of the resume, ie the other lists
    //Right now there are 
    public static List<Experience> MasterExperiences { get; set; }
    public static List<Experience> ResumeExperiences1 = new List<Experience>();
    public static List<Experience> ResumeExperiences2 = new List<Experience>();
    public static List<Experience> ResumeExperiences3 = new List<Experience>();

    public static Job current_job;

    public static int current_job_progress = 0;

    public static Job dream_job;

    public static Job failed_interview_job;


    //static vars for skill entry from Questionnaire.cs/CharacterSelection scene
    public static bool HighSchoolDiplomaBool = false;
    public static bool GEDBool = false;
    public static bool TwoYearDegree = false;
    public static bool FourYearDegree = false;
    public static bool TourofDutyBool = false;
    public static string TourName = "";
    public static string TourBranch = "";
    public static bool VolunteerBool = false;
    public static string VolunteerTitle = "";


    public static void CreateExperiences()
    {
        //placeholder variables for demonstration for the alpha
        //These are no way meant to be reused for the final game

        Experience NullSkill = new Experience();

        NullSkill.name = "N/A";

        ResumeExperiences1.Add(NullSkill);

        ResumeExperiences2.Add(NullSkill);

        ResumeExperiences3.Add(NullSkill);
    }


}