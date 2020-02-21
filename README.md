## AspNetCore
Asp.Net Core 相关学习


### 默认配置、服务注册管道
系统环境变量
AspNetCoreHostingModel属性：指定应用程序托管形式

 * 进程内托管 InProcess 使用IIS服务器托管
 * 进程外托管 OutProcess 使用自带Kestrel服务器托管
 ```c#
 <PropertyGroup>
    <TargetFramework>netcoreapp2.2</TargetFramework>
    <AspNetCoreHostingModel>InProcess</AspNetCoreHostingModel>
  </PropertyGroup>
```

### 中间件


# TagHelpers
