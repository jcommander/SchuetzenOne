using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SchuetzenOne.Models;

public class TrainingDays
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull]
    public DateTime Date { get; set; }

    [ManyToMany(typeof(UserTrainings), CascadeOperations = CascadeOperation.All)]
    public List<User> Users { get; set; }
}
