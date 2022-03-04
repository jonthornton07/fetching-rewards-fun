FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FetchRewardsTakeHome/FetchRewardsTakeHome.csproj", "FetchRewardsTakeHome/"]
RUN dotnet restore "FetchRewardsTakeHome/FetchRewardsTakeHome.csproj"
COPY . .
WORKDIR "/src/FetchRewardsTakeHome"
RUN dotnet build "FetchRewardsTakeHome.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FetchRewardsTakeHome.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FetchRewardsTakeHome.dll"]
