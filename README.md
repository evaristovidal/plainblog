# PlainBlog

Plain blog test application

## Build Status

&nbsp; | `CI pipeline` | `coverage`
--- | --- | --- 
**main branch** | [![CI](https://github.com/evaristovidal/plainblog/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/evaristovidal/plainblog/actions/workflows/ci.yml) | [![codecov](https://codecov.io/gh/evaristovidal/plainblog/graph/badge.svg?token=6OAWBDQFUW)](https://codecov.io/gh/evaristovidal/plainblog)


## Overview

The application PlainBlog is an example architecture of a fake blogging system providing some basic endpoints. The get services can return Json or XML based on the Accept header.

- `GET api/post` (gets all posts)

- `GET api/post/{id}` (gets the post for the specified id)

- `GET api/post/{id}?includeAuthor=true` (gets the post for the specified id including the author details)

- `POST api/post` (adds a new blog entry)


## Installation
The easiest way to run the application is using WSL. But first add this hostname `app.plainblog.local` to your C:\Windows\System32\drivers\etc file. To get the ip address of your WSL you can use this Powershell command
```powershell
wsl -- ip -o -4 -json addr list eth0 | ConvertFrom-Json | % { $_.addr_info.local } | ? { $_ }
```

You need to execute `docker compose up`. Inside the root folder of the project run these commands.
```console
cd /mnt/c/workspace/plainbog/deploy
docker compose up
```

A container with PostgreSQL will start and create a default database, then during the application startup the EF migrations will be executed to create the database tables and seed initial data. The API will be available in http://app.plainblog.local/api/*


## Request method examples

The application can be tested using the [REST Client Extension for Visual Studio Code](https://github.com/Huachao/vscode-restclient) plugin for VSCode
Get blog posts in json format
```http
GET http://app.plainblog.local/api/post
Accept: application/json
```

Get blog posts in xml format
```http
GET http://app.plainblog.local/api/post
Accept: application/xml
```

Get blog posts by id in json format
```http
GET http://app.plainblog.local/api/post/1
Accept: application/json
```

Get blog posts by id in xml format
```http
GET http://app.plainblog.local/api/post/1
Accept: application/xml
```

Get blog posts by id including author
```http
GET http://app.plainblog.local/api/post/1?includeAuthor=true
Accept: application/json
```

Create a new blog post using json
```http
POST http://app.plainblog.local/api/post
Content-Type: application/json

{
  "authorId": 1,
  "title": "Title6",
  "description": "Description6",
  "content": "Content6"
}
```

Create a new blog post using xml. **WARNING the result is not returned as XML**. As explained in [MS docs](https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-8.0#httpresults-type) "Some features like Content negotiation aren't available." We could find alternatives like not returning HttpCode 201 or build our own http result.
```http
POST http://app.plainblog.local/api/post
Content-Type: application/xml

<PostSave>
    <AuthorId>1</AuthorId>
    <Title>Title999</Title>
    <Description>Descriptionn99</Description>
    <Content>content99</Content>
</PostSave>
```