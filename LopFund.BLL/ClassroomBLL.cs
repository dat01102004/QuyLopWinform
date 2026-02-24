using System;
using System.Collections.Generic;
using LopFund.DAL;

namespace LopFund.BLL
{
    public class ClassroomBLL
    {
        private readonly ClassroomDAL _dal = new ClassroomDAL();

        public List<Classroom> GetAll()
        {
            return _dal.GetAll();
        }

        public void Add(string className, string inviteCode, int ownerUserId)
        {
            if (string.IsNullOrWhiteSpace(className))
                throw new Exception("Tên lớp không được để trống.");

            if (string.IsNullOrWhiteSpace(inviteCode))
                throw new Exception("Mã mời (InviteCode) không được để trống.");

            if (ownerUserId <= 0)
                throw new Exception("OwnerUserId không hợp lệ.");

            _dal.Add(className.Trim(), inviteCode.Trim(), ownerUserId);
        }

        public void Update(int classId, string className, string inviteCode, int ownerUserId)
        {
            if (classId <= 0) throw new Exception("ClassId không hợp lệ.");

            if (string.IsNullOrWhiteSpace(className))
                throw new Exception("Tên lớp không được để trống.");

            if (string.IsNullOrWhiteSpace(inviteCode))
                throw new Exception("Mã mời (InviteCode) không được để trống.");

            if (ownerUserId <= 0)
                throw new Exception("OwnerUserId không hợp lệ.");

            _dal.Update(classId, className.Trim(), inviteCode.Trim(), ownerUserId);
        }

        public void Delete(int classId)
        {
            if (classId <= 0) throw new Exception("ClassId không hợp lệ.");
            _dal.Delete(classId);
        }
    }
}
