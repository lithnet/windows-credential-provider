![](https://github.com/lithnet/miis-powershell/wiki/images/logo-ex-small.png)

# Windows Credential Provider
A library for creating secure Windows Credential Providers in .NET, without the COM complications.

The Lithnet Credential Provider for Windows provides an easy way to create a credential provider, without having to implement the COM components. The COM components are still there, but abstracted away into a fully managed implementation.

## Getting started
* Create a new Class Library project. You can use .NET Framework 4.6.1 or higher, or you can use .NET 6.0 or higher) to create your provider. You must build as either an x64 or x86 binary. You cannot use AnyCPU.
* Install the package from nuget `Install-Package Lithnet.CredentialProvider`

* Modify the `csproj` file and set `RegisterForComInterop` to `false`
```xml
<PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <RegisterForComInterop>false</RegisterForComInterop>
    <Platform>x64</Platform>
</PropertyGroup>
```

* If you are using .NET 6 or higher, you must also set `EnableComHosting` to `true`

```xml
<PropertyGroup>
    <TargetFramework>net6.0-windows</TargetFramework>
    <RegisterForComInterop>false</RegisterForComInterop>
    <Platform>x64</Platform>
    <EnableComHosting>true</EnableComHosting>
</PropertyGroup>
```

* Create a new class an inherit from `CredentialProviderBase`, as shown below, replacing the `ProgId` and `Guid` values with ones of your own

```cs
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ProgId("MyCredentialProvider")]
[Guid("00000000-0000-0000-0000-000000000000")]
public class MyCredentialProvider : CredentialProviderBase
{
}
```

* Override the `IsUsageScenarioSupported` method, to specify which scenarios you want to support with your credential provider

```cs
public override bool IsUsageScenarioSupported(UsageScenario cpus, CredUIWinFlags dwFlags)
{
    switch (cpus)
    {
        case UsageScenario.Logon:
        case UsageScenario.UnlockWorkstation:
        case UsageScenario.CredUI:
        case UsageScenario.ChangePassword:
            return true;

        default:
            return false;
    }
}
```

* Override the  `GetControls` method, and provide the controls to render your UI. You can conditionally render based on the current scenario
```cs
public override IEnumerable<ControlBase> GetControls(UsageScenario cpus)
{
    yield return new CredentialProviderLabelControl("CredProviderLabel", "My first credential provider");
    
    var infoLabel = new SmallLabelControl("InfoLabel", "Enter your username and password please!");
    infoLabel.State = FieldState.DisplayInSelectedTile;
    yield return infoLabel;

    yield return new TextboxControl("UsernameField", "Username");
    var password = new SecurePasswordTextboxControl("PasswordField", "Password");
    yield return password;

    if (cpus == UsageScenario.ChangePassword)
    {
        var confirmPassword = new SecurePasswordTextboxControl("ConfirmPasswordField", "Confirm password");
        yield return confirmPassword;
        yield return new SubmitButtonControl("SubmitButton", "Submit", confirmPassword);
    }
    else
    {
        yield return new SubmitButtonControl("SubmitButton", "Submit", password);
    }
}
```

* Windows will ask for the tiles to show. You can determine if you want to show a generic tile (that is, a tile not associated with a user), or a user-specific tile. Windows will provide the list of known users for you to create tiles for.
```cs
public override bool ShouldIncludeUserTile(CredentialProviderUser user)
{
    return true;
}

public override bool ShouldIncludeGenericTile()
{
    return true;
}

public override CredentialTile1 CreateGenericTile()
{
    return new MyTile(this);
}

public override CredentialTile2 CreateUserTile(CredentialProviderUser user)
{
    return new MyTile(this, user);
}
```

* Create your tile class. Inherit from `CredentialTile2` if you want to create personalized tiles supported by Windows 8 and later, or `CredentialTile1` if you only want to implement a generic tile. Grab the instances of your controls in the `Initialize` method, so you can attach to their properties to read and respond to value changes. Finally, override the `GetCredentials` method, which is called when the user clicks the submit button.

```cs
public class MyTile : CredentialTile2
{
    private TextboxControl UsernameControl;
    private SecurePasswordTextboxControl PasswordControl;
    private SecurePasswordTextboxControl PasswordConfirmControl;

    public MyTile(CredentialProviderBase credentialProvider) : base(credentialProvider)
    {
    }

    public MyTile(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider, user)
    {
    }

    public string Username
    {
        get => UsernameControl.Text;
        set => UsernameControl.Text = value;
    }

    public SecureString Password
    {
        get => PasswordControl.Password;
        set => PasswordControl.Password = value;
    }

    public SecureString ConfirmPassword
    {
        get => PasswordConfirmControl.Password;
        set => PasswordConfirmControl.Password = value;
    }

    public override void Initialize()
    {
        if (UsageScenario == UsageScenario.ChangePassword)
        {
            this.PasswordConfirmControl = this.Controls.GetControl<SecurePasswordTextboxControl>("ConfirmPasswordField");
        }

        this.PasswordControl = this.Controls.GetControl<SecurePasswordTextboxControl>("PasswordField");
        this.UsernameControl = this.Controls.GetControl<TextboxControl>("UsernameField");

        Username = this.User?.QualifiedUserName;
    }

    protected override CredentialResponseBase GetCredentials()
    {
        string username;
        string domain;

        if (Username.Contains("\\"))
        {
            domain = Username.Split('\\')[0];
            username = Username.Split('\\')[1];
        }
        else
        {
            username = Username;
            domain = Environment.MachineName;
        }

        var spassword = Controls.GetControl<SecurePasswordTextboxControl>("PasswordField").Password;

        return new CredentialResponseSecure()
        {
            IsSuccess = true,
            Password = spassword,
            Domain = domain,
            Username = username
        };
    }
}

* Build your project and you have a functional credential provider!

```
## Installing the credential provider
You can use our PowerShell module to automatically register your credential provider.

```powershell
Install-Module Lithnet.CredentialProvider.Management
Register-CredentialProvider -File C:\path-to-your-provider.dll
```

You can disable, enable, and uninstall the provider with the following commands

```powershell
Disable-CredentialProvider -File "C:\path-to-your-provider.dll"
Enable-CredentialProvider -File "C:\path-to-your-provider.dll"
Unregister-CredentialProvider -File "C:\path-to-your-provider.dll"
```

Once the credential provider is registered, you can use the included `Lithnet.CredentialProvider.TestApp.x64` from this repository to invoke CredUI and see your credential provider in action.

## How can I contribute to the project?
* Found an issue and want us to fix it? [Log it](https://github.com/lithnet/windows-credential-provider/issues)
* Want to fix an issue yourself or add functionality? Clone the project and submit a pull request

## Enteprise support
Enterprise support is not currently offered for this product.

## Keep up to date
* [Visit our blog](http://blog.lithnet.io)
* [Follow us on twitter](https://twitter.com/lithnet_io)![](http://twitter.com/favicon.ico)
