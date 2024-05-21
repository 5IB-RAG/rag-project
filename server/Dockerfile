FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY server/server.csproj .
RUN dotnet restore

COPY ./server/ .

CMD ["dotnet", "watch", "run", "--non-interactive", "--project" , "server/server.csproj",  "--urls"]


#Toccalo e sei morto