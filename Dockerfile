# Usa a imagem do SDK .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos da solução e do projeto
COPY esferasAPI.sln ./
COPY esferasAPI/*.csproj ./esferasAPI/
RUN dotnet restore

# Copia todo o código e publica a aplicação
COPY esferasAPI/. ./esferasAPI/
WORKDIR /app/esferasAPI
RUN dotnet publish -c Release -o /out

# Usa a imagem do runtime para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /out ./

# Copia os arquivos de configuração (credenciais e variáveis de ambiente)
# COPY esferasAPI/credentialGoogleAPI.json /app/credentialGoogleAPI.json
# COPY esferasAPI/.env /app/.env

# Expõe as portas da API
EXPOSE 5101

# Define o comando de entrada do container
ENTRYPOINT ["dotnet", "esferasAPI.dll"]
