## Build Instructions

### Preparing to build

Before being able to build OpenAnnounce, there are a couple steps that need to be taken in order to prepare the solution to be built using Visual Studio.

Firstly, some dependencies need to be pulled in through NuGet. To begin, open the solution in Visual Studio 2013 or later and open the package manager console. From there, execute `Update-Package -Reinstall -ProjectName OpenAnnounce` to fetch the necessary NuGet packages.

Unfortunately, due to some weirdness with the CKEditor for ASP.NET package in NuGet, (for some reason, it tries to install an older version of the CKEditor scripts) it is necessary to install this dependency manually. This is done by downloading the latest version of CKEditor.NET from [the CKEditor website](ckeditor.com) and dropping the DLL into the OpenAnnounce project folder. (**not** the solution folder)

### Building

OpenAnnounce uses the usual Visual Studio build system in order to build. It should work the same as any other ASP.NET Web Forms project and is built in the same way.
