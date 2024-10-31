FROM mcr.microsoft.com/dotnet/aspnet:6.0 
COPY ./bin/Debug/net6.0/publish /app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080/
ENTRYPOINT ["dotnet", "DevIO.API.dll"]