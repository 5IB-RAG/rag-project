FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY client/client.csproj .
RUN dotnet restore

COPY ./client/ .

EXPOSE 3000

CMD ["dotnet", "watch", "run", "--non-interactive", "--project" , "client/client.csproj"]


#Toccalo e sei morto