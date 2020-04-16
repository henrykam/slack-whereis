#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#docker run --rm -it -p 8080:5000 --name=slack-whereis hkam/slack-whereis:latest

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["slack-whereis-app/slack-whereis-app.csproj", "./slack-whereis-app/"]
RUN dotnet restore "./slack-whereis-app/slack-whereis-app.csproj"
COPY . .
WORKDIR "/src/slack-whereis-app"
RUN dotnet build "slack-whereis-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "slack-whereis-app.csproj" -c Release -o /app/publish

FROM base AS final

RUN sed -i "s|DEFAULT@SECLEVEL=2|DEFAULT@SECLEVEL=1|g" /etc/ssl/openssl.cnf
RUN apt-get clean
RUN apt-get update
RUN apt-get install -y apt-transport-https
RUN apt-get install -y gss-ntlmssp
RUN apt-get install -y tzdata
# set your timezone
RUN ln -fs /usr/share/zoneinfo/America/Vancouver /etc/localtime
RUN dpkg-reconfigure --frontend noninteractive tzdata
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "slack-whereis-app.dll"]