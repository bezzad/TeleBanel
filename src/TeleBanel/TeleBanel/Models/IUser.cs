namespace TeleBanel.Models
{
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}