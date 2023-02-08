using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SchuetzenOne.Models;

public class UserTrainings
{
    [ForeignKey(typeof(User))]
    public int UserID { get; set; }

    [ForeignKey(typeof(TrainingDays))]
    public int TrainingDayID { get; set; }
}
