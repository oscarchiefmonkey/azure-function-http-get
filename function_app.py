import azure.functions as func
import logging
import requests

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="HttpExample")
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')
    response = requests.get('http://voosl-srv-ba03.no.capio.net:8090/hrm_ws/secure/persons/company/1/start-id/0/end-id/99999')
    
    # Return the response from the target URL
    return func.HttpResponse(
        response.text,
        status_code=response.status_code,
    )