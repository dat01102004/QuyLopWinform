using System.Linq;

namespace LopFund.DAL
{
    public class UserDAL
    {
        private readonly DataClasses1DataContext db =
            new DataClasses1DataContext(DbFactory.ConnStr);

        public User GetByEmail(string email)
        {
            return db.Users.SingleOrDefault(x => x.Email == email);
        }
    }
}
