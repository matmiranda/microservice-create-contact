FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CriarContatos.Api/CriarContatos.Api.csproj", "CriarContatos.Api/"]
COPY ["CriarContatos.Domain/CriarContatos.Domain.csproj", "CriarContatos.Domain/"]
COPY ["CriarContatos.Infrastructure/CriarContatos.Infrastructure.csproj", "CriarContatos.Infrastructure/"]
COPY ["CriarContatos.Service/CriarContatos.Service.csproj", "CriarContatos.Service/"]
RUN dotnet restore "CriarContatos.Api/CriarContatos.Api.csproj"
COPY . .
WORKDIR "/src/CriarContatos.Api"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CriarContatos.Api.dll"]
