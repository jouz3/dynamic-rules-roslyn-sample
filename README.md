# Dynamic Rules Sample with Roslyn

Dynamic rules evaluation using [.NET Roslyn Scripting API](https://github.com/dotnet/roslyn/wiki/Scripting-API-Samples)

Sample project to implement a business dynamic rules engine using .NET Roslyn Scripting API. The class **_Evaluate.cs_** is currently used in a production environment, the rest of the code is only used for this demo.

![](img/img1.png)

The only requirement to replicate this project is the **Scripting API NuGet Package**, the use of the **_Evaluate.cs_** class is optional:

```
Install-Package Microsoft.CodeAnalysis.CSharp.Scripting
```
