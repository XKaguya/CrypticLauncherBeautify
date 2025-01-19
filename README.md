<div align="center">

[![Version](https://img.shields.io/github/v/release/XKaguya/CrypticLauncherBeautify?sort=semver&style=flat-square&color=8DBBE9&label=Version)]()
[![GitHub Issues](https://img.shields.io/github/issues/XKaguya/CrypticLauncherBeautify/total?style=flat-square&label=Issues&color=d77982)](https://github.com/XKaguya/CrypticLauncherBeautify)
![Downloads](https://img.shields.io/github/downloads/XKaguya/CrypticLauncherBeautify/total?style=flat-square&label=Downloads&color=d77982)

</div>

# CrypticLauncherBeautify
A tool that allows you to change the Cryptic Game Launcher's theme.

Currently only work for Star Trek Online.

## Usage
1. Download from [Releases](https://github.com/XKaguya/CrypticLauncherBeautify/releases/latest).
2. Open Cmd or PowerShell near the `.exe` file.
3. Type: `CrypticLauncherBeautify.exe -t / --Theme / ThemeName`.

### Examples:
* `CrypticLauncherBeautify.exe -t Default`
* `CrypticLauncherBeautify.exe --Theme Default`
* `CrypticLauncherBeautify.exe Default`

All of the above are equivalent but differ in syntax.

You can change almost everything you've seen, But something is defined in the css file.

*However, for css file, You need a Public IPv4 Server with SSL certificate that you're able to modify it.*

### Default Theme Template
```
<?xml version="1.0" encoding="utf-8"?>
<LoginPage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ThemeName>Default</ThemeName>
  <ArcIcon>/static/img/publisher.png</ArcIcon>
  <CssLink>/static/css/sto.css?v3.4</CssLink>
  <ForumString>Forums</ForumString>
  <SupportString>Support</SupportString>
  <AccountGuardString>Account Guard</AccountGuardString>
  <OptionString>Options</OptionString>
  <ReleaseNoteString>Release Notes</ReleaseNoteString>
  <MyAccountString>My Account</MyAccountString>
  <SignUpString>Sign Up</SignUpString>
  <ForgotPasswordString>Forgot Password</ForgotPasswordString>
  <BackgroundString>/static/img/sto/bg-login.jpg</BackgroundString>
  <HintString>Log in with your account</HintString>
  <LoginContentString>Login</LoginContentString>
  <LogoString>/static/img/sto/logo.png</LogoString>
  <AccountPlaceholderString>Account Name / Email</AccountPlaceholderString>
  <PasswordPlaceholderString>Password</PasswordPlaceholderString>
</LoginPage>
```
```
<?xml version="1.0" encoding="utf-8"?>
<EngagePage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ThemeName>Default</ThemeName>
  <ArcIcon>/static/img/publisher.png</ArcIcon>
  <CssLink>/static/css/sto.css?v3.4</CssLink>
  <ForumString>Forums</ForumString>
  <SupportString>Support</SupportString>
  <AccountGuardString>Account Guard</AccountGuardString>
  <OptionString>Options</OptionString>
  <ReleaseNoteString>Release Notes</ReleaseNoteString>
  <MyAccountString>My Account</MyAccountString>
  <BackgroundString>/static/img/sto/bg-login.jpg</BackgroundString>
  <EngageContentString>Login</EngageContentString>
  <ShardContentString>Shard</ShardContentString>
  <NewsContentString>News</NewsContentString>
  <ViewAllNewsContentString>View All</ViewAllNewsContentString>
  <HolodeckContentString>Holodeck</HolodeckContentString>
  <TribbleContentString>Tribble</TribbleContentString>
</EngagePage>
```

## Contributing
Contributions are welcome! Feel free to submit [issues](https://github.com/XKaguya/CrypticLauncherBeautify/issues) or [pull requests](https://github.com/XKaguya/CrypticLauncherBeautify/pulls) to improve the project.
