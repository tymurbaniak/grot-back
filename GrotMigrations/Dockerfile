FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
RUN dotnet tool install -g dotnet-ef --version 3.1.1 
ENV PATH $PATH:/root/.dotnet/tools
WORKDIR /src
COPY ["GrotWebApi/GrotWebApi.csproj", "GrotWebApi/"]
COPY ["UserManagement/UserManagement.csproj", "UserManagement/"]
COPY ["Grot/Grot.csproj", "Grot/"]
COPY ["GrotMigrations/start.bash", "UserManagement/"]
RUN dotnet restore "GrotWebApi/GrotWebApi.csproj"
COPY . .
WORKDIR "/src/UserManagement"
RUN rm -rf "/src/UserManagement/Migrations"
RUN dotnet ef migrations add InitialCreate
WORKDIR "/src/UserManagement"
CMD ["bash", "start.bash"]