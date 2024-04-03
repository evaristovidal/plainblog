FROM mcr.microsoft.com/dotnet/sdk:8.0
COPY . /src
WORKDIR /src
RUN dotnet build ./PlainBlog.Application -c Debug -o /build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=0 /build .
CMD ["/app/PlainBlog.Application"]