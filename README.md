Title
===
Abstract:xxx
## Papar Information
- Title:  `CrowdLaunch Backend Codebase`
- Authors:  `Folarin Akinloye`

## Install & Dependence
- Microsoft.AspNetCore.Authentication.JwtBeare
- Microsoft.IdentityModel.Tokens
- MongoDB.Driver
- Swashbuckle.AspNetCore
- System.IdentityModel.Tokens.Jwt
- DotNetEnv

## Use
- dotnet build
  ```
  dotnet run
  ```

## Directory Hierarchy
```
|—— Controllers
|    |—— AuthController.cs
|    |—— CategoryController.cs
|    |—— NftController.cs
|    |—— NftLikeController.cs
|    |—— PortfolioController.cs
|    |—— ProjectController.cs
|    |—— ProjectDetailController.cs
|    |—— ProjectLikeController.cs
|    |—— ProjectUpdateController.cs
|    |—— UserController.cs
|    |—— UserNft.cs
|—— Helpers
|    |—— DatabaseConfig.cs
|—— Interfaces
|    |—— IAuthService.cs
|    |—— IDefaultService.cs
|    |—— INftLikeService.cs
|    |—— INftService.cs
|    |—— IPortfolioService.cs
|    |—— IProjectDetailService.cs
|    |—— IProjectLikeService.cs
|    |—— IProjectUpdateService.cs
|    |—— IUserNftService.cs
|    |—— IUserService.cs
|    |—— IprojectService.cs
|—— MiddleWare
|    |—— ErrorLoggingMiddleware.cs
|    |—— TokenMiddleware.cs
|—— Models
|    |—— Category.cs
|    |—— CustomColour.cs
|    |—— DefaultModel.cs
|    |—— MarketPlaceDBSettings.cs
|    |—— Nft.cs
|    |—— NftLike.cs
|    |—— Portfolio.cs
|    |—— Project.cs
|    |—— ProjectDetail.cs
|    |—— ProjectLike.cs
|    |—— ProjectUpdate.cs
|    |—— Socials.cs
|    |—— User.cs
|    |—— UserNft.cs
|—— Program.cs
|—— Properties
|    |—— launchSettings.json
|—— Services
|    |—— AuthService.cs
|    |—— CategoryService.cs
|    |—— NftLikeService.cs
|    |—— NftService.cs
|    |—— PortfolioService.cs
|    |—— ProjectDetailService.cs
|    |—— ProjectLikeService.cs
|    |—— ProjectService.cs
|    |—— ProjectUpdateService.cs
|    |—— UserNftService.cs
|    |—— UserService.cs
|—— Startup.cs
|—— appsettings.Development.json
|—— appsettings.json
|—— bin
|    |—— Debug
|        |—— net7.0
|            |—— AWSSDK.Core.dll
|            |—— AWSSDK.SecurityToken.dll
|            |—— DnsClient.dll
|            |—— DotNetEnv.dll
|            |—— Microsoft.AspNetCore.Authentication.JwtBearer.dll
|            |—— Microsoft.AspNetCore.OpenApi.dll
|            |—— Microsoft.IdentityModel.Abstractions.dll
|            |—— Microsoft.IdentityModel.JsonWebTokens.dll
|            |—— Microsoft.IdentityModel.Logging.dll
|            |—— Microsoft.IdentityModel.Protocols.OpenIdConnect.dll
|            |—— Microsoft.IdentityModel.Protocols.dll
|            |—— Microsoft.IdentityModel.Tokens.dll
|            |—— Microsoft.OpenApi.dll
|            |—— MongoDB.Bson.dll
|            |—— MongoDB.Driver.Core.dll
|            |—— MongoDB.Driver.dll
|            |—— MongoDB.Libmongocrypt.dll
|            |—— SharpCompress.dll
|            |—— Snappier.dll
|            |—— Sprache.dll
|            |—— Swashbuckle.AspNetCore.Swagger.dll
|            |—— Swashbuckle.AspNetCore.SwaggerGen.dll
|            |—— Swashbuckle.AspNetCore.SwaggerUI.dll
|            |—— System.IdentityModel.Tokens.Jwt.dll
|            |—— ZstdSharp.dll
|            |—— appsettings.Development.json
|            |—— appsettings.json
|            |—— crowdlaunch-backend
|            |—— crowdlaunch-backend.deps.json
|            |—— crowdlaunch-backend.dll
|            |—— crowdlaunch-backend.pdb
|            |—— crowdlaunch-backend.runtimeconfig.json
|            |—— runtimes
|                |—— linux
|                    |—— native
|                        |—— libmongocrypt.so
|                |—— osx
|                    |—— native
|                        |—— libmongocrypt.dylib
|                |—— win
|                    |—— native
|                        |—— mongocrypt.dll
|—— crowdlaunch-backend.csproj
|—— dto
|    |—— CategoryDto.cs
|    |—— NftDto.cs
|    |—— NftLikeDto.cs
|    |—— PortfolioDto..cs
|    |—— ProjectDetailDto.cs
|    |—— ProjectDto.cs
|    |—— ProjectLikeDto.cs
|    |—— ProjectUpdateDto.cs
|    |—— UserDto.cs
|    |—— UserNftDto.cs
|—— exceptions.log
|—— obj
|    |—— Debug
|        |—— net7.0
|            |—— .NETCoreApp,Version=v7.0.AssemblyAttributes.cs
|            |—— apphost
|            |—— crowdlaunch-backend.AssemblyInfo.cs
|            |—— crowdlaunch-backend.AssemblyInfoInputs.cache
|            |—— crowdlaunch-backend.GeneratedMSBuildEditorConfig.editorconfig
|            |—— crowdlaunch-backend.GlobalUsings.g.cs
|            |—— crowdlaunch-backend.MvcApplicationPartsAssemblyInfo.cache
|            |—— crowdlaunch-backend.MvcApplicationPartsAssemblyInfo.cs
|            |—— crowdlaunch-backend.assets.cache
|            |—— crowdlaunch-backend.csproj.AssemblyReference.cache
|            |—— crowdlaunch-backend.csproj.CopyComplete
|            |—— crowdlaunch-backend.csproj.CoreCompileInputs.cache
|            |—— crowdlaunch-backend.csproj.FileListAbsolute.txt
|            |—— crowdlaunch-backend.dll
|            |—— crowdlaunch-backend.genruntimeconfig.cache
|            |—— crowdlaunch-backend.pdb
|            |—— project.razor.json
|            |—— ref
|                |—— crowdlaunch-backend.dll
|            |—— refint
|                |—— crowdlaunch-backend.dll
|            |—— staticwebassets
|                |—— msbuild.build.crowdlaunch-backend.props
|                |—— msbuild.buildMultiTargeting.crowdlaunch-backend.props
|                |—— msbuild.buildTransitive.crowdlaunch-backend.props
|            |—— staticwebassets.build.json
|    |—— crowdlaunch-backend.csproj.nuget.dgspec.json
|    |—— crowdlaunch-backend.csproj.nuget.g.props
|    |—— crowdlaunch-backend.csproj.nuget.g.targets
|    |—— project.assets.json
|    |—— project.nuget.cache
```
```
