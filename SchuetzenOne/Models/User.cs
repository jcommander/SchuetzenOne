using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace SchuetzenOne.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    [NotNull]
    public DateTime Birthday { get; set; } = DateTime.UtcNow;

    [Ignore]
    public bool HasBirthday { get; set; } = false;

    [ManyToMany(typeof(UserTrainings))]
    public ObservableCollection<TrainingDays> TrainingDays { get; set; } = new ObservableCollection<TrainingDays>();

    public bool Active { get; set; } = false;
}
