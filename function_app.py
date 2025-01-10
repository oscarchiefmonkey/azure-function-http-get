import azure.functions as func
import logging
import requests  # Import the requests library for making HTTP requests

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="HttpExample")
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    # Extract the target URL from the query parameters
    target_url = req.params.get('url')
    if not target_url:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            target_url = req_body.get('url')

    if target_url:
        try:
            # Perform the GET request to the specified URL
            response = requests.get(target_url)
            
            # Return the response from the target URL
            return func.HttpResponse(
                response.text,
                status_code=response.status_code,
                headers={"Content-Type": response.headers.get("Content-Type", "text/plain")}
            )
        except requests.RequestException as e:
            logging.error(f"Error making GET request to {target_url}: {e}")
            return func.HttpResponse(
                f"Failed to fetch data from {target_url}. Error: {str(e)}",
                status_code=400
            )
    else:
        return func.HttpResponse(
            "Please provide a 'url' parameter in the query string or request body.",
            status_code=400
        )
