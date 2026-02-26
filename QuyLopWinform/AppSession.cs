namespace QuyLopWinform
{
    public static class AppSession
    {
        public static int CurrentUserId { get; set; }
        public static int CurrentClassId { get; set; }

        public static void Clear()
        {
            CurrentUserId = 0;
            CurrentClassId = 0;
        }
    }
}