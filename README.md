# WiFi Password Generator

Generates WiFi passwords for WPA2-PSK networks.

Target Framework: .NET Core 3.1

# Platform-specific publishing steps

If you want to publish a single-file executable application, you need to publish for each configuration, such as Linux x64, Linux ARM64, Windows x64, and so forth. Single-file apps are always OS and architecture-specific.

## Single-File Executable, Self-Contained

https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#publish-a-single-file-app---visual-studio

Pros:
* Control .NET version
* Platform-specific targeting

Cons:
* Larger deployments
* Harder to update the .NET version

## Single-File Executable, Framework-Dependent

https://docs.microsoft.com/en-us/dotnet/core/deploying/#publish-framework-dependent

Pros:
* Small deployment
* Cross-platform
* Uses the latest patched runtime

Cons:
* Requires pre-installing the runtime
* .NET may change