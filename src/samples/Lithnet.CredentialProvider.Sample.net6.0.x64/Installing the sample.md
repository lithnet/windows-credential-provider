# Using the sample credential provider

## Installing the sample

In order to install and run the sample app, you have to register the COM component

Build the EXE, and from an elevated command prompt, change to the bin folder, and run the following commands

```
regsvr32 "Lithnet.CredentialProvider.Sample.net6.0.x64.comhost.dll"

REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4cd12d80-9259-4f38-94dc-1828080ad9ff}" /f
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4cd12d80-9259-4f38-94dc-1828080ad9ff}" /ve /t REG_SZ /f /d "Lithnet.CredentialProvider.Sample.net6.0.x64"
```

## Disable the sample

To disable the credential provider, run the following command.

```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4cd12d80-9259-4f38-94dc-1828080ad9ff}" /v "Disabled" /t REG_DWORD /f /d 1
```

## Re-enable the sample
To enable the provider again after disabling it, run the following command.
```
REG ADD "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4cd12d80-9259-4f38-94dc-1828080ad9ff}" /v "Disabled" /t REG_DWORD /f /d 0
```

## Uninstalling the sample
To remove the credential provider, run the following command.

```
regsvr32 /u "Lithnet.CredentialProvider.Sample.net6.0.x64.comhost.dll"
REG DELETE "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{4cd12d80-9259-4f38-94dc-1828080ad9ff}" /f
```