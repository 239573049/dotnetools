FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /src
COPY ["Dotnetools.Backend/Dotnetools.Backend.csproj", "Dotnetools.Backend/"]
COPY ["Dotnetools/Dotnetools.csproj", "Dotnetools/"]
RUN dotnet restore "Dotnetools.Backend/Dotnetools.Backend.csproj"
COPY . .
WORKDIR "/src/Dotnetools.Backend"
RUN dotnet build "Dotnetools.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnetools.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnetools.Backend.dll"]
