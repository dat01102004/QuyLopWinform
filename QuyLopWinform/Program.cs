using System;
using System.Windows.Forms;

namespace QuyLopWinform
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var fLogin = new FrmLogin())
            {
                var rs = fLogin.ShowDialog();

                if (rs == DialogResult.OK && fLogin.LoggedInUser != null && fLogin.SelectedClassId.HasValue)
                {
                    Application.Run(new FrmMain(fLogin.LoggedInUser, fLogin.SelectedClassId.Value));
                }
                // else: thoát app
            }
        }
    }
}
