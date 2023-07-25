FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# copy csproj and restore as distinct layers
COPY ["LibrarianWorkplaceAPI.csproj", "./"]
RUN dotnet restore "./LibrarianWorkplaceAPI.csproj"
COPY . . 

# copy everything else and build app
COPY . .
RUN dotnet build "LibrarianWorkplaceAPI.csproj" -o /app/build

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "LibrarianWorkplaceAPI.dll", "--environment=Development"]