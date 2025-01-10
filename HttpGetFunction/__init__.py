import logging
import requests
import azure.functions as func
import os

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('HTTP GET function processed a request.')

    # Retrieve query parameters
    url = req.params.get('url')
    username = req.params.get('username')
    password = req.params.get('password')

    if not url or not username or not password:
        return func.HttpResponse(
            "Please provide 'url', 'username', and 'password' as query parameters.",
            status_code=400
        )

    try:
        # Make the HTTP GET request with basic authentication
        response = requests.get(url, auth=(username, password))
        return func.HttpResponse(response.text, status_code=response.status_code)
    except Exception as e:
        logging.error(f"Error during the HTTP GET request: {e}")
        return func.HttpResponse(
            f"Error occurred: {str(e)}",
            status_code=500
        )
