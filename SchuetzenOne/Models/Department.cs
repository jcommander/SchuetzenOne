using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SchuetzenOne.Models;

public class Department
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull]
    public string Name { get; set; }

    [NotNull]
    public int Fee { get; set; }

    [ManyToMany(typeof(UserDepartments))]
    public List<User> Users { get; set; }
}
