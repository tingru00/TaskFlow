FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiera projektfilerna och återställ beroenden
COPY ["TaskFlow.Blazor/TaskFlow.Blazor.csproj", "TaskFlow.Blazor/"]
RUN dotnet restore "TaskFlow.Blazor/TaskFlow.Blazor.csproj"

# Kopiera resten av koden och bygg appen
COPY . .
WORKDIR "/src/TaskFlow.Blazor"
RUN dotnet build "TaskFlow.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskFlow.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Skapa slutgiltig container som kör appen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "TaskFlow.Blazor.dll"]
