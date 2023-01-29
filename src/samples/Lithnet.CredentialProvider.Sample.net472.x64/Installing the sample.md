# Using the sample credential provider

## Installing the sample

In order to install and run the sample app, you have to register the COM component

Build the EXE, and from an elevated command prompt, change to the bin folder, and run the following commands

```
SETLOCAL
SET CLSID={4EB911FA-CA18-40EA-86DF-19AFF5D1DA58}
SET BinaryPath=D:\dev\git\lithnet\windows-credential-provider\src\samples\Lithnet.CredentialProvider.Sample.net472.x64\bin\Debug\net472\Lithnet.CredentialProvider.Sample.net472.x64.dll
REM %windir%\Microsoft.NET\Framework64\v4.0.30319\regasm /codebase "Lithnet.CredentialProvider.Sample.net472.x64.dll"

REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%" /ve /t REG_SZ /f /d  "Lithnet.CredentialProvider.Samples.TestCredentialProvider"
REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\Implemented Categories\{62C8FE65-4EBB-45e7-B440-6E39B2CDBF29}"
REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\InprocServer32" /ve /t /REG_SZ /f /d "mscoree.dll"
REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\InprocServer32" /v "ThreadingModel" /t REG_SZ /f /d "Both"
REM REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\InprocServer32" /v "Class" /t REG_SZ /f /d "Both"
REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\InprocServer32" /v "RuntimeVersion" /t REG_SZ /f /d "v4.0.30319"
REG ADD "HKLM\SOFTWARE\Classes\CLSID\%CLSID%\InprocServer32" /v "CodeBase" /t REG_SZ /f /d "%BinaryPath%"

REG ADD "HKLM\SOFTWARE\Classes\Lithnet.CredentialProvider.Sample.net472.x64" /ve /t REG_SZ /f /d "Lithnet.CredentialProvider.Samples.TestCredentialProvider"
REG ADD "HKLM\SOFTWARE\Classes\Lithnet.CredentialProvider.Sample.net472.x64\CLSID" /ve /t REG_SZ /f /d "%CLSID%"


REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\%CLSID%" /ve /t REG_SZ /f /d "Lithnet.CredentialProvider.Sample.net472.x64"
```

## Disable the sample

To disable the credential provider, run the following command.

```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4eb911fa-ca18-40ea-86df-19aff5d1da58"}" /v "Disabled" /t REG_DWORD /f /d 1
```

## Re-enable the sample
To enable the provider again after disabling it, run the following command.
```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4eb911fa-ca18-40ea-86df-19aff5d1da58"}" /v "Disabled" /t REG_DWORD /f /d 0
```

## Uninstalling the sample
To remove the credential provider, run the following command.

```
%windir%\Microsoft.NET\Framework64\v4.0.30319\regasm /u "Lithnet.CredentialProvider.Sample.net472.x64.dll"
REG DELETE "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4eb911fa-ca18-40ea-86df-19aff5d1da58"}" /f
```