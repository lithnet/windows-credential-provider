# Using the sample credential provider

## Installing the sample

In order to install and run the sample app, you have to register the COM component

Build the EXE, and from an elevated command prompt, change to the bin folder, and run the following commands

```
%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm /codebase "Lithnet.CredentialProvider.Samples.exe"

REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{1A3993B6-EB2B-44BB-A788-7AB1711DFF16}" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{1A3993B6-EB2B-44BB-A788-7AB1711DFF16}" /ve /t REG_SZ /f /d "Lithnet.CredentialProvider.Samples.TestCredentialProvider"
```

## Disable the sample

To disable the credential provider, run the following command.

```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{1A3993B6-EB2B-44BB-A788-7AB1711DFF16}" /v "Disabled" /t REG_DWORD /f /d 1
```

## Re-enable the sample
To enable the provider again after disabling it, run the following command.
```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{1A3993B6-EB2B-44BB-A788-7AB1711DFF16}" /v "Disabled" /t REG_DWORD /f /d 0
```

## Uninstalling the sample
To remove the credential provider, run the following command.

```
%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm /u "Lithnet.CredentialProvider.Samples.exe"
REG DELETE "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{1A3993B6-EB2B-44BB-A788-7AB1711DFF16}" /f
```