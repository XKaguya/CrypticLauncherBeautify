namespace CrypticLauncherBeautify.Native
{
    public class CrypticNativeValue
    {
        public enum NativeValue
        {
            Forum = 1,
            Support = 2,
            AccountGuard = 3,
            Options = 4,
            ReleaseNotes = 5,
            MyAccount = 6,
            SignUp = 7,
            ForgotPassword = 8,
            Shard = 9,
            News = 10,
            ViewAll = 11,
            // AND MORE
        }

        public static string NativeEnumToString(NativeValue value)
        {
            switch (value)
            {
                case NativeValue.Forum:
                    return "Forums";
                case NativeValue.Support:
                    return "Support";
                case NativeValue.AccountGuard:
                    return "Account Guard";
                case NativeValue.Options:
                    return "Options";
                case NativeValue.ReleaseNotes:
                    return "Release Notes";
                case NativeValue.MyAccount:
                    return "My Account";
                case NativeValue.SignUp:
                    return "Sign Up";
                case NativeValue.ForgotPassword:
                    return "Forgot Password";
                case NativeValue.Shard:
                    return "Shard";
                case NativeValue.News:
                    return "News";
                case NativeValue.ViewAll:
                    return "View All";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}