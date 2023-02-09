using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace SchuetzenOne.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    
    /* Properties */
    
    public string Name { get; set; }
    public string Email { get; set; }

    [NotNull]
    public DateTime Birthday { get; set; } = DateTime.UtcNow;

    [Ignore]
    public bool HasBirthday { get; set; } = false;

    [Ignore]
    public int Fee { get; set; } = 25;

    [Ignore]
    public bool Active { get; set; } = false;

    
    /* Lists */

    [ManyToMany(typeof(UserDepartments), CascadeOperations = CascadeOperation.All)]
    public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

    [ManyToMany(typeof(UserTrainings), CascadeOperations = CascadeOperation.All)]
    public ObservableCollection<TrainingDays> TrainingDays { get; set; } = new ObservableCollection<TrainingDays>();

}
