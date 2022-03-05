# Overview
The application is designed to be able to add/spend/get points for a set of transactions.  
Assumptions being made:
- The payer name is designed to be unique and case insensitive.  Meaning that Payer = PAYER and should be treated that way.
- The `payer/points/add` endpoint is allowed to have transaction out of order and therfore cannot do any running validation.
    - The highes amoutn of points allowed is the max integer value.  If this needs to be larger we can just change the structure for storing points.
- The `points/spend` is designed to not redeem any transactions if there are not enough points.
    - Another approach could be partial redeems, but that would change the logic.

## Test Coverage
- If you are curious about coverage there is an HTML file called `coverage.html` with the numbers.

## Running As Docker Container
From the project root:
- Run the build - `docker build . -f FetchRewardsTakeHome/Dockerfile -t fetch:latest`
- Run the docker container - `docker run -p 5279:80 fetch:latest`
- Go to `http://localhost:5279/swagger/index.html` in your browser and it should open the swagger page
- There is a postman collection if you would like to look at that `Fetch.potman_collection.json`

This maps the project to HTTP to avoid SSL cert issue.

## Running locally
I am using rider with .NET Core 6.  You would need to have that setup to run.  You could also use Visual Studio, but docker might be the recommended route in this case.

## Future Improvements
- The GET `payer/points` method could return a paginated response.  This way if there were a lot of responses then it could be queried and limited to a subset. This URL would then also have query params.
Example Payload
```json
{
    "items": [...],
    "count": 25,
    "next": "/payer/points?count=25&offset=25" //this would be null if there was no next
}
```
- Logging, for now it is not there.  However, if i was logging to another system I would be logging null values returns and bad requests.
- Authentication, I figured for this it isn't needed.
