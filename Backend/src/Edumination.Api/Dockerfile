# ============ build stage ============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy manifest trước
COPY nuget.config ./
COPY Edumination.Api.csproj ./

# restore (dùng đúng nuget.config)
RUN dotnet restore ./Edumination.Api.csproj --configfile ./nuget.config --disable-parallel -v minimal

# copy toàn bộ source và publish
COPY . .
RUN dotnet publish ./Edumination.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# ============ runtime stage ============
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Edumination.Api.dll"]
