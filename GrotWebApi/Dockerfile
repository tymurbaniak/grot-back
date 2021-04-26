#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
RUN dotnet tool install -g dotnet-ef --version 3.1.1 
ENV PATH $PATH:/root/.dotnet/tools
WORKDIR /src
COPY ["GrotWebApi/GrotWebApi.csproj", "GrotWebApi/"]
COPY ["UserManagement/UserManagement.csproj", "UserManagement/"]
COPY ["Grot/Grot.csproj", "Grot/"]
RUN dotnet restore "GrotWebApi/GrotWebApi.csproj"
COPY . .
WORKDIR "/src/GrotWebApi"
RUN dotnet build "GrotWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrotWebApi.csproj" -c Release -o /app/publish

FROM base AS final
COPY ["Grot/script", "/app/script"]
COPY ["Grot/script/grot/input.txt", "/app/input.txt"]
COPY ["Grot/script/grot/gallery.txt", "/app/gallery.txt"]
RUN apt-get update && apt-get install -y \
  python3 \
  python3-pip
RUN python3 -m pip install -r /app/script/grot/requirements.txt
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrotWebApi.dll"]