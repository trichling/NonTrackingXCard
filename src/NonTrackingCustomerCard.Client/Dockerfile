# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["NonTrackingCustomerCard.Client/NonTrackingCustomerCard.Client.csproj", "NonTrackingCustomerCard.Client/"]
RUN dotnet restore "NonTrackingCustomerCard.Client/NonTrackingCustomerCard.Client.csproj"

# Copy the rest of the application and build it
COPY . .
WORKDIR "/src/NonTrackingCustomerCard.Client"
RUN dotnet publish "NonTrackingCustomerCard.Client.csproj" -c Release -o /app/publish

# Stage 2: Serve the application
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]