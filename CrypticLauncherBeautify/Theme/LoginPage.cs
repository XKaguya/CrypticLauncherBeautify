namespace CrypticLauncherBeautify.Theme;

[Serializable]
public class LoginPage
{
    public string ThemeName { get; set; } = "Default";
    public string ArcIcon { get; set; } = "/static/img/publisher.png";
    public string CssLink { get; set; } = "/static/css/sto.css?v3.4";
    public string ForumString { get; set; } = "Forum";
    public string SupportString { get; set; } = "Support";
    public string AccountGuardString { get; set; } = "Account Guard";
    public string OptionString { get; set; } = "Options";
    public string ReleaseNoteString { get; set; } = "Release Notes";
    public string MyAccountString { get; set; } = "My Account";
    public string SignUpString { get; set; } = "Sign Up";
    public string ForgotPasswordString { get; set; } = "Forgot Password";
    public string BackgroundString { get; set; } = "/static/img/sto/bg-login.jpg";
    public string HintString { get; set; } = "Log in with your account";
    public string LoginContentString { get; set; } = "Login";
    public string LogoString { get; set; } = "/static/img/sto/logo.png";
    public string AccountPlaceholderString { get; set; } = "Account Name / Email";
    public string PasswordPlaceholderString { get; set; } = "Password";
    // AND MORE
}