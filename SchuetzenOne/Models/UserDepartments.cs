using SQLite;
using SQLiteNetExtensions.Attributes;

namespace SchuetzenOne.Models;

public class UserDepartments
{
    [ForeignKey(typeof(User))]
    public int UserID { get; set; }

    [ForeignKey(typeof(Department))]
    public int DepartmentID { get; set; }
}
