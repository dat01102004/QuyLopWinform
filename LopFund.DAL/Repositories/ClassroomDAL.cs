using System;
using System.Collections.Generic;
using System.Linq;

namespace LopFund.DAL
{
    public class ClassroomDAL
    {
        private readonly DataClasses1DataContext db =
            new DataClasses1DataContext(DbFactory.ConnStr);

        public List<Classroom> GetAll()
        {
            return db.Classrooms.OrderByDescending(x => x.ClassId).ToList();
        }

        public Classroom GetById(int classId)
        {
            return db.Classrooms.SingleOrDefault(x => x.ClassId == classId);
        }

        public void Add(string className, string inviteCode, int ownerUserId)
        {
            var entity = new Classroom
            {
                ClassName = className,
                InviteCode = inviteCode,
                OwnerUserId = ownerUserId
            };

            db.Classrooms.InsertOnSubmit(entity);
            db.SubmitChanges();
        }

        public void Update(int classId, string className, string inviteCode, int ownerUserId)
        {
            var entity = GetById(classId);
            if (entity == null) throw new Exception("Không tìm thấy lớp cần cập nhật.");

            entity.ClassName = className;
            entity.InviteCode = inviteCode;
            entity.OwnerUserId = ownerUserId;

            db.SubmitChanges();
        }

        public void Delete(int classId)
        {
            var entity = GetById(classId);
            if (entity == null) throw new Exception("Không tìm thấy lớp cần xoá.");

            db.Classrooms.DeleteOnSubmit(entity);
            db.SubmitChanges();
        }
    }
}
