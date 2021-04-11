FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["4.WebApi/Masiv.Casino.WebApi/Masiv.Casino.WebApi.csproj", "4.WebApi/Masiv.Casino.WebApi/"]
COPY ["1.Domain/Masiv.Casino.Domain.Entities/Masiv.Casino.Domain.Entities.csproj", "1.Domain/Masiv.Casino.Domain.Entities/"]
COPY ["2.Infraestructure/Masiv.Casino.Infra.IoC/Masiv.Casino.Infra.IoC.csproj", "2.Infraestructure/Masiv.Casino.Infra.IoC/"]
COPY ["3.Application/Masiv.Casino.Application.Services/Masiv.Casino.Application.Services.csproj", "3.Application/Masiv.Casino.Application.Services/"]
COPY ["3.Application/Masiv.Casino.Application.Contracts/Masiv.Casino.Application.Contracts.csproj", "3.Application/Masiv.Casino.Application.Contracts/"]
COPY ["3.Application/Masiv.Casino.Application.Interfaces/Masiv.Casino.Application.Interfaces.csproj", "3.Application/Masiv.Casino.Application.Interfaces/"]
COPY ["1.Domain/Masiv.Casino.Domain.Services/Masiv.Casino.Domain.Services.csproj", "1.Domain/Masiv.Casino.Domain.Services/"]
COPY ["1.Domain/Masiv.Casino.Domain.Interfaces/Masiv.Casino.Domain.Interfaces.csproj", "1.Domain/Masiv.Casino.Domain.Interfaces/"]
COPY ["2.Infraestructure/Masiv.Casino.Infra.Data/Masiv.Casino.Infra.Data.csproj", "2.Infraestructure/Masiv.Casino.Infra.Data/"]
RUN dotnet restore "4.WebApi/Masiv.Casino.WebApi/Masiv.Casino.WebApi.csproj"
COPY . .
WORKDIR "/src/4.WebApi/Masiv.Casino.WebApi"
RUN dotnet build "Masiv.Casino.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masiv.Casino.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Masiv.Casino.WebApi.dll"]