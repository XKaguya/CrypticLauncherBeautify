<div align="center">
  
[![Version](https://img.shields.io/github/v/release/XKaguya/CrypticLauncherBeautify?sort=semver&style=flat-square&color=8DBBE9&label=Version)]()
[![GitHub Issues](https://img.shields.io/github/issues/XKaguya/CrypticLauncherBeautify/total?style=flat-square&label=Issues&color=d77982)](https://github.com/XKaguya/CrypticLauncherBeautify)
![Downloads](https://img.shields.io/github/downloads/XKaguya/CrypticLauncherBeautify/total?style=flat-square&label=Downloads&color=d77982)

</div>

# CrypticLauncherBeautify

A tool that allows you to change the Cryptic Game Launcher's theme.

Currently, it only works for **Star Trek Online**.

## Usage

### Auto Update
Recommend using the Auto Update feature for faster updates and automatic updating.

#### Auto Update Usage:
1. Download `CommonUpdater.exe` from the [CommonUpdater](https://github.com/XKaguya/CommonUpdater/releases/latest) release page.
2. Place `CommonUpdater.exe` near the project `.exe` file.

---
### Ring Mode
#### Auto Start
1. Download the latest release from [Releases](https://github.com/XKaguya/CrypticLauncherBeautify/releases/latest).
2. Open Cmd or PowerShell near the `.exe` file.
3. Type the following command:
   ```bash
   CrypticLauncherBeautify.exe -t / --Theme
   ```

---

#### Manual Start
1. Download the latest release from [Releases](https://github.com/XKaguya/CrypticLauncherBeautify/releases/latest).
2. Open Cmd or PowerShell near the `.exe` file.
3. Type the following command:
   ```bash
   CrypticLauncherBeautify.exe -t / --Theme / ThemeName
   ```
4. Go to the STO root folder, which should look like:
   ```plaintext
   Steam\steamapps\common\Star Trek Online
   ```
5. Right-click a blank space, open Cmd or PowerShell here, and type:
   ```bash
   Star Trek Online.exe --remote-debugging-port=port
   ```
   - The `port` can be any value if no process is being accessed.

---
### Launcher Mode
1. Setup configs correctly:
```
<Settings>
  <!--Set to true to allow program self update. -->
  <!--Default value: true-->
  <AutoUpdate>true</AutoUpdate>
  <!--Set to true to change to Launcher mode.-->
  <!--Launcher mode is let Cryptic Launcher Beautify launch the STO Launcher. -->
  <!--Default value: false-->
  <LauncherMode>true</LauncherMode>
  <!--Set STO Launcher, the Star Trek Online.exe. -->
  <!--Default value: SET_YOUR_STO_LAUNCHER_PATH_HERE-->
  <LauncherPath>C:\Program Files (x86)\Steam\steamapps\common\Star Trek Online\Star Trek Online.exe</LauncherPath>
</Settings>
```
2. Open Cmd or PowerShell near the `.exe` file.
3. Type the following command:
   ```bash
   CrypticLauncherBeautify.exe -t / --Theme
   ```

### Examples
You can run the following commands:

- `CrypticLauncherBeautify.exe -t Default`
- `CrypticLauncherBeautify.exe --Theme Default`

Both commands are equivalent, but the syntax differs slightly.

---

### Notes
You can modify almost everything you've seen, but some elements are defined in the **CSS file**.

### **Important Note**
For the CSS file, you'll need a Public IPv4 server with an SSL certificate, and then you can modify it.

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
