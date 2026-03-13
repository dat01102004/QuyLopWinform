using System;
using System.Linq;

namespace LopFund.DAL
{
    public class UserDAL
    {
        private readonly DataClasses1DataContext db =
            new DataClasses1DataContext(DbFactory.ConnStr);

        public User GetByEmail(string email)
        {
            email = (email ?? "").Trim();
            return db.Users.SingleOrDefault(x => x.Email == email);
        }

        public bool ExistsEmail(string email)
        {
            email = (email ?? "").Trim();
            return db.Users.Any(x => x.Email == email);
        }

        public User Insert(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            db.Users.InsertOnSubmit(user);
            db.SubmitChanges();
            return user;
        }
    }
}