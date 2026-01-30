# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and project files first to leverage Docker cache for restore
# We assume the build context is the 'backend' folder
COPY ["Bookstore.Web/Bookstore.Web.csproj", "Bookstore.Web/"]
COPY ["Bookstore.Business/Bookstore.Business.csproj", "Bookstore.Business/"]
COPY ["Bookstore.DataAccess/Bookstore.DataAccess.csproj", "Bookstore.DataAccess/"]
COPY ["Bookstore.Models/Bookstore.Models.csproj", "Bookstore.Models/"]

# Restore dependencies
RUN dotnet restore "Bookstore.Web/Bookstore.Web.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the application
WORKDIR "/src/Bookstore.Web"
RUN dotnet publish "Bookstore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bookstore.Web.dll"]
