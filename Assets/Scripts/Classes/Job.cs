using System.Collections.Generic;

[System.Serializable]
public class Job // this is used for any job a player can have -JP
{
    public string Company;// { get; set; }
    public string Title;// { get; set; }
    public double Income;// { get; set; }
    public string Supervisor;// { get; set; }
    public List<bool> Schedule = new List<bool>();// { false, true, true, true, true, false, false }; 
    // Sunday - Saturday, false means you don't work and true means you do. -JP //

    public Job(string Company, string Title, double Income, string Supervisor)
    {
        this.Company = Company;
        this.Title = Title;
        this.Income = Income;
        this.Supervisor = Supervisor;
    }
}