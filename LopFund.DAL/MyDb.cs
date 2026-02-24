namespace LopFund.DAL
{
    public class MyDb : DataClasses1DataContext   // tên này lấy đúng trong dbml
    {
        public MyDb() : base(DbFactory.ConnStr) { }
    }
}
