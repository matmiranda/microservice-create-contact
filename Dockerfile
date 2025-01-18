# # Use a imagem oficial do .NET SDK
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# # Defina o diretório de trabalho
# WORKDIR /app

# # Copie os arquivos do projeto
# COPY . .

# # Restaure as dependências
# RUN dotnet restore

# # Compile o projeto
# RUN dotnet build --configuration Release --no-restore

# # Execute os testes
# RUN dotnet test FiapGrupo57Fase2.Test/FiapGrupo57Fase2.Test.csproj --configuration Release --no-build --logger "trx;LogFileName=TestResults/all-test-results.trx"

# # Publicar a aplicação
# RUN dotnet publish FiapGrupo57Fase2.WebAPI/FiapGrupo57Fase2.WebAPI.csproj --configuration Release --output /app/publish

# # Use uma imagem mais leve para o runtime
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# # Defina o diretório de trabalho
# WORKDIR /app

# # Copie os arquivos publicados do estágio de build
# COPY --from=build /app/publish .

# # Exponha a porta
# EXPOSE 80

# # Comando para iniciar a aplicação
# ENTRYPOINT ["dotnet", "FiapGrupo57Fase2.WebAPI.dll"]