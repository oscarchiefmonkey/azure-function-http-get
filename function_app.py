import azure.functions as func
import logging
import urllib.request

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="HttpExample")
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    # Get the URL from query parameters
    url = req.params.get('url')
    if not url:
        return func.HttpResponse(
            "Please provide a URL in the query string, like ?url=http://example.com",
            status_code=400
        )

    try:
        # Make an HTTP GET request to the provided URL
        with urllib.request.urlopen(url) as response:
            # Read and decode the response data
            data = response.read().decode('utf-8')
        
        # Return the fetched data as the response
        return func.HttpResponse(data, status_code=200)

    except urllib.error.URLError as e:
        logging.error(f"Failed to fetch URL: {e.reason}")
        return func.HttpResponse(
            f"Error fetching URL because: {e.reason}",
            status_code=400
        )