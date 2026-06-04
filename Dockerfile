# Vi använder SDK:n för .NET 10 preview för att bygga appen
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Kopiera hela lösningen på en gång 
COPY . .

# Återställ beroenden för Blazor-projektet
RUN dotnet restore "TaskFlow.Blazor/TaskFlow.Blazor.csproj"

# Bygg projektet
WORKDIR "/src/TaskFlow.Blazor"
RUN dotnet build "TaskFlow.Blazor.csproj" -c Release -o /app/build

# Publicera projektet
FROM build AS publish
RUN dotnet publish "TaskFlow.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Slutgiltig container som körs på .NET 10 runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "TaskFlow.Blazor.dll"]
