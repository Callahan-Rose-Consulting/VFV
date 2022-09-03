/*
 This class holds the data for our NACE competency skills. The player can do things to increase each skill.
 So far, all a skill can do is allow you to do certain activities like talk to certain people.
 It is not the intention that a higher skill in something will cause the player to be more successful in an interview,
    rather, skills will act as a barrier for certain things. Such as, a character cannot get a certain job without a high enough skill or skills
 */
[System.Serializable]
public class Skill
{
    public Skill(string name, int level)
    {
        Name = name;
        Level = level;
    }
    public string Name; //{ get; set; }

    public int Level; //{ get; set; }

    public bool SkillCheck(int EnemyLevel)
    {
        return Level >= EnemyLevel;
    }

    public void LevelUp()
    {
        Level++;
    }
    
    public void LevelUp(int levels)
    {
        Level += levels;
    }
}