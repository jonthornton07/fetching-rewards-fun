
## Running As Docker Container
From the project root:
- Run the build - `docker build . -f FetchRewardsTakeHome/Dockerfile -t fetch:latest`
- Run the docker container - `docker run -p 5003:80 fetch:latest`
- Go to `http://localhost:5003/swagger/index.html` in your browser and it should open the swagger page

This maps the project to HTTP to avoid SSL cert issue.

## Running locally
I am using rider with .NET Core 6.  You would need to have that setup