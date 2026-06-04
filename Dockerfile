# Vi använder förhandsversionen av SDK:n för .NET 10 för att kunna bygga appen
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Kopiera ALLA lösningsfiler och projektfiler först så att referenserna hittas
COPY ["TaskFlow.Blazor/TaskFlow.Blazor.csproj", "TaskFlow.Blazor/"]
COPY ["TaskFlow.Application/TaskFlow.Application.csproj", "TaskFlow.Application/"] 2>/dev/null || true
COPY ["TaskFlow.Domain/TaskFlow.Domain.csproj", "TaskFlow.Domain/"] 2>/dev/null || true
COPY ["TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj", "TaskFlow.Infrastructure/"] 2>/dev/null || true

# Återställ beroenden för huvudprojektet
RUN dotnet restore "TaskFlow.Blazor/TaskFlow.Blazor.csproj"

# Kopiera resten av all källkod i hela repositoryt
COPY . .

# Bygg projektet
WORKDIR "/src/TaskFlow.Blazor"
RUN dotnet build "TaskFlow.Blazor.csproj" -c Release -o /app/build

# Publicera projektet
FROM build AS publish
RUN dotnet publish "TaskFlow.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Slutgiltig container – körs också på .NET 10-miljön
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "TaskFlow.Blazor.dll"]
