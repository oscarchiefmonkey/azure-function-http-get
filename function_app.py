import logging
import urllib.request
import base64
import azure.functions as func

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="HttpExample")
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Python HTTP trigger function processed a request.")

    # Extract the URL, username, and password from the query string
    url = req.params.get('url')
    username = req.params.get('username')
    password = req.params.get('password')

    if not url or not username or not password:
        return func.HttpResponse(
            "Please provide 'url', 'username', and 'password' as query parameters.",
            status_code=400
        )

    try:
        # Encode the credentials for Basic Authentication
        credentials = f"{username}:{password}"
        encoded_credentials = base64.b64encode(credentials.encode("utf-8")).decode("utf-8")

        # Create the request and add the Authorization header
        request = urllib.request.Request(url)
        request.add_header("Authorization", f"Basic {encoded_credentials}")

        # Perform the HTTP GET request
        with urllib.request.urlopen(request, timeout=30) as response:  # Set a timeout
            data = response.read().decode("utf-8")

        # Return the response data
        return func.HttpResponse(data, status_code=200)

    except urllib.error.URLError as e:
        logging.error(f"Error fetching URL: {e.reason}")
        return func.HttpResponse(f"Error fetching URL: {e.reason}", status_code=400)
    except Exception as ex:
        logging.error(f"Unexpected error: {ex}")
        return func.HttpResponse(f"Unexpected error: {ex}", status_code=400)
