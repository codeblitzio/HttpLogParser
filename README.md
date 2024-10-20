# HttpLogParser

Thanks for inviting me to do this coding test for Mantel. I enjoyed the challenge. Please reach out to me if you have any questions.

## Assumptions

I assume that the format of the log file is the Apache Combined format. I parse the sample file using a regular expression matching this format and it seems to work. If any log entry fails to match, eg by appending extra characters, then these entries are considered invalid and won't be processed.

For simplicity, I load and parse the log file in-memory. In reality, log files can be quite large and a more performant approach might be a better option for production. Ideally, analysing log files might be better achieved by analytic services such as Datadog or Splunk.

There are also some incomplete requirements such as what should be done in the case of ties when determining most widely accessed IPs and URLs. For brevity, I haven't tackled this.

## Architecture

The app has been build as a .NET 8 WebAPI. There are several reasons for this. Firstly, I'm an API developer and am comfortable with the ASP.NET framework. Secondly, this framework provides out-of-the box features such as configuration, dependency injection and easy testing via Swagger.

In the code I use the mediator pattern with the Mediatr library as this provides a clean separation of concerns. For this task, a mediator handler does all the co-ordination and heavy lifting. It delegates specifics to a file-system loader class, a regex parser class and an in-memory repository class. The app is extensible as each of these classes could be replaced by another implementation such as AWS S3 loader or another type of parser. The mediator handler is invoked from a minimal API endpoint. Dependency injection is used throughout and this greatly aids unit testing.

## Running the code

The app can be run from Visual Studio. There's a single GET endpoint that can be invoked from the Swagger UI. This endpoint will parse a log file and return the required answers via a Json payload. The sample file included with this coding test is used by default with the app. This can be changed via a configuration setting in the appsettings.json file "HttpLogUri". If you'd like to test another log file simply change this setting to point to its location and hit the endpoint again. You can also test this endpoint from Postman or from a browser using the address GET "http://localhost:5076/HttpLogReport". 

## Testing

A suite of unit tests has been provided. My test libraries of choice are xUnit and Moq. These tests cover the handler as well as the other classes such as parser and repository. For brevity, the tests aren't as exhaustive as I typically write but they do give me confidence that the functionality works as I expect.   